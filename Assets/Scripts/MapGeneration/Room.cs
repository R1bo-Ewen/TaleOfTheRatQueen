using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private int _id;
    private Vector2Int _coords;
    private List<Room> _neighbors;

    public Room(int id, Vector2Int coords)
    {
        this._id = id;
        this._coords = coords;
        this._neighbors = new List<Room>();
    }

    public Vector2Int GetCoords()
    {
        return _coords;
    }

    public List<Room> GetNeighbors()
    {
        return _neighbors;
    }

    public void AddNeighbor(Room neighbor)
    {
        _neighbors.Add(neighbor);
    }
}
