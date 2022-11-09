using Game.Quiz;
using Game.UI.Windows;

namespace Game
{
    public class GameLoopStateInGame : GameLoopState
    {
        public override GameMode GetMode() => GameMode.InGame;
        
        private readonly UIWindowsDispatcher _windowsDispatcher;
        private readonly QuizService _quizService;

        public GameLoopStateInGame()
        {
            ServiceLocator.Get(out _windowsDispatcher);
            ServiceLocator.Get(out _quizService);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            _quizService.Reset();

            _quizService.quizEnded += HandleQuizEnded;
            
            _windowsDispatcher.ShowWindow<GameFieldWindow>();
            _windowsDispatcher.ShowWindow<Hud>();
        }

        public override void OnExit()
        {
            base.OnExit();
            
            _quizService.quizEnded -= HandleQuizEnded;
            
            _windowsDispatcher.HideWindow<GameFieldWindow>();
            _windowsDispatcher.HideWindow<Hud>();
            _windowsDispatcher.HideWindow<PauseWindow>();
            _windowsDispatcher.HideWindow<EndGameWindow>();
        }

        private void HandleQuizEnded(bool win)
        {
            var args = new EndGameWindowArgs
            {
                isWin = win
            };
            
            _windowsDispatcher.ShowWindow<EndGameWindow>(args);
        }
    }
}