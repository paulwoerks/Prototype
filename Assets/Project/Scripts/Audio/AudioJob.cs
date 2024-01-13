using UnityEngine;

namespace PocketHeroes.Audio
{
    public class AudioJob
    {
        public AudioTrack Track { get; private set; }
        public AudioJobAction Action { get; private set; }

        // Audio Settings
        public float SpatialBlend { get; private set; }

        public float FadeInTime => Audio.FadeIn;
        public float FadeOutTime => Audio.FadeOut;

        public Transform Parent { get; private set; }

        public PoolableAudio Audio { get; private set; }

        public AudioJob(AudioJobAction action, PoolableAudio audio, AudioTrack track, Transform poolerTransform)
        {
            this.Audio = audio;
            Track = track;
            Action = action;

            if (action.Equals(AudioJobAction.Play))
            {
                AudioSource source = audio.SetupAudioSource(track.Source);

                Parent = audio.Parent;

                if (audio.Parent == null) 
                { 
                    source.spatialBlend = 0f;
                    Parent = poolerTransform; 
                }

                track.TrackGameObject.transform.SetParent(Parent, false);
                track.TrackGameObject.transform.localPosition = Vector3.zero;

                if (track.Source.loop) 
                { 
                    Action = AudioJobAction.Loop;
                }
            }
        }
    }
}
