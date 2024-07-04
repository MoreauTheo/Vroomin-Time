using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{/*
    private GameObject hitbox;
    public float speed = 15;
    public float accelSpeed = 15;
    private float targetSpeed;
    public float turnSpeed = 200;
    public CarInput carInput;
    private Vector3 moveDirection;
    public float driftCatchSpeed = 2.25f;
    public float maxDriftAngle = 65;
    private bool left;
    private bool right;
    private bool down;
    private float LRF;
    private enum Coordone{X,Y };
    public float bounceFallback = 3;

    [Header("DEBUG")]
    public bool isWalled;
    public bool wasWalled;

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
        hitbox = GameObject.Find("Hitbox");
        targetSpeed = speed;
        
    }

    // Update is called once per frame
    void Update()
    {
        LRF = carInput.Car.Turn.ReadValue<float>();
        if (left && down)
        { LRF = -1; }
        if (right && down)
        { LRF = 1; }
        //tout le bloc ci dessus sert à check les inputs tactiles pour le téléphone


        ActualiseSpeed();//permet de d'augmenter graduellement la vitesse 


        isWalled = false;
        CheckMovement(Coordone.X);
        CheckMovement(Coordone.Y);
        //permet de slide sur les murs sans passer à traver

        if (isWalled && !wasWalled)
        {
            Bump();
        }
        //si le joueur viens de rentrer dans un mur, il est bump


        Turning(LRF);
        //faire tourner le joueur

        moveDirection = Vector3.RotateTowards(moveDirection, transform.forward, driftCatchSpeed*Time.deltaTime, 0.0f);
        if (Vector3.Angle(transform.forward, moveDirection) >= maxDriftAngle)
        {
            moveDirection = Quaternion.AngleAxis(maxDriftAngle * -LRF, Vector3.up) * transform.forward;
            Debug.Log("OULA, CA DRIFT LA NAN ?");
        }
        //permet au drift de ne pas avoir trop d'écart avec l'orientation de la voiture

        
        wasWalled = isWalled;
        

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

        singleAxisVectorPreview *= speedActu*Time.deltaTime;

        Collider[] touchingMovement = Physics.OverlapBox(hitbox.transform.position+singleAxisVectorPreview, hitbox.transform.lossyScale/2, hitbox.transform.rotation);
        bool isWalledTemporary = false;
        foreach(Collider obstacle in touchingMovement)
        {
            if(obstacle.gameObject.tag == "wall")
            {
                isWalledTemporary = true;
                isWalled = true;

            }
        }

       

        if(!isWalledTemporary)
        {
            transform.position += singleAxisVectorPreview;

        }

        //MoveBounceBack();
    }

    void Turning(float LR)
    {
            transform.Rotate(new Vector3(0, LR * turnSpeed * Time.deltaTime, 0));
          


    }

    void ActualiseSpeed()
    {
        if((speedActu < targetSpeed) )
        {
            speedActu += accelSpeed * Time.deltaTime;
        }
        else
        {
            speedActu = targetSpeed;
        }
    }
  
    void Bump()
    {
        Debug.Log("Bump");

    }*/
}
