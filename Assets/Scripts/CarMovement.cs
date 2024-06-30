using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    public CarInput carInput;
    private Vector3 moveDirection;
    public float driftCatchSpeed;
    public float maxDriftAngle;
    private bool left;
    private bool right;
    private bool down;
    private float LRF;
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
    }

    // Update is called once per frame
    void Update()
    {
        LRF = carInput.Car.Turn.ReadValue<float>();
        if (left && down)
        { LRF = -1; }

        if (right && down)
        { LRF = 1; }

       
        transform.position += moveDirection * speed * Time.deltaTime;
        Turning(LRF);
        moveDirection = Vector3.RotateTowards(moveDirection, transform.forward, driftCatchSpeed*Time.deltaTime, 0.0f);
        if (Vector3.Angle(transform.forward, moveDirection) >= maxDriftAngle)
        {
            moveDirection = Quaternion.AngleAxis(maxDriftAngle * -LRF, Vector3.up) * transform.forward;
            Debug.Log("OULA, CA DRIFT LA NAN ?");
        }

        


    }

    public void lefting(bool tr)
    {
        left = tr;
    }

  
    public void righting(bool tr)
    {
        right = tr;
    }
    public void downing(bool tr)
    {
        down = tr;
    }
       

    void CheckMovement()
    {
        //Collider[] touchingMovement = Physics.OverlapBox( , transform.forward); 
    }

    void Turning(float LR)
    {
            transform.Rotate(new Vector3(0, LR * turnSpeed * Time.deltaTime, 0));
            //moveDirection = Quaternion.AngleAxis(LR * turnSpeed * Time.deltaTime, Vector3.up) * moveDirection;


    }

}
