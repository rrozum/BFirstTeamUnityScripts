using System;
using UnityEngine;

namespace Person
{
    public class PersonController : MonoBehaviour
    {
        public float moveSpeed = 100f;
        Rigidbody2D rb;
        public Direction direction = Direction.Up;
        private SpriteRenderer _sprite;
        public bool canMove = true;
        public enum Direction
        {
            Up = 1,
            Down = 2,
            Left = 3,
            Right = 4,
            UpLeft = 5,
            UpRight = 6,
            DownLeft = 7,
            DownRight = 8
        }

        private Vector3 _movement;

        private void Start()
        {
            _movement = EnumDirectionToVector2(direction);
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement.Normalize();
            direction = Vector2ToEnumDirection(_movement);
//            changeFirePointPosition();
            if (canMove)
            {
                Vector3 vector = _movement * moveSpeed;
                rb.AddForce(vector * Time.deltaTime);
            }
        }

        public Direction Vector2ToEnumDirection(Vector2 vector2)
        {
            Direction direction = new Direction();
            if (vector2.x > 0 && vector2.y > 0)
            {
                direction = Direction.UpRight;
            } else if (vector2.x < 0 && vector2.y < 0)
            {
                direction = Direction.DownLeft;
            } else if (vector2.x > 0 && vector2.y < 0)
            {
                direction = Direction.DownRight;
            } else if (vector2.x < 0 && vector2.y > 0)
            {
                direction = Direction.UpLeft;
            } else if (vector2.x == 0 && vector2.y > 0)
            {
                direction = Direction.Up;
            } else if (vector2.x == 0 && vector2.y < 0)
            {
                direction = Direction.Down;
            } else if (vector2.x > 0 && vector2.y == 0)
            {
                direction = Direction.Right;
            } else if (vector2.x < 0 && vector2.y == 0)
            {
                direction = Direction.Left;
            }
            else
            {
                direction = this.direction;
            }

            return direction;
        }

        public Vector2 EnumDirectionToVector2(Direction direction)
        {
            Vector2 vector2Direction = transform.position;
            switch (direction)
            {
                case Direction.Up:
                    vector2Direction = Vector2.up;
                    break;
                case Direction.Down:
                    vector2Direction = Vector2.down;
                    break;
                case Direction.Left:
                    vector2Direction = Vector2.left;
                    break;
                case Direction.Right:
                    vector2Direction = Vector2.right;
                    break;
                case Direction.DownLeft:
                    vector2Direction = new Vector2(-1, -1);
                    break;
                case Direction.DownRight:
                    vector2Direction = new Vector2(1, -1);
                    break;
                case Direction.UpLeft:
                    vector2Direction = new Vector2(-1, 1);
                    break;
                case Direction.UpRight:
                    vector2Direction = new Vector2(1, 1);
                    break;
            }

            vector2Direction = vector2Direction.normalized;

            return vector2Direction;
        }

        private void changeFirePointPosition()
        {
            Transform firePointLocalPosition = transform.Find("FirePoint");

            switch (direction)
            {
                case Direction.Up:
                    
                    break;
                case Direction.Down:
                    firePointLocalPosition.localPosition = new Vector2(0.41f, -2.69f);
                    break;
                case Direction.Left:
                    break;
                case Direction.Right:
                    break;
                case Direction.DownLeft:
                    break;
                case Direction.DownRight:
                    break;
                case Direction.UpLeft:
                    break;
                case Direction.UpRight:
                    break;
            }
        }
    }
}