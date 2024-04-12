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

    [Header("========== PREFABS ====================")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject defaultRoomPrefab;
    [SerializeField] private List<GameObject> kitchenPrefabs;
    [SerializeField] private List<GameObject> bedroomPrefabs;
    [SerializeField] private List<GameObject> bigRoomPrefabs;

    private bool[,] mapGrid;                                                  // Virtual map grid
    private List<Room> rooms;

    public enum RoomType
    {
        CORRIDOR,
        KITCHEN,
        BEDROOM,
        BIGROOM
    }

    private List<RoomType> roomsTypes;
    private List<Door> doors;
    
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    [NonSerialized] public List<Direction> DIRECTIONS = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
    [NonSerialized] public Vector3 spawnLocation = Vector3.zero;

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

        ShuffleRoomsTypes();
        
        Vector2Int firstRoomCoords = new Vector2Int(mapSize.x / 2, mapSize.y / 2);
        CreateRoom(firstRoomCoords);

        for (int i = 1; i < nbRoomMax; i++)
        {
            ExploreRoom(rooms.Last());
        }
        
        SpawnMapElements();
        SetSpawnLocation();
        print("Map generated");
    }

    // Shuffle the room types
    private void ShuffleRoomsTypes()
    {
        // Add the special room types to the list
        roomsTypes = new List<RoomType>
        {
            RoomType.KITCHEN,
            RoomType.BEDROOM,
            RoomType.BIGROOM
        };

        // Fill the list with default room type
        for(int i = roomsTypes.Count; i < nbRoomMax; i++)
        {
            roomsTypes.Add(RoomType.CORRIDOR);
        }

        Shuffle(roomsTypes);
    }
    
    // Shuffle a list
    private void Shuffle<T> (List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Create a room and update the map grid and the list of rooms.
    private Room CreateRoom(Vector2Int coords)
    {
        Room room = new Room(coords, roomsTypes.Last());
        rooms.Add(room);
        mapGrid[coords.x, coords.y] = true;
        roomsTypes.RemoveAt(roomsTypes.Count - 1);
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
        
        foreach (Direction direction in DIRECTIONS)
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

    // Spawns the rooms, doors and walls in the game world based on their positions.
    private void SpawnMapElements()
    {
        GameObject roomsParent = new GameObject();
        roomsParent.name = "Rooms";
        GameObject doorsParent = new GameObject();
        doorsParent.name = "Doors";
        GameObject wallParent = new GameObject();
        wallParent.name = "Walls";
        
        
        // Rooms
        foreach (Room room in rooms)
        {
            GameObject roomPrefab;

            switch (room.type)
            {
                case RoomType.KITCHEN:
                    roomPrefab = kitchenPrefabs[Random.Range(0, kitchenPrefabs.Count)];
                    break;
                case RoomType.BEDROOM:
                    roomPrefab = bedroomPrefabs[Random.Range(0, bedroomPrefabs.Count)];
                    break;
                case RoomType.BIGROOM:
                    roomPrefab = bigRoomPrefabs[Random.Range(0, bigRoomPrefabs.Count)];
                    break;
                default:
                    roomPrefab = defaultRoomPrefab;
                    break;
            }

            room.tile = Instantiate(roomPrefab);
            room.tileSize = room.tile.GetComponent<PrefabBounds>().GetSize();
            room.tile.transform.position = new Vector3(room.tileSize.x * room.GetCoords().x, 0.0f, room.tileSize.z * room.GetCoords().y);
            room.tile.transform.SetParent(roomsParent.transform);
        }
        
        // Doors
        foreach (Door door in doors)
        {
            door.UpdatePosition();
            door.tile = Instantiate(doorPrefab);
            door.tile.transform.position = new Vector3(door.GetPosition().x, 0.0f, door.GetPosition().y);
            Direction orientation = GetNeighborDirection(door.GetRoomA(), door.GetRoomB());

            if (orientation == Direction.NORTH | orientation == Direction.SOUTH)
            {
                door.tile.transform.Rotate(0.0f, 90.0f, 0.0f);
            }

            int directionIndex = DIRECTIONS.FindIndex(d => d == orientation);
            door.GetRoomA().spawnedWalls.Add(orientation);
            door.GetRoomB().spawnedWalls.Add(DIRECTIONS[(directionIndex + 2) % 4]);
            door.tile.transform.SetParent(doorsParent.transform);
        }

        // Walls
        // TO FIX : 2 walls can spawn at same location if 2 rooms are neighbors without being connected by a door
        foreach (Room room in rooms)
        {
            foreach (Direction orientation in DIRECTIONS)
            {
                if(room.spawnedWalls.Contains(orientation))
                {
                    continue;
                }
                
                GameObject wallObject = Instantiate(wallPrefab);
                Vector3 position = Vector3.zero;
                Vector3 rotation = Vector3.zero;
                
                switch (orientation)
                {
                    case Direction.NORTH:
                        position = new Vector3(room.tile.transform.position.x, 0.0f, room.tile.transform.position.z + room.tileSize.z / 2);
                        rotation = new Vector3(0.0f, 90.0f, 0.0f);
                        break;
                    
                    case Direction.EAST:
                        position = new Vector3(room.tile.transform.position.x + room.tileSize.x / 2, 0.0f, room.tile.transform.position.z);
                        break;
                    
                    case Direction.SOUTH:
                        position = new Vector3(room.tile.transform.position.x, 0.0f, room.tile.transform.position.z - room.tileSize.z / 2);
                        rotation = new Vector3(0.0f, 90.0f, 0.0f);
                        break;
                    
                    case Direction.WEST:
                        position = new Vector3(room.tile.transform.position.x - room.tileSize.x / 2, 0.0f, room.tile.transform.position.z);
                        break;
                }
                
                wallObject.transform.position = position;
                wallObject.transform.Rotate(rotation);
                wallObject.transform.SetParent(wallParent.transform);
            }
        }
    }

    // Get the direction of a neighboring room in relation to the current room.
    // TO FIX : Add the case where the 2 tested rooms are not neighbors (currently should never happen).
    private Direction GetNeighborDirection(Room myRoom, Room neighbor)
    {
        Vector2Int offset = neighbor.GetCoords() - myRoom.GetCoords();
        
        if (offset == new Vector2Int(0, 1))
        {
            return Direction.NORTH;
        }
        else if (offset == new Vector2Int(1, 0))
        {
            return Direction.EAST;
        }
        else if (offset == new Vector2Int(0, -1))
        {
            return Direction.SOUTH;
        }
        else
        {
            return Direction.WEST;
        }
    }

    private void SetSpawnLocation()
    {
        spawnLocation = rooms[0].tile.transform.position;
    }
}
