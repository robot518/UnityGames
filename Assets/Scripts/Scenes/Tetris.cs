using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Tetris : MonoBehaviour {
	const int DY = 40;
	const int DX = 40;
	const int DESY = -360;
	const int IR = 10;
	const int IL = 20;
	const int ICOUNT = 4;
	Transform goMain;
	Transform goNext;
	Transform _item;
	Transform _shadow;
	Transform _block;
	Transform goTips;
	Coroutine _playItem;
	int _iRow;
	int _iLine;
	int _idx;
	int _iNextIdx;
	List<List<int>> lMain = new List<List<int>>();
	Sprite _sprite;
	int _iMoveLine;
	Text labLv;
	Text labLine;
	float _iTime;
	int _iLv;
	int _iMoveCount;
	float _py;
	float _px;
	bool _bShowShadow = true;
	bool _bPlay;
	bool _bStop;
	Transform _iSPItem;
	AudioMgr adMgr;

	// Use this for initialization
	void Start () {
		initParas ();
		initShow ();
		initEvent ();
	}
	
	// Update is called once per frame
	void Update () {
		if ( Application.platform == RuntimePlatform.Android &&(Input.GetKeyDown(KeyCode.Escape)))  
		{
			Application.Quit ();
		}
	}

	void initParas(){
		var lab = transform.Find ("goLeft/lab");
		labLv = lab.GetChild (1).GetComponent<Text> ();
		labLine = lab.GetChild (3).GetComponent<Text> ();
		goMain = transform.Find ("goMain");
		goNext = transform.Find ("goLeft/goNext");
		for (int i = 0; i < IL; i++) {
			lMain.Add (new List<int> ());
			for (int j = 0; j < IR; j++) {
				lMain [i].Add (0);
			}
		}
		goTips = transform.Find ("goTips");
		_sprite = Resources.Load<Sprite> ("Res/btnGrid");
		adMgr = AudioMgr.getInstance ();
	}

	void initShow(){
		for (int i = 0; i < goNext.childCount; i++) {
			goNext.GetChild (i).gameObject.SetActive (false);
		}
		goTips.gameObject.SetActive (false);
		labLine.text = "0";
		labLv.text = "0";
	}

	void initEvent(){
        transform.Find("goLeft/back").GetComponent<Button>().onClick.AddListener(delegate
        {
            SceneManager.LoadScene("Lobby");
        });

        var goDown = transform.Find ("goDown");
		UnityAction[] tFunc = {onLeft, onRight, onUp, onDown, onStart, onStop, onConvert};
		for (var i = 0; i < tFunc.Length; i++){
			var btn = goDown.GetChild (i);
			if (i == 0 || i == 1 || i == 3) {
				btn.GetComponent<Control> ().init (this, i);
			} else {
				btn.GetComponent<Button> ().onClick.AddListener (tFunc [i]);
			}
		}
		var tog = transform.Find ("goLeft/tog").GetComponent<Toggle> ();
		tog.onValueChanged.AddListener (delegate(bool isOn) {
			_bShowShadow = isOn;
			_shadow.gameObject.SetActive (isOn);
		});
	}

	void addLv(){
		_iLv++;
		labLv.text = _iLv.ToString ();
		_iTime -= 0.1f;
		if (_iTime < 0.3f)
			_iTime = 0.3f;
	}

	void addLine(){
		_iMoveCount++;
		if (_iMoveCount < 100)
			labLine.text = _iMoveCount.ToString ();
		else {
			labLine.text = 0.ToString ();
			addLv ();
			_iMoveCount = 0;
		}
	}

	void reset(){
		initShow ();
		var goRow = goMain.GetChild (0);
		for (int i = 0; i < IL; i++) {
			var lData = lMain [i];
			var goLine = goRow.GetChild (i);
			for (int j = 0; j < IR; j++) {
				lData [j] = 0;
				goLine.GetChild (j).GetComponent<CGrid> ().initShow ();
			}
		}
		_iTime = 1.2f;
		_iMoveCount = 0;
		_iLv = 0;
		_iLine = 0;
		_iRow = 3;
		_iMoveLine = -1;
		_bPlay = false;
		_bStop = false;
		if (_playItem != null)
			StopCoroutine (_playItem);
		if (_item != null)
			Destroy (_item.gameObject);
		if (_shadow != null)
			Destroy (_shadow.gameObject);
		if (_iSPItem != null)
			Destroy (_iSPItem.gameObject);
	}

	void onStart(){
		adMgr.PlaySound ("click");
		reset ();
		_idx = getNext ();
		getItem ();
		hideNext ();
		_iNextIdx = getNext ();
	}

	void onLeft(){
		adMgr.PlaySound ("click");
		if (_item == null)
			return;
		var iRow = _iRow;
		iRow--;
		var iType = _item.GetComponent<Block> ().getIType ();
		if (_idx != 3 && iType != 1 && iRow < 0)
			return;
		if ((_idx == 3 || iType == 1) && iRow < -1)
			return;
		_iRow = iRow;
		var posX = (_iRow - 3) * DX + _px;
		_item.localPosition = new Vector3 (posX, _item.localPosition.y, 0);
		var iLineShadow = getILine();
		_shadow.GetComponent<Block> ().setBMove ();
		_shadow.localPosition = new Vector3 (posX, _py - (iLineShadow - 1) * DY, 0);
	}

	void onRight(){
		adMgr.PlaySound ("click");
		if (_item == null)
			return;
		var iRow = _iRow;
		iRow++;
		var iType = _item.GetComponent<Block> ().getIType ();
		switch(_idx){
		case 6:
			if (iType == 0 && iRow > 6)
				return;
			else if (iType == 1 && iRow > 8)
				return;
			break;
		case 3:
		case 4:
		case 5:
			if (iRow > 7)
				return;
			break;
		case 0:
		case 1:
		case 2:
			if (iType != 3 && iRow > 7)
				return;
			else if (iType == 3 && iRow > 8)
				return;
			break;
		}
		_iRow = iRow;
		var posX = (_iRow - 3) * DX + _px;
		_item.localPosition = new Vector3 (posX, _item.localPosition.y, 0);
		var iLineShadow = getILine();
		_shadow.GetComponent<Block> ().setBMove ();
		_shadow.localPosition = new Vector3 (posX, _py - (iLineShadow - 1) * DY, 0);
	}

	void onStop(){
		adMgr.PlaySound ("click");
		_bStop = !_bStop;
		if (_bStop == true) {
			if (_playItem != null)
				StopCoroutine (_playItem);
			showTips ("暂停");
		} else {
			_playItem = StartCoroutine (playItem ());
			showTips ("继续");
		}
	}

	int getILine(){
		var iLine = _iLine;
		var iType = _item.GetComponent<Block> ().getIType ();
		while(true) {
			iLine++;
			if (getBDown (iLine, iType) == false) {
				return iLine;
			}
		}
	}

	void onUp(){
		if (_item == null)
			return;
		_iLine = getILine ();
		if (_playItem != null)
			StopCoroutine (_playItem);
		_shadow.GetComponent<Block> ().setBMove ();
		setLMain (_item.GetComponent<Block> ().getIType ());
		if (_bPlay == false)
			onNext ();
	}

	void onDown(){
		adMgr.PlaySound ("click");
		if (_item == null)
			return;
		_iLine++;
		var iType = _item.GetComponent<Block> ().getIType ();
		if (getBDown (_iLine, iType) == false) {
			transform.Find ("goDown/btn").GetComponent<Control> ().OnPointerUp (null);
			if (_playItem != null)
				StopCoroutine (_playItem);
			_shadow.GetComponent<Block> ().setBMove ();
			setLMain (_item.GetComponent<Block> ().getIType ());
			if (_bPlay == false)
				onNext ();
			return;
		}
		_item.localPosition = new Vector3 (_item.localPosition.x, _py - _iLine * DY, 0);
	}

	public void onControl(int pos){
		switch (pos) {
		case 0:
			onLeft ();
			break;
		case 1:
			onRight ();
			break;
		case 3:
			onDown ();
			break;
		}
	}
		
	void onConvert(){
		adMgr.PlaySound ("click");
		if (_item != null){
			if (_idx == 3)
				return;
			var block = _item.GetComponent<Block> ();
			var iType = 0;
			var iPreType = block.getIType ();
			if (_idx >= 0 && _idx <= 2) {
				iType = iPreType + 1;
				if (iType == 4)
					iType = 0;
			} else if (_idx >= 4 && _idx <= 6) {
				iType = 1 - iPreType;
			}
			var pos = _item.localPosition;
			var dxCount = 0;
			if (getBConvert (iPreType) == true) {
				switch (_idx) {
				case 6:
					if (iPreType == 1) {
						if (_iRow == -1) {
							_iRow = 0;
							dxCount = 1;
						} else if (_iRow == 8) {
							_iRow -= 2;
							dxCount = -2;
						} else if (_iRow == 7) {
							_iRow -= 1;
							dxCount = -1;
						}
					} else {
						if (_iLine <= 1)
							_iLine += 2;
					}
					break;
				case 5:
				case 4:
					if (iPreType == 1 && _iRow == -1) {
						_iRow = 0;
						dxCount = 1;
					}
					break;
				case 2:
				case 1:
				case 0:
					if (iPreType == 1 && _iRow == -1) {
						_iRow = 0;
						dxCount = 1;
					}else if(iPreType == 3 && _iRow == 8) {
						_iRow = 7;
						dxCount = -1;
					}
					break;
				}
				_item.localPosition = new Vector3 (pos.x + dxCount * DX, _py - _iLine * DY, 0);
				block.showType (iType);
				var blockShadow = _shadow.GetComponent<Block> ();
				blockShadow.showType (iType);
				blockShadow.setBMove ();
				var iLine = getILine ();
				_shadow.localPosition = new Vector3 (_item.localPosition.x, _py - (iLine - 1) * DY, 0);
			}
		}
	}

	void onNext(){
		var bWin = true;
		for (int i = 0; i < lMain.Count; i++) {
			var lData = lMain [i];
			for (int j = 0; j < lData.Count; j++) {
				if (lData[j] == 1){
					bWin = false;
					break;
				}
			}
		}
		if (bWin == true) {
			showTips ("你赢了");
			return;
		}
		_idx = _iNextIdx;
		var bItem = getItem ();
		if (bItem == false){
			var item = Transform.Instantiate (_block);
			item.SetParent (goMain);
			if (_idx == 3 || _idx == 6) {
				_px = 0;
				_py = 400;
			} else {
				_px = -20;
				_py = 339;
			}
			item.localPosition = new Vector3 (_px, _py, 0);
			item.localScale = Vector3.one;
			_iSPItem = item;
			for (int i = 0; i < 4; i++) {
				item.GetChild(i).GetComponent<Image> ().color = Color.red;
			}
			showTips ("你输了");
			return;
		}
		hideNext ();
		_iNextIdx = getNext ();
	}

	int getNext(){
		var idx = Random.Range (0, 7);
//		idx = 6;
		var obj = goNext.GetChild (idx);
		obj.gameObject.SetActive (true);
		_block = obj;
		return idx;
	}

	void hideNext(){
		_block.gameObject.SetActive (false);
	}

	bool getItem(){
		_iLine = 0;
		_iRow = 3;
		var iLine = _iLine;
		switch(_idx){
		case 6:
			for (int i = 0; i < 4; i++) {
				if (lMain [0] [3 + i] == 1) {
					return false;
				}
			}
			break;
		case 5:
			for (int i = 0; i < 2; i++) {
				if (lMain [iLine + 1] [_iRow + i] == 1)
					return false;
				if (lMain [iLine] [_iRow + 1 + i] == 1)
					return false;
			}
			break;
		case 4:
			for (int i = 0; i < 2; i++) {
				if (lMain [iLine + 1] [_iRow + 1 + i] == 1)
					return false;
				if (lMain [iLine] [_iRow + i] == 1)
					return false;
			}
			break;
		case 3:
			for (int i = 0; i < 2; i++) {
				if (lMain [iLine + 1] [_iRow + i] == 1)
					return false;
				if (lMain [iLine] [_iRow + i] == 1)
					return false;
			}
			break;
		case 2:
			for (int i = 0; i < 3; i++) {
				if (lMain [iLine + 1] [_iRow + i] == 1)
					return false;
			}
			if (lMain [iLine] [_iRow + 2] == 1)
				return false;
			break;
		case 1:
			for (int i = 0; i < 3; i++) {
				if (lMain [iLine + 1] [_iRow + i] == 1)
					return false;
			}
			if (lMain [iLine] [_iRow] == 1)
				return false;
			break;
		case 0:
			for (int i = 0; i < 3; i++) {
				if (lMain [iLine + 1] [_iRow + i] == 1) {
					return false;
				}
			}
			if (lMain [iLine] [_iRow + 1] == 1) {
				return false;
			}
			break;
		}
		var item = Transform.Instantiate (_block);
		item.SetParent (goMain);
		if (_idx == 3 || _idx == 6) {
			_px = 0;
			_py = 400;
		} else {
			_px = -20;
			_py = 339;
		}
		item.localPosition = new Vector3 (_px, _py, 0);
		item.localScale = Vector3.one;
		_item = item;
		var shadow = Transform.Instantiate (_block);
		shadow.SetParent (goMain);
		shadow.localScale = Vector3.one;
		shadow.localPosition = item.localPosition;
		var iLineShadow = getILine();
		for (int i = 0; i < shadow.childCount; i++) {
			shadow.GetChild(i).GetComponent<Image> ().color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
		}
		var posY = _py - (iLineShadow - 1) * DY;
		shadow.GetComponent<Block> ().setBMove (posY);
		_shadow = shadow;
		_shadow.gameObject.SetActive (_bShowShadow);
		_playItem = StartCoroutine (playItem ());
		return true;
	}

	IEnumerator playItem(){
		while (true) {
			yield return new WaitForSeconds (_iTime);
			_iLine++;
			var iType = _item.GetComponent<Block> ().getIType ();
			if (getBDown(_iLine, iType) == false) {
				setLMain (iType);
//				Invoke ("onNext", 0.1f);
				if (_bPlay == false)
					onNext ();
				break;
			}
			var posY = _py - _iLine * DY;
			_item.localPosition = new Vector3 (_item.localPosition.x, posY, 0);
		}
	}

	//iLine为新的iLine
	bool getBDown(int iLine, int iType){
		switch(_idx){
		case 6:
			if (iType == 0 && iLine >= IL || (iType == 1 && iLine >= IL - 1))
				return false;
			if (iType == 0) {
				for (int i = 0; i < 4; i++) {
					if (lMain [iLine] [i + _iRow] == 1)
						return false;
				}
			} else {
				if (lMain [iLine + 1] [_iRow + 1] == 1)
					return false;
			}
			break;
		case 5:
			if (iType == 0 && iLine >= IL - 1 || (iType == 1 && iLine >= IL - 2))
				return false;
			if (iType == 0) {
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 1] [_iRow + i] == 1)
						return false;
				}
				if (lMain [iLine] [_iRow + 2] == 1)
					return false;
			} else {
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 1 + i] [_iRow + 1 + i] == 1)
						return false;
				}
			}
			break;
		case 4:
			if (iType == 0 && iLine >= IL - 1 || (iType == 1 && iLine >= IL - 2))
				return false;
			if (iType == 0) {
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 1] [_iRow + i + 1] == 1)
						return false;
				}
				if (lMain [iLine] [_iRow] == 1)
					return false;
			} else {
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 2 - i] [_iRow + 1 + i] == 1)
						return false;
				}
			}
			break;
		case 3:
			if (iLine == IL - 1)
				return false;
			for (int i = 0; i < 2; i++) {
				if (lMain [iLine + 1] [_iRow + i + 1] == 1)
					return false;
			}
			break;
		case 2:
			if (iType == 0 && iLine >= IL - 1 || (iType != 0 && iLine >= IL - 2))
				return false;
			switch (iType) {
			case 0:
				for (int i = 0; i < 3; i++) {
					if (lMain [iLine + 1] [_iRow + i] == 1)
						return false;
				}
				break;
			case 3:
				if (lMain [iLine + 2] [_iRow + 1] == 1)
					return false;
				if (lMain [iLine] [_iRow] == 1)
					return false;
				break;
			case 2:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 1] [_iRow + i + 1] == 1)
						return false;
				}
				if (lMain [iLine + 2] [_iRow] == 1)
					return false;
				break;
			case 1:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 2] [_iRow + i + 1] == 1)
						return false;
				}
				break;
			}
			break;
		case 1:
			if (iType == 0 && iLine >= IL - 1 || (iType != 0 && iLine >= IL - 2))
				return false;
			switch (iType) {
			case 0:
				for (int i = 0; i < 3; i++) {
					if (lMain [iLine + 1] [_iRow + i] == 1)
						return false;
				}
				break;
			case 1:
				if (lMain [iLine + 2] [_iRow + 1] == 1)
					return false;
				if (lMain [iLine] [_iRow + 2] == 1)
					return false;
				break;
			case 2:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 1] [_iRow + i] == 1)
						return false;
				}
				if (lMain [iLine + 2] [_iRow + 2] == 1)
					return false;
				break;
			case 3:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 2] [_iRow + i] == 1)
						return false;
				}
				break;
			}
			break;
		case 0:
			if (iType == 0 && iLine >= IL - 1 || (iType != 0 && iLine >= IL - 2))
				return false;
			for (int i = 0; i < 3; i++) {
				if (iType == i + 1)
					continue;
				if (lMain [iLine + 1] [_iRow + i] == 1)
					return false;
			}
			if (iType != 0) {
				if (lMain [iLine + 2] [_iRow + 1] == 1)
					return false;
			}
			break;
		}
		return true;
	}

	bool getBConvert(int iType){
		var iLine = _iLine;
		var iRow = _iRow;
		if (_idx != 6 && iLine >= IL - 2)
			return false;
		if (iRow == -1)
			iRow = 0;
		if (_idx != 6 && iRow == 8)
			iRow = 7;
		switch (_idx) {
		case 6:
			switch (iType) {
			case 0:
				if (iLine >= IL - 1)
					return false;
				if (iLine <= 1)
					iLine += 2;
				for (int i = 0; i < 4; i++) {
					if (iLine - 2 + i < 0)
						continue;
					if (lMain [iLine - 2 + i] [iRow + 1] == 1) {
						return false;
					}
				}
				break;
			case 1:
				if (iRow == 7)
					iRow -= 1;
				if (iRow == 8)
					iRow -= 2;
				for (int i = 0; i < 4; i++) {
					if (lMain [iLine] [iRow + i] == 1) {
						return false;
					}
				}
				break;
			}
			break;
		case 5:
			switch (iType) {
			case 0:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 1 + i] [iRow + 2] == 1)
						return false;
				}
				break;
			case 1:
				if (lMain [iLine] [iRow + 2] == 1)
					return false;
				if (lMain [iLine + 1] [iRow] == 1)
					return false;
				break;
			}
			break;
		case 4:
			switch (iType) {
			case 0:
				if (lMain [iLine] [iRow + 2] == 1)
					return false;
				if (lMain [iLine + 2] [iRow + 1] == 1)
					return false;
				break;
			case 1:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine] [iRow + i] == 1)
						return false;
				}
				break;
			}
			break;
		case 2:
			switch (iType) {
			case 0:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 2] [iRow + 1 + i] == 1)
						return false;
				}
				if (lMain [iLine] [iRow + 1] == 1)
					return false;
				break;
			case 1:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 1 + i] [iRow] == 1)
						return false;
				}
				if (lMain [iLine + 1] [iRow + 2] == 1)
					return false;
				break;
			case 2:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine] [iRow + i] == 1)
						return false;
				}
				if (lMain [iLine + 2] [iRow + 1] == 1)
					return false;
				break;
			case 3:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + i] [iRow + 2] == 1)
						return false;
				}
				if (lMain [iLine + 1] [iRow] == 1)
					return false;
				break;
			}
			break;
		case 1:
			switch (iType) {
			case 0:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine] [iRow + 1 + i] == 1)
						return false;
				}
				if (lMain [iLine + 2] [iRow + 1] == 1)
					return false;
				break;
			case 1:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 1 + i] [iRow + 2] == 1)
						return false;
				}
				if (lMain [iLine + 1] [iRow] == 1)
					return false;
				break;
			case 2:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + 2] [iRow + i] == 1)
						return false;
				}
				if (lMain [iLine] [iRow + 1] == 1)
					return false;
				break;
			case 3:
				for (int i = 0; i < 2; i++) {
					if (lMain [iLine + i] [iRow] == 1)
						return false;
				}
				if (lMain [iLine + 1] [iRow + 2] == 1)
					return false;
				break;
			}
			break;
		case 0:
			switch (iType) {
			case 0:
				if (lMain [iLine + 2] [iRow + 1] == 1)
					return false;
				break;
			case 1:
				if (lMain [iLine + 1] [iRow] == 1)
					return false;
				break;
			case 2:
				if (lMain [iLine] [iRow + 1] == 1)
					return false;
				break;
			case 3:
				if (lMain [iLine + 1] [iRow + 2] == 1)
					return false;
				break;
			}
			break;
		}
		return true;
	}

	void setLMain(int iType){
		var iLine = _iLine - 1;
		var goRow = goMain.GetChild (0);
		switch(_idx){
		case 6:
			switch (iType) {
			case 0:
				for (int i = 0; i < 4; i++) {
					lMain [iLine] [i + _iRow] = 1;
					goRow.GetChild (iLine).GetChild (i + _iRow).GetComponent<CGrid> ().showDown (_sprite);
				}
				break;
			case 1:
				for (int i = 0; i < 4; i++) {
					lMain [iLine + 1 - i] [1 + _iRow] = 1;
					goRow.GetChild (iLine + 1 - i).GetChild (1 + _iRow).GetComponent<CGrid> ().showDown (_sprite);
				}
				break;
			}
			break;
		case 5:
			switch (iType) {
			case 0:
				for (int i = 0; i < 2; i++) {
					lMain [iLine + 1] [_iRow + i] = 1;
					goRow.GetChild (iLine + 1).GetChild (_iRow + i).GetComponent<CGrid> ().showDown (_sprite);
					lMain [iLine] [_iRow + i + 1] = 1;
					goRow.GetChild (iLine).GetChild (_iRow + i + 1).GetComponent<CGrid> ().showDown (_sprite);
				}
				break;
			case 1:
				for (int i = 0; i < 2; i++) {
					lMain [iLine + i + 1] [_iRow + 2] = 1;
					goRow.GetChild (iLine + i + 1).GetChild (_iRow + 2).GetComponent<CGrid> ().showDown (_sprite);
					lMain [iLine + i] [_iRow + 1] = 1;
					goRow.GetChild (iLine + i).GetChild (_iRow + 1).GetComponent<CGrid> ().showDown (_sprite);
				}
				break;
			}
			break;
		case 4:
			switch (iType) {
			case 0:
				for (int i = 0; i < 2; i++) {
					lMain [iLine + 1] [_iRow + 1 + i] = 1;
					goRow.GetChild (iLine + 1).GetChild (_iRow + 1 + i).GetComponent<CGrid> ().showDown (_sprite);
					lMain [iLine] [_iRow + i] = 1;
					goRow.GetChild (iLine).GetChild (_iRow + i).GetComponent<CGrid> ().showDown (_sprite);
				}
				break;
			case 1:
				for (int i = 0; i < 2; i++) {
					lMain [iLine + i] [_iRow + 2] = 1;
					goRow.GetChild (iLine + i).GetChild (_iRow + 2).GetComponent<CGrid> ().showDown (_sprite);
					lMain [iLine + 1 + i] [_iRow + 1] = 1;
					goRow.GetChild (iLine + 1 + i).GetChild (_iRow + 1).GetComponent<CGrid> ().showDown (_sprite);
				}
				break;
			}
			break;
		case 3:
			for (int i = 0; i < 2; i++) {
				lMain [iLine + 1] [_iRow + i + 1] = 1;
				goRow.GetChild (iLine + 1).GetChild (_iRow + i + 1).GetComponent<CGrid> ().showDown (_sprite);
				lMain [iLine] [_iRow + i + 1] = 1;
				goRow.GetChild (iLine).GetChild (_iRow + i + 1).GetComponent<CGrid> ().showDown (_sprite);
			}
			break;
		case 2:
		case 1:
			switch (iType) {
			case 0:
				for (int i = 0; i < 3; i++) {
					lMain [iLine + 1] [_iRow + i] = 1;
					goRow.GetChild (iLine + 1).GetChild (_iRow + i).GetComponent<CGrid> ().showDown (_sprite);
				}
				lMain [iLine] [_iRow + 2 * (_idx - 1)] = 1;
				goRow.GetChild (iLine).GetChild (_iRow + 2 * (_idx - 1)).GetComponent<CGrid> ().showDown (_sprite);
				break;
			case 1:
				for (int i = 0; i < 3; i++) {
					lMain [iLine + i] [_iRow + 1] = 1;
					goRow.GetChild (iLine + i).GetChild (_iRow + 1).GetComponent<CGrid> ().showDown (_sprite);
				}
				lMain [iLine + 2 * (_idx - 1)] [_iRow + 2] = 1;
				goRow.GetChild (iLine + 2 * (_idx - 1)).GetChild (_iRow + 2).GetComponent<CGrid> ().showDown (_sprite);
				break;
			case 2:
				for (int i = 0; i < 3; i++) {
					lMain [iLine + 1] [_iRow + i] = 1;
					goRow.GetChild (iLine + 1).GetChild (_iRow + i).GetComponent<CGrid> ().showDown (_sprite);
				}
				lMain [iLine + 2] [_iRow + 2 * (2 - _idx)] = 1;
				goRow.GetChild (iLine + 2).GetChild (_iRow + 2 * (2 - _idx)).GetComponent<CGrid> ().showDown (_sprite);
				break;
			case 3:
				for (int i = 0; i < 3; i++) {
					lMain [iLine + i] [_iRow + 1] = 1;
					goRow.GetChild (iLine + i).GetChild (_iRow + 1).GetComponent<CGrid> ().showDown (_sprite);
				}
				lMain [iLine + 2 * (2 - _idx)] [_iRow] = 1;
				goRow.GetChild (iLine + 2 * (2 - _idx)).GetChild (_iRow).GetComponent<CGrid> ().showDown (_sprite);
				break;
			}
			break;
		case 0:
			for (int i = 0; i < 3; i++) {
				if (iType != 2 && iType == i + 1)
					continue;
				lMain [iLine + 1] [_iRow + i] = 1;
				goRow.GetChild (iLine + 1).GetChild (_iRow + i).GetComponent<CGrid> ().showDown (_sprite);
			}
			if (iType != 2) {
				lMain [iLine] [_iRow + 1] = 1;
				goRow.GetChild (iLine).GetChild (_iRow + 1).GetComponent<CGrid> ().showDown (_sprite);
			}
			if (iType != 0) {
				lMain [iLine + 2] [_iRow + 1] = 1;
				goRow.GetChild (iLine + 2).GetChild (_iRow + 1).GetComponent<CGrid> ().showDown (_sprite);
			}
			break;
		}
		if (_idx == 6) {
			if (iType == 0)
				showRemove (iLine);
			else {
				for (int i = 0; i < 4; i++) {
					showRemove (iLine - 2 + i);
				}
			}
		} else {
			for (int i = 0; i < 3; i++) {
				showRemove (iLine + i);
			}
		}
		Destroy (_item.gameObject);
		Destroy (_shadow.gameObject);
	}

	void showRemove(int iLine){
		if (iLine >= lMain.Count)
			return;
		var lData = lMain [iLine];
		var bRemove = true;
		for (int i = 0; i < lData.Count; i++) {
			if (lData [i] == 0) {
				bRemove = false;
				break;
			}
		}
		if (bRemove == true) {
			_iMoveLine = iLine;
			StartCoroutine (playRemove ());
		}
	}

	IEnumerator playRemove(){
		_bPlay = true;
		var iLine = _iMoveLine;
		var goRow = goMain.GetChild (0);
		var transN = goRow.GetChild (iLine);
		setTouchable (false);
		for (int k = 0; k < 2; k++) {
			yield return new WaitForSeconds (0.1f);
			for (int i = 0; i < IR; i++) {
				transN.GetChild (i).GetComponent<Image> ().color = Color.black;
			}
			yield return new WaitForSeconds (0.05f);
			for (int i = 0; i < IR; i++) {
				transN.GetChild (i).GetComponent<Image> ().color = new Color (1, 1, 1, 0);
			}
		}
		addLine ();
		for (int i = iLine; i > 0; i--) {
			for (int j = 0; j < 10; j++) {
				lMain [i] [j] = lMain [i - 1] [j];
			}
			var trans = goRow.GetChild (i - 1);
			var pos = trans.localPosition;
			trans.localPosition = new Vector3 (pos.x, pos.y - 40, 0);
		}
		var posN = transN.localPosition;
		transN.localPosition = new Vector3 (posN.x, posN.y + 40 * iLine, 0);
		transN.SetSiblingIndex (0);
		for (int i = 0; i < IR; i++) {
			lMain [0] [i] = 0;
			transN.GetChild (i).GetComponent<CGrid> ().initShow ();
		}
		yield return new WaitForSeconds (0.1f);
		if (_bPlay == true)
			onNext ();
		_bPlay = false;
		setTouchable (true);
	}

	IEnumerator playTips(){
		goTips.gameObject.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		goTips.gameObject.SetActive (false);
	}

	void showTips(string str){
		goTips.GetChild (0).GetComponent<Text> ().text = str;
		StartCoroutine (playTips ());
	}

	void setTouchable(bool bTouch){
		var control = GetComponent<CanvasGroup> ();
		control.blocksRaycasts = bTouch;
	} 
}
