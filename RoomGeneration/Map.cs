using System.Collections.Generic;
using UnityEngine;
public class Map
{
    [SerializeField] private int Map_Size = 100;

    [SerializeField] private int width = 0;
    [SerializeField] private int height = 0;

    [SerializeField] private List<Room> rooms = new List<Room>();

    //что за C пока не понял
    [SerializeField] private object C;
    
    public void Init()
    {
        var main_room = new RoomContainer(0, 0, Map_Size, Map_Size);

        growRooms();
    }

    public void growRooms()
    {
        var leafs = room_tree.getLeafs();
        for (int i = 0; i < leafs.length; i++)
        {
            leafs[i].growRoom();
            rooms.Add(leafs[i].room);
        }
    }
}