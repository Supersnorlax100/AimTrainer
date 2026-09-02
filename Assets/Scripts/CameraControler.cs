using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("Camera Variables")]
    public float mouseSpeed;
    [SerializeField] private float cameraBounds;
    [SerializeField] private Vector3 offset;

    private float xRotation;
    private float yRotation;
    Transform rootTransform;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Mouse X") * mouseSpeed;
        float vertical = Input.GetAxis("Mouse Y") * -mouseSpeed;

        yRotation += horizontal;
        xRotation += vertical;

        rootTransform = transform.root;
        // Camera Constraints Along Y-axis
        xRotation = Mathf.Clamp(xRotation, -cameraBounds, cameraBounds);

        MoveCamera(new Vector3(xRotation, yRotation, 0f));

        rootTransform.position = player.transform.position + offset;

  
    }

    private void MoveCamera(Vector3 movement)
    {
        rootTransform.rotation = Quaternion.Euler(new Vector3(movement.x, movement.y, movement.z));
    }
}
