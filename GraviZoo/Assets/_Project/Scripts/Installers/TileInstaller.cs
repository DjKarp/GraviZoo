using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class TileInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _config;

        public override void InstallBindings()
        {
            // Регистрируем фабрику через PoolableMemoryPool
            Container
                .BindMemoryPool<Tile, TilePool>()
                .WithInitialSize(_config.MaxCountTiles)
                .FromComponentInNewPrefab(_config.TilePrefab)
                .UnderTransformGroup("TilesPool");

            Container
                .BindFactory<TileModel, Tile, TileFactory>()
                .FromPoolableMemoryPool<TileModel, Tile, TilePool>(poolBinder => poolBinder
                    .WithInitialSize(_config.MaxCountTiles)
                    .FromComponentInNewPrefab(_config.TilePrefab)
                    .UnderTransformGroup("TilesPool"));
        }
    }
}
