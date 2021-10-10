using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 1f;
    public Transform playerBody;
    float xRotation = 0;
    public static MouseLook instance;

    private bool cameraLock;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
            print("Multiple mouse look instances occured.");
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (!cameraLock)
        {
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.transform.Rotate(Vector3.up * mouseX);
        }
    }

    public void LockCamera(bool toState) => cameraLock = toState;

    public void SetXRotation(float value) => xRotation = value;
}