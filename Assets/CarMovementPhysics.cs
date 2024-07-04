using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarMovementPhysics : MonoBehaviour
{
    public float speed = 15;
    public float accelSpeed = 15;
    public float turnSpeed;

    public float driftCatchSpeed = 2.25f;
    public float maxDriftAngle = 65;
    private float targetSpeed;
    public GameObject ChocVFX;


    public CarInput carInput;

    private Vector3 moveDirection;
    private float LRF;
    private Rigidbody rb;
    public float speedActu = 0;

    private void Awake()
    {
        carInput = new CarInput();
    }
    private void OnEnable()
    {
        carInput.Enable();
    }
    void Start()
    {
        moveDirection = transform.forward;
        rb = GetComponent<Rigidbody>();
        targetSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        LRF = carInput.Car.Turn.ReadValue<float>();
    
        rb.velocity = moveDirection * speedActu;
        Turning(LRF);

        moveDirection = Vector3.RotateTowards(moveDirection, transform.forward, driftCatchSpeed * Time.deltaTime, 0.0f);
        if (Vector3.Angle(transform.forward, moveDirection) >= maxDriftAngle)
        {
            moveDirection = Quaternion.AngleAxis(maxDriftAngle * -LRF, Vector3.up) * transform.forward;
            Debug.Log("OULA, CA DRIFT LA NAN ?");
        }

        ActualiseSpeed();



    }
    void Turning(float LR)
    {
        transform.Rotate(new Vector3(0, LR * turnSpeed * Time.deltaTime, 0));



    }

    void ActualiseSpeed()
    {
        if ((speedActu < targetSpeed))
        {
            speedActu += accelSpeed * Time.deltaTime;
        }
        else
        {
            speedActu = targetSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //speedActu /= 1.5f;
        rb.AddForce(collision.contacts[0].normal * 10);
        GameObject choc = Instantiate(ChocVFX, collision.contacts[0].point,Quaternion.identity);
        choc.transform.LookAt(choc.transform.position + collision.contacts[1].normal );
    }
}
