using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    private Vector3 dir;
    private Vector3 targetLoc;
    private int currentIndex;
    private int arrayNumber;
    private bool isMoving = false;
    private bool isJumping = false;
    
    public Transform[] wayPoints;

    [SerializeField] private TunnelManager tunnelScript;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float jumpPower;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();

        // middle waypoint
        currentIndex = 1;

        targetLoc = wayPoints[1].position;

        


    }

    

    // Update is called once per frame
    void Update()
    {
        //m_rigidBody.velocity = Vector3.forward * speed;

        



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
        else if (other.gameObject.tag == "WayPoint")
        {
            //int tempIndex = other.gameObject.GetComponent<WayPointInfo>().wayPointNumber;

            currentIndex = other.gameObject.GetComponent<WayPointInfo>().wayPointNumber;
            Debug.Log("hit waypoint trigger " + currentIndex);
            
        }
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


        Jump();

        

        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            Debug.Log("left arrow pressed");


            switch (currentIndex)
            {
                case 0:
                    // move to left waypoint
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[0].position, turnSpeed * Time.deltaTime);
                    //targetLoc = wayPoints[0].position;
                    
                    break;
                case 1:
                    // move to left waypoint
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[0].position, turnSpeed * Time.deltaTime);
                    //targetLoc = wayPoints[0].position;
                    
                    break;
                case 2:
                    // move to middle waypoint
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[1].position, turnSpeed * Time.deltaTime);
                    //targetLoc = wayPoints[1].position;
                    
                    break;
            }


        }
        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            Debug.Log("right arrow pressed");

            switch (currentIndex)
            {
                case 0:
                    // move to middle waypoint
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[1].position, turnSpeed * Time.deltaTime);
                    //targetLoc = wayPoints[1].position;
                    break;
                case 1:
                    // move to right waypoint
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[2].position, turnSpeed * Time.deltaTime);
                    //targetLoc = wayPoints[2].position;
                    break;
                case 2:
                    // move to right waypoint
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[2].position, turnSpeed * Time.deltaTime);
                    //targetLoc = wayPoints[2].position;
                    break;
            }


        }
       
    }

    private void VrController()
    {

    }

    private void Jump()
    {


        //transform.position = Vector3.MoveTowards(transform.position, targetLoc, turnSpeed * Time.deltaTime);
        if (Input.GetButton("Jump"))
            isJumping = true;

        if (!isJumping)
        {
            // need to increase speed slowly over time - TODO
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            //transform.Translate(Vector3.forward * Mathf.Sin(3f) * speed * Time.deltaTime);
        }
    }


}
