using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using static GPS;

// Generates a map by creating rooms and connecting them with doors.
public class MapGeneration : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize = new Vector2Int(20, 20);     // Virtual map grid size
    [SerializeField, Range(5, 50)] private int nbRoomMax = 10;               // Maximum number of rooms

    [Header("---------- ROOMS PREFABS ----------")]
    [SerializeField] private List<GameObject> corridorPrefabs;
    [SerializeField] private List<GameObject> kitchenPrefabs;
    [SerializeField] private List<GameObject> bedroomPrefabs;

    [Header("---------- WALLS AND DOORS PREFABS ----------")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject exitWallPrefab;
    [SerializeField] private List<GameObject> doorPrefabs;


    public enum RoomType
    {
        CORRIDOR,
        KITCHEN,
        BEDROOM
    }

    private List<RoomType> roomsTypes;

    public Map map;
    [NonSerialized] public Vector3 spawnLocation = Vector3.zero;
    private int seed;

    private void Start()
    {
        seed = Random.Range(0, 1000000);
        Random.InitState(seed);
        print("Seed : " + seed);
        GenerateMap();
    }

    // Generates a map by creating rooms and connecting them with doors.
    // This method initializes the map grid, creates the first room, explores neighboring rooms, and spawns the rooms.
    private void GenerateMap()
    {
        map = new Map(mapSize);

        // Generate the list of room types to spawn.
        GenerateRoomsTypesOrder();

        // Create the first room at the center of the map.
        Vector2Int firstRoomCoords = new Vector2Int(mapSize.x / 2, mapSize.y / 2);
        CreateRoom(firstRoomCoords);

        // Create the other rooms and connect them with doors.
        for (int i = 1; i < nbRoomMax; i++)
        {
            ExploreRoom(map.rooms.Last());
        }

        // Spawn the rooms, doors, and walls in the game world.
        SpawnMapElements();

        // Set the spawn location to the first room's position.
        SetSpawnLocation();

        print("Map generated");
    }

    // Create a list of room types in a random order.
    private void GenerateRoomsTypesOrder()
    {
        // Add the special room types to the list
        roomsTypes = new List<RoomType>
        {
            RoomType.KITCHEN,
            RoomType.BEDROOM,
        };

        // Fill the list with default room type
        for (int i = roomsTypes.Count; i < nbRoomMax - 1; i++)
        {
            roomsTypes.Add(RoomType.CORRIDOR);
        }

        Shuffle(roomsTypes);

        // Add the default room type at the end of the list to avoid the spawn
        // room to be a special room (the list is backwards iterated)
        roomsTypes.Add(RoomType.CORRIDOR);
    }

    // Shuffle a list
    private void Shuffle<T>(List<T> list)
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
        map.rooms.Add(room);
        map.grid[coords.x, coords.y] = true;
        roomsTypes.RemoveAt(roomsTypes.Count - 1);
        return room;
    }

    // Explores a room by creating a new neighbor room and connecting them with a door.
    private void ExploreRoom(Room room)
    {
        // Get the available directions to explore from the current room.
        List<Direction> directionsToExplore = AvailableDirectionsToExplore(room, map);

        // If there are no available directions to explore, return.
        if (directionsToExplore.Count == 0)
            return;

        // Randomly select a direction to explore.
        int neighborDirectionIndex = Random.Range(0, directionsToExplore.Count);

        // Get the coordinates of the neighboring room based on the selected direction.
        Vector2Int connectedNeighborCoords = GetNeighborCoords(room.coords, directionsToExplore[neighborDirectionIndex]);

        // Create a new room at the neighboring coordinates.
        Room connectedNeighbor = CreateRoom(connectedNeighborCoords);

        // Add the neighboring room to the current room's list of neighbors.
        room.AddConnectedNeighbor(connectedNeighbor);

        // Add the current room to the neighboring room's list of neighbors.
        connectedNeighbor.AddConnectedNeighbor(room);

        // Create a door between the current room and the neighboring room.
        map.doors.Add(new Door(room, connectedNeighbor));
    }

    // Spawns the rooms, doors and walls in the game world based on their positions.
    private void SpawnMapElements()
    {
        // Create empty game objects to organize the spawned elements.
        GameObject roomsParent = new GameObject();
        roomsParent.name = "Rooms";
        roomsParent.transform.SetParent(this.transform);

        GameObject doorsParent = new GameObject();
        doorsParent.name = "Doors";
        doorsParent.transform.SetParent(this.transform);

        GameObject wallParent = new GameObject();
        wallParent.name = "Walls";
        wallParent.transform.SetParent(this.transform);


        // Rooms
        foreach (Room room in map.rooms)
        {
            GameObject roomPrefab;

            // Set the room prefab based on its type.
            switch (room.type)
            {
                case RoomType.KITCHEN:
                    roomPrefab = kitchenPrefabs[Random.Range(0, kitchenPrefabs.Count)];
                    break;
                case RoomType.BEDROOM:
                    roomPrefab = bedroomPrefabs[Random.Range(0, bedroomPrefabs.Count)];
                    break;
                default:
                    roomPrefab = corridorPrefabs[Random.Range(0, corridorPrefabs.Count)];
                    break;
            }

            room.tile = Instantiate(roomPrefab);

            // Get the size of the room tile and set its position accordingly.
            room.tileSize = room.tile.GetComponent<PrefabBounds>().GetSize();
            room.tile.transform.position = new Vector3(room.tileSize.x * room.coords.x, 0.0f, room.tileSize.z * room.coords.y);

            room.tile.transform.SetParent(roomsParent.transform);
        }

        // Doors
        foreach (Door door in map.doors)
        {
            // Update the door position based on the positions of the rooms it connects.
            door.UpdatePosition();

            // If the door connects two corridors maybe we won't spawn it.
            if(door.GetRoomA().type == RoomType.CORRIDOR && door.GetRoomB().type == RoomType.CORRIDOR)
            {
                // 50% chance to spawn the door.
                bool doorWillSpawn = Random.value < 0.5f;

                if (!doorWillSpawn)
                {
                    // To avoid walls to spawn here.
                    door.UpdateRoomsSpawnedWalls();
                    continue;
                }
            }

            door.tile = Instantiate(doorPrefabs[Random.Range(0, doorPrefabs.Count)]);
            door.tile.transform.position = new Vector3(door.position.x, 0.0f, door.position.y);

            if (door.direction == Direction.NORTH | door.direction == Direction.SOUTH)
            {
                door.tile.transform.Rotate(0.0f, 90.0f, 0.0f);
            }

            // 50% chance to flip the door, for the door to be at another position on the wall.
            bool isDoorFlipped = Random.value < 0.5f;
            if (isDoorFlipped)
            {
                door.tile.transform.Rotate(0.0f, 180.0f, 0.0f);
            }

            door.UpdateRoomsSpawnedWalls();
            door.tile.transform.SetParent(doorsParent.transform);
        }

        // Walls
        for (int i = 0; i < map.rooms.Count; i++)
        {
            // If the room is the first one, spawn an exit wall.
            if (i == 0)
            {
                List<Direction> availableDirection = new List<Direction> { };

                foreach (Direction direction in DIRECTIONS)
                {
                    if (!IsThereNeighbor(map.rooms[i], direction, map))
                    {
                        availableDirection.Add(direction);
                    }
                }

                Direction spawnDirection = availableDirection[Random.Range(0, availableDirection.Count)];
                SpawnWall(map.rooms[i], spawnDirection, exitWallPrefab);
            }

            // Spawn the walls of the room.
            foreach (Direction direction in DIRECTIONS)
            {
                // If the wall is already spawned, continue.
                if (map.rooms[i].GetSpawnedWalls().Contains(direction))
                {
                    continue;
                }

                SpawnWall(map.rooms[i], direction, wallPrefab);

                // To avoid the neighbor to spawn the same wall.
                if (IsThereNeighbor(map.rooms[i], direction, map))
                {
                    Vector2Int neighborCoords = GetNeighborCoords(map.rooms[i].coords, direction);
                    Room neighbor = map.rooms.Find(r => r.coords == neighborCoords);
                    neighbor.AddSpawnedWall(DIRECTIONS[(DIRECTIONS.FindIndex(d => d == direction) + 2) % 4]);
                }
            }
        }
    }

    // Spawns a room wall based on its direction.
    private void SpawnWall(Room room, Direction direction, GameObject prefab)
    {
        GameObject wallObject = Instantiate(prefab);
        Vector3 position = Vector3.zero;
        Vector3 rotation = Vector3.zero;

        switch (direction)
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
        wallObject.transform.SetParent(GameObject.Find("Walls").transform);

        // If exit wall, set the WinZone script with current Player object.
        if(wallObject.GetComponent<WinZone>() != null)
        {
            wallObject.GetComponent<WinZone>().player = GameObject.Find("Player").GetComponent<Player>();
        }

        room.AddSpawnedWall(direction);
    }

    // Sets the spawn location to the first room's position.
    private void SetSpawnLocation()
    {
        spawnLocation = map.rooms[0].tile.transform.position;
    }
}
