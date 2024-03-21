using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

// Generates a map by creating rooms and connecting them with doors.
public class MapGeneration : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize = new Vector2Int(20, 20);     // Virtual map grid size
    [SerializeField, Range(5, 50)] private int nbRoomMax = 10;               // Maximum number of rooms
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject doorPrefab;
    private bool[,] mapGrid;                                                  // Virtual map grid
    private List<Room> rooms;
    private List<Door> doors;
    
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

    // Generates a map by creating rooms and connecting them with doors.
    // This method initializes the map grid, creates the first room, explores neighboring rooms, and spawns the rooms.
    private void GenerateMap()
    {
        mapGrid = new bool[mapSize.x, mapSize.y];
        rooms = new List<Room>();
        doors = new List<Door>();
        
        Vector2Int firstRoomCoords = new Vector2Int(0, 0);
        CreateRoom(firstRoomCoords);

        for (int i = 1; i < nbRoomMax; i++)
        {
            ExploreRoom(rooms.Last());
        }
        
        SpawnRooms();
    }

    // Create a room and update the map grid and the list of rooms.
    private Room CreateRoom(Vector2Int coords)
    {
        Room room = new Room(coords);
        rooms.Add(room);
        mapGrid[coords.x, coords.y] = true;
        return room;
    }

    // Explores a room by creating a new neighbor room and connecting them with a door.
    private void ExploreRoom(Room room)
    {
        // Get the available directions to explore from the current room.
        List<Direction> directionsToExplore = AvailableNeighborDirection(room);

        // If there are no available directions to explore, return.
        if (directionsToExplore.Count == 0)
            return;

        // Randomly select a direction to explore.
        int neighborDirectionIndex = Random.Range(0, directionsToExplore.Count);

        // Get the coordinates of the neighboring room based on the selected direction.
        Vector2Int neighborCoords = GetNeighborCoords(directionsToExplore[neighborDirectionIndex], room.GetCoords());

        // Create a new room at the neighboring coordinates.
        Room neighbor = CreateRoom(neighborCoords);

        // Add the neighboring room to the current room's list of neighbors.
        room.AddNeighbor(neighbor);

        // Add the current room to the neighboring room's list of neighbors.
        neighbor.AddNeighbor(room);

        // Create a door between the current room and the neighboring room.
        doors.Add(new Door(room, neighbor));
    }

    // Returns the coordinates of a neighboring room based on a direction and the current coordinates.
    private Vector2Int GetNeighborCoords(Direction direction, Vector2Int coord)
    {
        Vector2Int newCoords = coord;
        
        switch (direction)
        {
            case Direction.NORTH:
                newCoords = new Vector2Int(coord.x, coord.y - 1);
                break;
            case Direction.EAST:
                newCoords = new Vector2Int(coord.x + 1, coord.y);
                break;
            case Direction.SOUTH:
                newCoords = new Vector2Int(coord.x, coord.y + 1);
                break;
            case Direction.WEST:
                newCoords = new Vector2Int(coord.x - 1, coord.y);
                break;
        };

        return newCoords;
    }

    // Returns the available directions to explore from a room.
    private List<Direction> AvailableNeighborDirection(Room room)
    {
        List<Direction> directions = new List<Direction>();
        
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Vector2Int neighborCoords = GetNeighborCoords(direction, room.GetCoords());

            // If the neighboring room is out of the map, continue.
            if (
                neighborCoords.x < 0 |
                neighborCoords.x >= mapSize.x |
                neighborCoords.y < 0 |
                neighborCoords.y >= mapSize.y
                )
            {
                continue;
            }

            // If the neighboring room already exists, continue.
            if (mapGrid[neighborCoords.x, neighborCoords.y])
            {
                continue;
            }
            
            directions.Add(direction);
        }

        return directions;
    }

    // Spawns the rooms and doors in the game world based on their positions.
    private void SpawnRooms()
    {
        GameObject roomsParent = new GameObject();
        roomsParent.name = "Rooms";
        GameObject doorsParent = new GameObject();
        doorsParent.name = "Doors";
        
        foreach (Room room in rooms)
        {
            GameObject roomTile = Instantiate(roomPrefab);
            Vector3 roomTileSize = roomTile.GetComponent<PrefabBounds>().GetSize();
            room.SetPosition(new Vector2(roomTileSize.x * room.GetCoords().x, roomTileSize.z * room.GetCoords().y));
            roomTile.transform.position = new Vector3(room.GetPosition().x, 0.0f, room.GetPosition().y);
            roomTile.transform.SetParent(roomsParent.transform);
        }

        foreach (Door door in doors)
        {
            GameObject doorTile = Instantiate(doorPrefab);
            door.UpdatePosition();
            Vector3 doorTilePos = new Vector3(door.GetPosition().x, 0.0f, door.GetPosition().y);
            doorTile.transform.position = doorTilePos;
            doorTile.transform.SetParent(doorsParent.transform);
        }
    }
}
