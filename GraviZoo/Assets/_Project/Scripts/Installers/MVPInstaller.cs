using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class MVPInstaller : MonoInstaller
    {
        [SerializeField] private GamePresenter _gamePresenter;
        [SerializeField] private GameModel _gameModel;
        [SerializeField] private TileFactory _tileFactory;

        public override void InstallBindings()
        {
            BindGamePresenter();
            BindGameModel();

            BindTileFactory();
        }

        private void BindGamePresenter()
        {
            Container
                .Bind<GamePresenter>()
                .FromInstance(_gamePresenter)
                .AsSingle();
        }

        private void BindGameModel()
        {
            Container
                .Bind<ICheckTiles>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<IWorkedWhitTiles>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<ICheckFinishGame>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
        
        private void BindTileFactory()
        {
            Container
                .Bind<TileFactory>()
                .FromInstance(_tileFactory)
                .AsSingle();
        }
    }
}