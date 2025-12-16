using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float minSpeed;
    [SerializeField] bool isFast = false;
    void Start()
    {
        minSpeed = forwardSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float moveForward = Time.deltaTime * forwardSpeed;
        transform.Translate(moveForward,0,0);
        if ((Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift)) && !isFast )
        {
            isFast = true;
            forwardSpeed = maxSpeed;
        }
        else
        {
            isFast= false;
            forwardSpeed = minSpeed;
        }
        
    }
}
