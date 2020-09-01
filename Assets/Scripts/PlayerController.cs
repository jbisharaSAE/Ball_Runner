using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;


public class PlayerController : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    private int m_currentIndex;
    private int index = 1;
    private bool isMoving = false;
    private bool isJumping = false;
    private int bulletCounter = 3;
    private Vector3[] wpLocations;

    public Transform[] wayPoints;

    [SerializeField] private Transform rightHandAnchor;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject lightningState;
    [SerializeField] private TunnelManager tunnelScript;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_Offset;
    [SerializeField] private float m_Time = 0f;



    public int currentIndex { get { return m_currentIndex; } set { m_currentIndex = value; } }

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();

        // middle waypoint
        m_currentIndex = 1;

        wpLocations = new Vector3[3];
    }



    // Update is called once per frame
    void Update()
    {

        //m_rigidBody.velocity = Vector3.forward * speed;

        Jump();

        for (int i = 0; i < 3; ++i)
        {
            wpLocations[i] = wayPoints[i].position;
        }

        if (isMoving)
            MoveToWayPoint();



#if UNITY_EDITOR
        {
            // use pc controls
            PcController();
        }
#else
        {
            // use vr controls
            VrController();
        }
#endif


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TunnelTrigger")
        {
            Debug.Log("testing tunnel trigger");
            tunnelScript.SendMessage("ShiftTunnel");
            DestroyTunnel(other.gameObject.transform.parent.gameObject);
        }
        else if (other.gameObject.tag == "JumpTrigger")
        {
            
            isJumping = false;
            ResetPos();
        }
        else if (other.gameObject.tag == "Wall")
        {
            //other.gameObject.AddComponent<Rigidbody>();
            //other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, other.gameObject.transform.position, 5.0f, 3.0f, ForceMode.Impulse);
            //GameObject parentObj = other.transform.parent.gameObject;
            other.gameObject.GetComponent<Wall>().SendMessage("ExplodeWall");
            
            
        }
        else if(other.gameObject.tag == "Pickup")
        {
            Debug.Log("testing Trigger");
            ActivatePickup(other.gameObject.transform.parent.gameObject.GetComponent<Pickup>().pickupType);
            //other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Destroy(other.gameObject);
            

        }

    }

    private void ActivatePickup(PickupType item)
    {
        Debug.Log("testing function");
        int myInt = (int)item;
        switch (myInt)
        {
            case 0:
                // boost speed and immune function
                StartCoroutine(BoostSpeed());
                break;
            case 1:
                ++bulletCounter;
                break;
        }
    }

    IEnumerator BoostSpeed()
    {
        lightningState.SetActive(true);
        speed = 45f;
        yield return new WaitForSeconds(4f);
        lightningState.SetActive(false);
        speed = 15f;
    }

    private void ResetPos()
    {
        transform.position = new Vector3(transform.position.x, -1.2f, transform.position.z);

        //switch (currentIndex)
        //{
        //    case 1:
        //        transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        //        break;
        //    default:
        //        transform.position = new Vector3(transform.position.x, -1.1f, transform.position.z);
        //        break;
        //}
    }


    private void DestroyTunnel(GameObject obj)
    {
        Destroy(obj, 3f);
    }

    private void PcController()
    {
        // comparing player location to waypoint location
        // checking what direction the player is turning
        // need to check if


        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            MoveLeft();
        }
        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            MoveRight();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            ShootProjectile();
        }

    }

    private void VrController()
    {
        // vr controls

        //if(OVRInput.GetLocalControllerRotation)

        if (VRDevice.Device.GetButtonDown(VRButton.One))
        {
            ShootProjectile();
        }

        if (rightHandAnchor.position.z > 45f)
        {
            //move left
            MoveLeft();
        }
        else if (rightHandAnchor.position.z < -45f)
        {
            //move right
            MoveRight();
        }
    }

    private void MoveLeft()
    {
        isMoving = true;

        switch (m_currentIndex)
        {
            case 0:
                // move to left waypoint
                index = 0;
                break;
            case 1:
                // move to left waypoint
                index = 0;
                break;
            case 2:
                // move to middle waypoint
                index = 1;
                break;
        }

    }

    private void MoveRight()
    {
        isMoving = true;

        switch (m_currentIndex)
        {
            case 0:
                // move to middle waypoint
                index = 1;
                break;
            case 1:
                // move to right waypoint
                index = 2;
                break;
            case 2:
                // move to right waypoint
                index = 2;
                break;
        }
    }

    private void ShootProjectile()
    {
        if(bulletCounter >= 0)
        {
            Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
            --bulletCounter;
        }
        
    }

    private void Jump()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);


        //transform.position = Vector3.MoveTowards(transform.position, targetLoc, turnSpeed * Time.deltaTime);
        if (Input.GetButton("Jump"))
            isJumping = true;

        if (isJumping)
        {
            m_Time += m_JumpSpeed * Time.deltaTime;

            Vector3 position = transform.localPosition;
            position.y = (-1.5f) + Mathf.PingPong(m_Time, 1.5f);
            transform.localPosition = position;
        }
    }


    private void MoveToWayPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, wpLocations[index], turnSpeed * Time.deltaTime);
        float distance = Vector3.Distance(gameObject.transform.position, wpLocations[index]);

        switch (currentIndex)
        {
            case 0:
                transform.right = Vector3.RotateTowards(transform.right, wayPoints[0].right, turnSpeed * Time.deltaTime, 1.0f);
                break;
            case 1:
                transform.right = Vector3.RotateTowards(transform.right, wayPoints[1].right, turnSpeed * Time.deltaTime, 1.0f);
                break;
            case 2:
                transform.right = Vector3.RotateTowards(transform.right, wayPoints[2].right, turnSpeed*Time.deltaTime, 1.0f);
                break;
        }

        if(distance < 0.05f)
        {
            isMoving = false;
        }
    }

   

}
