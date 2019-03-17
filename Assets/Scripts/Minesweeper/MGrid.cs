using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MGrid : MonoBehaviour {
	Text txt;
	Transform imgMine;
	Transform imgFlag;
	Transform btn;
    Minesweeper _delt;
	int _idx;
	int _iNum;
	AudioMgr adMgr;
	string[] tStrColor;
//	float _iDown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void initParas() {
//		_iDown = -1;

		txt = transform.GetChild (0).GetComponent<Text>();
		imgMine = transform.GetChild (1);
		btn = transform.GetChild (2);
		imgFlag = transform.GetChild (3);
		adMgr = AudioMgr.getInstance ();
		tStrColor = new string[] {
			"1611FFFF",
			"1E781EFF",
			"F01111FF",
			"001979FF",
			"73150DFF",
			"377D7DFF",
			"000000FF",
			"7A7A7AFF"
		};
	}

	void initEvent(){
		btn.GetComponent<Button> ().onClick.AddListener (onClick);
		transform.GetComponent<Button> ().onClick.AddListener (onClickNum);
	}

	public void onClickNum(){
		if (_iNum == 0)
			return;
		_delt.onClickNum(_idx, _iNum);
	}

	public void onClick(){
		adMgr.PlaySound("check");
		if (_delt.getBGameOver() == true)
			return;
		var bShowFlag = imgFlag.gameObject.activeSelf;
		if (_delt.getMode () == 0 && bShowFlag == true)
			return;
		if (_delt.getMode () == 0) {
			if (imgMine.gameObject.activeSelf == true) {
				showRedMine ();
				adMgr.PlaySound ("bomb");
				_delt.showMines (_idx);
			} else {
				_delt.setTSearch ();
				_delt.showGrids (_idx);
				_delt.showWin ();
			}
		} else {
			onFlagEvent (bShowFlag);
		}
	}

	public void onFlagEvent(bool bShow){
		bShow = !bShow;
		if (_delt.getMineCount () == 0 && bShow == true)
			return;
		var iNum = bShow == false ? 1 : -1;
		_delt.setMineCount (iNum);
		showFlag (bShow);
	}

	public void init(int idx, Minesweeper delt){
		_idx = idx;
		_delt = delt;
		_iNum = 0;
		initParas ();
		initEvent ();
	}

	public void showRedMine(){
		imgMine.GetComponent<Image>().color = Color.red;
	}

	public void resetMine(){
		imgMine.GetComponent<Image>().color = Color.white;
	}

	public void showLab(int iNum){
		_iNum = iNum;
		if (iNum <= 0)
			txt.gameObject.SetActive (false);
		else {
			txt.gameObject.SetActive (true);
			txt.text = "<color=#" + tStrColor[iNum - 1] + ">" + iNum.ToString () + "</color>";
		}
	}

	public int getNum(){
		return _iNum;
	}

	public void setNum(int iNum){
		_iNum = iNum;
	}

	public bool getMine(){
		return imgMine.gameObject.activeSelf;
	}

	public void showMine(bool bShow){
		imgMine.gameObject.SetActive (bShow);
	}

	public void showBtn(bool bShow){
		btn.gameObject.SetActive (bShow);
	}

	public bool getBBtnShow(){
		return btn.gameObject.activeSelf;
	}

	public bool getFlagShow(){
		return imgFlag.gameObject.activeSelf;
	}

	public void showFlag(bool bShow){
		imgFlag.gameObject.SetActive (bShow);
	}
}
