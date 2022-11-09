using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class GameFieldItemWidgetLayout : UIBaseLayout
    {
        public CanvasGroup canvasGroup;
        public Button button;
        public GameObject inactiveImage;
        public GameObject activeImage;
        public TMP_Text text;
    }
}