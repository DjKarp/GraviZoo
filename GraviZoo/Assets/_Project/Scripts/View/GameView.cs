using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private ActionBarModel _topPanel;
        [SerializeField] private GameObject _winner;
        [SerializeField] private GameObject _looser;
        private ReloadTilesButton _reloadButton;

        private GamePresenter _gamePresenter;
        private GameConfig _gameplayData;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(GamePresenter gamePresenter, GameConfig gameplayData, SignalBus signalBus, ReloadTilesButton reloadTilesButton)
        {
            _gamePresenter = gamePresenter;
            _gameplayData = gameplayData;
            _reloadButton = reloadTilesButton;

            _signalBus = signalBus;
        }

        private void Start()
        {
            _reloadButton.Hide();

            _winner.gameObject.SetActive(false);
            _looser.gameObject.SetActive(false);
            _signalBus.Subscribe<TileOnTopPanelSignal>(AddedTileOnPanel);
        }

        public void GoTileOnPanel(Tile tile)
        {
            Vector3 position = _topPanel.GetNextTilePosition(tile);
            if (position != Vector3.zero)
                tile.MoveToTopPanel(position, _gameplayData.MoveTileTime);
        }

        public void GoTileToFinish(Tile tile)
        {
            tile.MoveToFinish(transform.position + new Vector3(0.0f, 5.0f, 0.0f), _gameplayData.MoveTileTime / 2.0f);
            _topPanel.RemoveTileFromPanel(tile);
        }

        public void ShowScreenGameOver()
        {
            StartStopGameplay(false);
            _looser.SetActive(true);
        }

        public void ShowScreenWinner()
        {
            StartStopGameplay(false);
            _winner.SetActive(true);
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

                yield return new WaitForSeconds(_gameplayData.TimeSpawn);
            }

            StartStopGameplay(true);
        }

        public void ReloadTiles()
        {
            _gamePresenter.ReloadTiles();
        }

        public void EraseGameField(List<Tile> tiles)
        {
            for (int i = tiles.Count - 1; i >= 0; i--)
                tiles[i].DestroyFromGamefield();

            for (int i = _topPanel.TilesContainer.Length - 1; i >= 0; i--)
                if (_topPanel.TilesContainer[i] != null)
                    _topPanel.RemoveTileFromPanel(_topPanel.TilesContainer[i], true);
        }

        public void AddedTileOnPanel(TileOnTopPanelSignal tileOnTopPanelSignal)
        {
            _topPanel.AddedTileOnPanel(tileOnTopPanelSignal.Tile, tileOnTopPanelSignal.NumberPosition);
        }

        public bool IsTopPanelHaveFreePlace()
        {
            return _topPanel.IsHaveFreePlace;
        }

        public void StartStopGameplay(bool isStart)
        {
            if (isStart)
            {
                _topPanel.Show();
                _reloadButton.Show();
            }
            else
            {
                _topPanel.Hide();
                _reloadButton.Hide();
            }

            _signalBus.Fire(new IsGameplayActiveSignal(isStart));
        }
    }
}
