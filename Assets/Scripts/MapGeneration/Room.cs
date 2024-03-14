using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a room in a map.
public struct Room
{
    private Vector2Int m_coords;
    private List<Room> m_neighbors;
    private Vector2 m_position;

    // Initializes a new instance of the <see cref="Room"/> struct.
    public Room(Vector2Int mCoords)
    {
        m_coords = mCoords;
        m_neighbors = new List<Room>();
        m_position = new Vector2(m_coords.x * 10, m_coords.y * 10);
    }

    // Gets the coordinates of the room.
    public Vector2Int GetCoords()
    {
        return m_coords;
    }

    // Gets the position of the room.
    public Vector2 GetPosition()
    {
        return m_position;
    }

    // Adds a neighbor to the room.
    public void AddNeighbor(Room neighbor)
    {
        m_neighbors.Add(neighbor);
    }
}
