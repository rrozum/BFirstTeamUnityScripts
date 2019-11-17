using System;
using UnityEngine;

namespace Person
{
    public class Bullet : MonoBehaviour
    {
        public GameObject brokenBulletPrefab;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.gameObject.CompareTag("Player"))
            {
                Instantiate(brokenBulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}