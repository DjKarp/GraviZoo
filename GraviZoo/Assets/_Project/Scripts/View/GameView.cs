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
        //[SerializeField] private Cloud _cloud;
        /*[SerializeField] private ScreenWinner _screenWinner;
        [SerializeField] private ScreenLooser _screenLooser;
        [SerializeField] private ScreenMainMenu _screenMainMenu;*/
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
            /*
            _screenWinner.gameObject.SetActive(true);
            _screenLooser.gameObject.SetActive(true);
            _screenMainMenu.gameObject.SetActive(true);
            */
            _signalBus.Subscribe<TileOnTopPanelSignal>(AddedTileOnPanel);
            _signalBus.Subscribe<PlayGameSignals>(HideMainMenu);
        }

        public void GoTileOnPanel(Tile tile)
        {
            Vector3 position = _topPanel.GetNextTilePosition(tile);
            if (position != Vector3.zero)
                tile.MoveToTopPanel(position, _gameplayData.MoveTileTime);
        }

        public void GoTileToFinish(Tile tile)
        {
            tile.MoveToFinish(transform.position, _gameplayData.MoveTileTime / 2.0f);
            _topPanel.RemoveTileFromPanel(tile);
        }

        public void ShowScreenGameOver()
        {
            StartStopGameplay(false);
            //_screenLooser.Show();
        }

        public void ShowScreenWinner()
        {
            StartStopGameplay(false);
            //_screenWinner.Show();
        }

        public void HideMainMenu(PlayGameSignals playGameSignals)
        {
            //_screenMainMenu.Hide();
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
