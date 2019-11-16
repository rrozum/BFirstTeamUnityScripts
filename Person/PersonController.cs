using System;
using UnityEngine;

namespace Person
{
    public class PersonController : MonoBehaviour
    {
        public float moveSpeed = 100f;
        public Rigidbody2D rb;

        private Vector3 _movement;

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement.Normalize();
            Vector3 vector = _movement * moveSpeed;
            rb.AddForce(vector * Time.deltaTime);
        }
    }
}