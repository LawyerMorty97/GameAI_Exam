using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 GridBase;
    private static Grid _inst;

    public static Grid GetInstance()
    {
        return _inst;
    }

    public GameObject prefab;

    public int MaxWidth = 10;
    public int MaxHeight = 30;
    private float offset = 0.5f;

    private Node[,] grid;
    private Dictionary<GameObject, Node> objects;

    public Pathfinder pf;

    private Vector2 _startPos;
    private Vector2 _endPos;

    public void ResetGrid()
    {
        for (int x = 0; x < MaxWidth; x++)
            for (int y = 0; y < MaxHeight; y++)
            {
                ToggleNode(grid[x, y], NodeType.Empty);
            }
    }

    public void ShowGrid()
    {
        for (int x = 0; x < MaxWidth; x++)
            for (int y = 0; y < MaxHeight; y++)
            {
                grid[x, y].obj.SetActive(true);
            }
    }

    public void HideGrid(bool hideWalls = true)
    {
        for (int x = 0; x < MaxWidth; x++)
            for (int y = 0; y < MaxHeight; y++)
            {
                if (!hideWalls)
                {
                    if (grid[x, y].walkable)
                    {
                        grid[x, y].obj.SetActive(false);
                    }
                }
                else
                {
                    grid[x, y].obj.SetActive(false);
                }
            }
    }

    public Node GetNode(int x, int y)
    {
        Node n = null;

        if (x >= 0 && x < MaxWidth && y >= 0 && y < MaxHeight)
        {
            n = grid[x, y];
        }

        return n;
    }

    public Node GetNodeFromObject(GameObject gObject)
    {
        Node n = null;

        if (objects.ContainsKey(gObject))
            n = objects[gObject];

        return n;
    }

    public Node GetNode(Vector2 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.x);

        return GetNode(x, y);
    }

    public void ToggleNode(Node n, NodeType e)
    {
        switch(e)
        {
            case NodeType.Block:
                n.walkable = false;
                n.mat.color = Color.black;
                break;
            default:
                n.walkable = true;
                n.mat.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                break;
        }
    }

    public enum NodeType
    {
        Block,
        Empty
    }

    void Awake()
    {
        _inst = this;
    }

    void Start()
    {
        objects = new Dictionary<GameObject, Node>();
        // Create a Grid object to store stuff in
        GameObject pathObjs = new GameObject("Path");
        pathObjs.transform.SetParent(transform);

        grid = new Node[MaxWidth, MaxHeight];

        for (int x = 0; x < MaxWidth; x++)
        {
            for (int y = 0; y < MaxHeight; y++)
            {
                Vector3 position = new Vector3(GridBase.x + x + offset, 0f,GridBase.y + y + offset);

                GameObject obj = Instantiate(prefab);
                obj.name = "Path[" + x + "." + y + "]";
                obj.transform.SetParent(pathObjs.transform);
                obj.transform.position = position;

                Material mat = obj.GetComponent<Renderer>().material;
                mat.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

                Node node = new Node()
                {
                    x = x,
                    y = y,
                    obj = obj,
                    mat = mat,
                    walkable = true
                };

                grid[x, y] = node;
                objects.Add(obj, node);
            }
        }
    }
}
