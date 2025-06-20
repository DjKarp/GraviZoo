using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class ViewInstaller : MonoInstaller
    {
        [SerializeField] private ReloadTilesButton _reloadTilesButton;
        [SerializeField] private GameView _gameView;

        public override void InstallBindings()
        {
            BindGameplayButtonsInstallers();
            BindGameViewInstallers();
        }

        private void BindGameplayButtonsInstallers()
        {
            Container
                .Bind<ReloadTilesButton>()
                .FromInstance(_reloadTilesButton)
                .AsSingle();
        }

        private void BindGameViewInstallers()
        {
            Container
                .Bind<IGameView>()
                .FromInstance(_gameView)
                .AsSingle();

            Container
                .Bind<IMovedTile>()
                .FromInstance(_gameView)
                .AsSingle();

            Container
                .Bind<ITileFieldService>()
                .FromInstance(_gameView)
                .AsSingle();
        }
    }
}