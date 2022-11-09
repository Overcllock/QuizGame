using Game.Content.UI;

namespace Game.UI.Windows
{
    public class MainMenuWindow : UIWindow<MainMenuWindowLayout, MainMenuWindowArgs>
    {
        public override string id => WindowType.MAIN_MENU;

        private GameLoopService _gameLoopService;

        protected override void OnInitialize()
        {
            ServiceLocator.Get(out _gameLoopService);
        }

        protected override void OnShow(MainMenuWindowArgs args)
        {
            base.OnShow(args);
            
            if (_layout != null)
            {
                _layout.startButton.onClick.AddListener(HandleStartClick);
                
                if (!_entry.useFader)
                {
                    _layout.rootObject.SetActive(true);
                }
            }
        }

        protected override void OnHide()
        {
            base.OnHide();

            if (_layout != null)
            {
                _layout.startButton.onClick.RemoveAllListeners();
                
                if (!_entry.useFader)
                {
                    _layout.rootObject.SetActive(false);
                }
            }
        }

        private void HandleStartClick()
        {
            _gameLoopService.TrySwitchTo(GameMode.InGame);
            RequestClose();
        }
    }

    public class MainMenuWindowArgs : IWindowArgs
    {
    }
}