using UnityEngine;

namespace Procedural
{
	public class TerrainChunkRenderer : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		private MeshRenderer meshRenderer = null;

		[SerializeField]
		private MeshFilter meshFilter = null;
		
		private Bounds bounds = new Bounds();
		
		private Vector2 coords = Vector2.zero;

		#endregion Variables

		#region Properties

		public Vector2 Coords
		{
			get
			{
				return coords;
			}
		}

		#endregion Properties

		#region Public methods

		public void SetVisible(bool visible)
		{
			gameObject.SetActive(visible);
		}

		public void Initialize(Vector2 coord, int size, Transform parent)
		{
			this.coords = coord;
			
			Vector2 position = coord * size;
			bounds = new Bounds(position, Vector2.one * size);
			
			transform.position = new Vector3(position.x, 0, position.y);
			transform.parent = parent;
			SetVisible(false);
		}

		public void SetTexture(Texture2D newTexture)
		{
			meshRenderer.sharedMaterial = meshRenderer.material;
			meshRenderer.sharedMaterial.mainTexture = newTexture;
		}

		public void SetMesh(Mesh mesh)
		{
			meshFilter.mesh = mesh;
		}
		
		public float BoundsToPositionDistance(Vector3 position)
		{
			return Mathf.Sqrt(bounds.SqrDistance(position));
		}

		#endregion Public methods
	}
}