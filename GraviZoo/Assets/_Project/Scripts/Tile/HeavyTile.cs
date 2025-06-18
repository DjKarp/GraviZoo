using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class HeavyTile : TileWhitEffect
    {
        public override void Init(TileModel tileModel, Sprite shape, Sprite animals, GameObject collider, SignalBus signalBus)
        {
            base.Init(tileModel, shape, animals, collider, signalBus);

            Rigidbody2D.gravityScale *= 5.0f;
        }
    }
}
