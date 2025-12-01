using UnityEngine;

public class ParralaxLayerController : MonoBehaviour
{
    [SerializeField] private Camera viewCamera;
    [SerializeField] private float cameraDeltaScalar = 1f;

    private Vector3 cameraStartPos;
    private Vector3 layerStartPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraStartPos = transform.position; 
        layerStartPos = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 cameraDelta = viewCamera.transform.position - cameraStartPos;

        float layerDeltaX = cameraDelta.x * cameraDeltaScalar;
        float layerDeltaY = cameraDelta.y * cameraDeltaScalar;

        Vector3 newPosition = layerStartPos+ new Vector3(layerDeltaX, layerDeltaY);


        transform.position = Vector3.Lerp(transform.position, newPosition, cameraDeltaScalar);
    }
    // Update is called once per frame

}
