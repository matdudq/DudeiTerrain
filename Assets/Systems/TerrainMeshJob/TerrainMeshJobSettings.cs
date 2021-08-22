using Unity.Collections;
using UnityEngine;

namespace Procedural
{
    public struct TerrainMeshJobSettings
    {
        #region Variables

        public int simplifiedMeshResolution;
        public int fullMeshResolution;
        public float heightRange;
        public Vector3 meshOffset;
        public AnimationCurve heightCurve;
        
        public NativeArray<Color> noiseMap;

        #endregion Variables
    }
}