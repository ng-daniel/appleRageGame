using System;
using System.Collections.Generic;
public class PathAlgorithm
{
    public static bool IsBoardPossible(int[][] grid, int[] start, int[] end)
    {
        Queue<int[]> queue = new Queue<int[]>();
        List<int[]> visited = new List<int[]>();
        queue.Enqueue(start);
        int count = 0;
        while (queue.Count > 0)
        {
            //Console.WriteLine("Iteration #" + count);
            //count++
            int[] value = queue.Dequeue();
            if (value[0] == end[0] && value[1] == end[1])
            {
                return true;
            }
            if (!Visited(visited, value))
            {
                visited.Add(value);
                //Console.WriteLine("Visited: " + value[0] + " " + value[1]);
                List<int[]> neighbors = Neighbors(grid, value);
                //Console.WriteLine("Neighbors: " + neighbors.Count);
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (!Visited(visited, neighbors[i]))
                    {
                        queue.Enqueue(neighbors[i]);
                    }
                }
            }
        }
        //foreach (int[] item in visited)
        //{
        //    Console.WriteLine("Visited: " + item[0] + " " + item[1]);
        //}
        return false;
    }
    public static bool Visited(List<int[]> list, int[] value)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (value[0] == list[i][0] && value[1] == list[i][1])
            {
                return true;
            }
        }
        return false;
    }
    public static List<int[]> Neighbors(int[][] grid, int[] value)
    {
        int[] xdir = new int[] { 1, 0, -1, 0 };
        int[] ydir = new int[] { 0, 1, 0, -1 };

        List<int[]> neighbors = new List<int[]>();

        for (int i = 0; i < 4; i++)
        {
            int x = value[0] + xdir[i];
            int y = value[1] + ydir[i];
            int[] tile = new int[] { x, y };

            //Console.WriteLine("Checked Neighbor: " + tile[0] + " " + tile[1]);

            if (ValidTile(grid, tile) && grid[x][y] == 0)
            {
                neighbors.Add(tile);
            }
        }
        return neighbors;
    }
    public static bool ValidTile(int[][] grid, int[] tile)
    {
        return tile[0] >= 0 && tile[1] >= 0 && tile[0] < grid.Length && tile[1] < grid[0].Length;
    }

    public static void Main(string[] args)
    {
        int[][] grid = new int[][] {
            new int[] { 1, 0, 0, 1 },
            new int[] { 0, 1, 0, 0 },
            new int[] { 0, 0, 0, 1 }
        };

        int[] start = { 1, 0 };
        int[] end = { 0, 1 };
        bool result = IsBoardPossible(grid, start, end);

        // Function Call
        Console.WriteLine("Output: " + result);
    }
}