using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    public GameObject collider;
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
    private enum Coordone{X,Y };
    private Vector3 bounceDirection;
    public float bounceFallback;
    public float bounceForce;


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
        CheckMovement(Coordone.X);
        CheckMovement(Coordone.Y);


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
       

    void CheckMovement(Coordone co)
    {
        Vector3 singleAxisVectorPreview = Vector3.zero;
        if (co == Coordone.X)
        {
            singleAxisVectorPreview = new Vector3(moveDirection.x, 0, 0);
        }
        else
        {
            singleAxisVectorPreview = new Vector3(0, 0, moveDirection.z);
        }

        singleAxisVectorPreview *= speed*Time.deltaTime;

        Collider[] touchingMovement = Physics.OverlapBox( collider.transform.position+singleAxisVectorPreview, collider.transform.lossyScale/2, collider.transform.rotation);
        bool isWall = false;
        foreach(Collider obstacle in touchingMovement)
        {
            if(obstacle.gameObject.tag == "wall")
            {
                isWall = true;
            }
        }
        if(!isWall)
        {
            transform.position += singleAxisVectorPreview;
        }
        else
        {
            CalculBounceBack();
        }
        MoveBounceBack();
    }

    void Turning(float LR)
    {
            transform.Rotate(new Vector3(0, LR * turnSpeed * Time.deltaTime, 0));
            //moveDirection = Quaternion.AngleAxis(LR * turnSpeed * Time.deltaTime, Vector3.up) * moveDirection;


    }

    void CalculBounceBack()
    {
        bounceDirection = -moveDirection;
    }

    void MoveBounceBack()
    {
        transform.position += bounceDirection *bounceForce* Time.deltaTime;
        bounceDirection -= bounceDirection/bounceFallback *Time.deltaTime;
        





    }

}
