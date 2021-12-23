using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool disabled;
    public int speed;
    public int turnspeed;
    public float smoothMovementTime;
    public Rigidbody rigidBody
    {
        get {return GetComponent<Rigidbody>();}
    }
    public Vector3 position 
    {
        get { return transform.position; }   
    }

    void Start()
    {
        speed = 7;
        turnspeed = 8;
        smoothMovementTime = 0.1f;
        disabled = false;

        Guard.GuardHasSpottedPlayer += Disable;
        FinishZone.PlayerWins += Disable;
    }

    void Disable()
    {
        disabled = true;
    }

    void OnDestroy()
    {
        Guard.GuardHasSpottedPlayer -= Disable;
        FinishZone.PlayerWins -= Disable;
    }
}
