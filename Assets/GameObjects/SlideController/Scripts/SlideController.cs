using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    public GameObject slideSectionPrefab;
    public float sectionLength = 5;
    public float maxAngle = 1f;
    public List<SlideSection> sections;

    public int forwardBufferSections = 5; 
    public int rearBufferSections = 5; 
    public float centerProgress = 0;
    int generatedSections = 0;
    float lastAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        sections = new List<SlideSection>();

        NewNode(Vector3.zero, Vector3.forward * sectionLength, 0, 0);
        UpdateSlide();
    }

    private void Update()
    {
        UpdateSlide(); 
    }

    void UpdateSlide()
    {
        // Generate new sections
        while (generatedSections < centerProgress + forwardBufferSections)
        {
            Vector3 startPosition = sections[sections.Count - 1].endPosition;
            Vector3 positionChange = new Vector3(Mathf.Sin(lastAngle), 0, Mathf.Cos(lastAngle)) * sectionLength;
            Vector3 endPosition = startPosition + positionChange;
            float angleChange = maxAngle * RandomSignedFloat() * Mathf.PI / 180;

            NewNode(startPosition, endPosition, lastAngle, lastAngle + angleChange);
            lastAngle += angleChange;
        }
        // Delete old sections
        int deleteCount = 0;
        foreach (SlideSection section in sections)
        {
            if (section.number < centerProgress - rearBufferSections)
            {
                deleteCount += 1;
                Destroy(section.gameObject);
            }
        }
        sections.RemoveRange(0, deleteCount);
    }

    GameObject NewNode(Vector3 startPosition, Vector3 endPosition, float startAngle, float endAngle)
    {
        GameObject node = Instantiate(slideSectionPrefab, transform);
        //node.transform.position = position;
        SlideSection section = node.GetComponent<SlideSection>();

        section.number = generatedSections;
        section.startPosition = startPosition;
        section.endPosition = endPosition;
        section.startAngle = startAngle;
        section.endAngle = endAngle;
        section.UpdateMesh();

        sections.Add(section);
        generatedSections += 1;
        return node;
    }

    SlideSection GetSectionAt(float d)
    {
        return GetSectionAt(Mathf.FloorToInt(d));
    }

    SlideSection GetSectionAt(int d)
    {
        return sections[d - sections[0].number];
    }

    public Vector3 GetPositionOnSlide(float d)
    {
        SlideSection section = GetSectionAt(d);
        Vector3 last = section.startPosition;
        Vector3 next = section.endPosition;
        float nextWeight = d % 1;
        return last + (next - last) * nextWeight;
    }

    public float GetAngleOnSlide(float d)
    {
        int nodeNum = Mathf.RoundToInt(d);
        if (0 < nodeNum && nodeNum < generatedSections)
        {
            SlideSection lastSection = GetSectionAt(d);
            SlideSection nextSection = GetSectionAt(d + 1);

            Vector3 lastNode = lastSection.startPosition;
            Vector3 closestNode = lastSection.endPosition;
            Vector3 nextNode = nextSection.endPosition;

            Vector3 lastLine = closestNode - lastNode;
            Vector3 nextLine = nextNode - closestNode;

            float progress = (d) % 1;

            Vector3 p1 = lastNode + lastLine * progress;
            Vector3 p2 = closestNode + nextLine * progress;

            Vector3 direction = p2 - p1;
            return Mathf.Atan2(direction.x, direction.z) * 180 / Mathf.PI;
        } else
        {
            return 0;
        }

        //return 180 + Vector3.Angle(Vector3.forward, diff);
    }

    float RandomUnsignedFloat()
    {
        return Mathf.Abs(RandomSignedFloat());
    }

    float RandomSignedFloat()
    {
        return Random.Range(-1000, 1000) / 1000f;
    }
}

