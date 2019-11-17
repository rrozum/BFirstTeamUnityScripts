using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoomDoor : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.tag == "Finish")
        {
            sNextRoomGen.NextRoom();
        }
    }
}
