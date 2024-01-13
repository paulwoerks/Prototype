using System.Collections;
using UnityEngine;

namespace PocketHeroes.Audio
{
    /// <summary>
    /// A Track hoding an Audio Source, playing a Poolable AudioFile
    /// </summary>
    public class AudioTrack
    {
        public IEnumerator RunningJob { get; private set; }
        public GameObject TrackGameObject { get; private set; }
        public AudioSource Source { get; private set; }

        public AudioTrack(GameObject gameObject, Transform parent)
        {
            TrackGameObject = gameObject;
            Source = gameObject.AddComponent<AudioSource>();
            gameObject.transform.SetParent(parent);
            TrackGameObject.SetActive(false);
        }

        public void SetRunningJob(IEnumerator runningJob)
        {
            RunningJob = runningJob;
        }
    }
}
