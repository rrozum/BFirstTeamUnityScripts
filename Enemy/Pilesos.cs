using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilesos : MonoBehaviour
{
    Transform Player;
    Vector3 naprav;
    public float speed = 1f;
    bool move = true;
    public float waitT = 1f;
    void Start()
    {
        Player = GameObject.Find("Player").transform; 
        naprav = (Player.position - transform.position).normalized;
    }
    void Update()
    {
        if (move)
            transform.Translate(naprav * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && Player != null)
        {
            StartCoroutine(Wait(waitT));
            move = false;
        }
        else if (collision.transform == Player)
            Destroy(Player.gameObject);
    }
    IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
        naprav = (Player.position - transform.position).normalized;
        move = true;
    }
}