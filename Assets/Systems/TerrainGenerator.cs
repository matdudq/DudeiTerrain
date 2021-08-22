using System;
using DudeiNoise;
using Unity.Collections;
using UnityEngine;
using Utilities;

namespace Procedural
{
    public partial class TerrainGenerator : SingletonMonoBehaviour<TerrainGenerator>
    {
        #region Variables

        [SerializeField]
        private TerrainDefinition definition = null;

        #if UNITY_EDITOR
        [SerializeField, Tooltip("Component which displays preview in edit mode.")]
        private TerrainPreview terrainPreview = null;
        #endif

        private TerrainMeshJobManager terrainMeshJobManager = null;

        private TerrainTextureJobManager terrainTextureJobManager = null;
        
        #endregion Variables
        
        #region Public methods

        public void RequestTerrainByJob(int lod, Vector2 tile, Action<RequestedTerrainData> onRequest = null)
        {
            GenerateHeightMapForOrigin(tile, lod, OnGenerateHeightMapCompleted);
            
            void OnGenerateHeightMapCompleted(NoiseTexture heightMap)
            {
                GenerateTerrainData(lod, heightMap, onRequest);
            }
        }

        #endregion Public methods

        #region Private methods

        private void GenerateTerrainData(int lod, NoiseTexture noiseTexture, Action<RequestedTerrainData> onRequest = null)
        {
            ValidateManagers();
            
            RequestedTerrainData createdTerrainData = new RequestedTerrainData();
            
            int simplifiedTerrainResolution = GetSimplifiedTerrainResolution(TerrainDefinition.MAP_CHUNK_SIZE, lod);
            NativeArray<Color> noiseMap = noiseTexture.Texture.GetRawTextureData<Color>();
            
            //MESH
            TerrainMeshJobSettings terrainMeshJobSettings = new TerrainMeshJobSettings()
            {
                simplifiedMeshResolution = simplifiedTerrainResolution,
                fullMeshResolution = TerrainDefinition.MAP_CHUNK_SIZE,
                heightRange = definition.HeightRange,
                meshOffset = definition.TerrainOffset,
                heightCurve = definition.HeightCurve,
                noiseMap = noiseMap
            };
            
            terrainMeshJobManager.Generate(terrainMeshJobSettings,default, OnGenerateTerrainMeshCompleted);
            
            //TEXTURE
            TerrainTextureJobSettings terrainTextureJobSettings = new TerrainTextureJobSettings()
            {
                simplifiedTerrainResolution = simplifiedTerrainResolution,
                terrainLayers = definition.TerrainLayers,
                noiseMap =  noiseMap
            };
            
            terrainTextureJobManager.Generate(terrainTextureJobSettings,default, OnGenerateTerrainTextureCompleted);
            
            void OnGenerateTerrainMeshCompleted(Mesh mesh)
            {
                createdTerrainData.SetMesh(mesh);
                
                if (createdTerrainData.IsTextureDefined)
                {
                    onRequest?.Invoke(createdTerrainData);
                }
            }
            
            void OnGenerateTerrainTextureCompleted(Texture2D texture)
            {
                createdTerrainData.SetTexture(texture);
                
                if (createdTerrainData.IsMeshDefined)
                {
                    onRequest?.Invoke(createdTerrainData);
                }
            }
        }
        
        private void GenerateHeightMapForOrigin(Vector2 tile, int lod, Action<NoiseTexture> onCompleted)
        {
            NoiseTexture noiseTexture = new NoiseTexture(GetSimplifiedTerrainResolution(TerrainDefinition.MAP_CHUNK_SIZE, lod));
            
            Vector2 noiseSpaceOffset = new Vector2(tile.x * definition.NoiseSettings.scaleOffset.x, -tile.y * definition.NoiseSettings.scaleOffset.y);

            NoiseSettings currentSettings = definition.NoiseSettings.Copy();
            
            currentSettings.positionOffset = noiseSpaceOffset;
            
            noiseTexture.GenerateNoiseForChanelAsync(currentSettings, NoiseTextureChannel.RED, this, onCompleted);
        }

        private int GetSimplifiedTerrainResolution(int fullMeshResolution, int lod)
        {
            int meshSimplificationStep = lod == 0 ? 1 : lod * 2;
            return (fullMeshResolution - 1) / meshSimplificationStep + 1;
        }

        private void ValidateManagers()
        {
            if (terrainMeshJobManager == null)
            {
                terrainMeshJobManager = new TerrainMeshJobManager(this);
            }

            if (terrainTextureJobManager == null)
            {
                terrainTextureJobManager = new TerrainTextureJobManager(this);
            }
        }
        
        #endregion Private methods
        
        #region Editor

        #if UNITY_EDITOR

        private void GenerateAndDisplayTerrain()
        {
            if (definition == null)
            {
                return;
            }
            
            RequestTerrainByJob(definition.LevelOfDetails, definition.NoiseSettings.positionOffset, OnTerrainGenerated);
            
            void OnTerrainGenerated(RequestedTerrainData terrainData)
            {
                terrainPreview.DisplayMesh(terrainData.Mesh, terrainData.Texture);
            }
        }

        #endif

        #endregion Editor
    }
}

