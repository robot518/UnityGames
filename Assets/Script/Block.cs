using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	int _iType = 0;
	bool _bMove = false;
//	float _py;
	float _pyDes;
	float _dt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var dt = Time.deltaTime;
		if (_bMove == true) {
			_dt += dt;
			var rect = GetComponent<RectTransform> ();
			transform.Translate (0, -50 * _dt * _dt, 0);
			if (rect.anchoredPosition.y <= _pyDes) {
				transform.localPosition = new Vector3 (transform.localPosition.x, _pyDes, 0);
				_bMove = false;
				_dt = 0;
			}
		}
	}

	public void setBMove(float pyDes){
		_bMove = true;
		_pyDes = pyDes;
		_dt = 0;
	}

	public void setBMove(){
		_bMove = false;
	}

	public void showType(int iType){
		_iType = iType;
		if (iType == 0)
			transform.localRotation = Quaternion.AngleAxis (0, Vector3.forward);
		else
			transform.localRotation = Quaternion.AngleAxis (-90 * iType, Vector3.forward);
	}

	public int getIType(){
		return _iType;
	}
}
