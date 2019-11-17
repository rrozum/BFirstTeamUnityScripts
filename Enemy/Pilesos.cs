using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilesos : MonoBehaviour
{
    public Transform P;
    Vector3 naprav;
    public float speed = 1f;
    bool move = true;
    public float waitT = 1f;
    void Start()
    {
        naprav = NextNaprav(P.position);
    }
    void Update()
    {
        if (move)
            transform.Translate(naprav * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && P != null)
        {
            StartCoroutine(Wait(waitT));
            move = false;
        }
        else if (collision.transform == P)
            Destroy(P.gameObject);
    }
    Vector3 NextNaprav(Vector3 x)
    {
        return (x - transform.position).normalized;
    }
    IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
        naprav = NextNaprav(P.position);
        move = true;
    }
}