using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GPS;

// Represents a room in a map.
public class Room
{
    public Vector2Int coords;
    private List<Room> m_connectedNeighbors;
    public MapGeneration.RoomType type;

    public GameObject tile;
    public Vector3 tileSize;
    private List<Direction> m_spawnedWalls;

    public Room(Vector2Int mCoords, MapGeneration.RoomType mType)
    {
        coords = mCoords;
        m_connectedNeighbors = new List<Room>();

        type = mType;
        m_spawnedWalls = new List<Direction>();
    }

    public void AddConnectedNeighbor(Room neighbor)
    {
        m_connectedNeighbors.Add(neighbor);
    }

    public List<Direction> GetSpawnedWalls()
    {
        return m_spawnedWalls;
    }

    public void AddSpawnedWall(Direction direction)
    {
        if (m_spawnedWalls.Contains(direction)){
            return;
        }

        m_spawnedWalls.Add(direction);
    }
}
