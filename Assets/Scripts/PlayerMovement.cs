using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed;
    public int turnspeed;
    public float smoothMovementTime;

    float smoothMovementVelocity;
    float smoothMagnitude;
    Vector3 direction;
    Vector3 velocity;

    Quaternion lookAngle;
    Rigidbody playerRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        speed = 7;
        turnspeed = 8;
        smoothMovementTime = 0.1f;
        velocity = Vector3.zero;

        playerRigidBody = FindObjectOfType<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = direction.magnitude;
        smoothMagnitude = Mathf.SmoothDamp(smoothMagnitude, inputMagnitude, ref smoothMovementVelocity, smoothMovementTime);
        velocity = direction * speed * smoothMagnitude;
    }

    void FixedUpdate()
    {
        if (velocity.magnitude > 0f)
        {
            playerRigidBody.MovePosition(playerRigidBody.position + (velocity * Time.fixedDeltaTime));

            lookAngle = Quaternion.Lerp(lookAngle, Quaternion.LookRotation(direction), Time.fixedDeltaTime * turnspeed);
            playerRigidBody.MoveRotation(lookAngle);
        }
    }
}
