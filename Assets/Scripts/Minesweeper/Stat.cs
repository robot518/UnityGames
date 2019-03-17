using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Stat : MonoBehaviour {
	IOMgr ioMgr;
	Text labWin;
	Text labLose;
	Text labMinTime;
	Text labAvgTime;
	Text labRate;
	Text labCount;
	Text labMaxWinSteak;
	Text labCurWinSteak;

	Text labWinSP;
	Text labLoseSP;
	Text labMinTimeSP;
	Text labAvgTimeSP;
	Text labRateSP;
	Text labCountSP;
	Text labMaxWinSteakSP;
	Text labCurWinSteakSP;

	// Use this for initialization
	void Start () {
		initParas ();
		initEvent ();
		initShow ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void initParas() {
		ioMgr = new IOMgr ();
		var normal = transform.Find ("normal");
		labWin = normal.Find ("win").GetChild (0).GetComponent<Text> ();
		labLose = normal.Find ("lose").GetChild (0).GetComponent<Text> ();
		labMinTime = normal.Find ("mTime").GetChild (0).GetComponent<Text> ();
		labAvgTime = normal.Find ("aTime").GetChild (0).GetComponent<Text> ();
		labRate = normal.Find ("rate").GetChild (0).GetComponent<Text> ();
		labCount = normal.Find ("count").GetChild (0).GetComponent<Text> ();
		labMaxWinSteak = normal.Find ("mWinSteak").GetChild (0).GetComponent<Text> ();
		labCurWinSteak = normal.Find ("nWinSteak").GetChild (0).GetComponent<Text> ();

		var big = transform.Find ("big");
		labWinSP = big.Find ("win").GetChild (0).GetComponent<Text> ();
		labLoseSP = big.Find ("lose").GetChild (0).GetComponent<Text> ();
		labMinTimeSP = big.Find ("mTime").GetChild (0).GetComponent<Text> ();
		labAvgTimeSP = big.Find ("aTime").GetChild (0).GetComponent<Text> ();
		labRateSP = big.Find ("rate").GetChild (0).GetComponent<Text> ();
		labCountSP = big.Find ("count").GetChild (0).GetComponent<Text> ();
		labMaxWinSteakSP = big.Find ("mWinSteak").GetChild (0).GetComponent<Text> ();
		labCurWinSteakSP = big.Find ("nWinSteak").GetChild (0).GetComponent<Text> ();
	}

	void initEvent(){
		var back = transform.Find ("back").gameObject.GetComponent<Button> ();
		back.onClick.AddListener (delegate() {
			gameObject.SetActive(false);
		});
	}

	public void initShow(){
		if (ioMgr == null)
			return;
		var lLab1 = new List<Text> () {labWin, labLose, labMinTime, labAvgTime, labRate,
			labCount, labMaxWinSteak, labCurWinSteak
		};
		var lLabSP = new List<Text> () {labWinSP, labLoseSP, labMinTimeSP, labAvgTimeSP, labRateSP,
			labCountSP, labMaxWinSteakSP, labCurWinSteakSP
		};
		for (int j = 0; j < 2; j++) {
			var info = ioMgr.LoadFile (j);
			if (info == null) {
				info = new List<string> ();
			}
			var iL = info.Count;
			var lLab = j == 0 ? lLab1 : lLabSP;
			for (int i = 0; i < lLab.Count; i++) {
				if (iL - 1 < i)
					lLab [i].text = "0";
				else
					lLab [i].text = info [i];
			};
		}
	}
}