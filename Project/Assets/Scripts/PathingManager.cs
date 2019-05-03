using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathingManager : MonoBehaviour
{
    private enum Stage
    {
        Start,
        PlottingBlock,
        PlottingStart,
        PlottingEnd,
        Run
    }

    public Text helper;

    private GameManager _game;
    private Grid _grid;
    private Pathfinder _pather;
    private Vector2 _offset;
    private LineRenderer _line;

    private Stage _stage = Stage.Start;

    private Node _tempStart;
    private Node _tempEnd;
    private List<Node> _path;

    // Start is called before the first frame update
    void Start()
    {
        _game = GameManager.instance;
        _grid = Grid.GetInstance();
        _pather = new Pathfinder();
        _line = gameObject.AddComponent<LineRenderer>();

        _offset = new Vector2(_grid.GridBase.x + 0.5f, _grid.GridBase.y + 0.5f);
    }

    public void StartDrawing()
    {
        _stage = Stage.Start;
        AdvanceStage();
    }

    private void ResetEditor()
    {
        _tempStart = null;
        _tempEnd = null;
        _grid.ResetGrid();
        _path = null;
    }

    private void AdvanceStage()
    {
        if (_stage == Stage.Start)
        {
            _stage = Stage.PlottingBlock;
            ResetEditor();
        }
        else if (_stage == Stage.PlottingBlock)
            _stage = Stage.PlottingStart;
        else if (_stage == Stage.PlottingStart)
            _stage = Stage.PlottingEnd;
        else if (_stage == Stage.PlottingEnd)
            _stage = Stage.Run;
        else if (_stage == Stage.Run)
        {
            _stage = Stage.Start;
            AdvanceStage();
            return;
        }

        switch (_stage)
        {
            case Stage.PlottingBlock:
                helper.text = "Click on any square to toggle it\nPress space to plot a starting point";
                break;
            case Stage.PlottingStart:
                helper.text = "Click on any square to place a start position\nPress space to plot an ending point";
                break;
            case Stage.PlottingEnd:
                helper.text = "Click on any square to place an end position\nPress space to run the pathfinder";
                break;
            case Stage.Run:
                helper.text = "Press Enter to Play\nPress Space to Repath";
                _path = _pather.AStar(_tempStart, _tempEnd);

                if (_path.Count > 0)
                {
                    _line.positionCount = _path.Count + 1;
                    int i = 0;
                    foreach (Node node in _path)
                    {
                        _line.SetPosition(i, new Vector3(node.x, 0.25f, node.y));
                        i += 1;
                    }
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_game.GetState() != GameManager.GameState.Drawing)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (_grid.GetNodeFromObject(hit.transform.gameObject) != null)
                {
                    Node node = _grid.GetNodeFromObject(hit.transform.gameObject);

                    if (_stage == Stage.PlottingBlock)
                    {
                        if (node.walkable)
                            _grid.ToggleNode(node, Grid.NodeType.Block);
                        else
                            _grid.ToggleNode(node, Grid.NodeType.Empty);
                    }
                    else if (_stage == Stage.PlottingStart)
                    {
                        if (!node.walkable)
                            return;

                        if (_tempStart != null)
                        {
                            _tempStart.mat.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                        }
                        _tempStart = node;
                        _tempStart.mat.color = Color.green;
                    }
                    else if (_stage == Stage.PlottingEnd)
                    {
                        if (!node.walkable)
                            return;

                        if (node.x == _tempStart.x && node.y == _tempStart.y)
                            return;

                        if (_tempEnd != null)
                        {
                            _tempEnd.mat.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                        }
                        _tempEnd = node;
                        _tempEnd.mat.color = Color.red;
                    }
                    /*if (Input.GetMouseButtonDown(0))
                    {
                        if (node.walkable)
                            _grid.ToggleNode(node, Grid.NodeType.Block);
                        else
                            _grid.ToggleNode(node, Grid.NodeType.Empty);
                    }*/
                }
                /*Vector2 nodePosition = new Vector2(hit.transform.position.x, hit.transform.position.z) - _offset;
                if (_grid.GetNode(nodePosition) != null)
                {
                    Node node = _grid.GetNode(nodePosition);
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (node.walkable)
                            _grid.ToggleNode(node, Grid.NodeType.Block);
                        else
                            _grid.ToggleNode(node, Grid.NodeType.Empty);
                    }
                }*/

                //Debug.Log("Clicked on: " + hit.transform.name);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_stage != Stage.PlottingStart && _stage != Stage.PlottingEnd)
                AdvanceStage();
            else
            {
                if (_stage == Stage.PlottingStart && _tempStart != null)
                    AdvanceStage();

                if (_stage == Stage.PlottingEnd && _tempEnd != null)
                    AdvanceStage();
            }
        }
    }
}
