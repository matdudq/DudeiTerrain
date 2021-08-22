using System;
using UnityEngine;

namespace Procedural
{
    public partial class EndlessTerrain
    {
        private class LODTerrainData
        {
            #region Variables

            public int lod = 0;
            public Vector2 tilePosition = Vector2.zero;
            
            public Mesh mesh = null;
            public Texture2D texture2D = null;
            
            public bool hasRequestedMesh = false;
            public bool hasMesh = false;

            #endregion Variables

            #region Events

            private event Action meshReceivedCallback = null;

            #endregion Events

            #region Constructor

            public LODTerrainData(int lod, Vector2 tilePosition, Action meshReceivedCallback = null)
            {
                this.lod = lod;
                this.meshReceivedCallback = meshReceivedCallback;
                this.tilePosition = tilePosition;
            }

            #endregion Constructor

            #region Public methods

            public void RequestTerrainMesh()
            {
                hasRequestedMesh = true;
                TerrainGenerator.Instance.RequestTerrainByJob(lod, tilePosition, OnTerrainMeshReceived);
            }

            #endregion Public methods

            #region Private methods

            private void OnTerrainMeshReceived(RequestedTerrainData requestedTerrainMeshData)
            {
                hasMesh = true;
                mesh = requestedTerrainMeshData.Mesh;
                texture2D = requestedTerrainMeshData.Texture;
                meshReceivedCallback?.Invoke();
            }

            #endregion Private methods
        }
    }
    

}