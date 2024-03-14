using UnityEditorInternal;
using UnityEngine;

// Represents a door between two rooms in a map.
public struct Door
{
    private Room m_roomA;
    private Room m_roomB;
    private Vector2 m_position;
 
    // Initializes a new instance of the Door class.
    public Door(Room roomA, Room roomB)
    {
        m_roomA = roomA;
        m_roomB = roomB;
        m_position = new Vector2(
            (m_roomA.GetPosition().x + m_roomB.GetPosition().x) / 2.0f,
            (m_roomA.GetPosition().y + m_roomB.GetPosition().y) / 2.0f
        );
    }

    // Gets the position of the door.
    public Vector2 GetPosition()
    {
        return m_position;
    }
}
