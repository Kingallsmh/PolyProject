using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : NetworkBehaviour
{
    const string propertyName = "_V_WIRE_Color";

    public void CreateBrick(Color _col, Vector3 _size)
    {
        //TODO allow for different colors over network
        //GetComponent<Renderer>().material.SetColor(propertyName, _col);
        transform.localScale = _size;
    }

    public void DestroyBrick()
    {
        NetworkServer.Destroy(gameObject);
    }    
}
