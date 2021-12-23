using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZone : MonoBehaviour
{
    public static event System.Action PlayerWins;

    private void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.gameObject.name == "Player")
        {
            if (PlayerWins != null)
            {
                PlayerWins();
            }
        }
    }
}
