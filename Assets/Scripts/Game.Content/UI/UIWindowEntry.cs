using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Content.UI
{
    public class UIWindowEntry : ContentEntry
    {
        public int priority;
        public AssetReference prefabReference;

        public bool useFader;
        public float faderDuration;
        public float faderDelay;
    }
}