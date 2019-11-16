using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class BoardManager : MonoBehaviour
{
    public int boardRows, boardColumns;
    public int minRoomSize, maxRoomSize;
    public GameObject floorTile;
    public GameObject corridorTile;
    public GameObject wall;
    public GameObject wallAngle;
    public GameObject door;

    public GameObject debugSquare;



    public List<List<Vector3>> connectedPosition = new List<List<Vector3>>();
    private GameObject[,] boardPositionsFloor;



    public void CreateBSP(SubDungeon subDungeon)
    {
        //Debug.Log("Splitting sub-dungeon " + subDungeon.debugId + ": " + subDungeon.rect);
        if (subDungeon.IAmLeaf())
        {
            // if the sub-dungeon is too large split it
            if (subDungeon.rect.width > maxRoomSize
                || subDungeon.rect.height > maxRoomSize
                || Random.Range(0.0f, 1.0f) > 0.25)
            {

                if (subDungeon.Split(minRoomSize, maxRoomSize))
                {
                    //Debug.Log("Splitted sub-dungeon " + subDungeon.debugId + " in "
                    //    + subDungeon.left.debugId + ": " + subDungeon.left.rect + ", "
                    //    + subDungeon.right.debugId + ": " + subDungeon.right.rect);

                    CreateBSP(subDungeon.left);
                    CreateBSP(subDungeon.right);
                }
            }
        }
    }

    public void DrawRooms(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }
        if (subDungeon.IAmLeaf())
        {
            GameObject parent = Instantiate(new GameObject());
            parent.name = "";
            parent.name += subDungeon.debugId;
            parent.transform.position = new Vector3((int)subDungeon.room.x, (int)subDungeon.room.y);
            parent.transform.SetParent(transform);

            for (int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++)
            {
                for (int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++)
                {
                    GameObject instance = Instantiate(floorTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(parent.transform);
                    boardPositionsFloor[i, j] = instance;

                    //left wall
                    if (i == (int)subDungeon.room.x)
                    {
                        GameObject _wall = Instantiate(wall, new Vector3(i - 1, j, 0f), Quaternion.Euler(new Vector3(0, 0, wall.transform.eulerAngles.z +90))) as GameObject;
                        _wall.transform.SetParent(parent.transform);
                    }

                    //bottom wall
                    if (j == (int)subDungeon.room.y)
                    {
                        GameObject _wall = Instantiate(wall, new Vector3(i, j - 1, 0f), Quaternion.Euler(new Vector3(0, 0, wall.transform.eulerAngles.z + 180))) as GameObject;
                        _wall.transform.SetParent(parent.transform);
                    }

                    //rigth wall
                    if (i == (int)subDungeon.room.xMax - 1)
                    {
                        GameObject _wall = Instantiate(wall, new Vector3(i + 1, j, 0f), Quaternion.Euler(new Vector3(0, 0, wall.transform.eulerAngles.z - 90))) as GameObject;
                        _wall.transform.SetParent(parent.transform);
                    }

                    //up wall
                    if (j == (int)subDungeon.room.yMax - 1)
                    {
                        GameObject _wall = Instantiate(wall, new Vector3(i, j + 1, 0f), Quaternion.identity) as GameObject;
                        _wall.transform.SetParent(parent.transform);
                    }

                    //rigth up wall angle
                    if (i == (int)subDungeon.room.xMax - 1 && j == (int)subDungeon.room.yMax - 1)
                    {
                        GameObject _wall = Instantiate(wallAngle, new Vector3(i + 1, j + 1, 0f), Quaternion.identity) as GameObject;
                        _wall.transform.SetParent(parent.transform);
                    }

                    //left up wall angle
                    if (i == (int)subDungeon.room.x && j == (int)subDungeon.room.yMax - 1)
                    {
                        GameObject _wall = Instantiate(wallAngle, new Vector3(i - 1, j + 1, 0f), Quaternion.Euler(new Vector3(0, 0, wallAngle.transform.eulerAngles.z + 90))) as GameObject;
                        _wall.transform.SetParent(parent.transform);
                    }

                    //rigth bottom wall angle
                    if (i == (int)subDungeon.room.xMax - 1 && j == (int)subDungeon.room.y)
                    {
                        GameObject _wall = Instantiate(wallAngle, new Vector3(i + 1, j - 1, 0f), Quaternion.Euler(new Vector3(0, 0, wallAngle.transform.eulerAngles.z - 90))) as GameObject;
                        _wall.transform.SetParent(parent.transform);
                    }

                    // left bottom angle wall
                    if (i == (int)subDungeon.room.x && j == (int)subDungeon.room.y)
                    {
                        GameObject _wall = Instantiate(wallAngle, new Vector3(i - 1, j - 1, 0f), Quaternion.Euler(new Vector3(0, 0, wallAngle.transform.eulerAngles.z + 180))) as GameObject;
                        _wall.transform.SetParent(parent.transform);
                    }
                }
            }
        }
        else
        {
            DrawRooms(subDungeon.left);
            DrawRooms(subDungeon.right);
        }
    }

    void DrawCorridors(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }

        DrawCorridors(subDungeon.left);
        DrawCorridors(subDungeon.right);

        connectedPosition.Add(new List<Vector3>());
        foreach (Rect corridor in subDungeon.corridors)
        {
            for (int i = (int)corridor.x; i < corridor.xMax; i++)
            {
                for (int j = (int)corridor.y; j < corridor.yMax; j++)
                {
                    if (boardPositionsFloor[i, j] == null &&
                        (boardPositionsFloor[i + 1, j] != null))
                    {
                        GameObject instance = Instantiate(door, new Vector3(i, j, -1f), Quaternion.Euler(new Vector3(0, 0, door.transform.eulerAngles.z + 90))) as GameObject;
                        instance.transform.SetParent(transform);
                        //boardPositionsFloor[i, j] = instance;
                    }
                    else if (boardPositionsFloor[i, j] == null &&
                        (boardPositionsFloor[i, j + 1] != null))
                    {
                        GameObject instance = Instantiate(door, new Vector3(i, j, -1f), Quaternion.Euler(new Vector3(0, 0, door.transform.eulerAngles.z + 180))) as GameObject;
                        instance.transform.SetParent(transform);
                        //boardPositionsFloor[i, j] = instance;
                    }
                    else if (boardPositionsFloor[i, j] == null &&
                        (boardPositionsFloor[i - 1, j] != null))
                    {
                        GameObject instance = Instantiate(door, new Vector3(i, j, -1f), Quaternion.Euler(new Vector3(0, 0, door.transform.eulerAngles.z -  90))) as GameObject;
                        instance.transform.SetParent(transform);
                        //boardPositionsFloor[i, j] = instance;
                    }
                    else if (boardPositionsFloor[i, j] == null &&
                        (boardPositionsFloor[i, j - 1] != null))
                    {
                        GameObject instance = Instantiate(door, new Vector3(i, j, -1f), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(transform);
                        //boardPositionsFloor[i, j] = instance;
                    }
                    //else if (boardPositionsFloor[i, j] == null)
                    //{
                    //    GameObject instance = Instantiate(debugSquare, new Vector3(i, j, -1f), Quaternion.identity) as GameObject;
                    //    instance.transform.SetParent(transform);
                    //}
                    
                }
            }
        }
    }
    
    

    void Start()
    {
        SubDungeon rootSubDungeon = new SubDungeon(new Rect(0, 0, boardRows, boardColumns));
        CreateBSP(rootSubDungeon);
        rootSubDungeon.CreateRoom();

        boardPositionsFloor = new GameObject[boardRows, boardColumns];

        DrawRooms(rootSubDungeon);
        DrawCorridors(rootSubDungeon);

        //Debug.Log(rootSubDungeon.connectedRoom);
        
    }
}