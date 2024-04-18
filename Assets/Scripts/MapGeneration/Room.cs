using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GPS;

// Represents a room in a map.
public class Room
{
    private Vector2Int m_coords;
    private List<Room> m_connectedNeighbors;
    public MapGeneration.RoomType type;

    public GameObject tile;
    public Vector3 tileSize;
    public List<Direction> spawnedWalls;

    public Room(Vector2Int mCoords, MapGeneration.RoomType mType)
    {
        m_coords = mCoords;
        m_connectedNeighbors = new List<Room>();

        type = mType;
        spawnedWalls = new List<Direction>();
    }

    public Vector2Int GetCoords()
    {
        return m_coords;
    }

    public void AddConnectedNeighbor(Room neighbor)
    {
        m_connectedNeighbors.Add(neighbor);
    }
}
