using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSection_LineRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int points = 5;

    SlideSection section;

    void Awake()
    {
        section = GetComponent<SlideSection>(); 
    }

    void Update()
    {
        lineRenderer.positionCount = points;
        for (int i = 0; i < points; i ++)
        {
            lineRenderer.SetPosition(i, section.GetPoint(i * 1.0f / (points - 1)));
        }
    }
}
