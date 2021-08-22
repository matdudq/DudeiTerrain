using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Procedural
{
    public partial class TerrainMeshJobManager
    {
	    [BurstCompile(DisableSafetyChecks = true)]
		private struct TerrainMeshJob : IJobParallelFor
		{
			#region Variables

			[ReadOnly] 
			private NativeArray<int> vertexOffsetMap;
			
			[ReadOnly]
			public int fullMeshResolution;
			[ReadOnly]
			public int simplifiedMeshResolution;
			[ReadOnly]
			public float3 meshOffset;
			[ReadOnly]
			public float heightRange;
			[ReadOnly]
			public NativeArray<Color> noiseMap;
			[ReadOnly]
			public NativeArray<float> heightCurve;

			[WriteOnly]
			public NativeArray<float3> vertices;
			[WriteOnly]
			public NativeArray<float2> uvs;
			[WriteOnly]
			public NativeArray<int> triangles;

			public const int CURVE_SAMPLING_FREQUENCY = 256;

			private const int VERTEX_PER_QUAD = 6;
			
			#endregion Variables

			#region Public methods

			public void Initialize()
			{
				vertexOffsetMap = new NativeArray<int>(VERTEX_PER_QUAD, Allocator.TempJob)
				{
					[0] = 0,
					[1] = simplifiedMeshResolution + 1,
					[2] = simplifiedMeshResolution,
					[3] = simplifiedMeshResolution + 1,
					[4] = 0,
					[5] = 1
				};
			}

			public void Dispose()
			{
				vertexOffsetMap.Dispose();
			}
			
			public void Execute(int index)
			{
				int vertexInQuad = index % VERTEX_PER_QUAD;

				int quadIndex = (int)math.floor(index / (float)VERTEX_PER_QUAD);
				
				//Calculating current stripe - row of quads.
				//Last row of vertices should be missed, we don't create triangles for them.
				int stripeIndex = (int)math.floor(quadIndex / (float)(simplifiedMeshResolution-1));
				
				//Converting triangle index to vertex index.
				int vertexIndex = quadIndex + stripeIndex + vertexOffsetMap[vertexInQuad];
				
				//Converting current vertex index into x/y indexed-space.
				int x = vertexIndex % simplifiedMeshResolution;
				int y = (int) math.floor(vertexIndex / (float) simplifiedMeshResolution);

				float xRatio = x / (float) simplifiedMeshResolution;
				float yRatio = y / (float) simplifiedMeshResolution;
				
				float topLeftCornerX = (fullMeshResolution - 1) / -2f;
				float topLeftCornerZ = (fullMeshResolution - 1) / 2f;

				float heightRatio = noiseMap[x + simplifiedMeshResolution * y].r;
				float curvedHeightRatio = heightCurve[(int) (heightRatio * CURVE_SAMPLING_FREQUENCY)];
				
				float height = curvedHeightRatio * heightRange;
                
				vertices[index] = new float3(topLeftCornerX + xRatio * fullMeshResolution, height, topLeftCornerZ - yRatio * fullMeshResolution) + meshOffset;
                
				uvs[index] = new float2(xRatio, yRatio);
				
				triangles[index] = index;
			}

			#endregion Public methods
		}
    }
}