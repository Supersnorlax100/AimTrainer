using UnityEngine;

public class CameraControler : MonoBehaviour
{
    private GameObject player;

    [Header("Camera Variables")]

    [SerializeField] private float cameraBounds;
    [SerializeField] private Vector3 offset;

    float xRotation;
    float yRotation = 180;

    private void Start()
    {
        player = PlayerControler.instance.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        transform.position = player.transform.position + offset;
    }

    private void Update()
    {
        if (GameManager.instance.forceLock)
        {
            return;
        }
        if (GameManager.instance.isPaused && (! UiManager.instance || (UiManager.instance && UiManager.instance.activeMenu)))
        {
            return;
        }
        float horizontal = Input.GetAxis("Mouse X") * PlayerControler.instance.mouseSens;
        float vertical = Input.GetAxis("Mouse Y") * -PlayerControler.instance.mouseSens;

        yRotation += horizontal;
        xRotation += vertical;

        // Camera Constraints Along Y-axis
        xRotation = Mathf.Clamp(xRotation, -cameraBounds, cameraBounds);

        if (player == null)
        {
            return;
        }
        else
        {
            MoveCamera(new Vector3(xRotation, yRotation, 0f));

            transform.root.position = player.transform.position + offset;
        }


    }

    private void MoveCamera(Vector3 movement)
    {
        transform.root.rotation = Quaternion.Euler(new Vector3(movement.x, movement.y, movement.z));
    }
}
