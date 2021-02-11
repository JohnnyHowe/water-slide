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

        NewNode(Vector3.zero);
        NewNode(Vector3.one);
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
            lastAngle += maxAngle * RandomSignedFloat();
            Vector3 change = new Vector3(Mathf.Sin(lastAngle), 0, Mathf.Cos(lastAngle)) * sectionLength;
            NewNode(sections[sections.Count - 1].gameObject.transform.position + change);
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

    GameObject NewNode(Vector3 position)
    {
        GameObject node = Instantiate(slideSectionPrefab, transform);
        node.transform.position = position;
        SlideSection section = node.GetComponent<SlideSection>();
        section.number = generatedSections;
        sections.Add(section);
        generatedSections += 1;
        return node;
    }

    SlideSection GetSectionAt(int d)
    {
        return sections[d - sections[0].number];
    }

    public Vector3 GetPositionOnSlide(float d)
    {
        Vector3 last = GetSectionAt(Mathf.FloorToInt(d)).transform.position;
        Vector3 next = GetSectionAt(Mathf.CeilToInt(d)).transform.position;
        float nextWeight = d % 1;
        return last + (next - last) * nextWeight;
    }

    public float GetAngleOnSlide(float d)
    {
        int nodeNum = Mathf.RoundToInt(d);
        if (0 < nodeNum && nodeNum < generatedSections - 1)
        {
            Vector3 lastNode = GetSectionAt(nodeNum - 1).transform.position;
            Vector3 closestNode = GetSectionAt(nodeNum).transform.position;
            Vector3 nextNode = GetSectionAt(nodeNum + 1).transform.position;

            Vector3 lastLine = closestNode - lastNode;
            Vector3 nextLine = nextNode - closestNode;

            float progress = (d - 0.5f) % 1;

            Vector3 p1 = lastNode + lastLine * progress;
            Vector3 p2 = closestNode + nextLine * progress;

            Vector3 direction = p2 - p1;
            //return Vector3.Angle(Vector3.forward, direction);
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

