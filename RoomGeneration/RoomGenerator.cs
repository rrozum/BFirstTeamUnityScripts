using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomGenerator : MonoBehaviour
{
    
    public GameObject Floor;
    public GameObject Door;
    public GameObject Wall;
    public GameObject WallJoin;
    //RoomTileMap ThisRoom;

    public int MinTile;
    public int MaxTile;

    public string PrevWallSide = "DownWall";

    void Start()
    {
        genRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void genRoom()
    {
        GameObject room = new GameObject();
        room.transform.parent = transform;
        room.name = "room";
        room.transform.position = transform.position;
        room.AddComponent<RoomTileMap>();
        RoomTileMap rtm = room.GetComponent<RoomTileMap>();
        PrevWallSide = rtm.nextSide;
    }
}
