using UnityEngine;

public class CameraControler : MonoBehaviour
{
    private GameObject player;

    [Header("Camera Variables")]

    [SerializeField] private float cameraBounds;
    [SerializeField] private Vector3 offset;

    float xRotation;
    float yRotation = 180;
    Transform rootTransform;

    private void Start()
    {
        player = PlayerControler.instance.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // if (UiManager.instance && UiManager.instance.activeMenu)
        // {
        //     Debug.Log("menu exists");
        // }
        // if (GameManager.instance.isPaused)
        // {
        //     Debug.Log("is paused");
        // }
        // if (GameManager.instance.isPaused && UiManager.instance && ! UiManager.instance.activeMenu)
        // {
        //     Debug.Log("is paused and no menu");
        // }
        if (GameManager.instance.isPaused && (! UiManager.instance || (UiManager.instance && UiManager.instance.activeMenu)))
        {
            Debug.Log("return");
            return;
        }
        float horizontal = Input.GetAxis("Mouse X") * PlayerControler.instance.mouseSens;
        float vertical = Input.GetAxis("Mouse Y") * -PlayerControler.instance.mouseSens;

        yRotation += horizontal;
        xRotation += vertical;

        rootTransform = transform.root;
        // Camera Constraints Along Y-axis
        xRotation = Mathf.Clamp(xRotation, -cameraBounds, cameraBounds);

        if (player == null)
        {
            return;
        }
        else
        {
            MoveCamera(new Vector3(xRotation, yRotation, 0f));

            rootTransform.position = player.transform.position + offset;
        }


    }

    private void MoveCamera(Vector3 movement)
    {
        rootTransform.rotation = Quaternion.Euler(new Vector3(movement.x, movement.y, movement.z));
    }
}
