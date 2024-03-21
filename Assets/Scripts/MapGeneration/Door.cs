using UnityEditorInternal;
using UnityEngine;

// Represents a door between two rooms in a map.
public class Door
{
    private Room m_roomA;
    private Room m_roomB;
    private Vector2 m_position;
 
    public Door(Room roomA, Room roomB)
    {
        m_roomA = roomA;
        m_roomB = roomB;
        m_position = Vector2.negativeInfinity;
    }

    public Vector2 GetPosition()
    {
        return m_position;
    }

    public void UpdatePosition()
    {
        m_position = new Vector2(
            (m_roomA.GetPosition().x + m_roomB.GetPosition().x) / 2.0f,
            (m_roomA.GetPosition().y + m_roomB.GetPosition().y) / 2.0f
        );
    }
}
