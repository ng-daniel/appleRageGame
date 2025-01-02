using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRenderScript : MonoBehaviour
{

    GameBoardScript game;

    Dictionary<string, char> stringKey = new Dictionary<string, char>();
    [SerializeField] GameObject player;
    [SerializeField] GameObject hourglass;
    [SerializeField] List<GameObject> stumps;
    [SerializeField] List<GameObject> seeds;
    [SerializeField] List<GameObject> apples;
    [SerializeField] List<GameObject> cores;

    public static Vector2 hiddenPosition = new Vector2(0, -8);

    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<GameBoardScript>();
        stringKey = GameBoardScript.stringKey;
        InitializeRender();
    }
    void Update()
    {
        RenderBoard();
    }
    void RenderBoard()
    {
        HideAll(stumps);
        HideAll(seeds);
        HideAll(apples);
        HideAll(cores);

        //HideObject(player);
        HideObject(hourglass);

        char[][] board = game.GetBoard();
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                char tile = board[i][j];
                if (tile == stringKey["empty"])
                {
                    continue;
                }
                if (tile == stringKey["core"])
                {
                    RenderObject(NextReadyObject(cores), i, j);
                }
                if (tile == stringKey["apple"])
                {
                    RenderObject(NextReadyObject(apples), i, j);
                }
                if (tile == stringKey["stump"])
                {
                    RenderObject(NextReadyObject(stumps), i, j);
                }
                if (tile == stringKey["seed"])
                {
                    RenderObject(NextReadyObject(seeds), i, j);
                }
                if (tile == stringKey["player"])
                {
                    SmoothMoveObject(player, i, j, 0.005f);
                    //RenderObject(player, i, j);
                }
                if (tile == stringKey["hourglass"])
                {
                    RenderObject(hourglass, i, j);
                }
            }
        }
    }
    void InitializeRender()
    {
        player = Instantiate(player, new Vector2(3, 1), transform.rotation);
        player.transform.parent = transform;
        hourglass = Instantiate(hourglass);
        hourglass.transform.parent = transform;
        InstantiateAll(stumps);
        InstantiateAll(seeds);
        InstantiateAll(apples);
        InstantiateAll(cores);
    }
    void InstantiateAll(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject obj = Instantiate(list[i]);
            obj.transform.parent = transform;
            list[i] = obj;
        }
    }
    void HideAll(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            HideObject(list[i]);
        }
    }
    void SmoothMoveObject(GameObject obj, int x, int y, float time)
    {
        Vector2 start = new Vector2(obj.transform.position.x, obj.transform.position.y);
        Vector2 end = new Vector2(y, x);
        StartCoroutine(SmoothMoveCoroutine(obj, start, end, time, Vector2.zero));
    }
    void RenderObject(GameObject obj, int x, int y)
    {
        obj.transform.position = new Vector2(y, x);
    }
    void HideObject(GameObject obj)
    {
        obj.transform.position = hiddenPosition;
    }
    bool IsActive(GameObject obj)
    {
        return obj.transform.position.y > hiddenPosition.y / 2;
    }
    GameObject NextReadyObject(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject obj = list[i];
            if (!IsActive(obj))
            {
                return obj;
            }
        }
        return null;
    }
    IEnumerator SmoothMoveCoroutine(GameObject obj, Vector2 start, Vector2 end, float time, Vector2 velocity)
    {
        obj.transform.position = Vector2.SmoothDamp(obj.transform.position, end, ref velocity, time);
        time -= Time.deltaTime;
        if (time > 0)
        {
            yield return null;
        }
        //obj.transform.position = end;
    }
}
