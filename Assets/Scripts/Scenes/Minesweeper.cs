using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Minesweeper : MonoBehaviour {
	int _iLine;
	int _iRow;
	int _iTotal;
	int _iMineCount;
	int _iTime;
    int[] _tNum = new int[81];
    int[] _tBtns = new int[81];
	AudioMgr adMgr;
	bool _bGameOver;
	Coroutine coPlayTime;
	Coroutine coPlayTips;
	Text textTime;
	Text labLeftMine;
	Text labType;
	int _iMode = 0;
	int _iDiff;
	Transform goTogs;
	Transform goStory;
	Transform goStoryWin;
	Transform goTips;
	Transform goStat;
    public Transform tilemap;
	string _sStory = "";
	//SPMine spMine;
	string _sStoryLab = "zhichulian";
	const int DX = 70;
	const int DY = 70;
	float px;
	float py;
	IOMgr ioMgr;
    public TileBase[] tiles;
    Tilemap _tileMap;

    // Use this for initialization
    void Start () {
		initParas ();
		initEvent ();
		initShow ();
	}
	
	// Update is called once per frame
	void Update () {
		if ( Application.platform == RuntimePlatform.Android &&(Input.GetKeyDown(KeyCode.Escape)))  
		{
			Application.Quit ();
		}
	}

	void initParas(){
		_iDiff = 1;
		_iRow = 9;
		_iLine = 9;
		_iTotal = _iRow * _iLine;
		_iMode = 0;
		adMgr = AudioMgr.getInstance ();
		textTime = transform.Find ("goTop").GetChild (1).GetChild (0).GetComponent<Text> ();
		labLeftMine = transform.Find ("goTop").GetChild (0).GetChild (0).GetComponent<Text> ();
		//initTGrid ();
		//spMine = new SPMine (_iTotal, _iRow);
		goTogs = transform.Find ("goTogs");
		goStory = transform.Find ("goStory");
		goStoryWin = transform.Find ("goStoryWin");
		goTips = transform.Find ("goTips");
		goStat = transform.Find ("goStat");
		labType = transform.Find("goBtns").GetChild(1).GetChild (0).GetComponent<Text> ();
		reset ();
		ioMgr = new IOMgr ();
		ioMgr.CreateFile ();
        transform.Find("imgMines").GetComponent<MTouch>().init(this);
        _tileMap = tilemap.GetComponent<Tilemap>();
    }

	void initEvent(){
        transform.Find("goTop/back").GetComponent<Button>().onClick.AddListener(delegate
        {
            SceneManager.LoadScene("Lobby");
        });
        var btnStart = transform.Find ("goBtns/btnStart").gameObject.GetComponent<Button> ();
		btnStart.onClick.AddListener (onClickStart);
		var btnMode = transform.Find ("goBtns/btnMode").gameObject.GetComponent<Button> ();
		btnMode.onClick.AddListener (onClickDiffMode);
		var btnType = transform.Find ("goBtns/btnType").gameObject.GetComponent<Button> ();
		btnType.onClick.AddListener (onClickType);
		var btnStory = transform.Find ("goBtns/btnStory").gameObject.GetComponent<Button> ();
		btnStory.onClick.AddListener (onClickStory);
		var stat = transform.Find ("goBtns/btnStat").gameObject.GetComponent<Button> ();
		stat.onClick.AddListener (delegate {
			goStat.gameObject.SetActive(true);
			goStat.GetComponent<Stat>().initShow();
		});

		var panel = goTogs.GetChild (0);
		var iL = panel.childCount;
		for (var i = 0; i < iL; i++){
			var idx = i;
			var trans = panel.GetChild (i);
			var tog = trans.GetComponent<Toggle>();
			if (i == _iDiff - 1)
				tog.isOn = true;
			tog.onValueChanged.AddListener (
				delegate(bool isOn) {
					if (isOn == true) {
						adMgr.PlaySound ("click");
						_iDiff = idx + 1;
					}
				}
			);
		}
	}

	void initShow(){
		showTogs (false);
		showStory(false);
		hideStoryWin ();
		goTips.gameObject.SetActive(false);
		Invoke("onClickStart", 0.5f);
	}

	void showTogs(bool bShow){
		goTogs.gameObject.SetActive (bShow);
	}

	void showStory(bool bShow){
		goStory.gameObject.SetActive (bShow);
		if (bShow == true) {
			for (int i = 0; i < _sStoryLab.Length; i++) {
				var str = "s" + _sStoryLab [i];
				Transform trans = null;
				Transform trans2 = null;
				switch (str) {
				case "sz":
					trans = goStory.GetChild (1).GetChild (0); 
					break;
				case "sh":
					trans = goStory.GetChild (1).GetChild (1); 
					trans2 = goStory.GetChild (2).GetChild (1); 
					break;
				case "si":
					trans = goStory.GetChild (1).GetChild (2); 
					trans2 = goStory.GetChild (3).GetChild (1); 
					break;
				case "sc":
					trans = goStory.GetChild (2).GetChild (0); 
					break;
				case "su":
					trans = goStory.GetChild (2).GetChild (2); 
					break;
				case "sl":
					trans = goStory.GetChild (3).GetChild (0); 
					break;
				case "sa":
					trans = goStory.GetChild (3).GetChild (2); 
					break;
				case "sn":
					trans = goStory.GetChild (3).GetChild (3);
					break;
				default:
					break;
				}
				if (trans != null)
					trans.GetComponent<Text> ().CrossFadeAlpha (0, 0f, true);
				if (trans2 != null)
					trans2.GetComponent<Text> ().CrossFadeAlpha (0, 0f, true);
			}
		}
	}

	void showStoryWin(bool bShow){
		goStoryWin.gameObject.SetActive (bShow);
	}

	void hideStoryWin(){
		for (int i = 0; i < 5; i++) {
			goStoryWin.GetChild (i + 1).gameObject.SetActive (false);
		}
		showStoryWin (false);
	}

	void showStoryLab(string str){
		Transform trans = null;
		Transform trans2 = null;
		switch (str) {
		case "sz":
			trans = goStory.GetChild (1).GetChild (0); 
			break;
		case "sh":
			trans = goStory.GetChild (1).GetChild (1); 
			trans2 = goStory.GetChild (2).GetChild (1); 
			break;
		case "si":
			trans = goStory.GetChild (1).GetChild (2); 
			trans2 = goStory.GetChild (3).GetChild (1); 
			break;
		case "sc":
			trans = goStory.GetChild (2).GetChild (0); 
			break;
		case "su":
			trans = goStory.GetChild (2).GetChild (2); 
			break;
		case "sl":
			trans = goStory.GetChild (3).GetChild (0); 
			break;
		case "sa":
			trans = goStory.GetChild (3).GetChild (2); 
			break;
		case "sn":
			trans = goStory.GetChild (3).GetChild (3);
			break;
		default:
			break;
		}
		trans.GetComponent<Text>().CrossFadeAlpha(1, 1.5f, true);
		if (trans2 != null)
			trans2.GetComponent<Text>().CrossFadeAlpha(1, 1.5f, true);
		while (true) {
			var idx = _sStoryLab.IndexOf (str [1]);
			if (idx != -1)
				_sStoryLab = _sStoryLab.Remove (idx, 1);
			else
				break;
		}
	}

	void showTips(){
		var goPTips = transform.Find ("goMainTips");
		for (int i = 0; i < goPTips.childCount; i++) {
			var goTips = goPTips.GetChild (i);
			for (int j = 0; j < goTips.childCount; j++) {
				var item = goTips.GetChild(j).GetComponent<Text>();
				item.text = (j + 1).ToString ();
			}
		}
	}

	public void onClickDiffMode(){
		adMgr.PlaySound ("click");
		showTogs (!goTogs.gameObject.activeSelf);
	}

	public void onClickStory(){
		adMgr.PlaySound ("click");
		showStory (!goStory.gameObject.activeSelf);
	}

	public void onClickType(){
		adMgr.PlaySound ("click");
		onClickMode ();
	}

	public void onClickMode(){
		adMgr.PlaySound ("click");
		_iMode = 1 - _iMode;
		if (_iMode == 0) {
			labType.text = "翻 开";
			labType.color = Color.black;
		} else {
			labType.text = "插 旗";
			labType.color = new Color(160.0f/250, 50.0f/250, 40.0f/250);
		}
	}

	public int getMode(){
		return _iMode;
	}

	void onClickStart(){
		reset ();
		initMines();
		if (coPlayTime != null)
			StopCoroutine (coPlayTime);
        coPlayTime = StartCoroutine(playTextTime());
		//var iPreMode = getMode();
		//_iMode = 0;
		//initGridShow();
		//_iMode = iPreMode;
        showBtns();
	}

	void onDiffChanged(){
		int iDiff;
		if (_sStory != "")
			iDiff = 1;
		else
			iDiff = _iDiff;
		if (iDiff == 1) {
			_iRow = 9;
			_iLine = 9;
			_iMineCount = 10;
		}
		_iTotal = _iRow * _iLine;
		showMineCount (_iMineCount);
	}

	void reset(){
//		_iMode = 0;
		_bGameOver = false;
		_iTime = 0;
		textTime.text = "00:00";
		goTips.gameObject.SetActive (false);
		showTogs (false);
		onDiffChanged ();
//		showFlagTips (false);
	}

	void showMineCount(int iMineCount){
		labLeftMine.text = iMineCount.ToString ();
	}

	public void setMineCount(int iNum){
		_iMineCount += iNum;
		labLeftMine.text = _iMineCount.ToString ();
	}

	public int getMineCount(){
		return _iMineCount;
	}

	public bool getBGameOver(){
		return _bGameOver;
	}

	void initMines(){
		var tNum = new List<int>();
		var tMineNum = new int[_iTotal];
		for (var i = 0; i < _iTotal; i++) {
			tNum.Add(i);
			tMineNum[i] = 0;
            _tNum[i] = 0;
            _tBtns[i] = 1;
        };
		for (var i = 0; i < _iMineCount; i++) {
			var iRandom = Random.Range (0, tNum.Count);
			var iNum = tNum [iRandom];
			tNum.RemoveAt(iRandom);
			tMineNum[iNum] = 1;
		};
		for (var k = 0; k < _iTotal; k++) {
			if (tMineNum [k] == 1) {
                _tNum[k] = -1;
                int col = k % _iRow;
                int row = (int)Mathf.Floor(k / _iRow);
				for (int i = row-1; i < row+2; i++)
                {
                    for (int j = col - 1; j < col + 2; j++)
                    {
                        if (i > -1 && j > -1 && i < _iRow && j < _iRow)
                        {
                            int idx = _iRow * i + j;
                            if (_tNum[idx] != -1) _tNum[idx]++;
                        }
                    }
                }

            }
		};
	}

    void showBtns()
    {
        var px = -5;
        var py = 3;
        for (var i = 0; i < _iTotal; i++)
        {
            var col = i % _iRow;
            var row = (int)Mathf.Floor(i / _iRow);
            var pos = new Vector3Int(px+col, py-row, 0);
            if (_tBtns[i] == 0) _tileMap.SetTile(pos, tiles[2 + _tNum[i]]);
            else _tileMap.SetTile(pos, tiles[0]);
        }
    }

    public void onClick(int idx)
    {
        if (!_bGameOver)
        {
            if (_tNum[idx] == -1)
            { //地雷
                adMgr.PlaySound("bomb");
                var col = idx % _iRow;
                var row = (int)Mathf.Floor(idx / _iRow);
                var pos = new Vector3Int(-5 + col, 3 - row, 0);
                _tileMap.SetTile(pos, tiles[1]);
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

    void showGrids(int k){
        var iLabNum = _tNum[k];
		if (iLabNum != -1) {
            _tBtns[k] = 0;
			if (iLabNum == 0) {
				var row = Mathf.Floor (k / _iRow);
				var col = k % _iRow;
				for (var x = row-1; x < row+2; x++)
                {
                    for (var y = col-1; y < col+2; y++)
                    {
                        if (x >= 0 && y >= 0 && x < _iRow && y < _iRow)
                        {
                            var idx = (int)(_iRow * x + y);
                            if (_tBtns[idx] == 1) showGrids(idx);
                        }
                    }
                }
            }
		}
	}

    bool checkWin(){
		for (int i = 0; i < _iTotal; i++) {
            if (_tBtns[i] == 1 && _tNum[i] != -1) return false;
		}
        return true;
	}

	void setTouchable(bool bTouch){
		var control = GetComponent<CanvasGroup> ();
		control.blocksRaycasts = bTouch;
	}

	IEnumerator playTextTime(){
		while (true) {
			textTime.text = getStrTime (_iTime);
			_iTime++;
			if (_iTime > 3600){
				adMgr.PlaySound ("lose");
				Invoke ("onClickStart", 2.0f);
				yield break;
			}
			yield return new WaitForSeconds (1);
		}
	}

	string getStrTime(int iTime){
		string str = "";
		int[] time = new int[2];
		time[0] = (int)(iTime / 60);
		time[1] = iTime % 60;
		int v;
		for (int i = 0; i < time.Length; i++) {
			v = time [i];
			if (v < 10)
				str += "0" + v.ToString ();
			else
				str += v.ToString ();
			if (i == 0)
				str += ":";
		}

		return str;
	}

	IEnumerator playTips(){
		goTips.gameObject.SetActive (true);
		yield return new WaitForSeconds (2.0f);
		goTips.gameObject.SetActive (false);
	}

	void showResultTips(string str){
//		goTips.gameObject.SetActive (true);
		goTips.GetChild (1).GetComponent<Text> ().text = str;
		if (coPlayTips != null)
			StopCoroutine (coPlayTips);
		coPlayTips = StartCoroutine (playTips());
	}
}
