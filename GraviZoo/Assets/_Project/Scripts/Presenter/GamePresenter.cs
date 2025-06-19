using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class GamePresenter : MonoBehaviour
    {
        private SignalBus _signalBus;

        private GameView _gameView;
        private GameModel _gameModel;

        [Inject]
        public void Construct(GameView gameView, GameModel gameModel, SignalBus signalBus)
        {
            _gameView = gameView;
            _gameModel = gameModel;
            _signalBus = signalBus;
        }

        public void Init()
        {
            _gameView.DropTileOnScene(_gameModel.CreateTiles());

            _signalBus.Subscribe<ClickOnTileSignal>(OnTileClick);
            _signalBus.Subscribe<TileOnTopPanelSignal>(TryMatchTilesOnPanel);
        }

        public void ReloadTiles()
        {
            _gameView.StartStopGameplay(false);
            StartCoroutine(WaitBeforeDrop());
        }

        private IEnumerator WaitBeforeDrop()
        {
            _gameView.EraseGameField(_gameModel.ActiveTiles);

            yield return new WaitForSeconds(1.0f);

            _gameView.DropTileOnScene(_gameModel.RefreshTiles());
        }

        private void OnTileClick(ClickOnTileSignal clickOnTileSignal)
        {
            if (_gameView.IsTopPanelHaveFreePlace())
            {
                clickOnTileSignal.Tile.SwitchOffRigidbodyAndCollider();
                _gameView.GoTileOnPanel(clickOnTileSignal.Tile);
                _gameModel.RemoveTileFromListActiveTiles(clickOnTileSignal.Tile);
            }
        }

        private void TryMatchTilesOnPanel(TileOnTopPanelSignal tileOnTopPanelSignal)
        {
            var triplesMatch = _gameModel.CheckMatch();

            if (triplesMatch != null)
            {
                foreach (Tile tile in triplesMatch)
                {
                    _gameView.GoTileToFinish(tile);
                }                
            }
            else if (_gameModel.IsAllClickedTileMoveOnPanel())
            {
                CheckGameOver();
            }
        }

        private void CheckGameOver()
        {
            if (_gameModel.IsWinner() == false && _gameModel.IsGameOver())
            {
                _gameView.ShowScreenGameOver();
            }
        }

        public void CheckOnWin()
        {
            if (_gameModel.IsWinner())
            {
                _gameView.ShowScreenWinner();
            }
        }
    }
}
