using System;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = "Channel Events/Void")]
    public class VoidEventChannelSO : EventChannelSO
    {
        [SerializeField] event Action OnInvoke;

        public void Invoke()
        {
            if (OnInvoke == null) return;

            PrintSubscribers();

            OnInvoke();
        }

        public void Subscribe(Action call, object subscriber)
        {
            OnInvoke += call;
            base.AddSubscriber(subscriber);
        }

        public void Unsubscribe(Action call, object subscriber)
        {
            OnInvoke -= call;
            base.RemoveSubscriber(subscriber);
        }
    }
}
