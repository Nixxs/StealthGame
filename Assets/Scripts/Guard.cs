using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Transform pathholder;

    private void OnDrawGizmos()
    {
        Vector3 startPosition = transform.position;
        Vector3 previousPosition = startPosition;

        // loop through all the waypoints in the given pathholder and draw gizmos spheres for each
        for (int i = 0; i < pathholder.childCount; i++)
        {
            Transform currentWaypoint = pathholder.GetChild(i);
            Gizmos.DrawSphere(currentWaypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, currentWaypoint.position);
            previousPosition = currentWaypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);
    }
}