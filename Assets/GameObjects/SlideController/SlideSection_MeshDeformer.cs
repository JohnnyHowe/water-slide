using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deform the mesh such that it curves along with the section.
/// </summary>
public class SlideSection_MeshDeformer : MonoBehaviour
{
	public SlideSection section;
	public GameObject meshObj;
	public Vector3 centerPos;

    Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;

    void Start () {
		deformingMesh = meshObj.GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}

		centerPos = section.GetPoint(0.5f);
        meshObj.transform.position = centerPos;
		Deform();

        deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();
		deformingMesh.RecalculateBounds();
	}

	public void Deform()
    {
		// What is the default size of the mesh?		
		Vector3 minPos = originalVertices[0];
		Vector3 maxPos = originalVertices[0];
		foreach (Vector3 pos in originalVertices)
        {
			minPos.x = Mathf.Min(pos.x, minPos.x);
			minPos.y = Mathf.Min(pos.y, minPos.y);
			minPos.z = Mathf.Min(pos.z, minPos.z);
			maxPos.x = Mathf.Max(pos.x, maxPos.x);
			maxPos.y = Mathf.Max(pos.y, maxPos.y);
			maxPos.z = Mathf.Max(pos.z, maxPos.z);
        }
		Vector3 sizeChange = maxPos - minPos;

		// Scale vertices to be between 0 and 1
		Vector3[] scaledVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i ++)
        {
			Vector3 originalVertex = originalVertices[i];
			Vector3 scaledVertex = new Vector3(originalVertex.x, originalVertex.y, originalVertex.z);

            scaledVertex -= minPos;
            scaledVertex.x /= sizeChange.x;
			scaledVertex.y /= sizeChange.y;
			scaledVertex.z /= sizeChange.z;

			scaledVertex.x -= 0.5f;
			//scaledVertex.y -= 0.5f;

			scaledVertex.x *= section.scale.x;
			scaledVertex.y *= section.scale.y;

			scaledVertices[i] = scaledVertex;
        }

		// Deform
		Vector3[] rotatedVertices = new Vector3[scaledVertices.Length];
		for (int i = 0; i < originalVertices.Length; i ++)
        {
            Vector3 scaledPoint = scaledVertices[i];
            Vector3 point = section.GetFlatPoint(new Vector2(scaledPoint.x, scaledPoint.z));
			point.y = scaledPoint.y;

			Vector3 deformedPoint = point - centerPos;
			rotatedVertices[i] = deformedPoint;
        }

		displacedVertices = rotatedVertices;
    }
}
