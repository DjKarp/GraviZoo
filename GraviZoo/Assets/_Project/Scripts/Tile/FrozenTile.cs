using UnityEngine;
using Zenject;
using DG.Tweening;
using Shapes2D;

namespace GraviZoo
{
    public class FrozenTile : TileWhitEffect
    {
        private bool _isFreezed = true;
        public bool IsFreezed { get => _isFreezed; }

        public override void Init(TileModel tileModel, Shape shape, Sprite animals, SignalBus signalBus)
        {
            base.Init(tileModel, shape, animals, signalBus);
        }

        public override void OnTileClick()
        {
            if (_isFreezed == false)
                base.OnTileClick();
        }

        public void Unfreeze()
        {
            _isFreezed = false;
            TileModel.TileEffect = TileData.TileEffect.None;
            Tween = EffectSpriteRenderer
                .DOFade(0.0f, 1.0f)
                .OnComplete(() => EffectSpriteRenderer.enabled = false);
        }
    }
}
