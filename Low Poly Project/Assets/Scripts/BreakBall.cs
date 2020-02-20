using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBall : NetworkBehaviour
{
    public float speed = 1;
    public float minZ = 0.1f;
    Vector3 lastFrameVelocity;
    Rigidbody rb;

    public void InitBall()
    {
        InitBall(Vector3.zero);
    }

    public void InitBall(Vector3 _pos)
    {
        transform.position = new Vector3(_pos.x, 0, _pos.z);
        rb = GetComponent<Rigidbody>();

        int[] intArray = new int[] { -1, 1 };
        int direction = intArray[Random.Range(0, 2)];
        rb.velocity = new Vector3(0, 0, speed * direction);
    }

    Vector3 GetDifferenceFromCenter(Transform _playerBox)
    {
        Vector3 dif = (transform.position - _playerBox.position);
        dif.y = 0;
        return dif.normalized;
    }

    private void Update()
    {
        if (isServer)
        {
            lastFrameVelocity = rb.velocity;
        }        
    }

    [ServerCallback] // only call this on server
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.GetComponent<BreakPlayer>())
        {
            Vector3 dir = GetDifferenceFromCenter(col.transform);
            // Set Velocity with dir * speed
            SetVelocity(dir);
        }
        else
        {            
            Vector3 dir = Vector3.Reflect(lastFrameVelocity.normalized, col.GetContact(0).normal);
            SetVelocity(dir); 
            if (col.transform.GetComponent<BreakBrick>())
            {
                col.transform.GetComponent<BreakBrick>().DestroyBrick();
            }
        }

    }

    void SetVelocity(Vector3 _dir)
    {
        if (Mathf.Abs(_dir.z) < minZ)
        {
            if (_dir.z < 0)
            {
                _dir.z = -minZ;
            }
            else
            {
                _dir.z = minZ;
            }
        }
        rb.velocity = _dir * speed;
    }
}
