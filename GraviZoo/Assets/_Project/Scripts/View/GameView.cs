using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private ActionBarModel _actionBar;
        private ReloadTilesButton _reloadButton;

        private GamePresenter _gamePresenter;
        private GameConfig _gameConfig;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(GamePresenter gamePresenter, GameConfig gameConfig, SignalBus signalBus, ReloadTilesButton reloadTilesButton)
        {
            _gamePresenter = gamePresenter;
            _gameConfig = gameConfig;
            _reloadButton = reloadTilesButton;

            _signalBus = signalBus;
        }

        private void Start()
        {
            _reloadButton.Hide();

            _signalBus.Subscribe<TileOnTopPanelSignal>(AddedTileOnPanel);
        }

        public void GoTileOnPanel(Tile tile)
        {
            Vector3 position = _actionBar.GetNextTilePosition(tile);
            if (position != Vector3.zero)
                tile.MoveToTopPanel(position, _gameConfig.MoveTileTime);
        }

        public void GoTileToFinish(Tile tile)
        {
            tile.MoveToFinish(transform.position, _gameConfig.MoveTileTime / 2.0f);
            _actionBar.RemoveTileFromPanel(tile);
        }

        public void ShowLooseScreen()
        {
            StartStopGameplay(false);
            //_screenLooser.Show();
        }

        public void ShowVictoryScreen()
        {
            StartStopGameplay(false);
            //_screenWinner.Show();
        }

        public void DropTileOnScene(List<Tile> tiles)
        {
            StartCoroutine(DropTileWhitDelay(tiles));
        }

        private IEnumerator DropTileWhitDelay(List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                tile.Transform.position = transform.position;
                tile.gameObject.SetActive(true);

                yield return new WaitForSeconds(_gameConfig.TimeSpawn);
            }

            StartStopGameplay(true);
        }

        public void ReloadTiles()
        {
            _gamePresenter.OnRestartClicked();
        }

        public void EraseGameField(List<Tile> tiles)
        {
            for (int i = tiles.Count - 1; i >= 0; i--)
                tiles[i].OnDespawned();

            for (int i = _actionBar.TilesContainer.Length - 1; i >= 0; i--)
                if (_actionBar.TilesContainer[i] != null)
                    _actionBar.RemoveTileFromPanel(_actionBar.TilesContainer[i], true);
        }

        public void AddedTileOnPanel(TileOnTopPanelSignal tileOnTopPanelSignal)
        {
            _actionBar.AddedTileOnPanel(tileOnTopPanelSignal.Tile, tileOnTopPanelSignal.NumberPosition);
        }

        public bool IsTopPanelHaveFreePlace()
        {
            return _actionBar.IsHaveFreePlace;
        }

        public void StartStopGameplay(bool isStart)
        {
            if (isStart)
            {
                _actionBar.Show();
                _reloadButton.Show();
            }
            else
            {
                _actionBar.Hide();
                _reloadButton.Hide();
            }

            _signalBus.Fire(new IsGameplayActiveSignal(isStart));
        }
    }
}
