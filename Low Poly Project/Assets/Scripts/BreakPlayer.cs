using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlayer : NetworkBehaviour
{
    public float moveSpeed = 1;
    public Rigidbody rb;

    //[ClientRpc]
    //public void RpcInitPlayer()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    PlayerManager.Instance.ParentPlayerToManager(transform);
    //}
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) * moveSpeed;        
        rb.velocity = move;
    }
}
