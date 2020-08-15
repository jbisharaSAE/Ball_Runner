using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offSet;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position + offSet;
    }
}
