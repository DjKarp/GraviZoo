using Shapes2D;
using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class HeavyTile : TileWhitEffect
    {
        public override void Init(TileModel tileModel, Shape shape, Sprite animals, SignalBus signalBus)
        {
            base.Init(tileModel, shape, animals, signalBus);

            Rigidbody2D.gravityScale *= 5.0f;
        }
    }
}
