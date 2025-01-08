using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{

    [SerializeField] int cycles;
    [SerializeField] int bonus;
    [SerializeField] float time;
    float appleBonus;
    const float corePenalty = -2;
    const float hourglassBonus = 1f;

    const int boardSize = 8;
    char[][] board = new char[boardSize][];
    [SerializeField] string map;
    [SerializeField] float numApples;
    [SerializeField] float numCores;
    float appleInc;
    float coreInc;
    float maxApples;
    float maxCores;

    [SerializeField] float moveCd;
    float maxMoveCd;

    int[] playerPosition;
    int[] hourglassPosition;
    List<int[]> stumps = new List<int[]>();
    List<int[]> seeds = new List<int[]>();
    List<int[]> apples = new List<int[]>();
    List<int[]> cores = new List<int[]>();

    public static Dictionary<string, char> stringKey = new Dictionary<string, char>();
    public bool gamestart;
    public bool gameover;

    AudioPlayerScript playerSrc;
    AudioPlayerScript stumpSrc;

    public void Start()
    {
        playerSrc = GameObject.FindGameObjectWithTag("PlayerSFX").GetComponent<AudioPlayerScript>();
        stumpSrc = GameObject.FindGameObjectWithTag("StumpSFX").GetComponent<AudioPlayerScript>();

        SetDefaults();
        print(TestBoardPossible());
    }
    public void StartGame(float time)
    {
        RefreshBoard();
        ResetGame();
        StartBoard();
        this.time = time;
        gamestart = true;
    }
    public void GameOver(string message)
    {
        gameover = true;
    }
    public void ResetGame()
    {
        gamestart = false;
        gameover = false;

        stumps = new List<int[]>();

        time = 1f;
        cycles = 0;
        bonus = 0;

        numApples = 1;
        numCores = 1;

        playerPosition = new int[2] { 3, 2 };
        hourglassPosition = new int[2] { 3, 5 };

        ClearBoard();
        StartBoard();
    }
    public void SetDefaults()
    {
        ResetGame();

        appleInc = 0.1f;
        coreInc = 0.1f;

        maxApples = 4;
        maxCores = 5;
        maxMoveCd = 0.15f;

        stringKey.Add("empty", 'E');
        stringKey.Add("apple", 'A');
        stringKey.Add("core", 'C');
        stringKey.Add("hourglass", 'H');
        stringKey.Add("player", 'P');
        stringKey.Add("seed", 'D');
        stringKey.Add("stump", 'S');
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintBoard();
        }

        if (!gamestart)
        {
            return;
        }

        char input = !gameover ? ProcessPlayerInput() : 'x';
        if (input != 'x')
        {
            PlayerMove(input);
        }
        TimerLogic();
    }
    public void TimerLogic()
    {
        time -= !gameover ? Time.deltaTime : 0;
        if (time <= -0.5)
        {
            GameOver("You ran out of time.");
        }
    }
    public char ProcessPlayerInput()
    {
        char direction = 'x';

        if (Input.GetButtonDown("Horizontal"))
        {
            direction = Input.GetAxisRaw("Horizontal") < 0 ? 'a' : 'd';
            moveCd = maxMoveCd;
        }
        else if (Input.GetButton("Horizontal"))
        {
            moveCd -= moveCd > -1f ? Time.deltaTime : 0;
            if (moveCd <= 0)
            {
                direction = Input.GetAxisRaw("Horizontal") < 0 ? 'a' : 'd';
                moveCd = maxMoveCd;
            }
        }
        else if (Input.GetButtonDown("Vertical"))
        {
            direction = Input.GetAxisRaw("Vertical") > 0 ? 'w' : 's';
            moveCd = maxMoveCd;
        }
        else if (Input.GetButton("Vertical"))
        {
            moveCd -= moveCd > -1f ? Time.deltaTime : 0;
            if (moveCd <= 0)
            {
                direction = Input.GetAxisRaw("Vertical") > 0 ? 'w' : 's';
                moveCd = maxMoveCd;
            }
        }
        return direction;
    }
    public void PlayerMove(char input)
    {
        int[] direction = new int[] { 0, 0 };
        switch (input)
        {
            case 'w':
                direction = new int[] { 1, 0 };
                break;
            case 's':
                direction = new int[] { -1, 0 };
                break;
            case 'a':
                direction = new int[] { 0, -1 };
                break;
            case 'd':
                direction = new int[] { 0, 1 };
                break;
        }

        int[] newPos = new int[] { playerPosition[0] + direction[0], playerPosition[1] + direction[1] };
        char tile = GetPosition(newPos[0], newPos[1]);

        if (tile == 'X' ||
        tile == stringKey["stump"])
        {
            // DOOR STUCK
            return;
        }
        Remove(playerPosition);
        Place(newPos, "player");
        playerPosition = newPos;
        if (direction[0] != 0)
        {
            playerSrc.PlayClip(0);
        }
        else
        {
            playerSrc.PlayClip(1);
        }

        if (tile == stringKey["core"])
        {
            EnCore(newPos);
            playerSrc.PlayClip(6, 1);
        }
        else if (tile == stringKey["apple"])
        {
            EnApple(newPos);
            playerSrc.PlayClip(5, 1);
        }
        else if (tile == stringKey["hourglass"])
        {
            EnHourglass();
            playerSrc.PlayClip(4, 1);
        }

        RememberSeeds();
        PrintBoard();
    }
    public void EnCore(int[] tile)
    {
        ShiftTime(corePenalty);
        seeds.Add(tile);
    }
    public void EnApple(int[] tile)
    {
        appleBonus = numApples * 2;
        apples.RemoveAt(0);
    }
    public void EnHourglass()
    {
        ShiftTime(hourglassBonus);

        cycles++;
        int applesCollected = (int)numApples - apples.Count;
        int a = applesCollected == (int)numApples ? applesCollected * 2 : applesCollected;
        bonus += a;
        numApples += appleInc;
        numCores += coreInc;

        if (numApples > maxApples)
        {
            numApples = maxApples;
        }
        if (numCores > maxCores)
        {
            numCores = maxCores;
        }

        RefreshBoard();
    }
    public void RemoveFrom(List<int[]> list, int[] item)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(item))
            {
                list.Remove(item);
                return;
            }
        }
    }
    public int GetCycles()
    {
        return cycles;
    }
    public int GetBonus()
    {
        return bonus;
    }
    public float GetTime()
    {
        return time;
    }
    public void ShiftTime(float num)
    {
        time += num;
    }

    public char[][] GetBoard()
    {
        return board;
    }
    public void RefreshBoard()
    {
        int count = 0;
        while (count < 100)
        {
            // clear board
            ClearBoard();
            PrintBoard();

            // check for seeds
            // replace seeds with stumps
            if (seeds.Count > 0)
            {
                stumpSrc.PlayClip(0);
            }
            while (seeds.Count > 0)
            {
                int[] pos = seeds[0];
                stumps.Add(pos);
                seeds.RemoveAt(0);
            }

            // place stumps
            for (int i = 0; i < stumps.Count; i++)
            {
                Place(stumps[i], "stump");
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
            for (int i = 0; i < (int)numApples; i++)
            {
                int[] pos = GetRandomEmptyPosition();
                Place(pos, "apple");
                apples.Add(pos);
            }

            // clear and put hourglass somewhere
            Remove(hourglassPosition);
            hourglassPosition = GetRandomEmptyPosition();
            int[] p = new int[] { hourglassPosition[0], hourglassPosition[1] };
            int[] ppos = new int[] { playerPosition[0], playerPosition[1] };
            while (hourglassPosition[0] == p[0] && hourglassPosition[1] == p[1])
            {
                hourglassPosition = GetRandomEmptyPosition();
            }
            Place(hourglassPosition, "hourglass");

            if (IsBoardPossible(board))
            {
                PrintBoard();
                return;
            }
            print(count + ": A board was found to be impossible.");
            PrintBoard();
            count++;
        }
    }
    public void RememberSeeds()
    {
        for (int i = 0; i < seeds.Count; i++)
        {
            int[] pos = seeds[i];
            if (GetPosition(pos[0], pos[1]) != stringKey["player"])
            {
                Place(pos, "seed");
            }
        }
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
    public void StartBoard()
    {
        board = new char[8][]
        {
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','P','E','E','H','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'}
        };
    }
    public char GetPosition(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < board.Length && y < board[0].Length)
        {
            return board[x][y];
        }
        return 'X';

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
                numgrid[i][j] = value == stringKey["stump"] || value == stringKey["core"] ? 1 : 0;

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
            PathAlgorithm.IsBoardPossible(numgrid, p, h);
    }
    public bool TestBoardPossible()
    {
        char[][] newboard = new char[8][]
        {
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'},
            new char[] {'E','E','P','E','E','E','E','E'},
            new char[] {'E','E','E','C','E','C','E','E'},
            new char[] {'E','E','E','E','C','E','E','E'},
            new char[] {'E','H','E','E','E','E','E','E'},
            new char[] {'E','E','E','E','E','E','E','E'}
        };
        return IsBoardPossible(newboard);
    }
    void printNumMap(int[][] numMap)
    {
        map = "";
        for (int i = numMap.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < numMap[i].Length; j++)
            {
                map += numMap[i][j] + " ";
            }
            map += "\n";
        }

    }
}
