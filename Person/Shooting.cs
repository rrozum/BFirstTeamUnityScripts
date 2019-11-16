using System;
using System.Collections;
using UnityEngine;

namespace Person
{
    public class Shooting : MonoBehaviour
    {
        public Transform firePoint;
        public GameObject bulletPrefab;
        public float bulletForce = 20f;
        public Camera camera;

        private PersonController _personController;
        private IEnumerator _coroutine;

        private void Start()
        {
            _personController = GetComponent<PersonController>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (_personController.canMove)
                {
                    _coroutine = WaitAndEnableMove(1.0f);
                    StartCoroutine(_coroutine);
                }
                DisableMove();
            } else if (Input.GetButtonUp("Fire1"))
            {
                if (!_personController.canMove)
                {
                    Shoot();
                }
            }
        }

        private void DisableMove()
        {
            _personController.canMove = false;
        }

        private void EnableMove()
        {
            _personController.canMove = true;
        }

        private IEnumerator WaitAndEnableMove(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            EnableMove();
        }

        private void Shoot()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            PersonController.Direction direction = _personController.direction;
            Vector2 mouseDirection = getDirectionFromMouse();
            rb.AddForce(mouseDirection * bulletForce, ForceMode2D.Impulse);
        }

        private Vector2 getDirectionFromMouse()
        {
            Vector2 directionForMouse = new Vector2(0, 0);
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseVector2 = mousePosition - (Vector2)transform.position;
            foreach (var direction in Enum.GetValues(typeof(PersonController.Direction)))
            {
                Vector2 vector2Direction = 
                    _personController.EnumDirectionToVector2((PersonController.Direction)direction);
                float angle = Vector2.Angle(mouseVector2, vector2Direction);
                if (angle <= 22.5f)
                {
                    directionForMouse = vector2Direction;
                    break;
                }
            }

            return directionForMouse;
        }
    }
}