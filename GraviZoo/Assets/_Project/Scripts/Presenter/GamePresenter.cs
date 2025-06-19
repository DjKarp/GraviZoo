using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class GamePresenter : MonoBehaviour
    {
        private SignalBus _signalBus;

        private IGameView _view;
        private IMovedTile _movedTile;
        private ITileFieldService _tileField;

        private GameModel _gameModel;

        [Inject]
        public void Construct(IGameView view, IMovedTile movedTile, ITileFieldService tileField, GameModel gameModel, SignalBus signalBus)
        {
            _view = view;
            _movedTile = movedTile;
            _tileField = tileField;
            _gameModel = gameModel;
            _signalBus = signalBus;
        }

        public void Init()
        {
            _tileField.DropTileOnScene(_gameModel.CreateTiles());

            _signalBus.Subscribe<ClickOnTileSignal>(OnTileClick);
            _signalBus.Subscribe<TileOnTopPanelSignal>(TryMatchTilesOnPanel);
        }

        public void ReloadTiles()
        {
            _signalBus.Fire(new IsGameplayActiveSignal(false));
            StartCoroutine(WaitBeforeDrop());
        }

        private IEnumerator WaitBeforeDrop()
        {
            _tileField.EraseGameField(_gameModel.ActiveTiles);

            yield return new WaitForSeconds(1.0f);

            _tileField.DropTileOnScene(_gameModel.RefreshTiles());
        }

        private void OnTileClick(ClickOnTileSignal clickOnTileSignal)
        {
            if (_tileField.IsTopPanelHaveFreePlace())
            {
                clickOnTileSignal.Tile.SwitchOffRigidbodyAndCollider();
                _movedTile.GoTileOnPanel(clickOnTileSignal.Tile);
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
                    _movedTile.GoTileToFinish(tile);
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
                _view.ShowLooseScreen();
            }
        }

        public void CheckOnWin()
        {
            if (_gameModel.IsWinner())
            {
                _view.ShowVictoryScreen();
            }
        }
    }
}
