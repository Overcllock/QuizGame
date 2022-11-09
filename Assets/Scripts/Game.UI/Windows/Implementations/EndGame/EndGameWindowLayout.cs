using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class EndGameWindowLayout : UIBaseWindowLayout
    {
        public GameObject winTitle;
        public GameObject loseTitle;
        
        public Button homeButton;
        public Button restartButton;
        public TMP_Text score;
    }
}