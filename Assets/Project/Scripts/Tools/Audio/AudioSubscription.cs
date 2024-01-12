using UnityEngine;

namespace PocketHeroes.Audio
{
    /// <summary>
    /// Continues to play one audio file as long as at least 1 Subscriber is active.
    /// </summary>
    public class AudioSubscription
    {
        public GameObject Track { get; private set; }

        int subscriberCount;
        public bool HasSubscribers => subscriberCount > 0;

        public AudioSubscription(GameObject track)
        {
            Track = track;
        }

        public void AddSubscriber()
        {
            subscriberCount += 1;
        }

        public void RemoveSubscriber()
        {
            subscriberCount -= 1;
        }
    }
}