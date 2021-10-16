using UnityEngine;

namespace DudeiTerrain.EndlessTerrain
{
    public class ObserverMover : MonoBehaviour
    {
        [SerializeField]
        private float speed = 0;
        [SerializeField]
        private Camera observerCamera = null;

        private const string horizontalAxis = "Horizontal";
        private const string verticalAxis = "Vertical";

        private void Update()
        {
            Vector3 forwardMoveVector = observerCamera.transform.forward * Input.GetAxis(verticalAxis) * speed;
            Vector3 rightMoveVector = observerCamera.transform.right * Input.GetAxis(horizontalAxis) * speed;
            transform.position += (forwardMoveVector + rightMoveVector) * Time.deltaTime;
        }
    }
}

