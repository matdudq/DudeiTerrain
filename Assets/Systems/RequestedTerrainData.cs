using UnityEngine;

namespace Procedural
{
	public class RequestedTerrainData
	{
		#region Variables

		private Mesh mesh = null;
		private Texture2D texture = null;

		#endregion Variables

		#region Properties

		public Mesh Mesh
		{
			get
			{
				return mesh;
			}
		}

		public Texture2D Texture
		{
			get
			{
				return texture;
			}
		}

		public bool IsMeshDefined
		{
			get
			{
				return mesh != null;
			}
		}
		
		public bool IsTextureDefined
		{
			get
			{
				return texture != null;
			}
		}
		
		#endregion Propertiess
		
		public void SetMesh(Mesh mesh)
		{
			this.mesh = mesh;
		}

		public void SetTexture(Texture2D texture)
		{
			this.texture = texture;
		}
	}
}