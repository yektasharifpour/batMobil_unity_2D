using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class HorizontalClamp : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private float _speed = 5f;

    [Range(0f, 1f)]
    [SerializeField] private float _minViewportX = 0.05f;
    [Range(0f, 1f)]
    [SerializeField] private float _maxViewportX = 0.95f;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_cam == null)
            _cam = Camera.main;
    }

    void Update()
    {
        // حرکت چپ و راست
        float h = Input.GetAxis("Horizontal");
        if (h < 0f)
        {
            _spriteRenderer.flipX = true;
        }
        else if (h > 0f)
        {
            _spriteRenderer.flipX = false;  
        }
        transform.Translate(Vector3.right * h * _speed * Time.deltaTime, Space.World);

        // محدود کردن داخل دید دوربین
        ClampToViewport();
    }

    void ClampToViewport()
    {
        Vector3 viewportPos = _cam.WorldToViewportPoint(transform.position);

        viewportPos.x = Mathf.Clamp(viewportPos.x, _minViewportX, _maxViewportX);

        Vector3 worldPos = _cam.ViewportToWorldPoint(viewportPos);
        transform.position = worldPos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
