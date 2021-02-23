using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSection : MonoBehaviour
{
    Vector3 start;
    Vector3 origin;
    public Vector3 endPoint;
    public Vector3 endDirection;
    float radius;
    float length;

    public void Init(Vector3 start, Vector3 lastDir, float radius, float length)
    {
        this.start = start;
        this.radius = radius;
        this.length = length;

        this.origin = start + new Vector3(lastDir.z, 0, -lastDir.x).normalized * radius;
        endPoint = GetPointNoOffset(1);
        endDirection = GetDirection(1);
    }

    public Vector3 GetPoint(float d)
    {
        return GetPoint(new Vector2(0, d));
    }

    public Vector3 GetPoint(Vector2 slidePosition)
    {
        Vector3 point = GetPointNoOffset(slidePosition.y);
        Vector3 direction = GetDirection(slidePosition);
        Vector3 normal = new Vector3(direction.z, 0, -direction.x);
        return point + normal.normalized * slidePosition.x;
    }

    private Vector3 GetPointNoOffset(float d)
    {
        if (d > 1)
        {
            d = d % 1;
        }
        float angle = d * (length / radius);

        float ox = origin.x;
        float oy = origin.z;

        float px = start.x;
        float py = start.z;

        float qx = ox + Mathf.Cos(-angle) * (px - ox) - Mathf.Sin(-angle) * (py - oy);
        float qy = oy + Mathf.Sin(-angle) * (px - ox) + Mathf.Cos(-angle) * (py - oy);
        return new Vector3(qx, 0, qy);
    }

    public Vector3 GetDirection(float d)
    {
        return GetDirection(new Vector2(0, d));
    }

    public Vector3 GetDirection(Vector2 slidePosition)
    {
        Vector3 point = GetPointNoOffset(slidePosition.y);
        Vector3 perp = origin - point;

        if (radius < 0)
        {
            return new Vector3(perp.z, 0, -perp.x);
        } else
        {
            return new Vector3(-perp.z, 0, perp.x);
        }
    }
}
