using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovementPhysics : MonoBehaviour
{
    public float speed = 15;
    public float accelSpeed = 15;
    public float deccelSpeed = 15;
    public float turnSpeed;

    public float driftCatchSpeed = 2.25f;
    public float maxDriftAngle = 65;
    private float targetSpeed;
    public GameObject ChocVFX;

    public float bufferTiming;
    public float speedLost;
    public CarInput carInput;
    public MultiplayerManager multiplayerManager;

    private Vector3 moveDirection;
    private float buffer;
    private float LRF;
    private float isAccelerating;
    private Rigidbody rb;

    public float speedActu = 0;
    private void Awake()
    {
        carInput = new CarInput();
        multiplayerManager = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>();
        multiplayerManager.players.Add(gameObject);
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
    void Update()    {
        
        rb.linearVelocity = moveDirection * speedActu;
        Turning(LRF);

        moveDirection = Vector3.RotateTowards(moveDirection, transform.forward, driftCatchSpeed * Time.deltaTime, 0.0f);
        if (Vector3.Angle(transform.forward, moveDirection) >= maxDriftAngle)
        {
            moveDirection = Quaternion.AngleAxis(maxDriftAngle * -LRF, Vector3.up) * transform.forward;
            Debug.Log("OULA, CA DRIFT LA NAN ?");
        }

        ActualiseSpeed();

        buffer -= Time.deltaTime;

    }
    public void Turning(float LR)
    {
        Debug.Log("oui");
        transform.Rotate(new Vector3(0, LR * turnSpeed * Time.deltaTime, 0));
    }

   public void SetLRF(InputAction.CallbackContext ctx)
    {
        LRF = ctx.ReadValue<float>();
    }

    public void SetBoolAccel(InputAction.CallbackContext ctx)
    {
        isAccelerating = ctx.ReadValue<float>();
    }
    void ActualiseSpeed()
    {
        if (isAccelerating >= 1)
        {
            if (speedActu < targetSpeed)
                speedActu += accelSpeed * Time.deltaTime;
            else
                speedActu = speed;
        }
        else
        {
            if(speedActu > 0)
                speedActu -= deccelSpeed * Time.deltaTime;
            else
                speedActu = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if(buffer <= 0)
        {
            speedActu -= speedActu / 100 * speedLost;
            rb.AddForce(collision.contacts[0].normal * 10);
            GameObject choc = Instantiate(ChocVFX, collision.contacts[0].point, Quaternion.identity);
            choc.transform.LookAt(choc.transform.position + collision.contacts[1].normal);
        }
        buffer = bufferTiming;

    }

    public void StartRace(InputAction.CallbackContext ctx)
    {
        multiplayerManager.StartRaceM();
    }

    private void OnTriggerEnter(Collider other)
    {
        multiplayerManager.Victory(this.gameObject);
    }

    public void SendVictory(InputAction.CallbackContext ctx)
    {
        multiplayerManager.TPBack();
        speedActu = 0;
        rb.linearVelocity = Vector3.zero;
        multiplayerManager.StartRaceM();

    }
}
