using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Content.UI
{
    [CreateAssetMenu(menuName = "Content/GUI/Window Settings", fileName = "Window")]
    [DefinedConstants]
    public class UIWindowSettingsScrobject : BaseSettingsScrobject
    {
        public int priority;

        [Space]
        public GameObject prefabReference;

        [Space]
        public bool useFader;

        [ShowIf("useFader")]
        public float faderDuration = 0.5f;
        
        [ShowIf("useFader")]
        public float faderDelay;
    }
}