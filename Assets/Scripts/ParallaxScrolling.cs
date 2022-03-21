using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    [SerializeField] private Transform[] backgrounds;
    [SerializeField] private float[] paralaxOffset;
    [SerializeField] private float smoothing = 10f;

    private Vector3 previousCameraPosition;

    private void Start() => previousCameraPosition = transform.position;

    private void LateUpdate()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            Vector3 parallax = (previousCameraPosition - transform.position) * (paralaxOffset[i] / smoothing);

            var positionX = backgrounds[i].position.x + parallax.x;
            var positionY = backgrounds[i].position.y;
            var positionZ = backgrounds[i].position.z;
            backgrounds[i].position = new Vector3(positionX, positionY, positionZ);
        }

        previousCameraPosition = transform.position;
    }
}