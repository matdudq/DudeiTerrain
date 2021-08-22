using UnityEngine;

namespace Procedural
{
	public class TerrainPreview : MonoBehaviour
	{
		#if UNITY_EDITOR
		#region Variables

		[SerializeField] 
		private MeshFilter terrainFilter = null;
        
		[SerializeField] 
		private MeshRenderer terrainRenderer = null;

		#endregion Variables

		#region Unity methods

		private void Awake()
		{
			gameObject.SetActive(false);
		}

		#endregion Unity methods
        
		#region Public methods

		public void DisplayMesh(Mesh meshData, Texture2D texture)
		{
			terrainFilter.sharedMesh = meshData;
			terrainRenderer.sharedMaterial.mainTexture = texture;
		}

		#endregion Public methods
		#endif
	}
}