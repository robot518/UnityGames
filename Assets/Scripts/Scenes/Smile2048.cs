using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Smile2048 : MonoBehaviour {
	const int IMAX = 16;
	const int IDX = 160;
	const int IROW = 4;
	int _iScore;
	int _dx;
	int _dy;
	float _iCost;
	bool _bOver = true;
	List<int> lIdx = new List<int> ();
	List<Transform> lItem = new List<Transform> ();
	List<Item> lItemShow = new List<Item> ();
	List<Vector2> lVect = new List<Vector2> ();
	Transform goTips;
	Text labScore;
	AudioMgr adMgr;

	// Use this for initialization
	void Start () {
		initParas ();
		initEvent ();
		initShow ();
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
		_iScore = 0;
		var goTop = transform.Find ("goTop");
		labScore = goTop.GetChild (0).GetComponent<Text> ();
		adMgr = AudioMgr.getInstance ();
		var goPlay = transform.Find ("goPlay");
		for (int i = 0; i < IMAX; i++) {
			var item = goPlay.GetChild (i + 1);
			lItem.Add (item);
			lVect.Add (item.position);
		}
		goTips = transform.Find ("goTips");
	}

	void initEvent(){
        transform.Find("goTop/back").gameObject.GetComponent<Button>().onClick.AddListener (delegate() {
            SceneManager.LoadScene("Lobby");
        });
		var goBtns = transform.Find ("btns");
		UnityAction[] tFunc = {onClickStart};
		for (int i = 0; i < tFunc.Length; i++) {
			var btn = goBtns.GetChild (i).gameObject.GetComponent<Button> ();
			btn.onClick.AddListener (tFunc[i]);
		}
		var goControl = transform.Find ("control");
		for (int i = 0; i < 4; i++) {
			var idxTemp = i;
			var btn = goControl.GetChild (i).gameObject.GetComponent<Button> ();
			btn.onClick.AddListener (delegate() {
				if (_bOver == true)
					return;
				var li = new List<int>(){0, 0, 0, 0};
				var lnum = new List<List<int>>();
				for (int x = 0; x < IROW; x++) {
					lnum.Add(new List<int>());
				}
				var lrIdx = new List<int>();
				switch(idxTemp){
				case 0:
					for (int j = 0; j < lIdx.Count; j++) {
						var idx = lIdx[j];
						var iX = Mathf.FloorToInt (idx / IROW);
						var iY = idx % IROW;
						var newIdx = IROW*iX + li[iX];
						li[iX]++;
						var item = getItemByIdx(idx);
						var iNum = item.getNum();
						if (li[iX] > 1 && iNum == lnum[iX][li[iX] - 2]){
							var newItem = getItemByIdx(newIdx - 1);
							newItem.showLab(2*iNum);
							item.gameObject.SetActive(false);
							lrIdx.Add(idx);
							lItemShow.Remove (item);
							lItem.Add (item.transform);
							li[iX]--;
						}else{
							item.setIdx(newIdx);
//							item.transform.position = lVect[newIdx];
							lIdx[j] = newIdx;
							lnum[iX].Add(item.getNum());
							item.setMove(0, -1, lVect[newIdx].x);
						}
					}
					break;
				case 1:
					for (int j = 0; j < lIdx.Count; j++) {
						var idx = lIdx[j];
						var iX = Mathf.FloorToInt (idx / IROW);
						var iY = idx % IROW;
						var newIdx = idx - IROW*(iX - li[iY]);
						li[iY]++;
						var item = getItemByIdx(idx);
						var iNum = item.getNum();
						if (li[iY] > 1 && iNum == lnum[iY][li[iY] - 2]){
							var newItem = getItemByIdx(newIdx - IROW);
							newItem.showLab(2*iNum);
							item.gameObject.SetActive(false);
							lrIdx.Add(idx);
							lItemShow.Remove (item);
							lItem.Add (item.transform);
							li[iY]--;
						}else{
							item.setIdx(newIdx);
							item.transform.position = lVect[newIdx];
							lIdx[j] = newIdx;
							lnum[iY].Add(item.getNum());
						}
					}
					break;
				case 2:
					for (int j = lIdx.Count - 1; j >= 0; j--) {
						var idx = lIdx[j];
						var iX = Mathf.FloorToInt (idx / IROW);
						var iY = idx % IROW;
						var newIdx = idx + IROW*(IROW - 1 - iX - li[iY]);
						li[iY]++;
						var item = getItemByIdx(idx);
						var iNum = item.getNum();
						if (li[iY] > 1 && iNum == lnum[iY][li[iY] - 2]){
							var newItem = getItemByIdx(newIdx + IROW);
							newItem.showLab(2*iNum);
							item.gameObject.SetActive(false);
							lrIdx.Add(idx);
							lItemShow.Remove (item);
							lItem.Add (item.transform);
							li[iY]--;
						}else{
							item.setIdx(newIdx);
							item.transform.position = lVect[newIdx];
							lIdx[j] = newIdx;
							lnum[iY].Add(item.getNum());
						}
					}
					break;
				case 3:
					for (int j = lIdx.Count - 1; j >= 0; j--) {
						var idx = lIdx[j];
						var iX = Mathf.FloorToInt (idx / IROW);
						var iY = idx % IROW;
						var newIdx = IROW*iX + IROW - 1 - li[iX];
						li[iX]++;
						var item = getItemByIdx(idx);
						var iNum = item.getNum();
						if (li[iX] > 1 && iNum == lnum[iX][li[iX] - 2]){
							var newItem = getItemByIdx(newIdx + 1);
							newItem.showLab(2*iNum);
							item.gameObject.SetActive(false);
							lrIdx.Add(idx);
							lItemShow.Remove (item);
							lItem.Add (item.transform);
							li[iX]--;
						}else{
							item.setIdx(newIdx);
							item.transform.position = lVect[newIdx];
							lIdx[j] = newIdx;
							lnum[iX].Add(item.getNum());
						}
					}
					break;
				}
				for (int iy = 0; iy < lrIdx.Count; iy++) {
					lIdx.Remove(lrIdx[iy]);
				}
				onAddItem();
			});
		}
	}

	Item getItemByIdx(int idx){
		for (int i = 0; i < lItemShow.Count; i++) {
			var item = lItemShow [i];
			if (item.getIdx () == idx)
				return item;
		}
		return null;
	}

	void initShow(){
		showScore ();
		for (int i = 0; i < lItem.Count; i++) {
			var item = lItem [i];
			item.gameObject.SetActive (false);
		}
		goTips.gameObject.SetActive (false);
	}

	void onClickStart(){
		_bOver = false;
		_iScore = 0;
		_dx = 0;
		_dy = 0;
		_iCost = 0;
		initShow ();
		lIdx.Clear ();
		lItem.Clear ();
		lItemShow.Clear ();
		var goPlay = transform.Find ("goPlay");
		for (int i = 1; i < goPlay.childCount; i++) {
			var item = goPlay.GetChild (i);
			lItem.Add (item);
			item.gameObject.SetActive (false);
		}
		onAddItem ();
		onAddItem ();
	}
		
	void onAddItem(){
		if (lIdx.Count >= IMAX) {
			adMgr.PlaySound ("lose");
			showTipsSP ("lose");
			_bOver = true;
			return;
		}
		var iNum = Random.Range (0, 2);
		iNum = iNum == 0 ? 2 : 4;
		var vP = new List<int> ();
		for (int i = 0; i < IMAX; i++) {
			if (lIdx.Contains(i) == true) {
				continue;
			}
			vP.Add (i);
		}
		var idx = vP[Random.Range (0, vP.Count)];
		var item = lItem [lItem.Count - 1];
		lItem.RemoveAt (lItem.Count - 1);
		var sItem = item.GetComponent<Item> ();
		lItemShow.Add (sItem);
		item.gameObject.SetActive (true);
		item.position = lVect [idx];
		sItem.showLab (iNum);
		sItem.setIdx (idx);
		lIdx.Add (idx);
		lIdx.Sort ();
	}

	void showTipsSP(string str){
		goTips.GetChild (0).GetComponent<Text> ().text = str;
		goTips.gameObject.SetActive (true);
	}

	void showScore(){
		labScore.text = _iScore.ToString ();
	}
}
