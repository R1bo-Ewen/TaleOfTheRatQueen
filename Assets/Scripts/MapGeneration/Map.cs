using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public Vector2Int size;
    public bool[,] grid;
    public List<Room> rooms;
    public List<Door> doors;

    public Map(Vector2Int size)
    {
        this.size = size;
        grid = new bool[size.x, size.y];
        rooms = new List<Room>();
        doors = new List<Door>();
    }
}