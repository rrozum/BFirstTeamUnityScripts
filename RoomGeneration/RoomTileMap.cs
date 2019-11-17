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

    private bool frame = true;


    public void Awake()
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
        //ColliderGenerator();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (frame)
        {
            if (PrevRoomSide == "DownWall")
            {
                GameObject player = GameObject.Find("Player");
                player.transform.position = new Vector3(GameObject.Find("SpawnDoor").transform.position.x, GameObject.Find("SpawnDoor").transform.position.y - 1, -1);
            }
            if (PrevRoomSide == "UpWall")
            {
                GameObject player = GameObject.Find("Player");
                player.transform.position = new Vector3(GameObject.Find("SpawnDoor").transform.position.x, GameObject.Find("SpawnDoor").transform.position.y + 1, -1);
            }
            if (PrevRoomSide == "LeftWall")
            {
                GameObject player = GameObject.Find("Player");
                player.transform.position = new Vector3(GameObject.Find("SpawnDoor").transform.position.x - 1, GameObject.Find("SpawnDoor").transform.position.y, -1);
            }
            if (PrevRoomSide == "RightWall")
            {
                GameObject player = GameObject.Find("Player");
                player.transform.position = new Vector3(GameObject.Find("SpawnDoor").transform.position.x + 1, GameObject.Find("SpawnDoor").transform.position.y, -1);
            }
            frame = false;
        }
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

        int BeginX = (int)transform.position.x;
        int BeginY = (int)transform.position.y;
        int EndX = (int)transform.position.x + randHeight;
        int EndY = (int)transform.position.y + randWidth;

        for (int i = BeginX; i < EndX; i++)
        {
            for (int j = BeginY; j < EndY; j++)
            {
                GameObject tile = Instantiate(floor, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                tile.transform.SetParent(FloorObj.transform);

                if (i == BeginX)
                {
                    GameObject _wall = Instantiate(wall, new Vector3(i - 1, j, 0f), Quaternion.Euler(new Vector3(0, 0, wall.transform.eulerAngles.z + 90))) as GameObject;
                    _wall.transform.SetParent(LeftWall.transform);
                    _wall.name = "left wall";
                }

                //bottom wall
                if (j == BeginY)
                {
                    GameObject _wall = Instantiate(wall, new Vector3(i, j - 1, 0f), Quaternion.Euler(new Vector3(0, 0, wall.transform.eulerAngles.z + 180))) as GameObject;
                    _wall.transform.SetParent(DownWall.transform);
                }

                //rigth wall
                if (i == EndX - 1)
                {
                    GameObject _wall = Instantiate(wall, new Vector3(i + 1, j, 0f), Quaternion.Euler(new Vector3(0, 0, wall.transform.eulerAngles.z - 90))) as GameObject;
                    _wall.transform.SetParent(RightWall.transform);
                }

                //up wall
                if (j == EndY - 1)
                {
                    GameObject _wall = Instantiate(wall, new Vector3(i, j + 1, 0f), Quaternion.identity) as GameObject;
                    _wall.transform.SetParent(UpWall.transform);
                }

                //rigth up wall angle
                if (i == EndX - 1 && j == EndY - 1)
                {
                    GameObject _wall = Instantiate(wallJoin, new Vector3(i + 1, j + 1, 0f), Quaternion.identity) as GameObject;
                    _wall.transform.SetParent(WallJoinObj.transform);
                }

                //left up wall angle
                if (i == BeginX && j == EndY - 1)
                {
                    GameObject _wall = Instantiate(wallJoin, new Vector3(i - 1, j + 1, 0f), Quaternion.Euler(new Vector3(0, 0, wallJoin.transform.eulerAngles.z + 90))) as GameObject;
                    _wall.transform.SetParent(WallJoinObj.transform);
                }

                //rigth bottom wall angle
                if (i == EndX - 1 && j == BeginY)
                {
                    GameObject _wall = Instantiate(wallJoin, new Vector3(i + 1, j - 1, 0f), Quaternion.Euler(new Vector3(0, 0, wallJoin.transform.eulerAngles.z - 90))) as GameObject;
                    _wall.transform.SetParent(WallJoinObj.transform);
                }

                // left bottom angle wall
                if (i == BeginX && j == BeginY)
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
        _spawnDoor.tag = "Respawn";


        _nextRoomDoor.name = "NexRoomDoor";
        _nextRoomDoor.transform.parent = transform.GetChild(1);
        _nextRoomDoor.tag = "Finish";

        Destroy(wallTransform.GetChild(SpawnDoorIndex).gameObject);
        Destroy(nexRoomDoorSide.GetChild(NexRoomDoorIndex).gameObject);

        return nexRoomDoorSide.name;
    }

    //private void ColliderGenerator()
    //{
    //    transform.gameObject.AddComponent<PolygonCollider2D>();
    //    var collider = transform.gameObject.GetComponent<PolygonCollider2D>();
    //    Transform joinWalls = transform.GetChild(2);

    //    Vector2[] points = new Vector2[11];
    //    points[0] = new Vector2(joinWalls.GetChild(0).localPosition.x, joinWalls.GetChild(0).localPosition.y);
    //    points[1] = new Vector2(joinWalls.GetChild(1).localPosition.x + 1, joinWalls.GetChild(1).localPosition.y);
    //    points[2] = new Vector2(joinWalls.GetChild(3).localPosition.x + 1, joinWalls.GetChild(3).localPosition.y + 1);
    //    points[3] = new Vector2(joinWalls.GetChild(2).localPosition.x, joinWalls.GetChild(2).localPosition.y + 1);

    //    points[4] = new Vector2(joinWalls.GetChild(0).localPosition.x, joinWalls.GetChild(0).localPosition.y);
    //    points[5] = new Vector2(joinWalls.GetChild(0).localPosition.x + 1, joinWalls.GetChild(0).localPosition.y + 1);
    //    points[6] = new Vector2(joinWalls.GetChild(2).localPosition.x, joinWalls.GetChild(2).localPosition.y + 1);
    //    points[7] = new Vector2(joinWalls.GetChild(3).localPosition.x, joinWalls.GetChild(3).localPosition.y);

    //    points[8] = new Vector2(joinWalls.GetChild(1).localPosition.x + 1, joinWalls.GetChild(1).localPosition.y);
    //    points[9] = new Vector2(joinWalls.GetChild(0).localPosition.x + 1, joinWalls.GetChild(0).localPosition.y + 1);
    //    points[10] = new Vector2(joinWalls.GetChild(2).localPosition.x, joinWalls.GetChild(2).localPosition.y);

    //    collider.points = points;
    //}
}
