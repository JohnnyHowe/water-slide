using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // number between -1 and 1
    public float horizontalPosition = 0;
    public float maxMovement = 0.8f;
    public float movementMultiplier = 1;
    public float rotationSpeed = 1f;
    public float movementSpeed = 5;
    public float radius = 1;
    Vector2 rotateOrigin;

    // Start is called before the first frame update
    void Start()
    {
        rotateOrigin = (Vector2) transform.position + Vector2.up * radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TouchInput.touched)
        {
            horizontalPosition = Mathf.Min(Mathf.Max(-maxMovement, TouchInput.screenPosition.x * 2 - 1), maxMovement);

            float lastX = transform.position.x;
            float nextX = horizontalPosition * movementMultiplier;
            float smoothX = transform.position.x - (lastX - nextX) * movementSpeed * Time.deltaTime;

            transform.position = GetPositionOnCircle(smoothX, radius, rotateOrigin);
        }
    }

    Vector2 GetPositionOnCircle(float x, float radius, Vector2 origin)
    {
        float ox = x - origin.x;
        float y;
        if (ox >= radius)
        {
            y = origin.y;
            x = origin.x + radius;
        }
        else if (ox <= -radius)
        {
            y = origin.y;
            x = origin.x - radius;
        } else
        {
            y = -Mathf.Sqrt(radius * radius - ox * ox) + origin.y;
        }

        return new Vector2(x, y);
    }
}
