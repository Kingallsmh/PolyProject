using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBase : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    [SerializeField]
    Transform cam;
    public Transform cube;

    public float moveSpeed = 1;
    public float turnSpeed = 10;
    public float maxGroundAngle = 120;
    public bool debug;
    public float rayDistance = 3;
    public float currentGravAccel = 1;
    float gravitySpeed;

    Vector3 input;
    float angle;
    float groundAngle;

    Quaternion targetRotation;
    Vector3 targetVelocity;
    bool grounded = false;
    Vector3 forward;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        CalculateDirection();
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();
        Move();
        Rotate();

        anim.SetFloat("MoveSpeed", rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        
    }

    void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.z = Input.GetAxisRaw("Vertical");
    }

    void CalculateForward()
    {
        if (!grounded)
        {
            forward = transform.forward;
        }
        else
        {

            forward = Vector3.Cross(hitInfo.normal, -transform.right);
        }
        cube.forward = forward;
        DrawDebugLine(transform.position, transform.position + forward * 2, Color.blue);
    }

    void CalculateGroundAngle()
    {
        if (!grounded)
        {
            groundAngle = 90;
        }
        else
        {
            groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
        }
    }

    void CheckGround()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), -Vector3.up, out hitInfo, rayDistance))
        {
            DrawDebugLine(transform.position + new Vector3(0, 0.5f, 0), hitInfo.point, Color.red);
            grounded = true;
        }
        else
        {
            DrawDebugLine(transform.position + new Vector3(0, 0.5f, 0), transform.position + new Vector3(0, 0.5f, 0) + -Vector3.up * rayDistance, Color.red);
            grounded = false;
        }
    }

    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.z);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    void ApplyGravity()
    {
        if (!grounded)
        {
            gravitySpeed += currentGravAccel/10;
        }
        else
        {
            gravitySpeed = 0;
        }
    }

    void Rotate()
    {
        if(input.magnitude > 0)
        {
            targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }        
    }

    //Should rotate legs towards the wanted direction, leaving the torso facing whichever direction it is
    private void Move()
    {
        //if (groundAngle >= maxGroundAngle) return; //TODO, terrible way to handle this. Would prevent movement. Move this to a forward checking raycast.
        //Vector3 inputCamDir = cam.TransformDirection(input.normalized);
        //inputCamDir.y = 0;
        //Vector3 charForwardInput = transform.TransformDirection(input).normalized * moveSpeed;
        //targetVelocity = charForwardInput;
        if(input.magnitude > 0)
        {
            targetVelocity = forward * moveSpeed;
        }
        else
        {
            targetVelocity = Vector3.zero;
        }
        targetVelocity.y -= gravitySpeed;
        rb.velocity = targetVelocity * Time.deltaTime;
    }

    void DrawDebugLine(Vector3 _pointA, Vector3 _pointB)
    {
        if (debug)
        {
            Debug.DrawLine(_pointA, _pointB);
        }
    }

    void DrawDebugLine(Vector3 _pointA, Vector3 _pointB, Color _color)
    {
        if (debug)
        {
            Debug.DrawLine(_pointA, _pointB, _color);
        }
    }
}
