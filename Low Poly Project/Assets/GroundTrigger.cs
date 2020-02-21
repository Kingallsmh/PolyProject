using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    public List<Collider> ignoreList;
    bool isTriggered = false;

    public void AddToIgnoreList(Collider _toIgnore)
    {
        if(ignoreList == null)
        {
            ignoreList = new List<Collider>();
        }
        ignoreList.Add(_toIgnore);
    }

    private void FixedUpdate()
    {
        isTriggered = false;
        //Check if triggeroverlaps anything
        Collider[] colArray = Physics.OverlapBox(transform.position, transform.localScale/2);
        for(int i = 0; i < colArray.Length; i++)
        {
            if (!ignoreList.Contains(colArray[i]))
            {
                isTriggered = true;
            }
        }
    }

    public bool GetIsTriggered()
    {
        return isTriggered;
    }
}
