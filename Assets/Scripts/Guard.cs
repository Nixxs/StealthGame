using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Transform pathholder;
    public float speed;
    public float turnspeed;
    public float waitTime;
    IEnumerator currentBehavior;
    int waypointIndex;

    public Light spotLight;
    public float viewDistance;
    float viewAngle;

    float detectionCountDown;
    float detectionTimer;
    Color neutralSpotlightColour;
    
    void Start()
    {
        viewAngle = spotLight.spotAngle;
        viewDistance = spotLight.range - 2f;

        speed = 7f;
        turnspeed = 50f;
        waitTime = 0.5f;
        waypointIndex = 0;

        detectionTimer = 1.0f;
        detectionCountDown = detectionTimer;
        neutralSpotlightColour = spotLight.color;

        // snap the guard to his starting position
        transform.position = pathholder.GetChild(waypointIndex).position;
        currentBehavior = FollowWaypoints();
        StartCoroutine(currentBehavior);
    } 

    void Update()
    {
        if (CanSeePlayer())
        {
            if (detectionCountDown <= 0f)
            {
                spotLight.color = Color.red;
                Debug.Log("GAME OVER");
            }
            else
            {
                detectionCountDown -= Time.deltaTime;
            }
        }
        else
        {
            spotLight.color = neutralSpotlightColour;
            detectionCountDown = detectionTimer;
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
            while (Quaternion.Angle(transform.rotation, targetRotation) >= 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnspeed * Time.deltaTime);
                yield return null;
            }

            // if we've reached the target waypoint then stop if now keep moving toward it
            if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.2f)
            {
                yield return new WaitForSeconds(waitTime);
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

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
    bool CanSeePlayer()
    {
        Player player = FindObjectOfType<Player>();
        bool withinRangeOfPlayer = false;
        bool viewAngleOverlaps = false;
        bool noObstructions = false;

        // check for range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= viewDistance)
        {
            withinRangeOfPlayer = true;
        }

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer <= viewAngle / 2)
        {
            viewAngleOverlaps = true;
        }

        // if we are within range of the player and the viewangle overlaps then
        // we will check for obstructions
        if (withinRangeOfPlayer && viewAngleOverlaps)
        {
            // define a ray from position to the position of the player
            Ray ray = new Ray(transform.position, directionToPlayer);
            RaycastHit hitInfo;

            // cast the ray out limit distance to the view distance
            if (Physics.Raycast(ray, out hitInfo, viewDistance))
            {
                // the ray has hit the player
                if (hitInfo.transform.gameObject.tag == "player")
                {
                    Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
                    noObstructions = true;
                }
            }
        }

        if (withinRangeOfPlayer && viewAngleOverlaps && noObstructions)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}