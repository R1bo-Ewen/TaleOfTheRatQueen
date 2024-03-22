using UnityEditorInternal;
using UnityEngine;

// Represents a door between two rooms in a map.
public class Door
{
    private Room m_roomA;
    private Room m_roomB;
    private Vector2 m_position;

    public GameObject tile;
 
    public Door(Room roomA, Room roomB)
    {
        m_roomA = roomA;
        m_roomB = roomB;
        m_position = Vector2.negativeInfinity;
    }

    public Room GetRoomA()
    {
        return m_roomA;
    }

    public Room GetRoomB()
    {
        return m_roomB;
    }

    public Vector2 GetPosition()
    {
        return m_position;
    }

    public void UpdatePosition()
    {
        Vector3 roomATilePosition = m_roomA.tile.transform.position;
        Vector3 roomBTilePosition = m_roomB.tile.transform.position;
        m_position = new Vector2(
            (roomATilePosition.x + roomBTilePosition.x) / 2.0f,
            (roomATilePosition.z + roomBTilePosition.z) / 2.0f
        );
    }
}
