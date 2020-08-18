using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointInfo : MonoBehaviour
{
    public int wayPointNumber;

    [SerializeField] Transform player;

    private void Start()
    {
        //transform.parent = null;    
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z);
    }


}
