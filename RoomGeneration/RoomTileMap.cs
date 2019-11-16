using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTileMap : MonoBehaviour
{
    GameObject floor;
    GameObject SpawnDoor, NextRoomDoor;
    GameObject wall;
    GameObject wallJoin;
    int minTiles;
    int maxTile;
    string PrevRoomSide;

    public string nextSide;


    void Awake()
    {
        RoomGenerator generator = transform.GetComponentInParent<RoomGenerator>();

        floor = generator.Floor;
        SpawnDoor = generator.Door;
        NextRoomDoor = generator.Door;
        wall = generator.Wall;
        wallJoin = generator.WallJoin;
        minTiles = generator.MinTile;
        maxTile = generator.MaxTile;
        PrevRoomSide = generator.PrevWallSide;


        Generate();
        nextSide = AddDoors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Generate()
    {

        var randWidth = Random.Range(minTiles / 2, maxTile / 2);
        var randHeight = Random.Range(minTiles / 2, maxTile / 2);

        GameObject FloorObj = new GameObject();
        FloorObj.name = "floor";
        FloorObj.transform.parent = transform;
        GameObject WallObj = new GameObject();
        WallObj.name = "wall";
        WallObj.transform.parent = transform;
        GameObject WallJoinObj = new GameObject();
        WallJoinObj.name = "wall_join";
        WallJoinObj.transform.parent = transform;

        GameObject LeftWall= new GameObject();
        LeftWall.name = "LeftWall";
        LeftWall.transform.parent = transform.GetChild(1);
        GameObject RightWall = new GameObject();
        RightWall.name = "RightWall";
        RightWall.transform.parent = transform.GetChild(1);
        GameObject UpWall = new GameObject();
        UpWall.name = "UpWall";
        UpWall.transform.parent = transform.GetChild(1);
        GameObject DownWall = new GameObject();
        DownWall.name = "DownWall";
        DownWall.transform.parent = transform.GetChild(1);

        for (int i = 0; i < randHeight; i++)
        {
            for (int j = 0; j < randWidth; j++)
            {
                GameObject tile = Instantiate(floor, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                tile.transform.SetParent(FloorObj.transform);

                if (i == 0)
                {
                    GameObject _wall = Instantiate(wall, new Vector3(i - 1, j, 0f), Quaternion.Euler(new Vector3(0, 0, wall.transform.eulerAngles.z + 90))) as GameObject;
                    _wall.transform.SetParent(LeftWall.transform);
                    _wall.name = "left wall";
                }

                //bottom wall
                if (j == 0)
                {
                    GameObject _wall = Instantiate(wall, new Vector3(i, j - 1, 0f), Quaternion.Euler(new Vector3(0, 0, wall.transform.eulerAngles.z + 180))) as GameObject;
                    _wall.transform.SetParent(DownWall.transform);
                }

                //rigth wall
                if (i == randHeight - 1)
                {
                    GameObject _wall = Instantiate(wall, new Vector3(i + 1, j, 0f), Quaternion.Euler(new Vector3(0, 0, wall.transform.eulerAngles.z - 90))) as GameObject;
                    _wall.transform.SetParent(RightWall.transform);
                }

                //up wall
                if (j == randWidth - 1)
                {
                    GameObject _wall = Instantiate(wall, new Vector3(i, j + 1, 0f), Quaternion.identity) as GameObject;
                    _wall.transform.SetParent(UpWall.transform);
                }

                //rigth up wall angle
                if (i == randHeight - 1 && j == randWidth - 1)
                {
                    GameObject _wall = Instantiate(wallJoin, new Vector3(i + 1, j + 1, 0f), Quaternion.identity) as GameObject;
                    _wall.transform.SetParent(WallJoinObj.transform);
                }

                //left up wall angle
                if (i == 0 && j == randWidth - 1)
                {
                    GameObject _wall = Instantiate(wallJoin, new Vector3(i - 1, j + 1, 0f), Quaternion.Euler(new Vector3(0, 0, wallJoin.transform.eulerAngles.z + 90))) as GameObject;
                    _wall.transform.SetParent(WallJoinObj.transform);
                }

                //rigth bottom wall angle
                if (i == randHeight - 1 && j == 0)
                {
                    GameObject _wall = Instantiate(wallJoin, new Vector3(i + 1, j - 1, 0f), Quaternion.Euler(new Vector3(0, 0, wallJoin.transform.eulerAngles.z - 90))) as GameObject;
                    _wall.transform.SetParent(WallJoinObj.transform);
                }

                // left bottom angle wall
                if (i == 0 && j == 0)
                {
                    GameObject _wall = Instantiate(wallJoin, new Vector3(i - 1, j - 1, 0f), Quaternion.Euler(new Vector3(0, 0, wallJoin.transform.eulerAngles.z + 180))) as GameObject;
                    _wall.transform.SetParent(WallJoinObj.transform);
                }
            }
        }
    }

    private string AddDoors()
    {
        Transform wallTransform;

        if (PrevRoomSide == "DownWall")
        {
            wallTransform = transform.GetChild(1).Find("UpWall");
        }
        else if (PrevRoomSide == "UpWall")
        {
            wallTransform = transform.GetChild(1).Find("DownWall");
        }
        else if (PrevRoomSide == "RightWall")
        {
            wallTransform = transform.GetChild(1).Find("LeftWall");
        }
        else
        {
            wallTransform = transform.GetChild(1).Find("RightWall");
        }

        int SpawnDoorIndex = Random.Range(0, wallTransform.childCount);
        int RandSide = Random.Range(0, 4);

        while (wallTransform.name == transform.GetChild(1).GetChild(RandSide).name)
        {
            RandSide = Random.Range(0, 4);
        }
        Transform nexRoomDoorSide = transform.GetChild(1).GetChild(RandSide);
        int NexRoomDoorIndex = Random.Range(0, nexRoomDoorSide.childCount);


        GameObject _wallToSpawnDoor = wallTransform.GetChild(SpawnDoorIndex).gameObject;
        GameObject _wallToNextRoomDoor = nexRoomDoorSide.GetChild(NexRoomDoorIndex).gameObject;

        GameObject _spawnDoor = Instantiate(SpawnDoor, _wallToSpawnDoor.transform.position, _wallToSpawnDoor.transform.rotation);
        GameObject _nextRoomDoor = Instantiate(NextRoomDoor, _wallToNextRoomDoor.transform.position, _wallToNextRoomDoor.transform.rotation);

        _spawnDoor.name = "SpawnDoor";
        _spawnDoor.transform.parent = transform.GetChild(1);
        _nextRoomDoor.name = "NexRoomDoor";
        _nextRoomDoor.transform.parent = transform.GetChild(1);

        Destroy(wallTransform.GetChild(SpawnDoorIndex).gameObject);
        Destroy(nexRoomDoorSide.GetChild(NexRoomDoorIndex).gameObject);

        return nexRoomDoorSide.name;
    }

    //private void ColliderGenerator()
    //{
    //    var collider = GetComponent<PolygonCollider2D>();
    //    Transform joinWalls = transform.GetChild(2);

    //    Vector2[] points = new Vector2[10];
    //    points[0] = new Vector2(joinWalls.GetChild(0).position.x, joinWalls.GetChild(0).position.y);
    //}
}
