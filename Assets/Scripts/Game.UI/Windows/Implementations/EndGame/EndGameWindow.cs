using Game.Content.UI;
using Game.Quiz;

namespace Game.UI.Windows
{
    public class EndGameWindow : UIWindow<EndGameWindowLayout, EndGameWindowArgs>
    {
        public override string id => WindowType.END_GAME_WINDOW;
        
        private QuizService _quizService;
        private GameLoopService _gameLoopService;
        
        protected override void OnInitialize()
        {
            ServiceLocator.Get(out _quizService);
            ServiceLocator.Get(out _gameLoopService);
        }
        
        protected override void OnShow(EndGameWindowArgs args)
        {
            base.OnShow(args);
            
            if (_layout != null)
            {
                if (!_entry.useFader)
                {
                    _layout.rootObject.SetActive(true);
                }

                _layout.loseTitle.SetActive(!args.isWin);
                _layout.winTitle.SetActive(args.isWin);

                var data = _quizService.GetData();
                _layout.score.text = data.score.ToString();
                
                _layout.homeButton.onClick.AddListener(HandleQuitClick);
                _layout.restartButton.onClick.AddListener(HandleRestartClick);
            }
        }

        protected override void OnHide()
        {
            base.OnHide();

            if (_layout != null)
            {
                _layout.homeButton.onClick.RemoveAllListeners();
                _layout.restartButton.onClick.RemoveAllListeners();
                
                if (!_entry.useFader)
                {
                    _layout.rootObject.SetActive(false);
                }
            }
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

    public class EndGameWindowArgs : IWindowArgs
    {
        public bool isWin;
    }
}