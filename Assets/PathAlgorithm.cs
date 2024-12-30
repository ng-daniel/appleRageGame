// NOT MINE: https://www.geeksforgeeks.org/shortest-path-in-grid-with-obstacles/
// modified to fit problem

// C# implementation of the above approach
using System;
using System.Collections.Generic;

public class PathAlgorithm
{
    public static int PossiblePath(int n, int m, int[][] grid, int[] start)
    {
        // Check if the source or destination cell is blocked
        if (grid[start[0]][start[1]] == 1 || grid[n - 1][m - 1] == 1)
        {
            // Return -1 to indicate no path
            return -1;
        }

        // Create a queue to store the cells to explore
        Queue<int[]> q = new Queue<int[]>();

        // Add the source cell to the queue and mark its distance as 0
        q.Enqueue(new int[] { start[0], start[1] });

        // Define two arrays to represent the four directions of movement
        int[] dx = { -1, 0, 1, 0 };
        int[] dy = { 0, 1, 0, -1 };

        // Create a 2D array to store the distance of each cell
        // from the source
        int[][] dis = new int[grid.Length][];
        for (int i = 0; i < grid.Length; i++)
        {
            dis[i] = new int[grid[i].Length];
            for (int j = 0; j < grid[i].Length; j++)
            {
                dis[i][j] = -1;
            }
        }

        // Set the distance of the source cell as 0
        dis[start[0]][start[1]] = 0;

        // Loop until the queue is empty or the destination is reached
        while (q.Count > 0)
        {
            // Get the front cell from the queue and remove it
            int[] p = q.Dequeue();
            int x = p[0];
            int y = p[1];

            // Loop through the four directions of movement
            for (int i = 0; i < 4; i++)
            {
                // Calculate the coordinates of the neighboring cell
                int xx = x + dx[i];
                int yy = y + dy[i];

                // Check if the neighboring cell is inside the grid
                // and not visited before
                if (xx >= 0 && xx < n && yy >= 0 && yy < m && dis[xx][yy] == -1)
                {
                    // Check if the neighboring cell is free or special
                    if (grid[xx][yy] == 0 || grid[xx][yy] == 2)
                    {
                        // Set the distance of the neighboring cell as one
                        // more than the current cell
                        dis[xx][yy] = dis[x][y] + 1;

                        // Add the neighboring cell to the queue for
                        // further exploration
                        q.Enqueue(new int[] { xx, yy });
                    }

                    // Check if the neighboring cell is special
                    if (grid[xx][yy] == 2)
                    {
                        // Loop through the four directions of movement again
                        for (int j = 0; j < 4; j++)
                        {
                            // Calculate the coordinates of the adjacent cell
                            int xxx = xx + dx[j];
                            int yyy = yy + dy[j];

                            // Check if the adjacent cell is inside the grid
                            if (xxx >= 0 && xxx < n && yyy >= 0 && yyy < m)
                            {
                                // Check if the adjacent cell is blocked
                                if (grid[xxx][yyy] == 1)
                                {
                                    // Change the adjacent cell to free
                                    grid[xxx][yyy] = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Return the distance of the destination cell from the source
        return dis[n - 1][m - 1];
    }

    public static void Main(string[] args)
    {
        int n = 3;
        int m = 4;
        int[][] grid = new int[][] {
            new int[] { 0, 1, 2, 1 },
            new int[] { 2, 1, 0, 0 },
            new int[] { 0, 2, 1, 0 }
        };

        int[] start = { 1, 1 };
        int result = PossiblePath(n, m, grid, start);

        // Function Call
        Console.WriteLine("Output: " + result);
    }
}

// This code is contributed by Tapesh(tapeshdua420)