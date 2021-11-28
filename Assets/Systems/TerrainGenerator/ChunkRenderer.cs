using System;
using UnityEngine;

namespace DudeiTerrain
{
	public class ChunkRenderer : MonoBehaviour
	{
		#region Variables

		[SerializeField]
		protected MeshRenderer meshRenderer = null;

		[SerializeField]
		protected MeshFilter meshFilter = null;
		
		[SerializeField]
		protected bool disableOnAwake = false;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			SetVisible(!disableOnAwake);
		}

		#endregion Unity Methods
		
		#region Public Methods

		public virtual void SetVisible(bool visible)
		{
			gameObject.SetActive(visible);
		}

		public virtual void SetTexture(Texture2D newTexture)
		{
			meshRenderer.sharedMaterial.mainTexture = newTexture;
		}

		public virtual void SetMesh(Mesh mesh)
		{
			meshFilter.mesh = mesh;
		}
		
		#endregion Public Methods
	}
}