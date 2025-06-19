using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class GameModel : MonoBehaviour
    {
        private GamePresenter _gamePresenter;
        private GameConfig _gameConfig;
        private ActionBarModel _actionBar;
        private SignalBus _signalBus;

        private TileFactory _tileFactory;

        private List<Tile> _activeTiles = new List<Tile>();
        public List<Tile> ActiveTiles { get => _activeTiles; set => _activeTiles = value; }
        private Dictionary<string, List<Tile>> _topPanelsGroup = new Dictionary<string, List<Tile>>();

        [Inject]
        public void Construct(GamePresenter gamePresenter, GameConfig gameConfig, ActionBarModel actionBar, SignalBus signalBus, TileFactory tileFactory)
        {
            _gamePresenter = gamePresenter;
            _gameConfig = gameConfig;
            _actionBar = actionBar;
            _signalBus = signalBus;
            _tileFactory = tileFactory;
        }

        private void Start()
        {
            _signalBus.Subscribe<TileOnFinishSignal>(CheckConditions);

            _tileFactory.Init(_activeTiles);
        }

        public bool IsGameOver()
        {
            return _actionBar.TilesContainer.All(x => x != null) || (_actionBar.TilesContainer.Where(x => x != null).Any() && _activeTiles.Count == 0);
        }

        public bool IsWinner()
        {
            return _actionBar.TilesContainer.All(x => x == null) && _activeTiles.Count == 0;
        }

        public void RemoveTileFromListActiveTiles(Tile tile)
        {
            _activeTiles.Remove(tile);
        }

        public int GetTilesCount()
        {
            return _activeTiles.Count + _actionBar.PlaceUseCount;
        }

        public bool IsAllClickedTileMoveOnPanel()
        {
            return _actionBar.IsAllClickedTileMoveOnPanel();
        }

        public List<Tile> CheckMatch()
        {
            _topPanelsGroup.Clear();

            foreach (Tile tile in _actionBar.TilesContainer)
            {
                if (tile != null)
                {
                    string key = tile.TileModel.GetKey();

                    if (!_topPanelsGroup.ContainsKey(key))
                        _topPanelsGroup.Add(key, new List<Tile>());

                    _topPanelsGroup[key].Add(tile);

                    if (_topPanelsGroup[key].Count == _gameConfig.MatchCountTiles)
                    {
                        _activeTiles.RemoveAll(tile => _topPanelsGroup[key].Contains(tile));
                        CheckExplodindTile(_topPanelsGroup[key]);

                        return _topPanelsGroup[key];
                    }
                }
            }

            return null;
        }

        public void CheckExplodindTile(List<Tile> tiles)
        {
            foreach (Tile tile in tiles.Where(t => t is ExplodingTile).ToList())
            {
                int indexOnTopPanel = Array.IndexOf(_actionBar.TilesContainer, tile);

                if (indexOnTopPanel > 0)
                {
                    Tile checkTileLeft = _actionBar.TilesContainer[indexOnTopPanel - 1];

                    if (checkTileLeft != null && tiles.Contains(checkTileLeft) == false)
                    {
                        _actionBar.RemoveTileFromPanel(checkTileLeft, true);
                    }
                }

                if (indexOnTopPanel < _actionBar.TilesContainer.Length)
                {
                    Tile checkTileRight = _actionBar.TilesContainer[indexOnTopPanel + 1];

                    if (checkTileRight != null && tiles.Contains(checkTileRight) == false)
                    {
                        _actionBar.RemoveTileFromPanel(checkTileRight, true);
                    }
                }
            }
        }

        private void CheckConditions(TileOnFinishSignal tileOnFinishSignal)
        {
            _gamePresenter.CheckOnWin();
            CheckFrozedTiles();
        }

        private void CheckFrozedTiles()
        {
            if (_gameConfig.MaxCountTiles - GetTilesCount() > _gameConfig.NumberTilesToUnfreeze && _activeTiles.OfType<FrozenTile>().Where(t => t.IsFreezed).Any())
                foreach (FrozenTile tile in _activeTiles.OfType<FrozenTile>().Where(t => t.IsFreezed).ToList())
                    tile.Unfreeze();
        }

        public List<Tile> CreateTiles()
        {
            var random = new System.Random();

            _activeTiles.Clear();
            _activeTiles = _tileFactory.Create().OrderBy(_ => random.Next()).ToList();    // Перемешаем второй раз

            return _activeTiles;
        }

        public List<Tile> RefreshTiles()
        {
            _tileFactory.Init(_activeTiles);

            return CreateTiles();
        }
    }
}
