using System;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = "Channel Events/Float")]
    public class FloatEventChannelSO : EventChannelSO
    {
        [SerializeField] event Action<float> OnInvoke;

        public void Invoke(float value)
        {
            if (OnInvoke == null) return;

            PrintSubscribers();

            OnInvoke(value);
        }

        public void Subscribe(Action<float> call, object subscriber)
        {
            OnInvoke += call;
            base.AddSubscriber(subscriber);
        }

        public void Unsubscribe(Action<float> call, object subscriber)
        {
            OnInvoke -= call;
            base.RemoveSubscriber(subscriber);
        }
    }
}
