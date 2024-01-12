using UnityEngine;

namespace PocketHeroes
{
    public class BaseSO : ScriptableObject
    {
        public bool debug;
        public string notes;
        public string GetNotes()
        {
            return notes;
        }
    }
}
