using System;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = "Channel Events/Vector2")]
    public class Vector2EventChannelSO : EventChannelSO
    {
        [SerializeField] event Action<Vector2> OnInvoke;

        public void Invoke(Vector2 value)
        {
            if (OnInvoke == null) return;

            PrintSubscribers();

            OnInvoke(value);
        }

        public void Subscribe(Action<Vector2> call, object subscriber)
        {
            OnInvoke += call;
            base.AddSubscriber(subscriber);
        }

        public void Unsubscribe(Action<Vector2> call, object subscriber)
        {
            OnInvoke -= call;
            base.RemoveSubscriber(subscriber);
        }
    }
}