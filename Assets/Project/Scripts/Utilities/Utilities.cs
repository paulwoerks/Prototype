using UnityEngine;

namespace PocketHeroes.Characters
{
    public static class Utilities
    {
        public static Quaternion GetRotationTowards(Vector3 from, Vector3 to)
        {
            from.y = 0;
            to.y = 0;
            return Quaternion.LookRotation((to - from).normalized);
        }
    }
}
