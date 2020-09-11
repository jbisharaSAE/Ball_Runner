using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject explosionPrefab;
    public GameObject modelBullet;
    public AudioClip wallHitExplosion;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
            
            GetComponent<SphereCollider>().enabled = false;
            modelBullet.SetActive(false);
            audioSource.PlayOneShot(wallHitExplosion);
            Destroy(other.gameObject);
            Destroy(gameObject, 1f);
            

            if (explosionPrefab != null)
            {
                GameObject obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(obj, 0.5f);
            }
                
        }
        
    }
}
