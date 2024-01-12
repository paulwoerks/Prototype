using System;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = "Channel Events/Vector3")]
    public class Vector3EventChannelSO : EventChannelSO
    {
        [SerializeField] event Action<Vector3> OnInvoke;

        public void Invoke(Vector3 value)
        {
            if (OnInvoke == null) return;

            PrintSubscribers();

            OnInvoke(value);
        }

        public void Subscribe(Action<Vector3> call, object subscriber)
        {
            OnInvoke += call;
            base.AddSubscriber(subscriber);
        }

        public void Unsubscribe(Action<Vector3> call, object subscriber)
        {
            OnInvoke -= call;
            base.RemoveSubscriber(subscriber);
        }
    }
}
