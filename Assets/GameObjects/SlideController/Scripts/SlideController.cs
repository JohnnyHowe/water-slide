using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    public GameObject slideSectionPrefab;
    public Range sectionLengthRange = new Range(1, 2);
    public float maxAngle = 1f;
    public List<SlideSection> sections;

    // Start is called before the first frame update
    void Start()
    {
        sections = new List<SlideSection>();

        GameObject node1 = Instantiate(slideSectionPrefab, transform);
        node1.transform.position = Vector3.zero;
        sections.Add(node1.GetComponent<SlideSection>());
        GameObject node2 = Instantiate(slideSectionPrefab, transform);
        node2.transform.position = Vector3.forward;
        sections.Add(node2.GetComponent<SlideSection>());

        float lastAngle = 0;

        for (int i = 2; i < 100; i++)
        {
            lastAngle += maxAngle * RandomUnsignedFloat();

            GameObject node = Instantiate(slideSectionPrefab, transform);
            node.transform.position = sections[i - 1].gameObject.transform.position + new Vector3(Mathf.Sin(lastAngle), 0, Mathf.Cos(lastAngle));
            sections.Add(node.GetComponent<SlideSection>());
        }
    }

    public Vector3 GetPositionOnSlide(float d)
    {
        Vector3 last = sections[Mathf.FloorToInt(d)].transform.position;
        Vector3 next = sections[Mathf.CeilToInt(d)].transform.position;
        float nextWeight = d % 1;
        return last + (next - last) * nextWeight;
    }

    public float GetAngleOnSlide(float d)
    {
        int nodeNum = Mathf.RoundToInt(d);
        if (0 < nodeNum && nodeNum < sections.Count - 1)
        {
            Vector3 lastNode = sections[nodeNum - 1].transform.position;
            Vector3 closestNode = sections[nodeNum].transform.position;
            Vector3 nextNode = sections[nodeNum + 1].transform.position;

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
        return Random.Range(-1000, 1000) / 1000f;
    }
}

[System.Serializable]
public class Range
{
    [SerializeField]
    public float min;
    [SerializeField]
    public float max;

    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
