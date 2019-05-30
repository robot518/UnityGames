using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Smile2048 : MonoBehaviour {
    const int ROW = 4, TOTAL = 16;
    bool _bGameOver = false;
    public string[] colors = { "eee4da", "ede0c8", "f2b179", "f59563", "f67c5f", "f65e3b", "edcf72", "edcc61", "99cc00", "33b5e5", "0099cc" };
    public int[] sizes = { };
    public string[] numColors = { "eee4da", "ede0c8", "f2b179", "f59563", "f67c5f", "f65e3b", "edcf72", "edcc61", "99cc00", "33b5e5", "0099cc" };
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
		for (int i = 0; i < 4; i++) {
			var idxTemp = i;
			var btn = goControl.GetChild (i).gameObject.GetComponent<Button> ();
			btn.onClick.AddListener (delegate() {
				
			});
		}
	}

	void onClickStart(){
        _bGameOver = false;
        initNumShow();
    }

    void initNumShow()
    {
        for (int i = 0; i < ROW; i++) 
        {
            for (int j = 0; j < ROW; j++)
            {
                nums[i][j] = 0;
            }
        }
        var rand1 = Random.Range(0, TOTAL);
        var rand2 = Random.Range(1, TOTAL-1);
        if (rand1 == rand2) rand2--;
        var c1 = rand1 % ROW;
        var r1 = (int)Mathf.Floor(rand1 / ROW);
        nums[r1][c1] = 2;
        var c2 = rand2 % ROW;
        var r2 = (int)Mathf.Floor(rand2 / ROW);
        nums[r2][c2] = 2;
        group[r1][c1].play();
        group[r1][c1].showLab(2);
        group[r2][c2].play();
        group[r2][c2].showLab(2);
    }

    void onAddItem(){

	}

	void showTipsSP(string str){
		goTips.GetChild (0).GetComponent<Text> ().text = str;
		goTips.gameObject.SetActive (true);
	}

    bool checkWin()
    {
        return true;
    }

    bool checkLose()
    {
        return false;
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
}
