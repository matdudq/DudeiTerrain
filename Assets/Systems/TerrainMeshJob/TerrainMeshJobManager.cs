using System;
using System.Collections;
using Procedural.Utilities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Procedural
{
    public partial class TerrainMeshJobManager
    {
        #region Variables

        private readonly MonoBehaviour jobContext = null;

        #endregion Variables

        #region Constructor

        public TerrainMeshJobManager(MonoBehaviour jobContext)
        {
            this.jobContext = jobContext;
        }

        #endregion Constructor

        #region Public methods

        public JobHandle Generate(TerrainMeshJobSettings settings, JobHandle dependency = (default), Action<Mesh> onCompleted = null)
        {
            NativeArray<float> sampledCurve = new NativeArray<float>(TerrainMeshJob.CURVE_SAMPLING_FREQUENCY, Allocator.TempJob);
            settings.heightCurve.GetSampledCurve(ref sampledCurve);
            
            int fullMeshResolution = settings.fullMeshResolution;
            int meshSize = fullMeshResolution * fullMeshResolution;
            int simplifiedMeshResolution = settings.simplifiedMeshResolution;
            
            int trianglesCount = simplifiedMeshResolution * simplifiedMeshResolution * 6;
               
            NativeArray<float3> vertices = new NativeArray<float3>(trianglesCount, Allocator.TempJob);
            NativeArray<float2> uvs = new NativeArray<float2>(trianglesCount, Allocator.TempJob);
            NativeArray<int> triangles = new NativeArray<int>(trianglesCount, Allocator.TempJob);
            
            TerrainMeshJob terrainMeshJob = new TerrainMeshJob()
            {
                fullMeshResolution = fullMeshResolution,
                simplifiedMeshResolution = simplifiedMeshResolution,
                meshOffset = settings.meshOffset,
                heightCurve = sampledCurve,
                heightRange = settings.heightRange,
                noiseMap = settings.noiseMap,
                vertices = vertices,
                uvs = uvs,
                triangles = triangles
            };

            terrainMeshJob.Initialize();
            
            int jobIterations = (simplifiedMeshResolution - 1) * (simplifiedMeshResolution - 1) * 6;

            JobHandle generateMeshJobHandle = terrainMeshJob.Schedule(jobIterations, meshSize / 6, dependency);

            jobContext.StartCoroutine(GenerateMeshJobProcess());
            
            return generateMeshJobHandle;
            
            IEnumerator GenerateMeshJobProcess()
            {
                yield return new WaitWhile(() => !generateMeshJobHandle.IsCompleted);

                generateMeshJobHandle.Complete();
                
                onCompleted?.Invoke(ConstructMesh(vertices, uvs, triangles));

                terrainMeshJob.Dispose();
                
                sampledCurve.Dispose();
                vertices.Dispose();
                uvs.Dispose();
                triangles.Dispose();
            }
        }

        #endregion Public methods

        #region Private methods

        private Mesh ConstructMesh(NativeArray<float3> vertices, NativeArray<float2> uvs, NativeArray<int> triangles)
        {
            Mesh mesh = new Mesh();
            
            mesh.SetVertices(vertices);
            mesh.SetIndices(triangles,MeshTopology.Triangles,0);
            mesh.SetUVs(0,uvs);
            
            mesh.RecalculateNormals();
            return mesh;
        }

        #endregion Private methods
    }
}