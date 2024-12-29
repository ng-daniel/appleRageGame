using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class GameBoardScript : MonoBehaviour
{

    [SerializeField] int cycles;
    [SerializeField] float time;


    const int boardSize = 8;
    char[][] board = new char[boardSize][];
    [SerializeField] string map;
    [SerializeField] float numApples;
    float numCores;
    int[] playerPosition;
    int[] hourglassPosition;
    List<int[]> stumps = new List<int[]>();
    List<int[]> seeds = new List<int[]>();
    List<int[]> apples = new List<int[]>();
    List<int[]> cores = new List<int[]>();

    public static Dictionary<string, char> stringKey = new Dictionary<string, char>();
    public static Dictionary<char, string> charKey = new Dictionary<char, string>();


    public void Start()
    {
        SetDefaults();
        PrintBoard();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PrintBoard();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RefreshBoard();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearBoard();
            PrintBoard();
        }
    }

    public void SetDefaults()
    {
        numApples = 1;
        numCores = 1;
        playerPosition = new int[2] { 4, 2 };
        hourglassPosition = new int[2] { 4, 5 };

        stringKey.Add("empty", 'E');
        stringKey.Add("apple", 'A');
        stringKey.Add("core", 'C');
        stringKey.Add("hourglass", 'H');
        stringKey.Add("player", 'P');
        stringKey.Add("seed", 'D');
        stringKey.Add("stump", 'S');

        charKey = stringKey.ToDictionary(x => x.Value, x => x.Key);

        char e = stringKey["empty"];

        ClearBoard();

        print("Defaults Set");
        print(board.Length + "x" + board[3].Length);
        print("Apples: " + numApples);
        print("Cores: " + numCores);
        print("PlayerPos" + playerPosition);
        print(charKey['S'] + ": " + stringKey["stump"]);
    }

    public char[][] GetBoard()
    {
        return board;
    }
    public void RefreshBoard()
    {
        // clear board
        ClearBoard();
        PrintBoard();

        // check for seeds
        // replace seeds with stumps
        for (int i = 0; i < seeds.Count; i++)
        {
            int[] pos = seeds[i];
            board[pos[0]][pos[1]] = stringKey["stump"];
            stumps.Add(pos);
        }

        // clear apples and cores
        apples = new List<int[]>();
        cores = new List<int[]>();

        // put player back
        Place(playerPosition, "player");

        // make new cores based on numcores
        for (int i = 0; i < (int)numCores; i++)
        {
            int[] pos = GetRandomEmptyPosition();
            Place(pos, "core");
            cores.Add(pos);
        }
        // make new apples based on numapples
        for (int i = 0; i < (int)numCores; i++)
        {
            int[] pos = GetRandomEmptyPosition();
            Place(pos, "apple");
            apples.Add(pos);
        }

        // clear and put hourglass somewhere
        Remove(hourglassPosition);
        hourglassPosition = GetRandomEmptyPosition();
        Place(hourglassPosition, "hourglass");
    }
    public void Place(int[] pos, string key)
    {
        board[pos[0]][pos[1]] = stringKey[key];
    }
    public char Remove(int[] pos)
    {
        char c = board[pos[0]][pos[1]];
        Place(pos, "empty");
        return c;
    }
    public void PrintBoard()
    {
        map = "";
        for (int i = board.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                map += board[i][j] + " ";
            }
            map += "\n";
        }
    }
    public void ClearBoard()
    {
        board = new char[8][]
        {
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'}
        };
    }
    public void ProcessPlayerInput()
    {

    }
    public void BoardTick()
    {

    }
    public char GetPosition(int x, int y)
    {
        try
        {
            return board[x][y];
        }
        catch
        {
            return 'X';
        }

    }
    public bool IsEmptyPosition(int x, int y)
    {
        return GetPosition(x, y) == stringKey["empty"];
    }
    public int[] RandomPosition()
    {
        int x = UnityEngine.Random.Range(0, 7);
        int y = UnityEngine.Random.Range(0, 7);
        int[] position = { x, y };
        return position;
    }
    public int[] GetRandomEmptyPosition()
    {
        int[] position = RandomPosition();
        int count = 0;
        while (!IsEmptyPosition(position[0], position[1]))
        {
            position = RandomPosition();
            count++;
            if (count >= 100000)
            {
                return new int[] { -1, -1 };
            }
        }
        return position;
    }
    public bool IsBoardPossible(char[][] grid)
    {
        int[][] numgrid = new int[boardSize][];
        for (int i = 0; i < boardSize; i++)
        {
            numgrid[i] = new int[boardSize];
        }

        int[] p = null;
        int[] h = null;
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                char value = grid[i][j];
                numgrid[i][j] = value == stringKey["stump"] ? 1 : 0;

                if (value == stringKey["player"])
                {
                    p = new int[2];
                    p[0] = i;
                    p[1] = j;
                }
                if (value == stringKey["hourglass"])
                {
                    h = new int[2];
                    h[0] = i;
                    h[1] = j;
                }
            }
        }

        return p != null &&
            h != null &&
            PathAlgorithm.PossiblePath(h[0] + 1, h[1] + 1, numgrid, p) != 1;
    }
}
