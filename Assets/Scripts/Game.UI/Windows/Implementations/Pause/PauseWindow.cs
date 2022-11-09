using Game.Content.UI;
using Game.Quiz;

namespace Game.UI.Windows
{
    public class PauseWindow : UIWindow<PauseWindowLayout, PauseWindowArgs>
    {
        public override string id => WindowType.PAUSE_WINDOW;
        
        private QuizService _quizService;
        private GameLoopService _gameLoopService;
        
        protected override void OnInitialize()
        {
            ServiceLocator.Get(out _quizService);
            ServiceLocator.Get(out _gameLoopService);
        }
        
        protected override void OnShow(PauseWindowArgs args)
        {
            base.OnShow(args);
            
            if (_layout != null)
            {
                if (!_entry.useFader)
                {
                    _layout.rootObject.SetActive(true);
                }
                
                _layout.continueButton.button.onClick.AddListener(HandleContinueClick);
                _layout.quitButton.button.onClick.AddListener(HandleQuitClick);
                _layout.restartButton.button.onClick.AddListener(HandleRestartClick);
            }
        }

        protected override void OnHide()
        {
            base.OnHide();
            
            if (_layout != null)
            {
                _layout.continueButton.button.onClick.RemoveAllListeners();
                _layout.quitButton.button.onClick.RemoveAllListeners();
                _layout.restartButton.button.onClick.RemoveAllListeners();
                
                if (!_entry.useFader)
                {
                    _layout.rootObject.SetActive(false);
                }
            }
        }

        private void HandleContinueClick()
        {
            RequestClose();
        }
        
        private void HandleQuitClick()
        {
            _gameLoopService.TrySwitchTo(GameMode.TitleScreen);
            
            RequestClose();
        }
        
        private void HandleRestartClick()
        {
            _quizService.Reset();
            
            RequestClose();
        }
    }

    public class PauseWindowArgs : IWindowArgs
    {
    }
}