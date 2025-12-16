using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class HorizontalClamp : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float speed = 5f;

    [Range(0f, 1f)]
    [SerializeField] private float minViewportX = 0.05f;
    [Range(0f, 1f)]
    [SerializeField] private float maxViewportX = 0.95f;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        // حرکت چپ و راست
        float h = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.World);

        // محدود کردن داخل دید دوربین
        ClampToViewport();
    }

    void ClampToViewport()
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);

        viewportPos.x = Mathf.Clamp(viewportPos.x, minViewportX, maxViewportX);

        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);
        transform.position = worldPos;
    }
}
