using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using Liminal.SDK.Core;
using Liminal.SDK.VR.Avatars;
using Liminal.Core.Fader;
using TMPro;


public class PlayerController : MonoBehaviour
{
    private int m_currentIndex;
    private int index = 1;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool isBoosting = false;
    private bool isEnded = false;
    private int bulletCounter = 3;
    private int comboMultiplier;
    private float scoreTimer;
    private float scorePoints;
    private Vector3[] wpLocations;
    private AudioSource myAudioSource;


    public float endTime = 175f;
    public Transform[] wayPoints;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject lightningState;
    [SerializeField] private TunnelManager tunnelScript;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;

    [Header("End Game Objects")]
    [SerializeField] private GameObject fantasySun;
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private GameObject flameJet;
    [SerializeField] private GameObject[] fireworks;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboX;
    [SerializeField] private Image panelImage;
    [SerializeField] private Image []bulletImages;

    [Header("Audio SFX")]
    [SerializeField] private AudioClip gunFireSFX;
    [SerializeField] private AudioClip speedBoostSFX;
    [SerializeField] private AudioClip wallHitSFX;
    [SerializeField] private AudioClip bulletPickupSFX;

    public int currentIndex { get { return m_currentIndex; } set { m_currentIndex = value; } }

    // Start is called before the first frame update
    void Start()
    {   
        // middle waypoint
        m_currentIndex = 1;

        myAudioSource = GetComponent<AudioSource>();

        wpLocations = new Vector3[3];
    }



    // Update is called once per frame
    void Update()
    {
        if (isEnded)
            return;

        TrackingTime();

        Movement();

        Scoreboard();

        VrController();

    }

    private void PlaySFX(AudioClip clip)
    {
        myAudioSource.PlayOneShot(clip);
    }

    private void TrackingTime()
    {
       if(Time.time > endTime)
        {
            fantasySun.transform.parent = null;
            
        }
    }

    private void Scoreboard()
    {
        scoreTimer += Time.deltaTime;
        scorePoints += Time.deltaTime * comboMultiplier * 10f;
        scoreText.text = scorePoints.ToString("F0");

        if(scoreTimer > 0f && scoreTimer < 5f)
        {
            comboText.text = "Combo      1";
            comboMultiplier = 1;
        }
        else if (scoreTimer >= 5f && scoreTimer < 10f)
        {
            comboText.text = "Combo      2";
            comboMultiplier = 2;
        }
        else if (scoreTimer >= 10f && scoreTimer < 15f)
        {
            comboText.text = "Combo      4";
            comboMultiplier = 4;
        }
        else if (scoreTimer >= 15f && scoreTimer < 20f)
        {
            comboText.text = "Combo      8";
            comboMultiplier = 8;
        }

    }

    private void Movement()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);

        for (int i = 0; i < 3; ++i)
        {
            wpLocations[i] = wayPoints[i].position;
        }

        if (isMoving)
            MoveToWayPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TunnelTrigger")
        {
            
            if(Time.time < endTime)
                tunnelScript.SendMessage("ShiftTunnel");

            DestroyTunnel(other.gameObject.transform.parent.gameObject);
        }
     
        else if (other.gameObject.tag == "Wall")
        {
            
            if (!isBoosting)
            {
                comboMultiplier = 1;
                scoreTimer = 1f;
            }

            PlaySFX(wallHitSFX);
            other.gameObject.GetComponent<Wall>().SendMessage("ExplodeWall");
            
        }
        else if(other.gameObject.tag == "Pickup")
        {
            
            ActivatePickup(other.gameObject.GetComponent<Pickup>().pickupType);
            //other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Destroy(other.gameObject);
            
        }
        else if(other.gameObject.tag == "Sun")
        {
            Debug.Log("Sun Triggered");
            EndGame();
        }

    }

    private void ActivatePickup(PickupType item)
    {
        
        int myInt = (int)item;

        switch (myInt)
        {
            case 0:
                // boost speed and immune function
                StopAllCoroutines();
                StartCoroutine(BoostSpeed());
                break;
            case 1:

                PlaySFX(bulletPickupSFX);
                ++bulletCounter;

                if(bulletCounter >= 4)
                {
                    bulletCounter = 3;
                }

                UpdateBulletImages();
                break;
        }
    }

    IEnumerator BoostSpeed()
    {
        isBoosting = true;
        lightningState.SetActive(true);
        speed = 45f;
        PlaySFX(speedBoostSFX);
        yield return new WaitForSeconds(4f);

        lightningState.SetActive(false);
        speed = 15f;
        isBoosting = false;
    }

    private void UpdateBulletImages()
    {
        for(int i = 0; i< 4; ++i)
        {
            if(i <= bulletCounter)
            {
                bulletImages[i].enabled = true;
            }
            else
            {
                bulletImages[i].enabled = false;
            }
        }
        
    }

   

    private void DestroyTunnel(GameObject obj)
    {
        Destroy(obj, 5f);
    }

    private IEnumerator FadeToBlackRoutine()
    {
        yield return new WaitForSeconds(5f);
        ScreenFader.Instance.FadeTo(Color.black, duration: 1);
        yield return ScreenFader.Instance.WaitUntilFadeComplete();
        ScreenFader.Instance.FadeToClear(duration: 1);
    }

    private void EndGame()
    {
        // end the game
        isEnded = true;
        speed = 0f;

        foreach (GameObject obj in fireworks)
        {
            obj.transform.parent = null;
        }

        StartCoroutine(FadeToBlackRoutine());
        comboX.text = "";
        panelImage.enabled = false;
        fantasySun.SetActive(false);
        flameJet.SetActive(false);
        lightningState.SetActive(false);
        spaceShip.SetActive(false);
        comboText.text = "";
        
        foreach(Transform wp in wayPoints)
        {
            wp.transform.gameObject.SetActive(false);
        }

        foreach(GameObject obj in fireworks)
        {
            obj.SetActive(true);
        }

        foreach(Image img in bulletImages)
        {
            img.enabled = false;
        }

        GameObject[] tunnels;
        tunnels = GameObject.FindGameObjectsWithTag("Tunnel");

        foreach(GameObject obj in tunnels)
        {
            Destroy(obj);
        }
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
            //ShootProjectile();
        }

    }

    private void VrController()
    {
        // vr controls


        float zAngle = VRAvatar.Active.PrimaryHand.Transform.localEulerAngles.z;

        Debug.Log(zAngle);

        if (VRDevice.Device.GetButtonDown(VRButton.One))
        {
            ShootProjectile();
        }

        if(zAngle > 35f && zAngle < 90f)
        {
            // move left
            MoveLeft();
        }
        else if (zAngle > 180f && zAngle < 325f)
        {
            // move left
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
            UpdateBulletImages();
            PlaySFX(gunFireSFX);

        }
        if(bulletCounter < 0)
        {
            // to make sure player does not keep spamming trigger
            bulletCounter = -1;
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
