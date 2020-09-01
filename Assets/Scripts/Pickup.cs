using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType { Speed, Bullet }

public class Pickup : MonoBehaviour
{
    public Material[] pickupColour;
    public PickupType pickupType;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        

        float rand = Random.value;

        if(rand <= 0.25f)
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
                GetComponentInChildren<MeshRenderer>().material = pickupColour[0];
                break;
            case 1:
                GetComponentInChildren<MeshRenderer>().material = pickupColour[1];
                break;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
