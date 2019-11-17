using System;
using UnityEngine;

namespace Person
{
    public class BrokenBullet : MonoBehaviour
    {
        public GameObject player;
        private Shooting _shooting;
        private void Start()
        {
            _shooting = GameObject.Find("Player").GetComponent<Shooting>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                _shooting.bulletNumber++;
                Destroy(gameObject);
            }
        }
    }
}