using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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
    public float mouseSpeed;
    private Vector3 moveDirection;
    private float buffer;
    private float LRF;
    private float isAccelerating;
    private Rigidbody rb;

    public GameObject cursor;
    public GameObject selected;
    public float speedActu = 0;
    public int rotationAmount;
    public List<GameObject> instantiedTiles;
    public Vector2 mouseDirection;
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
        cursor.transform.position = new Vector3(12, 25, 3);
    }

    // Update is called once per frame
    void Update()    {
        
        rb.velocity = moveDirection * speedActu;
        Turning(LRF);

        moveDirection = Vector3.RotateTowards(moveDirection, transform.forward, driftCatchSpeed * Time.deltaTime, 0.0f);
        if (Vector3.Angle(transform.forward, moveDirection) >= maxDriftAngle)
        {
            moveDirection = Quaternion.AngleAxis(maxDriftAngle * -LRF, Vector3.up) * transform.forward;
            Debug.Log("OULA, CA DRIFT LA NAN ?");
        }

        ActualiseSpeed();

        buffer -= Time.deltaTime;


        if(multiplayerManager.tuileManager.isPosing && GetComponent<PlayerInput>().currentActionMap.name != "Vide")
        {
            PreviewTilePlayer();
        }
        cursor.transform.position += new Vector3(mouseDirection.x, 0, mouseDirection.y) * Time.deltaTime * mouseSpeed;
    
}


    void PreviewTilePlayer()
    {
        
        RaycastHit hit = CastRay();
        if(hit.collider && hit.collider!=null)
        {

            foreach (GameObject go in instantiedTiles)
            {
                Destroy(go);
            }
            instantiedTiles.Clear();
            foreach (Transform t in selected.transform)
            {

                GameObject go = Instantiate(t.gameObject, hit.transform.position + Vector3.up * 0.1f + t.localPosition, t.rotation);
                go.transform.RotateAround(hit.collider.transform.position, Vector3.up, 90 * rotationAmount);
                instantiedTiles.Add(go);
            }
        }
    }
    public void Turning(float LR)
    {
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

    public void MoveCursor(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.phase);
        if (ctx.phase == InputActionPhase.Performed)
        {
            mouseDirection = ctx.ReadValue<Vector2>();
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            mouseDirection = Vector2.zero;  // Réinitialise la direction du mouvement
        }

    }

    public void SendVictory(InputAction.CallbackContext ctx)
    {
        multiplayerManager.TPBack();
        speedActu = 0;
        rb.velocity = Vector3.zero;
        multiplayerManager.StartRaceM();

    }

    public void RotateSelected(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            rotationAmount++;
            if (rotationAmount > 3)
            {
                rotationAmount = 0;
            }
        }
    }

    public void Select()
    {
        RaycastHit hit = CastRay();
        if(hit.collider != null)
        { 
        if (hit.collider.tag != "panel")
        {
            selected = hit.collider.transform.parent.gameObject;
            hit.collider.transform.parent.position = new Vector3(-10, -10, -10);
            foreach (Transform t in hit.collider.transform.parent)
            {
                t.gameObject.GetComponent<Collider>().enabled = false;
            }
            GetComponent<PlayerInput>().SwitchCurrentActionMap("Vide");
            cursor.SetActive(false);
        }
        }
    }
    
    public void ApllyTile()
    {
        RaycastHit hit = CastRay();
        if (hit.collider != null)
        {
            multiplayerManager.tuileManager.Apply(instantiedTiles, hit.transform.parent);
            GetComponent<PlayerInput>().SwitchCurrentActionMap("Vide");
            Destroy(selected);
            selected = null;
        }

    }

    private RaycastHit CastRay()//permet de tirer un rayon la ou le joueur clic peut importe la direction de la camera
    {
       
        //tir du raycast
        RaycastHit hit;
        Physics.Raycast(cursor.transform.position,-Vector3.up*20, out hit);
        
        return hit;
    }
}
