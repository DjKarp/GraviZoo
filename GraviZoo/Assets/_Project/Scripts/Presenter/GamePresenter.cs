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

        private TileFactory _tileFactory;
        private GameConfig _config;

        [Inject]
        public void Construct(GameView gameView, GameModel gameModel, SignalBus signalBus, TileFactory tileFactory, GameConfig config)
        {
            _gameView = gameView;
            _gameModel = gameModel;
            _signalBus = signalBus;
            _tileFactory = tileFactory;
            _config = config;
        }

        public void Init()
        {
            _signalBus.Subscribe<ClickOnTileSignal>(OnTileClicked);
            _signalBus.Subscribe<TileOnTopPanelSignal>(TryMatchTilesOnPanel);

            for (int i = 0; i < _config.MaxCountTiles; i++)
            {
                var model = CreateRandomTileModel();
                var tile = _tileFactory.Create(model);
                tile.transform.position = _gameView.transform.position;
            }
        }

        public void OnTileClicked(ClickOnTileSignal clickOnTileSignal)
        {
            if (_gameView.IsTopPanelHaveFreePlace())
            {
                clickOnTileSignal.Tile.SwitchOffRigidbodyAndCollider();
                _gameView.GoTileOnPanel(clickOnTileSignal.Tile);
                _gameModel.RemoveTileFromListActiveTiles(clickOnTileSignal.Tile);
            }
        }

        public void OnRestartClicked()
        {
            _gameView.StartStopGameplay(false);
            StartCoroutine(WaitBeforeDrop());
        }

        private IEnumerator WaitBeforeDrop()
        {
            while (!_gameModel.IsAllClickedTileMoveOnPanel())
            {
                yield return new WaitForEndOfFrame();
            }

            _gameView.EraseGameField(_gameModel.ActiveTiles);

            yield return new WaitForSeconds(1.0f);
            
            _gameView.DropTileOnScene(_gameModel.RefreshTiles());
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
                _gameView.ShowLooseScreen();
            }
        }

        public void CheckOnWin()
        {
            if (_gameModel.IsWinner())
            {
                _gameView.ShowVictoryScreen();
            }
        }

        private TileModel CreateRandomTileModel()
        {

        }
    }
}
