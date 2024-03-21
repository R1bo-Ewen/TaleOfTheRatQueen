using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a room in a map.
public class Room
{
    private Vector2Int m_coords;
    private List<Room> m_neighbors;
    private Vector2 m_position;

    public Room(Vector2Int mCoords)
    {
        m_coords = mCoords;
        m_neighbors = new List<Room>();
        m_position = Vector2.negativeInfinity;
    }

    public Vector2Int GetCoords()
    {
        return m_coords;
    }

    public Vector2 GetPosition()
    {
        return m_position;
    }

    public void SetPosition(Vector2 pos)
    {
        m_position = pos;
    }

    public void AddNeighbor(Room neighbor)
    {
        m_neighbors.Add(neighbor);
    }
}
