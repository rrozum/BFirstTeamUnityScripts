using UnityEngine;

public class RoomContainer
{
    private int x;
    private int y;
    private int width;
    private int height;
    private Room _room;
    public RoomContainer(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public void growRoom()
    {
        int x, y, w, h;
        x = this.x + Random.Range(0, this.width / 3);
        y = this.y + Random.Range(0, this.height / 3);
        w = this.width - (x - this.x);
        h = this.height - (y - this.y);
        w -= Random.Range(0, w / 3);
        h -= Random.Range(0, h / 3);

        this._room = new Room(x, y, w, h);
    }
}