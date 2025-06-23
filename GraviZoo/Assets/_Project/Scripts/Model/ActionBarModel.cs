using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GraviZoo
{
    public class ActionBarModel : MonoBehaviour
    {
        [SerializeField] private Transform[] _tilesPositionContainer = new Transform[7];
        private Tile[] _tilesContainer;
        public Tile[] TilesContainer { get => _tilesContainer; }

        private bool[] _placesUse;

        public int PlaceUseCount { get => _placesUse.Count(x => x == true); }
        public bool IsHaveFreePlace { get => _placesUse.Count(x => x == true) < _tilesContainer.Length; }

        private Transform _transform;
        private float _animationTime = 0.5f;
        private Tween _tween;
        private Vector3 _startPosition;
        private Vector3 _hidePosition;

        private void Awake()
        {
            _transform = gameObject.transform;
            _startPosition = _transform.position;

            Init();
        }

        protected void Init()
        {
            _tilesContainer = new Tile[7];
            _placesUse = new bool[7];

            _hidePosition = _startPosition + new Vector3(20, 0, 0);
            _transform.position = _hidePosition;
        }

        public void Hide()
        {
            _tween = _transform
                .DOMoveX(_hidePosition.x, _animationTime)
                .From(_startPosition)
                .SetEase(Ease.InBounce);
        }

        public void Show(Action callback = null)
        {
            _tween = _transform
                .DOMoveX(_startPosition.x, _animationTime)
                .From(_hidePosition)
                .SetEase(Ease.OutBounce);
        }

        public Vector3 GetNextTilePosition(Tile tile)
        {
            for (int i = 0; i < _tilesContainer.Length; i++)
            {
                if (_placesUse[i] == false)
                {
                    tile.TopPanelSlotIndex = i;
                    _placesUse[i] = true;
                    return _tilesPositionContainer[i].position;
                }
            }

            return Vector3.zero;
        }

        public void RemoveTileFromPanel(Tile tile, bool isDestroy = false)
        {
            for (int i = 0; i < _tilesContainer.Length; i++)
                if (_tilesContainer[i] == tile)
                {
                    if (isDestroy)
                        _tilesContainer[i].DestroyFromGamefield();
                    else
                        _tilesContainer[i] = null;

                    _placesUse[i] = false;
                }
        }

        public void AddedTileOnPanel(Tile tile, int number)
        {
            _tilesContainer[number] = tile;
        }

        public bool IsAllClickedTileMoveOnPanel()
        {
            return _tilesContainer.Count(x => x != null) == _placesUse.Count(x => x == true);
        }
    }
}
