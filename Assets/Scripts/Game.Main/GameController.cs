using Cysharp.Threading.Tasks;
using Game.Content;
using Game.Content.Main;
using Game.Infrastructure;
using Game.Quiz;
using Game.UI;
using Game.UI.Windows;
using UnityEngine;

namespace Game.Main
{
    public class GameController : MonoBehaviour
    {
        //NOTE: this is so bad, ui containers must be managed not here
        [SerializeField]
        private Transform _uiRoot;

        [SerializeField]
        private MainSettingsScrobject _mainSettings;

        private void Awake()
        {
            var serviceLocatorManager = new SimpleServiceLocatorManager();
            ServiceLocator.Initialize(serviceLocatorManager);

            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            LoadContent();
            await UniTask.WaitWhile(() => !ContentManager.isLoaded);
            
            InitializeServices();
        }

        private void InitializeServices()
        {
            ServiceLocator.Add(this);
            
            ServiceLocator.Add(new UILayoutFactory(_uiRoot));
            ServiceLocator.Add(new UIWindowsDispatcher(new UIWindowsManager()));
            
            ServiceLocator.Add(new QuizService(_mainSettings.id));

            var gameLoop = new GameLoopService();
            ServiceLocator.Add(gameLoop);
            gameLoop.TrySwitchTo(GameMode.TitleScreen);
        }

        private void LoadContent()
        {
            var instantiator = new ContentInstantiator();
            var configurer = new RuntimeContentConfigurer(instantiator);
            var system = new ContentManagementSystem(configurer);
            
            ContentManager.Initialize(system);
        }
    }
}