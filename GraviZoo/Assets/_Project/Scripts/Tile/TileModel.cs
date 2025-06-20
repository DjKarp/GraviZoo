using UnityEngine;

namespace GraviZoo
{
    public class TileModel
    {
        public TileData.Shape Shape;
        public Color Color;
        public TileData.AnimalType AnimalType;
        public TileData.TileEffect TileEffect;

        public TileModel(TileData.Shape shape, Color color, TileData.AnimalType animalType)
        {
            Shape = shape;
            Color = color;
            AnimalType = animalType;
            TileEffect = TileData.TileEffect.None;
        }

        public TileModel(TileModel model)
        {
            Shape = model.Shape;
            Color = model.Color;
            AnimalType = model.AnimalType;
            TileEffect = model.TileEffect;
        }

        public string GetKey()
        {
            return $"{Shape}_{Color}_{AnimalType}";
        }
    }
}
