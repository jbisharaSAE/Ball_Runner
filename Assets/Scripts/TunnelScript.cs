using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TunnelScript : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;
    [SerializeField] private GameObject[] pickupItems;
    private int arrayLength;

    // Start is called before the first frame update
    void Start()
    {
        if (Time.time > 185f)
            Destroy(gameObject);

       

        for (int i = 0; i < walls.Length; i++)
        {
            float rand = Random.value;
            float randomFloat = Random.value;

            if (Time.time < 90f)
            {
                if (rand < 0.5f)
                {
                    walls[i].SetActive(true);

                }

                if (randomFloat < 0.25f)
                {
                    pickupItems[i].SetActive(true);
                }
            }
            else
            {
                if (rand < 0.85f)
                {
                    walls[i].SetActive(true);

                }

                if (randomFloat < 0.5f)
                {
                    pickupItems[i].SetActive(true);
                }
            }
            
        }
        
    }


}
