using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
	const int iSpeed = 3000;
	int _iNum = 0;
	int _idx = 0;
	Text lab;
	List<int> lLabSize = new List<int> (){100, 80, 70, 50};
	float _px;
	float _py;
	float _pxDes;
	float _pyDes;
	bool _bMove = false;

	void Awake(){
		initParas ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var dt = Time.deltaTime;
		if (_bMove == true) {
			transform.Translate (iSpeed * _px * dt, iSpeed * _py * dt, 0);
			var rect = transform.GetComponent<RectTransform> ().anchoredPosition;
			if (((_px < 0 && rect.x <= _pxDes) ||
				(_px > 0 && rect.x >= _pxDes)) ||
				((_py < 0 && rect.y <= _pyDes) ||
					(_py > 0 && rect.y >= _pyDes))) {
//				transform.gameObject.SetActive (false);
				if (_py == 0)
					transform.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (_pxDes - 60, rect.y);
				else
					transform.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (rect.x - 60, _pyDes);
				_bMove = false;
			}
		}
	}

	void initParas(){
		lab = transform.GetChild (0).GetComponent<Text> ();
	}

	public void setMove(int iType, float pm, float des){
		_bMove = true;
		if (iType == 0) {
			_px = pm;
			_py = 0;
			_pxDes = des;
		} else {
			_px = 0;
			_py = pm;
			_pxDes = des;
		}
	}

	public void showLab(int iNum){
		_iNum = iNum;
		lab.text = _iNum.ToString ();
		var iL = iNum.ToString ().Length;
		lab.fontSize = lLabSize [iL - 1];
	}

	public int getNum(){
		return _iNum;
	}

	public void setIdx(int idx){
		_idx = idx;
	}

	public int getIdx(){
		return _idx;
	}
}
