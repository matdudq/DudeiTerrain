using UnityEngine;

namespace Procedural
{
	public partial class EndlessTerrain
	{
		private class TerrainChunk
		{
			#region Variables

			private EndlessTerrain chunkOwner = null;

			private TerrainChunkRenderer chunkRenderer = null;
			
			private LODTerrainData[] lodMeshes;

			private int previousLOD = -1;

			#endregion Variables

			#region Properties
			
			private float MaxViewDistance
			{
				get
				{
					return chunkOwner.MaxViewDistance;
				}
			}

			private Vector2 ObserverPositionXZ
			{
				get
				{
					return chunkOwner.ObserverPositionXZ;
				}
			}

			private LODInfo[] LODs
			{
				get
				{
					return chunkOwner.lods;
				}
			}

			#endregion Properties

			#region Constructor

			public TerrainChunk(TerrainChunkRenderer chunkRenderer, EndlessTerrain chunkOwner)
			{
				this.chunkRenderer = chunkRenderer;

				this.chunkOwner = chunkOwner;
				
				lodMeshes = new LODTerrainData[LODs.Length];

				for (int i = 0; i < lodMeshes.Length; i++)
				{
					lodMeshes[i] = new LODTerrainData(LODs[i].lodLevel, chunkRenderer.Coords, UpdateVisibility);
				}
				
				UpdateVisibility();
			}
			
			#endregion Constructor

			#region Public methods

			public void UpdateVisibility()
			{
				float boundsToObserver = chunkRenderer.BoundsToPositionDistance(ObserverPositionXZ);
				bool isVisible = boundsToObserver <= MaxViewDistance;

				if (isVisible)
				{
					int lodIndex = 0;
					for (int i = 0; i < LODs.Length - 1; i++)
					{
						if (boundsToObserver > LODs[i].distanceThreshold)
						{
							lodIndex++;
						}
						else
						{
							break;
						}
					}

					if (previousLOD != lodIndex)
					{
						LODTerrainData lodTerrainData = lodMeshes[lodIndex];
						if (lodTerrainData.hasMesh)
						{
							chunkRenderer.SetMesh(lodTerrainData.mesh);
							chunkRenderer.SetTexture(lodTerrainData.texture2D);
						}
						else if (!lodTerrainData.hasRequestedMesh)
						{
							lodTerrainData.RequestTerrainMesh();
						}
					}
					
					chunkOwner.lastUpdateVisibleTerrainChunks.Add(this);
				}

				chunkRenderer.SetVisible(isVisible);
			}

			public void ForcedSetEnabled(bool enabled)
			{
				chunkRenderer.SetVisible(enabled);
			}

			#endregion Public methods
		}
	}
}