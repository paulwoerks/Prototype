using UnityEngine;
using UnityEngine.Events;

namespace PocketHeroes.Core
{
    /// <summary>
    /// Base Class for Anchors
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RuntimeAnchorBase<T> : BaseSO where T : Object
    {
        public UnityAction OnAnchorProvided;

        public bool IsSet => value != null; // Any script can check if the transform is null before using it, by just checking this bool

        public T Value => value;
        [SerializeField] T value;

        public void Provide(T value)
        {
            this.value = value;

            if (IsSet)
                OnAnchorProvided?.Invoke();
        }

        public void Unset() => value = null;
    }
}