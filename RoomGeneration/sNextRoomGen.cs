using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sNextRoomGen : MonoBehaviour
{
    public static void NextRoom()
    {
        GameObject RoomGen = GameObject.Find("Generator");
        Destroy(RoomGen.transform.GetChild(0).gameObject);
        RoomGenerator rg = RoomGen.GetComponent<RoomGenerator>();
        rg.genRoom();
    }
}
