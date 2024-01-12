using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace PocketHeroes.Audio
{
    /// <summary>
    /// Pools AudioFiles to Save Memory
    /// </summary>
    public class AudioPooler : Singleton<AudioPooler>
    {
        [SerializeField] int trackSize = 32;
        [SerializeField] PoolBehaviour behaviour;

        List<AudioTrack> tracks = new List<AudioTrack>();
        Dictionary<Transform, List<AudioJob>> activeJobs = new Dictionary<Transform, List<AudioJob>>(); // Parent, jobs on parent
        Dictionary<AudioClip, AudioSubscription> subscriptions = new Dictionary<AudioClip, AudioSubscription>(); // Sounds that can only be played in a single instance

        [SerializeField] GameObject[] subscriptionTracks;

        enum PoolBehaviour { Ignore, Expand, Prioritize }

        public override void Awake()
        {
            base.Awake();
            CreateTracks();
        }

        void OnDisable() { StopAllCoroutines(); }

        /// <summary>
        /// Plays a Poolable Audio File
        /// </summary>
        /// <param name="audio"></param>
        public static void Play(PoolableAudio audio)
        {
            Instance.TryPlay(audio);
        }

        /// <summary>
        /// Stops a Poolable Audio File if currently played
        /// </summary>
        public static void Stop(PoolableAudio audio)
        {
            Instance.TryStop(audio);
        }

        public static void Subscribe(PoolableAudio audio)
        {
            Instance.SubscribeToTrack(audio);
        }

        public static void Unsubscribe(PoolableAudio audio)
        {
            Instance.UnsubscribeFromTrack(audio);
        }

        async void TryPlay(PoolableAudio audio)
        {
            if (audio.DelayIn > 0f) 
            { 
                await Task.Delay((int)(audio.DelayIn * 1000));
            }

            RemoveConflictingJobs(audio);

            AudioTrack track = GetAvailableTrack();

            // no empty track found
            if (track == null)
            {
                switch (behaviour)
                {
                    // Ignore the sound request completely
                    case PoolBehaviour.Ignore:
                        return;
                    // Expand tracks
                    case PoolBehaviour.Expand:
                        trackSize += 1;
                        track = new (new GameObject("[Track " + trackSize + "]"), transform);
                        tracks.Add(track);
                        break;
                    // Replace lowest priority
                    case PoolBehaviour.Prioritize:
                        track = GetLowestPriority((int)audio.Priority);

                        if (track == null) { return; } // no lower priority found

                        track.Source.Stop();
                        track.TrackGameObject.SetActive(false);
                        StopCoroutine(track.RunningJob);

                        foreach (AudioJob activeJob in activeJobs[track.TrackGameObject.transform.parent])
                        {
                            bool isActiveDuplicate = activeJob.Track.Equals(track);
                            if (isActiveDuplicate)
                            {
                                activeJobs[track.TrackGameObject.transform.parent].Remove(activeJob);
                                break;
                            }
                        }

                        track.TrackGameObject.transform.SetParent(transform);
                        break;
                }

                if (track == null) 
                { 
                    this.LogWarning("No (free) track found");
                    return;
                }
            }

            this.Log($"Play: {track.TrackGameObject.name} - {audio.Clip.name}", debug);

            AudioJob job = new (AudioJobAction.Play, audio, track, transform);
            track.SetRunningJob(ExecuteJob(job));
            StartCoroutine(track.RunningJob);

            if (!activeJobs.ContainsKey(job.Parent))
            {
                activeJobs.Add(job.Parent, new List<AudioJob>());
            }

            activeJobs[job.Parent].Add(job);
        }

        async void TryStop(PoolableAudio audio)
        {
            if (this == null) { return; }

            if (audio.DelayOut > 0f) 
            {
                await Task.Delay((int)(audio.DelayOut * 1000));
            }

            Transform parent = audio.Parent;

            parent ??= transform;
            
            if (!activeJobs.ContainsKey(parent)) 
            {
                this.LogWarning($"Stop failed: '{parent.name}' - '{audio.Name}' doesn't exist."); return;
            }

            foreach (AudioJob job in activeJobs[parent])
            {
                if (job.Audio == audio)
                {
                    this.Log("Parent found", debug);
                    
                    await Task.Delay(1); // prevent errors

                    if (this == null) { return; }

                    if (job.Track.TrackGameObject == null) 
                    { 
                        this.LogError("No GameObject on Track found.");
                        return;
                    }

                    bool hasFadeOut = audio.FadeOut > 0f;
                    
                    if (hasFadeOut)
                    {
                        this.Log("Fade Out", debug);
                        StopCoroutine(job.Track.RunningJob);

                        AudioJob fadeOutJob = new AudioJob(AudioJobAction.Stop, audio, job.Track, transform);
                        fadeOutJob.Track.SetRunningJob(ExecuteJob(job));
                        StartCoroutine(fadeOutJob.Track.RunningJob);
                        return;
                    }

                    AudioTrack track = job.Track;
                    track.TrackGameObject.transform.SetParent(transform);
                    track.TrackGameObject.SetActive(false);

                    StopCoroutine(track.RunningJob);

                    if (activeJobs[parent].Count <= 1)
                    {
                        activeJobs.Remove(parent);
                    }
                    else
                    {
                        activeJobs[parent].Remove(job);
                    }

                    this.Log($"Stopped '{audio.Name}' on '{parent.name}' '{track.TrackGameObject.name}'", debug);
                    return;
                }
            }
            this.LogWarning($"Stopping failed: '{audio.Name}' on '{parent.name}' wasn't playing");
        }

        void SubscribeToTrack(PoolableAudio audio)
        {
            if (!subscriptions.ContainsKey(audio.Clip))
            {
                GameObject track = null;

                foreach (GameObject subscriptionTrack in subscriptionTracks)
                {
                    if (!subscriptionTrack.activeSelf)
                    {
                        track = subscriptionTrack;
                    }
                }

                if (track == null) 
                { 
                    this.LogError("SubscriptionTracks are full. Shouldnt happen.");
                    return;
                }

                AudioSubscription subscription = new AudioSubscription(track);
                subscriptions.Add(audio.Clip, subscription);

                AudioSource source = audio.SetupAudioSource(track.GetComponent<AudioSource>());
                track.SetActive(true);
                track.name = $"[Sub '{audio.Name}']";
                source.Play();
            }
            subscriptions[audio.Clip].AddSubscriber();
        }

        void UnsubscribeFromTrack(PoolableAudio audio)
        {
            if (!subscriptions.ContainsKey(audio.Clip))
            {
                this.LogWarning($"No Subscription active on '{audio.Name}'");
                return;
            }

            AudioSubscription subscription = subscriptions[audio.Clip];
            subscription.RemoveSubscriber();

            if (!subscription.HasSubscribers) { return; }

            subscription.Track.GetComponent<AudioSource>().Stop();
            subscription.Track.SetActive(false);

            subscriptions.Remove(audio.Clip);
        }

        void CreateTracks()
        {
            for (int i = 0; i < trackSize; i++)
            {
                AudioTrack track = new (new GameObject($"[Track {i}]"), transform);
                tracks.Add(track);
            }
            this.Log($"Created {trackSize} tracks", debug);
        }

        void RemoveConflictingJobs(PoolableAudio audio)
        {
            Transform parent = audio.Parent;

            parent ??= transform;

            bool hasDuplicateJob = activeJobs.ContainsKey(parent);
            
            if (!hasDuplicateJob) 
            {
                activeJobs.Add(parent, new List<AudioJob>()); 
                return; 
            }

            for (int i = 0; i < activeJobs[parent].Count; i++)
            {
                bool isAlreadyActive = activeJobs[parent][i].Audio.Clip.Equals(audio.Clip);

                if (isAlreadyActive)
                {
                    AudioTrack track = activeJobs[parent][i].Track;
                    track.TrackGameObject.transform.SetParent(transform);
                    track.TrackGameObject.SetActive(false);

                    StopCoroutine(track.RunningJob);

                    activeJobs[parent].RemoveAt(i);
                    this.Log($"Removed Duplicate: '{audio.Name}' on '{parent.name}' ({i})", debug);
                    return;
                }
            }
        }

        IEnumerator ExecuteJob(AudioJob job)
        {
            AudioTrack track = job.Track;
            AudioSource source = track.Source;

            source.loop = false;
            track.TrackGameObject.SetActive(true);

            float fadeInTime = 0f;
            switch (job.Action)
            {
                case AudioJobAction.Play:
                    if (job.FadeInTime > 0f) 
                    {
                        source.volume = 0f;
                        fadeInTime = job.FadeInTime;
                    }
                    source.Play();
                    break;
                case AudioJobAction.Loop:
                    source.loop = true;

                    if (job.FadeInTime > 0f) 
                    {
                        source.volume = 0f;
                        fadeInTime = job.FadeInTime;
                    }
                    source.Play();
                    break;
            }

            if (fadeInTime > 0f)
            {
                this.Log($"(Fade - {fadeInTime}s ): Start", debug);

                float initialVolume = job.Action != AudioJobAction.Stop ? 0f : 1f;

                float targetVolume = initialVolume == 0 ? 1 : 0;
                
                float timer = 0f;

                while (timer <= fadeInTime)
                {
                    track.Source.volume = Mathf.Lerp(initialVolume, targetVolume, timer / fadeInTime);
                    timer += Time.deltaTime;
                    yield return null;
                }
                this.Log($"(Fade - {fadeInTime}s ): Done", debug);
            }

            switch (job.Action)
            {
                case AudioJobAction.Play:
                    yield return new WaitForSeconds((source.clip.length / source.pitch) - fadeInTime);
                    break;
                case AudioJobAction.Loop:
                    while (true)
                    {
                        yield return new WaitForSeconds(source.clip.length / source.pitch);
                    }
            }

            source.Stop();

            track.TrackGameObject.transform.SetParent(transform, false);
            track.TrackGameObject.SetActive(false);

            bool isLastJobOnParent = activeJobs[job.Parent].Count <= 1;
            
            if (!isLastJobOnParent)
            {
                activeJobs[job.Parent].Remove(job);
            }
            else
            {
                activeJobs.Remove(job.Parent);
            }
        }

        AudioTrack GetAvailableTrack()
        {
            foreach (AudioTrack track in tracks)
            {
                if (track.Source == null) { continue; }

                if (track.Source.isPlaying) { continue; }

                return track;
            }
            return null;
        }

        AudioTrack GetLowestPriority(int lowestPriority)
        {
            foreach (AudioTrack track in tracks)
            {
                if (track.Source.priority <= lowestPriority)
                {
                    lowestPriority = track.Source.priority;
                    return track;
                }
            }
            return null;
        }
    }
}