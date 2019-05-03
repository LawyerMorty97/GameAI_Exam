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

    public Material lineMaterial;
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
        _line.material = lineMaterial;
        _line.startWidth = 0.28f;
        _line.endWidth = 0.28f;
        _line.startColor = Color.red;
        _line.endColor = Color.black;

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
        _grid.ShowGrid();
        _grid.ResetGrid();
        _path = null;

        _line.positionCount = 0;
    }

    IEnumerator RenderLine()
    {
        _grid.HideGrid();
        _path.Reverse();
        int i = 0;
        foreach (Node node in _path)
        {
            _line.SetPosition(i, new Vector3(_offset.x + node.x, 1f, _offset.y + node.y));
            i += 1;
            yield return new WaitForSeconds(0.05f);
        }
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

        switchStage:
        switch (_stage)
        {
            case Stage.PlottingBlock:
                helper.text = "Click on any square to toggle it (If you create a block path, the creator will force you to repeat)\nPress space to plot a starting point";
                break;
            case Stage.PlottingStart:
                helper.text = "Click on any square to place a start position\nPress space to plot an ending point";
                break;
            case Stage.PlottingEnd:
                helper.text = "Click on any square to place an end position\nPress space to run the pathfinder";
                break;
            case Stage.Run:
                _path = _pather.AStar(_tempStart, _tempEnd);

                if (_path.Count == 0)
                {
                    _stage = Stage.PlottingBlock;
                    goto switchStage;
                }

                helper.text = "Press Enter to Play\nPress Space to Repath";

                if (_path.Count > 0)
                {
                    _line.positionCount = _path.Count;
                    StartCoroutine(RenderLine());
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_game.GetState() != GameManager.GameState.Drawing)
        {
            if (helper.text != "")
                helper.text = "";
            return;
        }

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
                }
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
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_stage == Stage.Run)
            {
                ResetEditor();
                _game.DrawingComplete();
            }
        }
    }
}
