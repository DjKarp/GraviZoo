using System.Collections.Generic;
using UnityEngine;

namespace GraviZoo
{
    public class TileData : MonoBehaviour
    {
        public enum Shape
        {            
            Circle,
            Triangle,
            Hexagon,
            Pentagon,
            Rectangle
        }
        
        public enum AnimalType
        {
            Snake,
            Tiger,
            Mouse,
            Elk,
            Dog,
            Bear,
            Cat,
            Bull
        }

        public enum TileEffect
        {
            None,
            Heavy,
            Sticky,
            Exploding,
            Frozen
        }

        public List<Shapes2D.Shape> ShapePrefab = new List<Shapes2D.Shape>();

        public List<Color> Colors = new List<Color>();

        public List<Sprite> AnimalTexture = new List<Sprite>();

        public Tile Tile;
        public List<TilePrefabByEffect> TilePrefabByEffects = new List<TilePrefabByEffect>();
    }
}
