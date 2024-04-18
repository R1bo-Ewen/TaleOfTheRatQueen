using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GPS
{
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }
    public static List<Direction> DIRECTIONS = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();


    // Returns the coordinates of a neighboring room based on a direction and the current coordinates.
    public static Vector2Int GetNeighborCoords(Vector2Int currentCoords, Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH:
                return new Vector2Int(currentCoords.x, currentCoords.y + 1);
            case Direction.EAST:
                return new Vector2Int(currentCoords.x + 1, currentCoords.y);
            case Direction.SOUTH:
                return new Vector2Int(currentCoords.x, currentCoords.y - 1);
            case Direction.WEST:
                return new Vector2Int(currentCoords.x - 1, currentCoords.y);
            default:
                // Useless but needed to avoid warning.
                return currentCoords;
        };
    }

    // Get the direction of a neighboring room in relation to the current room.
    // TO FIX : Add the case where the 2 tested rooms are not neighbors (currently should never happen).
    public static Direction GetNeighborDirection(Room myRoom, Room neighbor)
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

    // Returns whether a neighboring room (connected or not) exists in a given direction.
    public static bool IsThereNeighbor(Room room, Direction direction, Map map)
    {
        Vector2Int neighborCoords = GetNeighborCoords(room.GetCoords(), direction);

        if (!IsCoordsInMap(neighborCoords, map))
        {
            return false;
        }

        return map.grid[neighborCoords.x, neighborCoords.y];
    }

    // Returns the available directions to explore from a room.
    public static List<Direction> AvailableDirectionsToExplore(Room room, Map map)
    {
        List<Direction> directions = new List<Direction>();

        foreach (Direction direction in DIRECTIONS)
        {
            Vector2Int neighborCoords = GetNeighborCoords(room.GetCoords(), direction);

            if (!IsCoordsInMap(neighborCoords, map))
            {
                continue;
            }

            // If the neighboring room already exists, continue.
            if (map.grid[neighborCoords.x, neighborCoords.y])
            {
                continue;
            }

            directions.Add(direction);
        }

        return directions;
    }

    private static bool IsCoordsInMap(Vector2Int coords, Map map)
    {
        return coords.x >= 0 && coords.x < map.size.x && coords.y >= 0 && coords.y < map.size.y;
    }
}
