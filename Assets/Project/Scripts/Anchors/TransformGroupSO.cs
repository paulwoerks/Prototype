using System.Collections.Generic;
using UnityEngine;
using System;

namespace PocketHeroes.Core
{
    [CreateAssetMenu(menuName = "Group/Transform Group")]
    public class TransformGroupSO : BaseSO
    {
        [SerializeField] List<Transform> transforms = new();
        public List<Transform> Transforms => transforms;
        public Action OnUpdated { get; private set; }

        void OnDisable() => transforms = new();

        /// <summary>
        /// Add transform to group, OnUpdated gets invoked.
        /// </summary>
        /// <param name="transform">Object to add</param>
        public void Add(Transform transform)
        {
            if (transforms.Contains(transform))
                return;

            transforms.Add(transform);
            OnUpdated?.Invoke();
        }
        /// <summary>
        /// Remove transform from group, OnUpdated gets invoked.
        /// </summary>
        /// <param name="transform">Object to remove</param>
        public void Remove(Transform transform)
        {
            if (transform == null || !transforms.Contains(transform))
                return;

            transforms.Remove(transform);
            OnUpdated?.Invoke();
        }

        /// <summary>
        /// Get the member closest to position.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public Transform GetClosest(Vector3 from, float range = float.MaxValue)
        {
            if (transforms.Count == 0)
                return null;

            float closestDistance = range;
            Transform closestTransform = null;

            foreach (Transform transform in transforms)
            {
                float distance = Vector3.Distance(from, transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTransform = transform;
                }
            }
            return closestTransform;
        }

        /// <summary>
        /// Get all members in Range of a certain distance.
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public Transform[] GetInRange(Vector3 fromPosition, float range)
        {
            List<Transform> transformsInRange = new();

            foreach (Transform transform in transforms)
            {
                if (Vector3.Distance(fromPosition, transform.position) <= range)
                    transformsInRange.Add(transform);
            }
            return transformsInRange.ToArray();
        }
    }
}
