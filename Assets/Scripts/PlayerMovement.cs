using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;

    int speed;
    int turnspeed;
    float smoothMovementTime;

    float smoothMovementVelocity;
    float smoothMagnitude;
    Vector3 direction;
    Vector3 velocity;

    Quaternion lookAngle;
    Rigidbody playerRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();

        speed = player.speed;
        turnspeed = player.turnspeed;
        smoothMovementTime = player.smoothMovementTime;
        velocity = Vector3.zero;

        playerRigidBody = player.rigidBody;
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.zero;
        if (player.disabled == false)
        {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
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
