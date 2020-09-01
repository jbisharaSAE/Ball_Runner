using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float explosionForce;
    [SerializeField] private float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    

    public void ExplodeWall()
    {
        rb.isKinematic = false;
        rb.AddExplosionForce(explosionForce, transform.position, 5.0f, 3.0f, ForceMode.Impulse);

        float x = Random.Range(-1, 1);
        float y = Random.Range(-1, 1);
        float z = Random.Range(-1, 1);

        Vector3 rotVector = new Vector3(x, y, z);

        rotVector.Normalize();

        transform.Rotate(rotVector * rotateSpeed * Time.deltaTime);

        Destroy(gameObject, 4f);
    }
}
