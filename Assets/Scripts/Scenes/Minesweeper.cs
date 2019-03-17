using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minesweeper : MonoBehaviour {
	int _iLine;
	int _iRow;
	int _iTotal;
	int _iMineCount;
	int _iTime;
	List<MGrid> _tGrid = new List<MGrid>();
	List<MGrid> _tNGrid = new List<MGrid>();
	List<MGrid> _tSPGrid = new List<MGrid>();
	List<int> _tMine = new List<int>();
	List<int> _tSearch = new List<int>();
	AudioMgr adMgr;
	bool _bGameOver;
	Coroutine coPlayTime;
	Coroutine coPlayTips;
	Text textTime;
	Text labLeftMine;
	Text labType;
	int _iMode = 0;
	int _iMineIdx = -1;
	int _iDiff;
	Transform goTogs;
	Transform goStory;
	Transform goStoryWin;
	Transform goTips;
	Transform goStat;
	string _sStory = "";
	SPMine spMine;
	string _sStoryLab = "zhichulian";
	const int DX = 70;
	const int DY = 70;
	float px;
	float py;
	IOMgr ioMgr;

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
		_iLine = 12;
		_iTotal = _iRow * _iLine;
		_iMode = 0;
		adMgr = AudioMgr.getInstance ();
		textTime = transform.Find ("goTop").GetChild (1).GetChild (0).GetComponent<Text> ();
		labLeftMine = transform.Find ("goTop").GetChild (0).GetChild (0).GetComponent<Text> ();
		//initTGrid ();
		spMine = new SPMine (_iTotal, _iRow);
		goTogs = transform.Find ("goTogs");
		goStory = transform.Find ("goStory");
		goStoryWin = transform.Find ("goStoryWin");
		goTips = transform.Find ("goTips");
		goStat = transform.Find ("goStat");
		labType = transform.Find("goBtns").GetChild(1).GetChild (0).GetComponent<Text> ();
		//reset ();
		ioMgr = new IOMgr ();
		ioMgr.CreateFile ();
	}

	void initEvent(){
        transform.Find("goTop/back").GetComponent<Button>().onClick.AddListener(delegate
        {
            SceneManager.LoadScene("Lobby");
        });
        var btnStart = transform.Find ("goBtns/btnStart").gameObject.GetComponent<Button> ();
		//btnStart.onClick.AddListener (onClickStart);
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
		var btnStartStory = transform.Find ("goStory/btnStartStory").gameObject.GetComponent<Button> ();
		btnStartStory.onClick.AddListener (onClickStartStory);

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
		//Invoke("onClickStart", 0.5f);
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

	void onClickStartStory(){
		var iRandom = Random.Range (0, _sStoryLab.Length);
		_sStory = "s" + _sStoryLab [iRandom];
		reset ();
		initSPMines();
		initLabs();
		for (var i = 0; i < _iTotal; i++) {
			var grid = _tGrid[i];
			grid.showLab(grid.getNum ());
			grid.showBtn (true);
//			grid.showBtn (false);
			grid.showFlag (false);
		};
		if (coPlayTime != null)
			StopCoroutine (coPlayTime);
		coPlayTime = StartCoroutine (playTextTime());
		showStory (false);
//		showWin ();
	}

	void initSPMines(){
		var tMineNum = spMine.getSPMine(_sStory);
		var idx = 0;
		for (var i = 0; i < _iTotal; i++) {
			if (tMineNum [i] == 1) {
				idx++;
				_tMine.Add(i);
				_tGrid [i].showMine (true);
				_tGrid [i].setNum (-1);
			} else {
				_tGrid [i].showMine (false);
				_tGrid [i].setNum (0);
			}
		};
		_iMineCount = idx;
		showMineCount (_iMineCount);
	}

	void initTGrid(){
		var tGrid = transform.Find ("goMain");
		var iLen = tGrid.childCount;
		for (int i = 0; i < iLen; i++) {
			var cGrid = tGrid.GetChild(i).GetComponent<MGrid> ();
			cGrid.init (i, this);
			_tNGrid.Add (cGrid); 
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

	void initGridShow(){
		var tNum = new List<int>();
		for (var i = 0; i < _iTotal; i++) {
			tNum.Add(i);
		};
		for (var i = 0; i < _iTotal; i++) {
			var iRandom = Random.Range (0, tNum.Count);
			var iNum = tNum [iRandom];
			tNum.RemoveAt(iRandom);
			var gird = _tGrid [iNum];
			if (gird.getNum() == 0) {
				gird.onClick ();
				break;
			}
		};
		coPlayTime = StartCoroutine (playTextTime());
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
		_sStory = "";
		reset ();
		initMines();
		initLabs();
		for (var i = 0; i < _iTotal; i++) {
			var grid = _tGrid[i];
			grid.showLab(grid.getNum ());
			grid.showBtn (true);
			grid.showFlag (false);
		};
		if (coPlayTime != null)
			StopCoroutine (coPlayTime);
		var iPreMode = getMode();
		_iMode = 0;
		initGridShow();
		_iMode = iPreMode;
	}

	void onDiffChanged(){
		int iDiff;
		if (_sStory != "")
			iDiff = 1;
		else
			iDiff = _iDiff;
		if (iDiff == 1) {
			_iRow = 9;
			_iLine = 12;
			_iMineCount = 17;
		}
		_iTotal = _iRow * _iLine;
		showMineCount (_iMineCount);
		if (_iRow == 9 && _iLine == 12) {
			_tGrid = _tNGrid;
		}
	}

	void reset(){
//		_iMode = 0;
		_bGameOver = false;
		_iTime = 0;
		textTime.text = "00:00";
		_tSearch.Clear ();
		_tMine.Clear ();
		if (_iMineIdx != -1) {
			_tGrid [_iMineIdx].resetMine ();
			_iMineIdx = -1;
		}
		_tSPGrid.Clear ();
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
			_tGrid [i].setNum (0);
		};
		for (var i = 0; i < _iMineCount; i++) {
			var iRandom = Random.Range (0, tNum.Count);
			var iNum = tNum [iRandom];
			tNum.RemoveAt(iRandom);
			tMineNum[iNum] = 1;
		};
		for (var i = 0; i < _iTotal; i++) {
			if (tMineNum [i] == 1) {
				_tMine.Add(i);
				_tGrid [i].showMine (true);
				_tGrid [i].setNum (-1);
			} else {
				_tGrid [i].showMine (false);
			}
		};
	}

	void initLabs(){
		for (var i = 0; i < _iMineCount; i++) {
			var iMine = _tMine[i];
			var iCurLine = Mathf.Floor(iMine/_iRow);
			var iCurRow = iMine % _iRow;
			for (var iLine = 0; iLine < 3; iLine++) {
				for (var iRow = 0; iRow < 3; iRow++) {
					var iRowTemp = iCurRow - 1 + iRow;
					var iLineTemp = iCurLine - 1 + iLine;
					if (iRowTemp > -1 && iRowTemp < _iRow && iLineTemp > -1 && iLineTemp < _iLine){
						var idx = iMine + (iRow - 1) + (iLine * _iRow - _iRow);
						var grid = _tGrid [idx];
						if (grid.getMine() == false)
							grid.setNum (grid.getNum () + 1);
					}
				};
			};
		};
	}

	public void showMines(int idx){
		if (idx != 0) {
			showResultTips ("你输了");
			if (_iDiff == 1 && _sStory == "")
				ioMgr.WriteLose ();
		}
		_iMineIdx = idx;
		_bGameOver = true;
		for (int i = 0, iLen = _tMine.Count; i < iLen; i++) {
			_tGrid [_tMine [i]].showBtn (false);
		}
		if (coPlayTime != null)
			StopCoroutine (coPlayTime);
	}

	public void setTSearch(){
		_tSearch.Clear ();
	}

	public void showGrids(int idxNum){
		var iLabNum = _tGrid [idxNum].getNum ();
		if (iLabNum >= 0) {
			_tGrid [idxNum].showBtn (false);
			if (iLabNum == 0) {
				var iMine = idxNum;
				var iCurLine = Mathf.Floor (iMine / _iRow);
				var iCurRow = iMine % _iRow;
				for (var iLine = 0; iLine < 3; iLine++) {
					for (var iRow = 0; iRow < 3; iRow++) {
						var iRowTemp = iCurRow - 1 + iRow;
						var iLineTemp = iCurLine - 1 + iLine;
						if (iRowTemp > -1 && iRowTemp < _iRow && iLineTemp > -1 && iLineTemp < _iLine) {
							var idx = iMine + (iRow - 1) + (iLine * _iRow - _iRow);
							var grid = _tGrid [idx];
							if (grid.getFlagShow() == false && grid.getBBtnShow() == true && _tSearch.Contains (idx) == false) {
								_tSearch.Add (idx);
								showGrids (idx);
							}
						}
					}
				}
			}
		}
	}

	public void onClickNum(int idxNum, int iNum){
		var iMine = idxNum;
		var iCurLine = Mathf.Floor (iMine / _iRow);
		var iCurRow = iMine % _iRow;
		//统计标记的地雷数量，标错return;
		var iShowNum = 0;
		for (var iLine = 0; iLine < 3; iLine++) {
			for (var iRow = 0; iRow < 3; iRow++) {
				var iRowTemp = iCurRow - 1 + iRow;
				var iLineTemp = iCurLine - 1 + iLine;
				if (iRowTemp > -1 && iRowTemp < _iRow && iLineTemp > -1 && iLineTemp < this._iLine) {
					var idx = iMine + (iRow - 1) + (iLine * _iRow - _iRow);
					var grid = _tGrid [idx];
					if (grid.getFlagShow() == true) {
						iShowNum++;
					}
				}
			}
		}
		//判断地雷是否均被标记
		if (iNum != iShowNum)
			return;
		adMgr.PlaySound("check");
		setTSearch();
		//展开地图
		for (var iLine = 0; iLine < 3; iLine++) {
			for (var iRow = 0; iRow < 3; iRow++) {
				var iRowTemp = iCurRow - 1 + iRow;
				var iLineTemp = iCurLine - 1 + iLine;
				if (iRowTemp > -1 && iRowTemp < _iRow && iLineTemp > -1 && iLineTemp < this._iLine) {
					var idx = iMine + (iRow - 1) + (iLine * _iRow - _iRow);
					var grid = _tGrid [idx]; 
					if (grid.getFlagShow () == false && grid.getBBtnShow () == true) {
						if (grid.getNum () == -1) {
							grid.showRedMine ();
							adMgr.PlaySound ("bomb");
							showMines (idx);
						} else if (_tSearch.Contains (idx) == false) {
							_tSearch.Add(idx);
							showGrids (idx);
						}
					}
				}
			}
		};
		showWin();
	}

	public void showWin(){
		var bWin = true;
		for (int i = 0; i < _iTotal; i++) {
			var grid = _tGrid [i];
			bool bMine = grid.getMine (), bBtnShow = grid.getBBtnShow ();
			if (bMine == false && bBtnShow == true) {
				bWin = false;
				break;
			}
		}
		if (bWin == true) {
			_bGameOver = true;
			adMgr.PlaySound ("win");
			showMines (0);
			if (_iDiff == 1 && _sStory == "")
				ioMgr.WriteWin (_iTime);
			if (_sStory != "") {
				StartCoroutine (playStory());
			} else
				showResultTips("用时" + _iTime + "秒");
		}
	}

	IEnumerator playStory(){
		setTouchable (false);
		for (int j = 0; j < 3; j++) {
			yield return new WaitForSeconds (0.3f);
			for (int i = 0; i < _iMineCount; i++) {
				_tGrid [_tMine [i]].showRedMine ();
			}
			yield return new WaitForSeconds (0.2f);
			for (int i = 0; i < _iMineCount; i++) {
				_tGrid [_tMine [i]].resetMine ();
			}
		}
		yield return new WaitForSeconds (0.2f);
		showStory (true);
		yield return new WaitForSeconds (0.3f);
		showStoryLab (_sStory);
		yield return new WaitForSeconds (1.8f);
		if (_sStoryLab.Length == 0) {
			goStory.GetChild (4).gameObject.SetActive (false);
			yield return new WaitForSeconds (1.2f);
			showStory (false);
			yield return new WaitForSeconds (0.2f);
			showStoryWin (true);
			for (int i = 0; i < 5; i++) {
				yield return new WaitForSeconds (1.2f);
				goStoryWin.GetChild (i + 1).gameObject.SetActive (true);
				if (i == 2)
					yield return new WaitForSeconds (1.2f);
			}
			yield return new WaitForSeconds (5.2f);
			showStoryWin (false);
		}
		setTouchable (true);
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
