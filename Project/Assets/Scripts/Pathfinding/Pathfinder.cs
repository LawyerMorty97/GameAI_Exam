using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private Grid _grid;

    public Pathfinder()
    {
        _grid = Grid.GetInstance();
    }

    public void BlockNode(Node n)
    {
        n.walkable = false;
        n.mat.color = Color.black;
    }

    public Node GetNode(int x, int y)
    {
        Node n = null;

        lock(_grid)
        {
            n = _grid.GetNode(x, y);
        }

        return n;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> n = new List<Node>();

        /* Offset the current node's X position by -1 to 1
         * Example: node.X = 10, The loop will check 9, and 11
        */
        for  (int x = -1; x <= 1; x++)
        {
            /* Offset the current node's Y position by -1 to 1
             * Example: node.Y = 5, The loop will check 4 and 6
             */
             for (int y = -1; y <= 1; y++)
            {
                // Since we already know the value of the node, we do not check 0
                if (x == 0 && y == 0)
                {

                }
                else
                {
                    /*
                     * Create new nodes in the offsetted positions.
                     * Check their neighbourhoods for walkable nodes
                     * 
                     * The positions of this node will be:
                     *  - Each corner
                     *  - Each side
                     */
                    Node child = new Node();
                    child.x = node.x + x;
                    child.y = node.y + y;

                    Node child_neighbourhood = GetNeighbouringNode(child, node);

                    if (child_neighbourhood != null)
                        n.Add(child_neighbourhood);
                }
            }
        }

        return n;
    }

    public Node GetNeighbouringNode(Node position, Node current)
    {
        Node neighbouring = null;
        Node node = GetNode(position.x, position.y);

        if (node != null && node.walkable)
        {
            neighbouring = node;
        }

        int diagonalX = position.x - current.x;
        int diagonalY = position.y - current.y;

        // Cross check each side and corner and make sure they're accessible
        if (Mathf.Abs(diagonalX) == 1 && Mathf.Abs(diagonalY) == 1)
        {
            Node xNode = GetNode(current.x + diagonalX, current.y + diagonalY);
            if (xNode == null || !xNode.walkable)
                neighbouring = null;

            /*Node yNode = GetNode(current.x, current.y + diagonalY);
            if (yNode == null || !yNode.walkable)
                neighbouring = null;*/
        }

        return neighbouring;
    }

    public int Distance(Node a, Node b)
    {
        int x = Mathf.Abs(b.x - a.x);
        int y = Mathf.Abs(b.y - a.y);

        if (x > y)
            return y * (x - y);

        return x * (y - x);
    }

    public List<Node> AStar(Node start, Node end)
    {
        List<Node> path = new List<Node>();

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        open.Add(start);

        while(open.Count > 0)
        {
            Node current = open[0];

            // Check scores of each node
            for (int i = 0; i < open.Count; i++)
            {
                bool isNextFBigger = open[i].F < current.F;

                bool areFEqual = open[i].F == current.F;
                bool isNextHBigger = open[i].H < current.H;

                if (isNextFBigger || (areFEqual && isNextFBigger))
                    if (!current.Equals(open[i]))
                        current = open[i];
            }

            open.Remove(current);
            closed.Add(current);

            if (current.Equals(end))
            {
                Reconstruct(start, current);
                break;
            }

            // Check the neighbourhood of each current node we pass
            foreach(Node n in GetNeighbours(current))
            {
                if (closed.Contains(n))
                    continue;

                // Tentative G Score
                int G = current.G + Distance(current, n);

                if (!open.Contains(n))
                    open.Add(n);
                else if (G >= n.G)
                    continue;

                n.G = G;
                n.H = Distance(n, end);
                n.parent = current;

                /*if (G > n.G || !open.Contains(n))
                {
                    n.G = G;
                    n.H = Distance(n, end);
                    n.parent = current;
                    if (!open.Contains(n))
                        open.Add(n);
                }*/
            }
        }

        return path;
    }

    /*
     * Reconstruct the path from the end until the start
     * How it works:
     *  Start at the end point
     *  Follow EACH parent until the start
     */
    public List<Node> Reconstruct(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }

        //path.Reverse();

        return path;
    }
}
