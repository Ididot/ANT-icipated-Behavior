using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    public float trailDecayRate = 1.0f;
    private List<Vector3> trailPositions = new List<Vector3>();
    private List<float> trailStrengths = new List<float>();

    public void AddTrailPoint(Vector3 position)
    {
        trailPositions.Add(position);
        trailStrengths.Add(1.0f); // Olika styrkor på feromontrailen
    }

    // Trailen blir svagare med tiden
    void Update()
    {
        for (int i = 0; i < trailStrengths.Count; i++)
        {
            trailStrengths[i] -= trailDecayRate * Time.deltaTime;
            if (trailStrengths[i] <= 0.0f)
            {
                trailPositions.RemoveAt(i);
                trailStrengths.RemoveAt(i);
                i--;
            }
        }
    }

    public Vector3 GetClosestTrailPoint(Vector3 position, float detectionRange)
    {
        Vector3 closestPoint = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < trailPositions.Count; i++)
        {
            float distance = Vector3.Distance(position, trailPositions[i]);
            if (distance < detectionRange && distance < closestDistance)
            {
                closestPoint = trailPositions[i];
                closestDistance = distance;
            }
        }

        return closestDistance < Mathf.Infinity ? closestPoint : Vector3.zero;
    }
}
