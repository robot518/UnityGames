using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class D3Minesweeper : MonoBehaviour, IPointerClickHandler
{
    const int COUNT = 5, TOTAL = COUNT* COUNT* COUNT, MINES = 10, AREA = COUNT*COUNT;
    int[] _tBtns = new int[TOTAL];
    int[] _tNum = new int[TOTAL];
    bool _bGameOver = false;
    public Transform cube;
    public Material[] materials;
    AudioMgr adMgr;

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
        
    }

    void initParas()
    {
        adMgr = AudioMgr.getInstance();
    }
    void initEvent()
    {
        transform.Find("back").GetComponent<Button>().onClick.AddListener(delegate
        {
            SceneManager.LoadScene("Lobby");
        });
        var btnStart = transform.Find("start").gameObject.GetComponent<Button>();
        btnStart.onClick.AddListener(onStart);
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
                for (var x = row - 1; x < row + 2; x++) for (var y = col - 1; y < col + 2; y++) for (var z = depth - 1; z < depth + 2; z++)
                        {
                            if (x == 0 || y == 0 || z == 0 || x == COUNT - 1 || y == COUNT - 1 || z == COUNT - 1)
                            {
                                var idx = z * AREA + x * COUNT + y;
                                if (_tBtns[idx] == 1) showGrids(idx);
                            }
                        };
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
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData);
        var startPoint = Camera.main.transform.position;
        //从摄像头发射一条射线，到鼠标点击的位置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //射线碰撞到的地方
        RaycastHit hit;
        //这里不懂的可以查看API。参数依次是表示射线，射线碰撞的位置，射线的距离,检测的层。
        if (Physics.Raycast(ray, out hit, 100, 1 << 8))
        {
            var endPoint = hit.point;
            //如果碰撞的是地面
            if (hit.collider.gameObject.name == "Terrain")
            {
                // hit.point 就是表示点击的碰撞的哪个位置;
                Debug.Log("我碰撞到了地面");
            }
            else
            {
                Debug.Log("我碰撞到了其他物体");
            }
        }
    }
}
