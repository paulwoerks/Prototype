using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    public class EventChannelSO : BaseSO
    {
        public EventChannelSubscribers subscribers = new();

        public virtual void PrintSubscribers()
        {
            if (!debug) return;

            string debugText = "Invoked GameObjects: ";

            foreach (GameObject subscriber in subscribers.GameObjects)
            {
                debugText += $"[*] '{subscriber.name}', ";
            }

            debugText += $"\nInvoked ScriptableObjects:";

            foreach (ScriptableObject so in subscribers.ScriptableObjects)
            {
                debugText += $"[*] '{so.name}', ";
            }

            this.Log(debugText, true);
        }

        public virtual void AddSubscriber(object subscriber)
        {
            if (subscriber == null) return;

            if (subscriber.GetType().Equals(typeof(GameObject)))
            {
                subscribers.GameObjects.Add((GameObject)subscriber);
                this.Log($"[+] '{((GameObject)subscriber).name}'", debug);
                return;
            }

            if (subscriber.GetType().Equals(typeof(ScriptableObject)))
            {
                subscribers.ScriptableObjects.Add((ScriptableObject)subscriber);
                this.Log($"[+] '{((ScriptableObject)subscriber).name}'", debug);
                return;
            }
        }

        public virtual void RemoveSubscriber(object subscriber)
        {
            if (subscriber == null) return;

            if (subscriber.GetType().Equals(typeof(GameObject)))
            {
                subscribers.GameObjects.Remove((GameObject)subscriber);
                this.Log($"[-] '{((GameObject)subscriber).name}'", debug);
                return;
            }

            if (subscriber.GetType().Equals(typeof(ScriptableObject)))
            {
                subscribers.ScriptableObjects.Remove((ScriptableObject)subscriber);
                this.Log($"[-] '{((ScriptableObject)subscriber).name}'", debug);
                return;
            }
        }
    }

    [System.Serializable]
    public class EventChannelSubscribers
    {
        public List<GameObject> GameObjects = new();
        public List<ScriptableObject> ScriptableObjects = new();
    }
}
