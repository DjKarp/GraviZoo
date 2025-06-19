using System.Collections.Generic;

namespace GraviZoo
{
    public interface ITileFieldService
    {
        public void DropTileOnScene(List<Tile> tiles);
        public void EraseGameField(List<Tile> tiles);
        public bool IsTopPanelHaveFreePlace();
    }
}
