using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CameraFollowPlayer : MonoBehaviour
{
    public Vector3 positionOffset = new Vector3(0, 5, -5);
    public Vector3 eulerAngleOffset = new Vector3(30, 0, 0);
    public float angleMultiplier = Mathf.PI;

    void Update()
    {
        Camera.main.transform.position = transform.position + positionOffset;    
        Camera.main.transform.eulerAngles = eulerAngleOffset + Vector3.forward * transform.position.x * angleMultiplier;
    }
}
