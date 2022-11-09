using UnityEngine;

namespace Game.Content.UI
{
    public class UIWindowEntry : ContentEntry
    {
        public int priority;
        public GameObject prefabReference;

        public bool useFader;
        public float faderDuration;
        public float faderDelay;
    }
}