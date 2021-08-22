using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Procedural
{
    public partial class TerrainTextureJobManager
    {
        #region Variables

        private readonly MonoBehaviour jobContext = null;

        #endregion Variables

        #region Constructor

        public TerrainTextureJobManager(MonoBehaviour jobContext)
        {
            this.jobContext = jobContext;
        }

        #endregion Constructor

        #region Public methods

        public JobHandle Generate(TerrainTextureJobSettings settings, JobHandle dependency = (default), Action<Texture2D> onCompleted = null)
        {
            int textureResolution = settings.simplifiedTerrainResolution;

            Texture2D meshTexture = new Texture2D(textureResolution, textureResolution, TextureFormat.RGBA32, false);
            meshTexture.filterMode = FilterMode.Point;
            meshTexture.wrapMode = TextureWrapMode.Clamp;

            NativeArray<TerrainLayer> terrainLayers = new NativeArray<TerrainLayer>(settings.terrainLayers, Allocator.TempJob);
            
            TerrainLayer topTerrainLayer = new TerrainLayer()
            {
                height = 1.0f,
                terrainColor = terrainLayers.Last().terrainColor
            };
            
            NativeArray<Color32> textureArray = meshTexture.GetRawTextureData<Color32>();
            
            int terrainResolution = textureArray.Length;

            TerrainTextureJob terrainTextureJob = new TerrainTextureJob()
            {
                topTerrainLayer = topTerrainLayer,
                noiseMap = settings.noiseMap,
                terrainLayers = terrainLayers,
                textureArray = textureArray
            };
            
            JobHandle generateTextureJobHandle = terrainTextureJob.Schedule(terrainResolution, terrainResolution / 6, dependency);

            jobContext.StartCoroutine(GenerateTextureJobProcess());
            
            return generateTextureJobHandle;
            
            IEnumerator GenerateTextureJobProcess()
            {
                yield return new WaitWhile(() => !generateTextureJobHandle.IsCompleted);

                generateTextureJobHandle.Complete();
                
                meshTexture.Apply();
                
                onCompleted?.Invoke(meshTexture);

                terrainLayers.Dispose();
            }
        }

        #endregion
    }
}