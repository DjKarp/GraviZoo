using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class TilePool : MonoPoolableMemoryPool<TileModel, IMemoryPool, Tile> { }
}
