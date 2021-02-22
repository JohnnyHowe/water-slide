using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public SlideController slideController;
    public float xMax = 3;
    public float speed = 1;

    public Transform centerSphere;

    public Vector2 slidePosition = Vector2.zero;

    void FixedUpdate()
    {
        UpdateHorizontalMovement();

        slidePosition.y += Time.deltaTime * speed;
        slideController.centerProgress = slidePosition.y;

        UpdateTransform();
    }

    void UpdateHorizontalMovement()
    {
        slidePosition.x = (TouchInput.screenPosition.x - 0.5f) * xMax * 2;
    }

    void UpdateTransform()
    {
        transform.position = slideController.GetPositionOnSlide(slidePosition);

        float angle = slideController.GetAngleOnSlide(slidePosition) * 180 / Mathf.PI;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
    }
}
