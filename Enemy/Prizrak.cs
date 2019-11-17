using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prizrak : MonoBehaviour
{
    public Transform P;
    public float speed = 1;
    void Start()
    {
        
    }
    void Update()
    {
        if(P!=null)
            transform.Translate((P.position - transform.position).normalized * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == P)
            Destroy(P.gameObject);
    }
}
