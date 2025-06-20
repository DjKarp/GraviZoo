using System.Collections.Generic;

namespace GraviZoo
{
    public interface ICheckTiles
    {
        public List<Tile> CheckMatch();
        public void CheckExplodindTile(List<Tile> tiles);
    }
}
