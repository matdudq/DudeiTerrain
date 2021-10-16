using System;
using System.Collections.Generic;
using DudeiNoise;
using UnityEngine;
using Utilities;

namespace DudeiTerrain.BlendingIsland
{
	public class BlendingIsland : SingletonMonoBehaviour<BlendingIsland>
	{
		[SerializeField]
		private TerrainDefinition definition = null;

		[SerializeField]
		private ChunkRenderer islandRenderer = null;
		
		private NoiseTexture currentTexture = null;

		private NoiseTexture nextTexture = null;
		
		private float currentTime = 0;

		private int preparedFrames = 0;
		
		[SerializeField]
		private float animationTime = 15.0f;

		[SerializeField]
		private int frameDuration = 1;

		private int CACHED_FRAMES_COUNT = 20;
		
		protected override void Awake()
		{
			base.Awake();
			
			currentTexture = new NoiseTexture(TerrainDefinition.MAP_CHUNK_SIZE);
			nextTexture = new NoiseTexture(TerrainDefinition.MAP_CHUNK_SIZE);
			
			GenerateRandomHeightMap(currentTexture);
			GenerateRandomHeightMap(nextTexture);
		}
		
		
		
		private void GenerateAnimation()
		{
			int animationFramesCount = Mathf.CeilToInt(animationTime * frameDuration);
			int currentFrames = 0;

			void OnFrameGenerated()
			{
				currentFrames = 0;
			}
		}

		private void GenerateRandomHeightMap(NoiseTexture noiseTexture, Action<NoiseTexture> onCompleted = null)
		{
			NoiseSettings currentSettings = definition.NoiseSettings.Copy();
            
			currentSettings.positionOffset = new Vector2(UnityEngine.Random.value * float.MaxValue , UnityEngine.Random.value * float.MaxValue);

			#if UNITY_EDITOR
			if (Application.isPlaying)
			{
				noiseTexture.GenerateNoiseForChanel(currentSettings, NoiseTextureChannel.RED, this, onCompleted);
			}
			else
			{
				noiseTexture.GenerateNoiseForChanelInEditor(currentSettings, NoiseTextureChannel.RED, this, onCompleted);
			}
			#else         
             noiseTexture.GenerateNoiseForChanel(currentSettings, NoiseTextureChannel.RED, this, onCompleted);
			#endif
		}
	}
}
