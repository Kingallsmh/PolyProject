using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BreakBall>())
        BreakFightNetworkManager.Instance.StopHost();
    }

}
