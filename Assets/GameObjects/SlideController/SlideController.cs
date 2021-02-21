using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    //public int rearBufferSections = 2;
    public int forwardBufferSections = 10;
    public float centerProgress = 0;
    public float sectionLength = 5;

    public float minRadius = 1;
    public float maxRadius = 2;

    public LineRenderer lineRenderer;
    public LineRenderer lineRenderer2;

    List<SlideSection> sections;

    void Start()
    {
        sections = new List<SlideSection>();
        sections.Add(new SlideSection(Vector3.zero, Vector3.forward, 10, sectionLength));
        for (int i = 0; i < forwardBufferSections; i++)
        {
            int sign = Mathf.RoundToInt(Random.value) * 2 - 1;
            float radius = ((float)Random.Range(0, 1000) / 1000) * (maxRadius - minRadius) + minRadius;

            SlideSection last = sections[sections.Count - 1];
            sections.Add(new SlideSection(last.GetPoint(1), last.GetDirection(1), radius * sign, sectionLength));
        }

        UpdateRenderer();
    }

    private void Update()
    {
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
    float radius;
    float length;

    public SlideSection(Vector3 start, Vector3 lastDir, float radius, float length)
    {
        this.start = start;
        this.radius = radius;
        this.length = length;

        this.origin = start + new Vector3(lastDir.z, 0, -lastDir.x).normalized * radius;
    }

    public Vector3 GetPoint(float d)
    {
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
        Vector3 point = GetPoint(d);
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
