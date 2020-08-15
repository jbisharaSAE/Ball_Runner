using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{
    public Transform[] tunnelObjects;

    private float increment;
    
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject tunnelPiecePrefab;

    // Start is called before the first frame update
    void Start()
    {
        increment = spawnLocation.position.z;
        //tunnelObjects = GetComponentsInChildren<Transform>();
        //Debug.Log(tunnelObjects.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShiftTunnel()
    {
        increment += 12f;
        spawnLocation.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y, increment);
        Instantiate(tunnelPiecePrefab, spawnLocation.position, tunnelPiecePrefab.transform.rotation);
    }

    
}
