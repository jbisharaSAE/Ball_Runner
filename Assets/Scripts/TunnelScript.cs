using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TunnelScript : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            float rand = Random.value;

            if(rand < 0.5f)
            {
                walls[i].SetActive(true);
            }

        }
            
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
