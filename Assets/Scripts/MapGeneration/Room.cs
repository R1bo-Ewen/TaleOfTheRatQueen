using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a room in a map.
public class Room
{
    private Vector2Int m_coords;
    private List<Room> m_neighbors;
    public MapGeneration.RoomType type;

    public GameObject tile;
    public Vector3 tileSize;
    public List<MapGeneration.Direction> spawnedWalls;

    public Room(Vector2Int mCoords, MapGeneration.RoomType mType)
    {
        m_coords = mCoords;
        m_neighbors = new List<Room>();

        type = mType;
        spawnedWalls = new List<MapGeneration.Direction>();
    }

    public Vector2Int GetCoords()
    {
        return m_coords;
    }

    public void AddNeighbor(Room neighbor)
    {
        m_neighbors.Add(neighbor);
    }
}
