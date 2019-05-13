using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class D3Minesweeper : MonoBehaviour
{
    const int COUNT = 5, TOTAL = COUNT* COUNT* COUNT, MINES = 18, AREA = COUNT*COUNT;
    int[] _tBtns = new int[TOTAL];
    int[] _tNum = new int[TOTAL];
    bool _bGameOver = false;
    public Transform cube;
    public Transform cam;
    public Material[] materials;
    AudioMgr adMgr;
    ArrayList group = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        initParas();
        initEvent();
        initGrids();
        Invoke("onStart", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Transform trans = hitInfo.collider.transform;
                var idx = group.IndexOf(trans);
                onClick(idx);
            }
        }
    }

    void initParas()
    {
        adMgr = AudioMgr.getInstance();
    }
    void initEvent()
    {
        transform.Find("top/back").GetComponent<Button>().onClick.AddListener(delegate
        {
            SceneManager.LoadScene("Lobby");
        });
        var btnStart = transform.Find("down/start").gameObject.GetComponent<Button>();
        btnStart.onClick.AddListener(onStart);
        transform.Find("down/reset").GetComponent<Button>().onClick.AddListener(delegate
        {
            cam.localPosition = new Vector3(0, 0, -10);
            cam.localRotation = Quaternion.identity;
        });
    }
    void initGrids()
    {
        var offset = (COUNT - 1) / 2;
        for (var i = 0; i < TOTAL; i++)
        {
            var z = Mathf.Floor(i / AREA);
            var left = i % AREA;
            var x = left % COUNT;
            var y = Mathf.Floor(left / COUNT);
            var item = i == 0 ? cube : Instantiate(cube);
            item.parent = cube.parent;
            item.localPosition = new Vector3(x - offset, offset - y, offset-z);
            group.Add(item);
        }
    }
    void initMines()
    {
        var tNum = new List<int>();
        var tMineNum = new int[TOTAL];
        for (var k = 0; k < TOTAL; k++)
        {
            var depth = (int)Mathf.Floor(k / AREA);
            var left = k % AREA;
            var col = left % COUNT;
            var row = (int)Mathf.Floor(left / COUNT);
            if (depth == 0 || depth == TOTAL - 1 || row == 0 || row == TOTAL - 1 || col == 0 || col == TOTAL - 1)
                tNum.Add(k);
            tMineNum[k] = 0;
            _tNum[k] = 0;
            _tBtns[k] = 1;
        };
        for (var i = 0; i < MINES; i++)
        {
            var iRandom = Random.Range(0, tNum.Count);
            var iNum = tNum[iRandom];
            tNum.RemoveAt(iRandom);
            tMineNum[iNum] = 1;
        };
        for (var k = 0; k < TOTAL; k++)
        {
            if (tMineNum[k] == 1)
            {
                _tNum[k] = -1;
                var depth = (int)Mathf.Floor(k / AREA);
                var left = k % AREA;
                var col = left % COUNT;
                var row = (int)Mathf.Floor(left / COUNT);
                for (int x = row - 1; x < row + 2; x++)
                {
                    for (int y = col - 1; y < col + 2; y++)
                    {
                        for (int z = depth-1; z < depth+2; z++)
                        {
                            if (x > -1 && y > -1 && z > -1 && x < COUNT && y < COUNT && z < COUNT)
                            {
                                int idx = z * AREA+COUNT * x + y;
                                if (_tNum[idx] != -1 && _tNum[idx] < 8) _tNum[idx]++;
                            }
                        }
                    }
                }

            }
        };
    }
    void showGrids(int k)
    {
        var iLabNum = _tNum[k];
        if (iLabNum != -1)
        {
            _tBtns[k] = 0;
            if (iLabNum == 0)
            {
                var depth = (int)Mathf.Floor(k / AREA);
                var left = k % AREA;
                var col = left % COUNT;
                var row = (int)Mathf.Floor(left / COUNT);
                for (var x = row - 1; x < row + 2; x++)
                {
                    for (var y = col - 1; y < col + 2; y++)
                    {
                        for (var z = depth - 1; z < depth + 2; z++)
                        {
                            if (x == 0 || y == 0 || z == 0 || x == COUNT - 1 || y == COUNT - 1 || z == COUNT - 1)
                            {
                                var idx = z * AREA + x * COUNT + y;
                                if (idx >= 0 && idx < TOTAL && _tBtns[idx] == 1) showGrids(idx);
                            }
                        };
                    }
                }
            }
        }
    }
    void showBtns()
    {
        var p = cube.parent;
        for (var i = 0; i < TOTAL; i++)
        {
            if (_tBtns[i] == 0) p.GetChild(i).GetComponent<Renderer>().material = materials[1+_tNum[i]];
            else p.GetChild(i).GetComponent<Renderer>().material = materials[0];
        };
    }
    bool checkWin()
    {
        for (var k = 0; k < TOTAL; k++)
        {
            var depth = (int)Mathf.Floor(k / AREA);
            var left = k % AREA;
            var col = left % COUNT;
            var row = (int)Mathf.Floor(left / COUNT);
            if (depth == 0 || depth == COUNT - 1 || row == 0 || row == COUNT - 1 || col == 0 || col == COUNT - 1)
                if (_tBtns[k] == 1 && _tNum[k] != -1) return false;
        }
        return true;
    }
    void onStart()
    {
        _bGameOver = false;
        initMines();
        showBtns();
    }
    void onClick(int idx)
    {
        if (!_bGameOver)
        {
            if (_tNum[idx] == -1)
            { //地雷
                adMgr.PlaySound("bomb");
                cube.parent.GetChild(idx).GetComponent<Renderer>().material = materials[10];
                _bGameOver = true;
                //showResult();
            }
            else if (_tBtns[idx] == 1)
            {
                showGrids(idx);
                showBtns();
                _bGameOver = checkWin();
                if (_bGameOver)
                {
                    adMgr.PlaySound("win");
                    //showResult();
                }
                else adMgr.PlaySound("check");
            }
        }
    }
}
