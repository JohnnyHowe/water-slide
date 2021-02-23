﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    public int rearBufferSections = 2;
    public int forwardBufferSections = 10;
    public float centerProgress = 0;
    public float sectionLength = 5;

    public float minRadius = 1;
    public float maxRadius = 2;

    public LineRenderer lineRenderer;
    public LineRenderer lineRenderer2;

    int generatedSections = 0;
    List<SlideSection> sections;

    void Start()
    {
        sections = new List<SlideSection>();
        sections.Add(new SlideSection(Vector3.zero, Vector3.forward, 10, sectionLength));
        generatedSections += 1;
        UpdateSections();
        UpdateRenderer();
    }

    public Vector3 GetPositionOnSlide(Vector2 slidePosition)
    {
        return GetSection(slidePosition).GetPoint(slidePosition);
    }

    public Vector3 GetDirectionOnSlide(Vector2 slidePosition)
    {
        return GetSection(slidePosition).GetDirection(slidePosition);
    }

    public float GetAngleOnSlide(Vector2 slidePosition)
    {
        Vector3 direction = GetDirectionOnSlide(slidePosition);
        return -Mathf.Atan2(direction.z, direction.x) + Mathf.PI / 2;
    }

    SlideSection GetSection(Vector2 slidePosition)
    {
        int startIndex = generatedSections - sections.Count;
        int index = Mathf.FloorToInt(slidePosition.y) - startIndex;
        return sections[index];
    }

    void UpdateSections()
    {
        for (int i = 0; i < centerProgress + forwardBufferSections - generatedSections; i++)
        {
            int sign = Mathf.RoundToInt(Random.value) * 2 - 1;
            float radius = (Random.Range(0, 1000) / 1000.0f) * (maxRadius - minRadius) + minRadius;
            SlideSection last = sections[sections.Count - 1];
            sections.Add(new SlideSection(last.endPoint, last.endDirection, radius * sign, sectionLength));
            generatedSections += 1;
        }

        int sectionsToDelete = Mathf.Max(sections.Count - (forwardBufferSections + rearBufferSections), 0);
        sections.RemoveRange(0, sectionsToDelete);
    }

    private void Update()
    {
        UpdateSections();
        UpdateRenderer();
    }

    void UpdateRenderer()
    {
        List<Vector3> pts = new List<Vector3>();
        int accuracy = 20;
        foreach (SlideSection section in sections)
        {
            for (int i = 0; i < accuracy; i ++)
            {
                pts.Add(section.GetPoint(i * 1.0f / accuracy));
            }
        }
        lineRenderer.positionCount = pts.Count;
        for (int i = 0; i < pts.Count; i ++)
        {
            lineRenderer.SetPosition(i, pts[i]);
        }
    }
}


class SlideSection
{
    Vector3 start;
    Vector3 origin;
    public Vector3 endPoint;
    public Vector3 endDirection;
    float radius;
    float length;

    public SlideSection(Vector3 start, Vector3 lastDir, float radius, float length)
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
