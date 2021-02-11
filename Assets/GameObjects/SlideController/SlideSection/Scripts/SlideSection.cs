using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSection : MonoBehaviour
{
    public int number;
    public Vector3 startPosition;
    public Vector3 startDirection;
    public Vector3 endPosition;
    public Vector3 endDirection;
    public GameObject slideSectionCubePrefab;
    GameObject slideSectionCube;

    private void Awake()
    {
        slideSectionCube = Instantiate(slideSectionCubePrefab, transform);
    }

    public void DestroySection()
    {
        Destroy(gameObject);
    }

    public void UpdateMesh()
    {
        Vector3 sectionVector = endPosition - startPosition;

        float yAngle = -Vector3.Angle(sectionVector, Vector3.forward);
        slideSectionCube.transform.localEulerAngles = new Vector3(0, yAngle, 0);

        slideSectionCube.transform.localScale = new Vector3(1, 1, sectionVector.magnitude);
        slideSectionCube.transform.position = (startPosition + endPosition) / 2;
    }
}
