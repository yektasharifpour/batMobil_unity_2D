using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject frontLight;
    [SerializeField] private GameObject backLight;

    [SerializeField] private Sprite normalLight;
    [SerializeField] private Sprite stealthLight;
    [SerializeField] private Sprite alertBlueLight;
    [SerializeField] private Sprite alertRedLight;

    [SerializeField] private float changeColorTime = 1.0f;

    private SpriteRenderer frontRenderer;
    private SpriteRenderer backRenderer;

    private Coroutine alertRoutine;

    enum PlayerState { Normal, Stealth, Alert }
    PlayerState currentState;

    void Start()
    {
        frontRenderer = frontLight.GetComponent<SpriteRenderer>();
        backRenderer = backLight.GetComponent<SpriteRenderer>();
        SetState(PlayerState.Normal);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) SetState(PlayerState.Normal);
        else if (Input.GetKeyDown(KeyCode.S)) SetState(PlayerState.Stealth);
        else if (Input.GetKeyDown(KeyCode.A)) SetState(PlayerState.Alert);
    }

    void SetState(PlayerState newState)
    {
        if (currentState == newState) return;

        // اگر داریم از Alert خارج می‌شیم، کوروتینش رو قطع کن
        if (currentState == PlayerState.Alert && alertRoutine != null)
        {
            StopCoroutine(alertRoutine);
            alertRoutine = null;
        }

        currentState = newState;
        UpdateLight();
    }

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
                // جلوگیری از چندبار شروع شدن کوروتین
                if (alertRoutine == null)
                    alertRoutine = StartCoroutine(AlertLightChange());
                break;
        }
    }
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