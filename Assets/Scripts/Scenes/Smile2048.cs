using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Smile2048 : MonoBehaviour {
    const int ROW = 4, TOTAL = 16, INTER = 30, MOVE = 160;
    int _cnt = 0;
    const float TIMEDELAY = 0.2f;
    bool _bGameOver = false;
    public string[] colors = { "eee4da", "ede0c8", "f2b179", "f59563", "f67c5f", "f65e3b", "edcf72", "edcc61", "99cc00", "33b5e5", "0099cc" };
    Item[][] group = new Item[ROW][];
    int[][] nums = new int[ROW][];
    Transform goTips;
	AudioMgr adMgr;

	// Use this for initialization
	void Start () {
		initParas ();
		initEvent ();
		Invoke ("onClickStart", 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if ( Application.platform == RuntimePlatform.Android &&(Input.GetKeyDown(KeyCode.Escape)))  
		{
			Application.Quit ();
		}
	}

	void initParas(){
		adMgr = AudioMgr.getInstance ();
		goTips = transform.Find ("goTips");

        for (int i = 0; i < ROW; i++)
        {
            nums[i] = new int[] { 0, 0, 0, 0 };
            group[i] = new Item[ROW];
        }
        var goPlay = transform.Find("goPlay");
        for (int i = 0, l = goPlay.childCount; i < l; i++)
        {
            var r = (int)Mathf.Floor(i / ROW);
            var c = i % ROW;
            group[r][c] = goPlay.GetChild(i).GetComponent<Item>();
            group[r][c].init(this);
            group[r][c].gameObject.SetActive(false);
        }
    }

    void initEvent(){
        transform.Find("goTop/back").gameObject.GetComponent<Button>().onClick.AddListener (delegate() {
            SceneManager.LoadScene("Lobby");
        });
        transform.Find("btns/start").gameObject.GetComponent<Button>().onClick.AddListener(onClickStart);
		var goControl = transform.Find ("control");
		for (int k = 0; k < 4; k++) {
			var idx = k;
			var btn = goControl.GetChild (k).gameObject.GetComponent<Button> ();
			btn.onClick.AddListener (delegate() {
                if (_bGameOver) return;
                adMgr.PlaySound("click");
                switch (idx)
                {
                    case 0: 
                        onLeft();
                        break;
                    case 1: 
                        onUp();
                        break;
                    case 2: 
                        onDown();
                        break;
                    case 3: 
                        onRight();
                        break;
                }
            });
		}
	}
    //通过封装精简代码
    void playMove(Item itemMove, Item itemHide, int mx, int my, bool bMerge, Item itemMerge)
    {
        var pos = itemMove.GetComponent<RectTransform>().anchoredPosition;
        var x = pos.x;
        var y = pos.y;
        itemHide.gameObject.SetActive(false);
        itemMove.transform.localPosition = itemHide.transform.localPosition;
        itemMove.playMove(x, y, mx, my, bMerge, itemMerge);
    }

    void onClickStart(){
        _cnt = 0;
        _bGameOver = false;
        goTips.gameObject.SetActive(false);
        initNumShow();
    }

    void initNumShow()
    {
        for (int i = 0; i < ROW; i++) 
        {
            for (int j = 0; j < ROW; j++)
            {
                nums[i][j] = 0;
                group[i][j].gameObject.SetActive(false);
            }
        }
        var rand1 = Random.Range(0, TOTAL);
        var rand2 = Random.Range(1, TOTAL-1);
        //rand1 = 2;
        //rand2 = 3;
        if (rand1 == rand2) rand2--;
        var c1 = rand1 % ROW;
        var r1 = (int)Mathf.Floor(rand1 / ROW);
        nums[r1][c1] = 2;
        var c2 = rand2 % ROW;
        var r2 = (int)Mathf.Floor(rand2 / ROW);
        nums[r2][c2] = 2;
        group[r1][c1].showLab(2);
        group[r1][c1].playScale();
        group[r2][c2].showLab(2);
        group[r2][c2].playScale();
        _cnt += 2;
    }
    //新生成方块
    void onAddItem(){
        var rand = Random.Range(0, TOTAL-_cnt);
        //rand = 12;
        for (var i = 0; i < ROW; i++)
        {
            for (var j = 0; j < ROW; j++)
            {
                if (nums[i][j] == 0 && --rand < 0)
                {
                    var num = Random.Range(0, 10) < 5 ? 2 : 4;
                    nums[i][j] = num;
                    group[i][j].showLab(num);
                    group[i][j].playScale();
                    _cnt++;
                    if (!_bGameOver) checkLose();
                    return;
                }
            }
        }
    }

    void showTipsSP(string str){
		goTips.GetChild (0).GetComponent<Text> ().text = str;
		goTips.gameObject.SetActive (true);
	}

    bool checkUp()
    {
        for (var i = 1; i < ROW; i++)
        {
            for (var j = 0; j < ROW; j++)
            {
                if (nums[i][j] != 0)
                {
                    if (nums[i - 1][j] == 0 || nums[i - 1][j] == nums[i][j]) return true;
                }
            }
        }
        return false;
    }
    bool checkDown()
    {
        for (var i = 0; i < ROW-1; i++)
        {
            for (var j = 0; j < ROW; j++)
            {
                if (nums[i][j] != 0)
                {
                    if (nums[i + 1][j] == 0 || nums[i + 1][j] == nums[i][j]) return true;
                }
            }
        }
        return false;
    }
    bool checkLeft()
    {
        for (var i = 0; i < ROW; i++)
        {
            for (var j = 1; j < ROW; j++)
            {
                if (nums[i][j] != 0)
                {
                    if (nums[i][j-1] == 0 || nums[i][j-1] == nums[i][j]) return true;
                }
            }
        }
        return false;
    }
    bool checkRight()
    {
        for (var i = 0; i < ROW; i++)
        {
            for (var j = 0; j < ROW-1; j++)
            {
                if (nums[i][j] != 0)
                {
                    if (nums[i][j + 1] == 0 || nums[i][j + 1] == nums[i][j]) return true;
                }
            }
        }
        return false;
    }

    void checkWin(int num)
    {
        if (num == 1024) //win
        {
            adMgr.PlaySound("win");
            showTipsSP("Win");
            _bGameOver = true;
        }
    }

    void checkLose()
    {
        if (_cnt == TOTAL && !checkRight() && !checkLeft() && !checkUp() && !checkDown())
        {
            adMgr.PlaySound("lose");
            showTipsSP("Lose");
            _bGameOver = true;
        }
    }

    public int getIdx(int iNum)
    {
        switch (iNum)
        {
            case 2: return 0;
            case 4: return 1;
            case 8: return 2;
            case 16: return 3;
            case 32: return 4;
            case 64: return 5;
            case 128: return 6;
            case 256: return 7;
            case 512: return 8;
            case 1024: return 9;
            case 2048: return 10;
        }
        return 0;
    }

    void onLeft()
    {
        if (checkLeft())
        {
            for (int i = 0; i < ROW; i++)
            {
                int cnt = 0;
                bool bMerge = false;
                for (int j = 0; j < ROW; j++)
                {
                    int num = nums[i][j];
                    if (num != 0)
                    {
                        if (!bMerge && cnt > 0)
                        {
                            for (int a = j - 1; a >= 0; a--)
                            {
                                if (nums[i][a] != 0)
                                {
                                    if (nums[i][a] == num)
                                    {
                                        nums[i][a] = num * 2;
                                        nums[i][j] = 0;
                                        if (j == cnt)
                                        {
                                            group[i][a].showLab(num * 2);
                                            group[i][j].gameObject.SetActive(false);
                                        }
                                        else
                                        {
                                            var pos = group[i][j].GetComponent<RectTransform>().anchoredPosition;
                                            group[i][j].playMove(pos.x, pos.y, -MOVE * (j - cnt) - INTER, 0, true, group[i][a]);
                                        }
                                        cnt--;
                                        bMerge = true;
                                        checkWin(num);
                                        _cnt--;
                                    }
                                    break;
                                }
                            }
                        }
                        if (nums[i][j] != 0 && j != cnt)
                        {
                            nums[i][cnt] = num;
                            nums[i][j] = 0;
                            group[i][cnt].showLab(num);
                            playMove(group[i][cnt], group[i][j], -MOVE * (j - cnt), 0, false, null);
                        }
                        cnt++;
                    }
                }
            }
            Invoke("onAddItem", TIMEDELAY);
        }
    }

    void onUp()
    {
        if (checkUp())
        {
            for (int i = 0; i < ROW; i++)
            {
                int cnt = 0;
                bool bMerge = false;
                for (int j = 0; j < ROW; j++)
                {
                    int num = nums[j][i];
                    if (num != 0)
                    {
                        if (!bMerge && cnt > 0)
                        {
                            for (int a = j - 1; a >= 0; a--)
                            {
                                if (nums[a][i] != 0)
                                {
                                    if (nums[a][i] == num)
                                    {
                                        nums[a][i] = num * 2;
                                        nums[j][i] = 0;
                                        if (j == cnt)
                                        {
                                            group[a][i].showLab(num * 2);
                                            group[j][i].gameObject.SetActive(false);
                                        }
                                        else
                                        {
                                            var pos = group[j][i].GetComponent<RectTransform>().anchoredPosition;
                                            group[j][i].playMove(pos.x, pos.y, 0, MOVE * (j - cnt) + INTER, true, group[a][i]);
                                        }
                                        cnt--;
                                        bMerge = true;
                                        checkWin(num);
                                        _cnt--;
                                    }
                                    break;
                                }
                            }
                        }
                        if (nums[j][i] != 0 && j != cnt)
                        {
                            nums[cnt][i] = num;
                            nums[j][i] = 0;
                            group[cnt][i].showLab(num);
                            playMove(group[cnt][i], group[j][i], 0, MOVE * (j - cnt), false, null);
                        }
                        cnt++;
                    }
                }
            }
            Invoke("onAddItem", TIMEDELAY);
        }
    }

    void onDown()
    {
        if (checkDown())
        {
            for (int i = 0; i < ROW; i++)
            {
                int cnt = 0;
                bool bMerge = false;
                for (int j = ROW-1; j >= 0; j--)
                {
                    int num = nums[j][i];
                    if (num != 0)
                    {
                        int tmp = ROW - 1 - cnt;
                        if (!bMerge && cnt > 0)
                        {
                            for (int a = j + 1; a < ROW; a++)
                            {
                                if (nums[a][i] != 0)
                                {
                                    if (nums[a][i] == num)
                                    {
                                        nums[a][i] = num * 2;
                                        nums[j][i] = 0;
                                        if (j == tmp)
                                        {
                                            group[a][i].showLab(num * 2);
                                            group[j][i].gameObject.SetActive(false);
                                        }
                                        else
                                        {
                                            var pos = group[j][i].GetComponent<RectTransform>().anchoredPosition;
                                            group[j][i].playMove(pos.x, pos.y, 0, MOVE * (j - tmp) - INTER, true, group[a][i]);
                                        }
                                        cnt--;
                                        bMerge = true;
                                        checkWin(num);
                                        _cnt--;
                                    }
                                    break;
                                }
                            }
                        }
                        if (nums[j][i] != 0 && j != tmp)
                        {
                            nums[tmp][i] = num;
                            nums[j][i] = 0;
                            group[tmp][i].showLab(num);
                            playMove(group[tmp][i], group[j][i], 0, MOVE * (j - tmp), false, null);
                        }
                        cnt++;
                    }
                }
            }
            Invoke("onAddItem", TIMEDELAY);
        }
    }

    void onRight()
    {
        if (checkRight())
        {
            for (int i = 0; i < ROW; i++)
            {
                int cnt = 0;
                bool bMerge = false;
                for (int j = ROW-1; j >= 0; j--)
                {
                    int num = nums[i][j];
                    if (num != 0)
                    {
                        int tmp = ROW - 1 - cnt;
                        if (!bMerge && cnt > 0)
                        {
                            for (int a = j + 1; a < ROW; a++)
                            {
                                if (nums[i][a] != 0)
                                {
                                    if (nums[i][a] == num)
                                    {
                                        nums[i][a] = num * 2;
                                        nums[i][j] = 0;
                                        if (j == tmp)
                                        {
                                            group[i][a].showLab(num * 2);
                                            group[i][j].gameObject.SetActive(false);
                                        }
                                        else
                                        {
                                            var pos = group[i][j].GetComponent<RectTransform>().anchoredPosition;
                                            group[i][j].playMove(pos.x, pos.y, -MOVE * (j - tmp) + INTER, 0, true, group[i][a]);
                                        }
                                        cnt--;
                                        bMerge = true;
                                        checkWin(num);
                                        _cnt--;
                                    }
                                    break;
                                }
                            }
                        }
                        if (nums[i][j] != 0 && j != tmp)
                        {
                            nums[i][tmp] = num;
                            nums[i][j] = 0;
                            group[i][tmp].showLab(num);
                            playMove(group[i][tmp], group[i][j], -MOVE * (j - tmp), 0, false, null);
                        }
                        cnt++;
                    }
                }
            }
            Invoke("onAddItem", TIMEDELAY);
        }
    }
}
