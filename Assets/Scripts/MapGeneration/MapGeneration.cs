using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize = new Vector2Int(10, 10);
    [SerializeField, Range(5, 15)] private int nbRoomMax = 10;
    private int nbRooms = 0;
    private bool[,] mapGrid;
    private List<Room> mapGraph = new List<Room>();
    private List<Room> roomsToExplore = new List<Room>();
    private enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        mapGrid = new bool[mapSize.x, mapSize.y];
        
        Vector2Int firstRoomCoords = new Vector2Int(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y));
        Room firstRoom = CreateRoom(firstRoomCoords);

        for (int i = 0; i < nbRoomMax; i++)
        {
            ExploreRoom(roomsToExplore[0]);
        }
    }

    private Room CreateRoom(Vector2Int coords)
    {
        nbRooms++;
        Room room = new Room(nbRooms, coords);
        mapGrid[coords.x, coords.y] = true;
        roomsToExplore.Add(room);
        return room;
    }

    private void ExploreRoom(Room room)
    {
        List<Direction> directionsToExplore = DirectionsToExplore(room);
        
        foreach (Direction direction in directionsToExplore)
        {
            bool isThereNeighbor = Random.value < 0.5f;
            
            if (isThereNeighbor)
            {
                Vector2Int neighborCoords = GetCoords(direction, room.GetCoords());
                Room neighbor = CreateRoom(neighborCoords);
                room.AddNeighbor(neighbor);
                neighbor.AddNeighbor(room);
            }
        }

        roomsToExplore.Remove(room);
    }

    // A faire
    private Vector2Int GetCoords(Direction direction, Vector2Int coord)
    {
        return coord;
    }

    // A faire
    private List<Direction> DirectionsToExplore(Room room)
    {
        return new List<Direction>
        {
            Direction.NORTH,
            Direction.EAST,
            Direction.SOUTH,
            Direction.WEST
        };
    }
}
