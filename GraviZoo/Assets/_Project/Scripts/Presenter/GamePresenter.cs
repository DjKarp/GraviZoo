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

        private ICheckTiles _checkTiles;
        private IWorkedWhitTiles _workedWhitTiles;
        private ICheckFinishGame _checkFinishGame;

        [Inject]
        public void Construct(IGameView view, IMovedTile movedTile, ITileFieldService tileField, ICheckTiles checkTiles, IWorkedWhitTiles workedWhitTiles, ICheckFinishGame checkFinishGame, SignalBus signalBus)
        {
            _view = view;
            _movedTile = movedTile;
            _tileField = tileField;

            _checkTiles = checkTiles;
            _workedWhitTiles = workedWhitTiles;
            _checkFinishGame = checkFinishGame;

            _signalBus = signalBus;
        }

        public void Init()
        {
            _tileField.DropTileOnScene(_workedWhitTiles.CreateTiles());

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
            _tileField.EraseGameField(_workedWhitTiles.GetActiveTiles());

            yield return new WaitForSeconds(1.0f);

            _tileField.DropTileOnScene(_workedWhitTiles.RefreshTiles());
        }

        private void OnTileClick(ClickOnTileSignal clickOnTileSignal)
        {
            if (_tileField.IsTopPanelHaveFreePlace())
            {
                clickOnTileSignal.Tile.SwitchOffRigidbodyAndCollider();
                _movedTile.GoTileOnPanel(clickOnTileSignal.Tile);
                _workedWhitTiles.RemoveTileFromListActiveTiles(clickOnTileSignal.Tile);
            }
            else
            {
                CheckGameOver();
            }
        }

        private void TryMatchTilesOnPanel(TileOnTopPanelSignal tileOnTopPanelSignal)
        {
            var triplesMatch = _checkTiles.CheckMatch();

            if (triplesMatch != null)
            {
                foreach (Tile tile in triplesMatch)
                {
                    _movedTile.GoTileToFinish(tile);
                }                
            }
            else if (_workedWhitTiles.IsAllClickedTileMoveOnPanel())
            {
                CheckGameOver();
            }
        }

        private void CheckGameOver()
        {
            if (!_checkFinishGame.IsWinner() && _checkFinishGame.IsGameOver())
            {
                _view.ShowLooseScreen();
            }
        }

        public void CheckOnWin()
        {
            if (_checkFinishGame.IsWinner())
            {
                _view.ShowVictoryScreen();
            }
        }
    }
}
