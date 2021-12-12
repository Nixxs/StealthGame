using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Transform pathholder;
    public float speed;
    public float turnspeed;
    IEnumerator currentBehavior;
    int waypointIndex;
    
    void Start()
    {
        speed = 5f;
        turnspeed = 35f;
        waypointIndex = 0;

        // snap the guard to his starting position
        transform.position = pathholder.GetChild(waypointIndex).position;
        currentBehavior = FollowWaypoints();
        StartCoroutine(currentBehavior);
    } 

    void Update()
    {
        // example of conditionals that change behaviour
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeBehavior(FollowWaypoints());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeBehavior(OtherBehaviour());
        }
    }

    // a test behaviour
    IEnumerator OtherBehaviour()
    {

        print("other behaviour");
        yield return null;
        
    }

    IEnumerator FollowWaypoints()
    {

        Transform currentWaypoint = pathholder.GetChild(waypointIndex);

        while (true)
        {
            // get the target vector direction to the next waypoint
            Vector3 targetDirection = (currentWaypoint.position - transform.position).normalized;
            // calculate the target rotation that we should end up in before moving
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // while the current angle is more than 0.01f from the target rotation rotate toward the target
            while (Quaternion.Angle(transform.rotation, targetRotation) >= 0.05f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnspeed * Time.deltaTime);
                yield return null;
            }

            // if we've reached the target waypoint then stop if now keep moving toward it
            if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.2f)
            {
                // reached the waypoint so update to the next waypoint
                waypointIndex += 1;
                if (waypointIndex >= pathholder.childCount)
                {
                    waypointIndex = 0;
                }
                // update current waypoint to the next waypoint
                currentWaypoint = pathholder.GetChild(waypointIndex);
            }
            else // move toward the current waypoint
            {
                Vector3 direction = (currentWaypoint.position - transform.position).normalized;
                Vector3 movement = direction * speed * Time.deltaTime;
                transform.position += new Vector3(movement.x, 0, movement.z);
                yield return null;
            }
        }
    }

    // method for switching behaviours at any time
    void ChangeBehavior(IEnumerator newBehavior)
    {
        StopCoroutine(currentBehavior);

        currentBehavior = newBehavior;
        StartCoroutine(newBehavior);
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