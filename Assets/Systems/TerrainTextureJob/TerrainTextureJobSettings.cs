using Unity.Collections;
using UnityEngine;

namespace Procedural
{
    public class TerrainTextureJobSettings
    {
        #region Variables

        public int simplifiedTerrainResolution;

        public TerrainLayer[] terrainLayers;
        
        public NativeArray<Color> noiseMap;
        
        #endregion Variables
    }
}