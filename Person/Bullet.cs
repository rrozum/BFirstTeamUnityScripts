using System;
using UnityEngine;

namespace Person
{
    public class Bullet : MonoBehaviour
    {
        private Collider2D _collider;
        private void Start()
        {
            _collider = gameObject.GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}