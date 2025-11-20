using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DrawLineController : MonoBehaviour
{
    public TrailRenderer trailRenderer;  // Reference to the pre-configured TrailRenderer component
    public float zOffset = -0.8f;  // Offset for Z to ensure the trail is in front of objects
    public float xLeftOffset = -1f;  // Left offset for the first position
    public float xRightOffset = 1f;  // Right offset for the last position
    private List<Vector3> positions = new List<Vector3>();
    public float timeLength = 0.05f;

    public GameObject impactVFXPrefab;  // Reference to the VFX prefab

    void Start()
    {
        if (trailRenderer == null)
        {
            Debug.LogError("TrailRenderer is not assigned!");
        }

        if (trailRenderer != null)
        {
            AnimationCurve widthCurve = new AnimationCurve();
            //widthCurve.AddKey(0f, 0.05f);  // Thin at the start
            //widthCurve.AddKey(0.5f, 0.45f);  // Thick in the middle
            //widthCurve.AddKey(1f, 0.05f);  // Thin at the end

            trailRenderer.widthCurve = widthCurve;

            //trailRenderer.enabled = false;
        }
    }

    public void SetPositions(List<Vector3> newPositions)
    {
        if (newPositions == null || newPositions.Count < 2)
        {
            Debug.LogWarning("Not enough positions to draw a trail.");
            return;
        }

        Vector3 firstPosition = newPositions[0] + new Vector3(xLeftOffset, 0f, 0f);
        Vector3 lastPosition = newPositions[newPositions.Count - 1] + new Vector3(xRightOffset, 0f, 0f);

        positions = new List<Vector3>(newPositions);
        positions[0] = firstPosition;
        positions[positions.Count - 1] = lastPosition;

        if (positions.Count > 1 && trailRenderer != null)
        {
            trailRenderer.enabled = true;

            GameObject trailObject = new GameObject("TrailObject");
            trailObject.transform.position = positions[0];  
            trailObject.AddComponent<TrailRenderer>(); 

            TrailRenderer objectTrail = trailObject.GetComponent<TrailRenderer>();

            objectTrail.time = trailRenderer.time;
            objectTrail.widthCurve = trailRenderer.widthCurve;
            objectTrail.startColor = trailRenderer.startColor;
            objectTrail.endColor = trailRenderer.endColor;
            objectTrail.minVertexDistance = trailRenderer.minVertexDistance;

            if (trailRenderer.material != null)
            {
                objectTrail.material = trailRenderer.material;
            }

            StartCoroutine(FollowPathWithTrail(trailObject, positions));
        }
    }

    private IEnumerator FollowPathWithTrail(GameObject trailObject, List<Vector3> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            trailObject.transform.position = path[i] + new Vector3(0f, 0f, zOffset);
            yield return new WaitForSeconds(timeLength);  
        }
        //trailRenderer.enabled = false;
        Destroy(trailObject);  
    }
}
