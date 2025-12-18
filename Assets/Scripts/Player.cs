using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _frontLight;
    [SerializeField] private GameObject _backLight;
    [SerializeField] private GameObject _batSignal;
    [SerializeField] private Sprite _normalLight;
    [SerializeField] private Sprite _stealthLight;
    [SerializeField] private Sprite _alertBlueLight;
    [SerializeField] private Sprite _alertRedLight;
    [SerializeField] private Transform cam;
    [SerializeField] private float _xOffset = -20f;
    [SerializeField] private bool _isSignalOn = false;
    [SerializeField] private AudioSource _alert;
    [SerializeField] private AudioSource _signalBat;





    [SerializeField] private float _changeColorTime = 1.0f;

    private SpriteRenderer frontRenderer;
    private SpriteRenderer backRenderer;
    private Coroutine alertRoutine;


    enum PlayerState { Normal, Stealth, Alert }
    PlayerState currentState;

    void Start()
    {
        //turnLightOn = true;
        _frontLight.gameObject.SetActive(true);
        _backLight.gameObject.SetActive(true);
        frontRenderer = _frontLight.GetComponent<SpriteRenderer>();
        backRenderer = _backLight.GetComponent<SpriteRenderer>();
        SetState(PlayerState.Normal);
        _batSignal.gameObject.SetActive(false);
        if (cam == null) cam = Camera.main.transform;
        //xOffset = transform.position.x - cam.position.x;


    }



    void LateUpdate()
    {
        if (cam == null) return;

          Vector3 pos = transform.position;
    pos.x = cam.position.x + _xOffset;
    transform.position = pos;
    }
    //کنترل حالت ها و بت سیگنال
    void Update()
    {
        if (_isSignalOn)
        {
            _frontLight.gameObject.SetActive(false);
        }
        else if(!_isSignalOn)
        {
            _frontLight.gameObject.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.N)) SetState(PlayerState.Normal);
        else if (Input.GetKeyDown(KeyCode.C)) SetState(PlayerState.Stealth);
        else if (Input.GetKeyDown(KeyCode.Space)) SetState(PlayerState.Alert);
        turnOnBatSignal();
        //turnOffLight();
     
    }

    // تابع تخصیص حالت
    void SetState(PlayerState newState)
    {
        if (currentState == newState) return;

        
        if (currentState == PlayerState.Alert && alertRoutine != null)
        {
            _alert.Stop();
            StopCoroutine(alertRoutine);
            alertRoutine = null;
        }

        currentState = newState;
        UpdateLight();
    }

    // تابع تغییر رنگ چراغ ها بر اساس حالت
    void UpdateLight()
    {
        switch (currentState)
        {
            case PlayerState.Normal:
                
                frontRenderer.sprite = _normalLight;
                backRenderer.sprite = _normalLight;
                break;

            case PlayerState.Stealth:
             frontRenderer.sprite = _stealthLight;
                backRenderer.sprite = _stealthLight;
     
                break;

            case PlayerState.Alert:

                if (alertRoutine == null)
                    _alert.Play();
                alertRoutine = StartCoroutine(AlertLightChange());
                break;
        }
    }


    // تابع روشن کردن بت سیگنال
    void turnOnBatSignal()
    {
        if (Input.GetKey(KeyCode.B))
        {
            _signalBat.Play();
            _isSignalOn = true;
            _frontLight.gameObject.SetActive(false);
            _batSignal.gameObject.SetActive(true);
        }
        else if(Input.GetKey(KeyCode.E))
        {
            _signalBat.Stop();
            _isSignalOn = false;
            _frontLight.gameObject.SetActive(true);
            _batSignal.gameObject.SetActive(false);
        }
    }



    // روتین برای چشمک زن کردن چراغ در حالت هشدار
    IEnumerator AlertLightChange()
    {

        while (true)
        {
            frontRenderer.sprite = _alertBlueLight;
            backRenderer.sprite = _alertBlueLight;
            yield return new WaitForSeconds(_changeColorTime);

            frontRenderer.sprite = _alertRedLight;
            backRenderer.sprite = _alertRedLight;
            yield return new WaitForSeconds(_changeColorTime);
        }
    }

}