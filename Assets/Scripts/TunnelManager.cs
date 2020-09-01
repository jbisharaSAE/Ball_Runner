using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{
    public Transform[] tunnelObjects;

    [SerializeField] private float increment;
    
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject tunnelPiecePrefab;


    private void Awake()
    {
        //for(int i = 0; i < tunnelObjects.Length; ++i)
        //{
        //    tunnelObjects[i].localPosition = new Vector3(tunnelObjects[i].position.x, tunnelObjects[i].position.y, (tunnelObjects[i].position.z + increment));
        //    increment += 30f;
        //}
    }

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
        increment += 30f;
        spawnLocation.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y, increment);
        Instantiate(tunnelPiecePrefab, spawnLocation.position, tunnelPiecePrefab.transform.rotation);
    }

    
}
