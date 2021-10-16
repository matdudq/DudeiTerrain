using UnityEngine;

namespace DudeiTerrain.EndlessTerrain
{
    public class ShowcaseSettings : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

}
