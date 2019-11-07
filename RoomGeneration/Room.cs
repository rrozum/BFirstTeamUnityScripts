using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private int x;
    private int y;
    private int width;
    private int heght;
    private Vector2 center;
    public Room(int x, int y, int width, int heght)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.heght = heght;
        this.center = new Vector2(this.x + this.width / 2, this.y + this.heght /2); 
    }
}