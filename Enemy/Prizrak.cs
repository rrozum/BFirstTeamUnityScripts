using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prizrak : MonoBehaviour
{
    Transform Player;
    public float speed = 1;
    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        if(Player!=null)
            transform.Translate((Player.position - transform.position).normalized * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == Player)
            Destroy(Player.gameObject);
    }
}
