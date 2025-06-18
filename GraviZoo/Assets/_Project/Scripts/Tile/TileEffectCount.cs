using System;

namespace GraviZoo
{
    [Serializable]
    public class TileEffectCount
    {
        public TileData.TileEffect TileEffect;
        public int Count;

        public TileEffectCount()
        {
            TileEffect = TileData.TileEffect.None;
            Count = 0;
        }
    }
}
