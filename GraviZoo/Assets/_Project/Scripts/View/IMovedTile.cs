using System.Collections.Generic;

namespace GraviZoo
{
    public interface IMovedTile
    {
        public void GoTileOnPanel(Tile tile);        
        public void GoTileToFinish(Tile tile);
    }
}
