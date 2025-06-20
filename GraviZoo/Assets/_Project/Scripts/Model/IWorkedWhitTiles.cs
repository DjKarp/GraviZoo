using System.Collections.Generic;

namespace GraviZoo
{
    public interface IWorkedWhitTiles
    {
        public List<Tile> GetActiveTiles();

        public List<Tile> CreateTiles();
        public List<Tile> RefreshTiles();

        public void RemoveTileFromListActiveTiles(Tile tile);
        public int GetTilesCount();
        public bool IsAllClickedTileMoveOnPanel();

    }
}
