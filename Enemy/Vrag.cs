using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vrag : MonoBehaviour
{
    public Transform P;
    Vector3 naprav;
    public float speed = 1;
    bool move = true;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        naprav = NextNaprav(P.position);
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if(move)
            transform.Translate(naprav * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && P != null)
        {
            StartCoroutine(Wait());
            move = false;

        }
        else if (collision.transform == P)
            Destroy(P.gameObject);
    }
    Vector3 NextNaprav(Vector3 x)
    {
        return (x - transform.position).normalized;
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
        naprav = NextNaprav(P.position);
        move = true;
    }
}