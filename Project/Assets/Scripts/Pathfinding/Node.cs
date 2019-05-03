using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int x;
    public int y;

    public int G, H = 0;
    public int F
    {
        get
        {
            return G + H;
        }
    }

    public Node parent;
    public bool walkable;

    public GameObject obj;
    public Material mat;
}