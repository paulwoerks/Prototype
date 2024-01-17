using System;
using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes.Characters
{
    /// <summary>
    /// Needs to be put on the same component as the animator. Scripts can subscribe functions to animation triggers that will be called once the animation trigger is fired.
    /// To keep it clean make sure to name the animation triggers always like the animation.
    /// </summary>
    public class AnimationEventHandler : MonoBehaviour
    {
        class ActionReference
        {
            public Action StoredAction { get; private set; } // Stores the function to be called

            public bool UnsubscribeAfterInvoke { get; private set; } // unsubscribes automatically after bieng called

            /// <summary>
            /// Store the function you want to call and remember if you want to keep subscribed after call.
            /// </summary>
            /// <param name="actionToStore"></param>
            /// <param name="unsubscribeAfterInvoke"></param>
            public ActionReference(Action actionToStore, bool unsubscribeAfterInvoke)
            {
                StoredAction = actionToStore;
                UnsubscribeAfterInvoke = unsubscribeAfterInvoke;
            }
        }

        Dictionary<string, ActionReference> eventDictionary = new Dictionary<string, ActionReference>();

        /// <summary>
        /// Subscribe to and animationEvent and be triggered once its invoked.
        /// </summary>
        /// <param name="animationName">Look up what the animation is called you want to subscribe to. For Example 'Attack_Sword'</param>
        /// <param name="action">The Function you want to be called when the animationEvent gets invoked.</param>
        /// /// <param name="unsubscribeAfterInvoke">If false, the event will be called continuesly. if true, the event will be only called once when the AnimationEvent is triggered.</param>
        public void Subscribe(string animationName, Action action, bool unsubscribeAfterInvoke = true)
        {
            eventDictionary.Add(animationName, new ActionReference(action, unsubscribeAfterInvoke));
        }

        /// <summary>
        /// Unsubscribe from animationEvent manually. Once the animation is triggered, the function unsubscribes automatically.
        /// </summary>
        public void Unsubscribe(string animationName)
        {
            if (!eventDictionary.ContainsKey(animationName)) { return; }

            eventDictionary.Remove(animationName);
        }

        /// <summary>
        /// Gets triggered by the Animator, once an animationEvent is invoked.
        /// </summary>
        /// <param name="animationName"></param>
        public void TriggerAnimationEvent(string animationName)
        {
            if (eventDictionary.TryGetValue(animationName, out ActionReference animationEvent))
            {
                animationEvent.StoredAction.Invoke();
                if (animationEvent.UnsubscribeAfterInvoke)
                {
                    eventDictionary.Remove(animationName);
                }
            }
        }
    }
}
