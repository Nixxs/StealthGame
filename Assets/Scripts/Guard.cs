using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Transform pathholder;
    public float speed;
    public float waitTime;
    float timer;
    Transform currentWaypoint;
    int waypointIndex;
    
    void Start()
    {
        waypointIndex = 0;
        currentWaypoint = pathholder.GetChild(waypointIndex);
        speed = 5f;
        waitTime = 2f;
        timer = 0f;

        // snap the guard to his starting position
        transform.position = currentWaypoint.position;
    } 

    void Update()
    {


    }

    private void FixedUpdate()
    {
        // if we've reached the target waypoint then stop if now keep moving toward it
        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.2f)
        {
            // if the timer is finished then set new waypoint and reset timer
            if (timer <= 0)
            {
                // change the current waypoint to the next one but if the index is larger than the possible
                // number of waypoints then set the waypoint back to the first waypoint again
                waypointIndex += 1;
                if (waypointIndex >= pathholder.childCount)
                {
                    waypointIndex = 0;
                }
                currentWaypoint = pathholder.GetChild(waypointIndex);
                timer = waitTime;
            } else // keep the timer ticking along while the guard has stopped moving
            {
                timer -= Time.fixedDeltaTime;
            }
        } else // move toward the current waypoint
        {
            Vector3 direction = (currentWaypoint.position - transform.position).normalized;
            transform.position += direction * speed * Time.fixedDeltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathholder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        // loop through all the waypoints in the given pathholder and draw gizmos spheres for each
        for (int i = 0; i < pathholder.childCount; i++)
        {
            Transform currentGizmoWaypoint = pathholder.GetChild(i);
            Gizmos.DrawSphere(currentGizmoWaypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, currentGizmoWaypoint.position);
            previousPosition = currentGizmoWaypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);
    }
}