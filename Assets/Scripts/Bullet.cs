using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject explosionPrefab;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wall")
        {
            Debug.Log("bullet hit wall");
            Destroy(other.gameObject);
            Destroy(gameObject);

            if (explosionPrefab != null)
                Instantiate(explosionPrefab, other.gameObject.transform.position, Quaternion.identity);
        }
        
    }
}
