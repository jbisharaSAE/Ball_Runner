using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType { Speed, Bullet }

public class Pickup : MonoBehaviour
{
    public Material[] pickupColour;
    //public GameObject[] particleEffects;
    public PickupType pickupType;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        int randomZ = (Random.Range(0, 7));
        float posZ = (float)randomZ +transform.position.z;


        transform.position = new Vector3(transform.position.x, transform.position.y,  posZ);

        float rand = Random.value;

        if(rand <= 0.35f)
        {
            pickupType = PickupType.Speed;
        }
        else
        {
            pickupType = PickupType.Bullet;
        }

        int integer = (int)pickupType;

        switch (integer)
        {
            case 0:
                GetComponentsInChildren<MeshRenderer>()[0].material = pickupColour[1];
                GetComponentsInChildren<MeshRenderer>()[1].material = pickupColour[1];
                //particleEffects[0].SetActive(true);
                break;
            case 1:
                GetComponentsInChildren<MeshRenderer>()[0].material = pickupColour[0];
                GetComponentsInChildren<MeshRenderer>()[1].material = pickupColour[0];
                //particleEffects[1].SetActive(true);
                break;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
