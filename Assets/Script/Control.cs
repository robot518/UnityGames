using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Control : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	bool _bAuto = false;
	float _iTime = 0;
	Main _delt;
	int _pos; //013左右下
	float _iLimit = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var dt = Time.deltaTime;
		if (_bAuto == true) {
			_iTime += dt;
			if (_iTime >= _iLimit) {
				_iTime = 0;
				_delt.onControl (_pos);
				if (_iLimit == 0.3f)
					_iLimit = 0.15f;
				_iLimit -= 0.01f;
				if (_iLimit < 0.05f)
					_iLimit = 0.05f;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData){
		if (_iLimit == -1) {
			_delt.onControl (_pos);
			_iLimit = 0.3f;
		}
		_bAuto = true;
	}

	public void OnPointerUp(PointerEventData eventData){
		_bAuto = false;
		_iTime = 0;
		_iLimit = -1;
	}

	public void init(Main delt, int pos){
		_delt = delt;
		_pos = pos;
	}
}
