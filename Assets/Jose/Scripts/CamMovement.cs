using UnityEngine;
using UnityEngine.InputSystem;

public class CamMovement : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public float edgePercentage = 0.2f;

    public float maxAngleOffset = 60f; // ±60 grados desde el inicio

    public float xWobbleAmplitude = 2.0f;
    public float xWobbleFrequency = 2.0f;
    public float xReturnSmoothTime = 0.15f;

    public float wobbleInSpeed = 6f;   // qué rápido entra el wobble
    public float wobbleOutSpeed = 8f;  // qué rápido sale

    float yRot;
    float xRot;
    float xVel;

    float minY;
    float maxY;

    float wobbleWeight = 0f;

    void Awake()
    {
        yRot = transform.eulerAngles.y;
        xRot = 0f;

        minY = yRot - maxAngleOffset;
        maxY = yRot + maxAngleOffset;
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

        yRot = Mathf.Clamp(yRot, minY, maxY);

        // ---------- WOBBLE SUAVE ----------
        if (rotating && yRot > minY && yRot < maxY)
        {
            wobbleWeight = Mathf.MoveTowards(
                wobbleWeight,
                1f,
                wobbleInSpeed * Time.deltaTime
            );
        }
        else
        {
            wobbleWeight = Mathf.MoveTowards(
                wobbleWeight,
                0f,
                wobbleOutSpeed * Time.deltaTime
            );
        }

        float targetX =
            Mathf.Cos(Time.time * xWobbleFrequency) *
            xWobbleAmplitude *
            wobbleWeight;

        xRot = Mathf.SmoothDampAngle(
            xRot,
            targetX,
            ref xVel,
            xReturnSmoothTime
        );

        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
    }
}
