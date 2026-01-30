using UnityEngine;
using UnityEngine.InputSystem;

public class CamMovement : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public float edgePercentage = 0.2f;
    public float minRotation = 0.0f;
    public float maxRotation = 120.0f;

    public float xWobbleAmplitude = 2.0f;
    public float xWobbleFrequency = 2.0f;
    public float xReturnSmoothTime = 0.15f;

    float yRot;
    float xRot;
    float xVel;

    void Awake()
    {
        yRot = transform.eulerAngles.y;
        xRot = 0f;
    }

    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        float screenWidth = Screen.width;
        float edgeSize = screenWidth * edgePercentage;

        bool rotating = false;
        
        if (mousePosition.x <= edgeSize)
        {
            float strength = 1f - (mousePosition.x / edgeSize);
            yRot -= rotationSpeed * strength * Time.deltaTime;
            rotating = true;
        }
        else if (mousePosition.x >= screenWidth - edgeSize)
        {
            float strength = (mousePosition.x - (screenWidth - edgeSize)) / edgeSize;
            yRot += rotationSpeed * strength * Time.deltaTime;
            rotating = true;
        }
        
        yRot = Mathf.Clamp(yRot, minRotation, maxRotation);
        
        if (rotating && yRot > minRotation && yRot < maxRotation)
        {
            xRot = Mathf.Cos(Time.time * xWobbleFrequency) * xWobbleAmplitude;
            xVel = 0f;
        }
        else
        {
            xRot = Mathf.SmoothDampAngle(xRot, 0f, ref xVel, xReturnSmoothTime);
        }

        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
    }
}