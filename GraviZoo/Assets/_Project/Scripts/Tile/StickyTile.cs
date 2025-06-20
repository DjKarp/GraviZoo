using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
using Shapes2D;

namespace GraviZoo
{
    public class StickyTile : TileWhitEffect
    {
        private List<Tile> _stickTiles = new List<Tile>();
        private int _maxStickTiles = 2;

        public override void Init(TileModel tileModel, Shape shape, Sprite animals, SignalBus signalBus)
        {
            base.Init(tileModel, shape, animals, signalBus);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_stickTiles.Count < _maxStickTiles)
            {
                Tile collisionTile = collision.gameObject.GetComponent<Tile>();

                if (collisionTile != null)
                    DetectCollision(collisionTile);
            }
        }

        public void DetectCollision(Tile collisionTile)
        {
            if (_stickTiles.Contains(collisionTile) == false)
            {
                _stickTiles.Add(collisionTile);
                collisionTile.AttachTile(Rigidbody2D);
            }
        }

        public override void OnTileClick()
        {
            base.OnTileClick();

            Tween = EffectSpriteRenderer
                .DOFade(0.0f, 1.0f)
                .OnComplete(() => EffectSpriteRenderer.enabled = false);

            foreach (Tile tile in _stickTiles)
                tile.DeattachTile();
        }
    }
}
