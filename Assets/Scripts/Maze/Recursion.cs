using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursion : Maze
{
    public override void Generate()
    {
        Generate(1, height - 2);
    }

    void Generate(int x, int y)
    {
        if (countSquareNeighbors(x, y) >= 2)
        {
            return;
        }

        map[x, y] = 0;
        mainPath.Add(new MapLocation(x, y));

        directions.Shuffle();

        Generate(x + directions[0].x, y + directions[0].y);
        Generate(x + directions[1].x, y + directions[1].y);
        Generate(x + directions[2].x, y + directions[2].y);
        Generate(x + directions[3].x, y + directions[3].y);
    }
}
