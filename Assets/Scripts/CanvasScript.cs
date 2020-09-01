using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float zOffset;

    private void Start()
    {
        //distance = Vector3.Distance(transform.position, player.position);
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        transform.position = new Vector3(transform.position.x, transform.position.y, (player.position.z + zOffset));
    }
}

