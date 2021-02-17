using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    public int rearBufferSections = 2;
    public int forwardBufferSections = 5;
    public float centerProgress = 0;
    public float sectionLength = 5;
    public float maxAngle = 60;

    float lastAngle = 0;
    int generatedNodes = 0;
    int generatedSections = 0;
    public LineRenderer lineRenderer;
    public LineRenderer lineRenderer2;

    List<Vector3> slideNodes;
    List<SlideSection> slideSections;

    void Start()
    {
        slideNodes = new List<Vector3>
        {
            Vector3.zero
        };
        generatedNodes = 1;
        slideSections = new List<SlideSection>();
        UpdateNodes();
        UpdateRenderer();
    }

    private void Update()
    {
        UpdateNodes();
        UpdateRenderer();
    }

    public SlideSection GetCurrentSection(float d)
    {
        return slideSections[Mathf.FloorToInt(d)];
    }
        
    private void UpdateNodes()
    {
        // create new nodes
        while (generatedSections < centerProgress + forwardBufferSections)
        {
            NewNode();
        } 
        //// delete old nodes
        //while (slideNodes.Count > forwardBufferSections + rearBufferSections)
        //{
        //    slideNodes.RemoveAt(0);
        //}
        //UpdateRenderer();
    }

    void NewNode()
    {
        lastAngle = (lastAngle + (Random.Range(-100, 100) / 100f) * maxAngle) % 360;
        float rads = Mathf.Deg2Rad * lastAngle;
        Vector3 directionVector = new Vector3(Mathf.Sin(rads), 0, Mathf.Cos(rads)).normalized;
        Vector3 position = slideNodes[slideNodes.Count - 1] + directionVector * sectionLength;
        slideNodes.Add(position);
        generatedNodes += 1;

        // Create new section
        if ((generatedNodes + 1) % 1 == 0 && generatedNodes >= 3)
        {
            int count = slideNodes.Count;
            SlideSection section = new SlideSection(slideNodes[count - 1], slideNodes[count - 2], slideNodes[count - 3]);
            slideSections.Add(section);
            generatedSections += 1;
        }
    }

    void UpdateRenderer()
    {
        //lineRenderer.positionCount = slideNodes.Count;
        //for (int i = 0; i < slideNodes.Count; i++)
        //{
        //    lineRenderer.SetPosition(i, slideNodes[i]);
        //}

        int steps = 20;
        List<Vector3> pts = new List<Vector3>();
        for (int i = 0; i < slideSections.Count; i++)
        {
            SlideSection section = slideSections[Mathf.FloorToInt(i)];
            for (int j = 0; j < steps; j ++)
            {
                float d = j * 1.0f / steps;
                Vector3 pt = GetCurrentSection(i).GetPositionOnSlide(d);
                pts.Add(pt);
            }
        }
        lineRenderer2.positionCount = pts.Count;
        for (int i = 0; i < pts.Count; i++)
        {
            lineRenderer2.SetPosition(i, pts[i]);
        }
    }
}
