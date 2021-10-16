using Unity.Collections;
using UnityEngine;

namespace DudeiTerrain
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