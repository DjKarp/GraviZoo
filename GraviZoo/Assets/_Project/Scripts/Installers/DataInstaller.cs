using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class DataInstaller : MonoInstaller
    {
        [SerializeField] private TileData _tileData;
        [SerializeField] private GameConfig _gameConfig;

        public override void InstallBindings()
        {
            BindGameplayData();
            BindTileData();
        }

        private void BindGameplayData()
        {
            Container
                .Bind<GameConfig>()
                .FromInstance(_gameConfig)
                .AsCached();
        }

        private void BindTileData()
        {
            Container
                .Bind<TileData>()
                .FromInstance(_tileData)
                .AsCached();
        }
    }
}