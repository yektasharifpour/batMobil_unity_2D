using System.Collections;
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
    [SerializeField] private float respawnTriggerDelay = 0.3f;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private bool isHidden = false;
    private float savedY;


    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        if (_cam == null)
            _cam = Camera.main;
    }

    void Update()
    {
        if (isHidden && Input.GetKeyDown(KeyCode.O))
        {
            ShowBatman();
        }
        if (isHidden) return;
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
        if (other.CompareTag("Player"))
        {
            HideBatman();
        }

    }
    void HideBatman()
    {
        isHidden = true;
        savedY = transform.position.y;
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        transform.position = new Vector3(0.8759f, -4.6489f, 0);
    }

    void ShowBatman()
    {
        isHidden = false;

        
        Vector3 p = transform.position;   
        float z_Dist = Mathf.Abs(_cam.transform.position.z);

        Vector3 centerWorld = _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, z_Dist));

        p.x = centerWorld.x;  
                               
        p.z = 0f;

        transform.position = p;

        _spriteRenderer.flipX = false;
        _spriteRenderer.enabled = true;
        StartCoroutine(EnableColliderWithDelay());
    }

    IEnumerator EnableColliderWithDelay()
    {
        yield return new WaitForSeconds(respawnTriggerDelay);
        _collider.enabled = true;
    }
}
