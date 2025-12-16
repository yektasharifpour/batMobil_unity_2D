using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject frontLight;
    [SerializeField] private GameObject backLight;
    [SerializeField] private GameObject batSignal;
    [SerializeField] private Sprite normalLight;
    [SerializeField] private Sprite stealthLight;
    [SerializeField] private Sprite alertBlueLight;
    [SerializeField] private Sprite alertRedLight;
    [SerializeField] private Transform cam;
    [SerializeField] private float xOffset = -20f;
    //[SerializeField] bool turnLightOn = true;

    [SerializeField] private float changeColorTime = 1.0f;

    private SpriteRenderer frontRenderer;
    private SpriteRenderer backRenderer;
    private Coroutine alertRoutine;


    enum PlayerState { Normal, Stealth, Alert }
    PlayerState currentState;

    void Start()
    {
        //turnLightOn = true;
        frontRenderer = frontLight.GetComponent<SpriteRenderer>();
        backRenderer = backLight.GetComponent<SpriteRenderer>();
        SetState(PlayerState.Normal);
        batSignal.gameObject.SetActive(false);
        if (cam == null) cam = Camera.main.transform;
        //xOffset = transform.position.x - cam.position.x;


    }



    void LateUpdate()
    {
        if (cam == null) return;

          Vector3 pos = transform.position;
    pos.x = cam.position.x + xOffset;
    transform.position = pos;
    }
    //کنترل حالت ها و بت سیگنال
    void Update()
    {
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
                frontRenderer.sprite = normalLight;
                backRenderer.sprite = normalLight;
                break;

            case PlayerState.Stealth:
                frontRenderer.sprite = stealthLight;
                backRenderer.sprite = stealthLight;
     
                break;

            case PlayerState.Alert:
                if (alertRoutine == null)
                    alertRoutine = StartCoroutine(AlertLightChange());
                break;
        }
    }


    // تابع روشن کردن بت سیگنال
    void turnOnBatSignal()
    {
        if (Input.GetKey(KeyCode.B))
        {   frontLight.gameObject.SetActive(false);
            batSignal.gameObject.SetActive(true);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            frontLight.gameObject.SetActive(true);
            batSignal.gameObject.SetActive(false);
        }
    }
    //void turnOffLight()
    //{
    //    if (!turnLightOn)
    //    {
    //        frontLight.gameObject.SetActive(false);
    //        backLight.gameObject.SetActive(false);

    //    }
    //    else
    //    {
    //        frontLight.gameObject.SetActive(true);
    //        backLight.gameObject.SetActive(true);
    //    }
    //}


    // روتین برای چشمک زن کردن چراغ در حالت هشدار
    IEnumerator AlertLightChange()
    {
        while (true)
        {
            frontRenderer.sprite = alertBlueLight;
            backRenderer.sprite = alertBlueLight;
            yield return new WaitForSeconds(changeColorTime);

            frontRenderer.sprite = alertRedLight;
            backRenderer.sprite = alertRedLight;
            yield return new WaitForSeconds(changeColorTime);
        }
    }

}