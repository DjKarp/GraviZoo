using Shapes2D;
using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class TileWhitEffect : Tile
    {
        [SerializeField] protected SpriteRenderer EffectSpriteRenderer;

        private int _startSortingOrderEffectSprite;

        public override void Init(TileModel tileModel, Shape shape, Sprite animals, SignalBus signalBus = null)
        {
            base.Init(tileModel, shape, animals, signalBus);

            _startSortingOrderEffectSprite = EffectSpriteRenderer.sortingOrder;
        }

        protected override void SetDefaultState()
        {
            base.SetDefaultState();

            EffectSpriteRenderer.sortingOrder = _startSortingOrderEffectSprite;
        }

        public override void SwitchOffRigidbodyAndCollider()
        {
            base.SwitchOffRigidbodyAndCollider();

            EffectSpriteRenderer.sortingOrder++;
        }
    }
}
