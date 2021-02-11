using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public SlideController slideController;
    public float xMax = 3;
    public float speed = 1;

    public Vector2 slidePosition = Vector2.zero;

    void FixedUpdate()
    {
        UpdateHorizontalMovement();
        slidePosition.y += Time.deltaTime * speed;
        Vector3 restPosition = slideController.GetPositionOnSlide(slidePosition.y);
        float angle = slideController.GetAngleOnSlide(slidePosition.y);
        transform.position = restPosition;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
    }

    void UpdateHorizontalMovement()
    {
        slidePosition.x = (TouchInput.screenPosition.x - 0.5f) * xMax * 2;
    }
}
