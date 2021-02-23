using System.Collections;
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

    public GameObject slideSectionPrefab;

    int generatedSections = 0;
    List<SlideSection> sections;

    void Start()
    {
        sections = new List<SlideSection>();
        //sections.Add(new SlideSection(Vector3.zero, Vector3.forward, 10, sectionLength));

        SlideSection section = Instantiate(slideSectionPrefab, transform).GetComponent<SlideSection>();
        section.Init(Vector3.zero, Vector3.forward, 10, sectionLength);
        sections.Add(section);

        generatedSections += 1;
        UpdateSections();
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

            SlideSection section = Instantiate(slideSectionPrefab, transform).GetComponent<SlideSection>();
            section.Init(last.endPoint, last.endDirection, radius * sign, sectionLength);
            sections.Add(section);

            generatedSections += 1;
        }

        int sectionsToDelete = Mathf.Max(sections.Count - (forwardBufferSections + rearBufferSections), 0);
        for (int i = 0; i < sectionsToDelete; i++)
        {
            Destroy(sections[i].gameObject);
        }
        sections.RemoveRange(0, sectionsToDelete);
    }

    private void Update()
    {
        UpdateSections();
    }

    //void UpdateRenderer()
    //{
    //    List<Vector3> pts = new List<Vector3>();
    //    int accuracy = 20;
    //    foreach (SlideSection section in sections)
    //    {
    //        for (int i = 0; i < accuracy; i ++)
    //        {
    //            pts.Add(section.GetPoint(i * 1.0f / accuracy));
    //        }
    //    }
    //    lineRenderer.positionCount = pts.Count;
    //    for (int i = 0; i < pts.Count; i ++)
    //    {
    //        lineRenderer.SetPosition(i, pts[i]);
    //    }
    //}
}

