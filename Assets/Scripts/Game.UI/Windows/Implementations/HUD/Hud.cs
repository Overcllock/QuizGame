using System;
using Game.Content.UI;
using Game.Quiz;
using UnityEngine;

namespace Game.UI.Windows
{
    public class Hud : UIWindow<HudLayout, HudWindowArgs>
    {
        public override string id => WindowType.HUD;

        private QuizService _quizService;
        
        private UIWindowsDispatcher _windowsDispatcher;
        
        protected override void OnInitialize()
        {
            ServiceLocator.Get(out _quizService);
            ServiceLocator.Get(out _windowsDispatcher);
        }
        
        protected override void OnShow(HudWindowArgs args)
        {
            base.OnShow(args);
            
            _quizService.dataChanged += HandleDataChanged;
            
            if (_layout != null)
            {
                _layout.pauseButton.onClick.AddListener(HandlePauseClick);
                
                UpdateLayout(_quizService.GetData());
                
                if (!_entry.useFader)
                {
                    _layout.rootObject.SetActive(true);
                }
            }
        }

        protected override void OnHide()
        {
            base.OnHide();

            _quizService.dataChanged -= HandleDataChanged;

            if (_layout != null)
            {
                _layout.pauseButton.onClick.RemoveAllListeners();
                
                if (!_entry.useFader)
                {
                    _layout.rootObject.SetActive(false);
                }
            }
        }
        
        private void UpdateLayout(QuizData data)
        {
            _layout.score.text = Mathf.Clamp(data.score, 0, Int32.MaxValue).ToString();
            _layout.attemptsCount.text = Mathf.Clamp(data.attempts, 0, Int32.MaxValue).ToString();
        }

        private void HandleDataChanged(QuizData data)
        {
            if (_layout != null)
            {
                UpdateLayout(data);
            }
        }

        private void HandlePauseClick()
        {
            _windowsDispatcher.ShowWindow<PauseWindow>();
        }
    }

    public class HudWindowArgs : IWindowArgs
    {
    }
}