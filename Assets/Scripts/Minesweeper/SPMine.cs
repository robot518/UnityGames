using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPMine {
	int _iTotal;
	int _iRow;

	public SPMine(int iTotal, int iRow){
		_iTotal = iTotal;
		_iRow = iRow;
	}

	public int[] getSPMine(string str){
		switch (str) {
		case "sz":
			return getZMine ();
//			break;
		case "sh":
			return getHMine ();
//			break;
		case "si":
			return getIMine ();
//			break;
		case "sc":
			return getCMine ();
//			break;
		case "su":
			return getUMine ();
//			break;
		case "sl":
			return getLMine ();
//			break;
		case "sa":
			return getAMine ();
//			break;
		case "sn":
			return getNMine ();
//			break;
		default:
			return getZMine ();
//			break;
		}
	}

	int[] getZMine(){
		var iTotal = _iTotal;
		var iRow = _iRow;
		var tMineNum = new int[iTotal];
		for (var i = 0; i < iTotal; i++) {
			var iLine = (int)Mathf.Floor (i / iRow) + 1;
			var iR = i % iRow;
			if (i < iRow)
				tMineNum[i] = 1;
			else if (i >= iTotal - iRow)
				tMineNum[i] = 1;
			else if (iLine == 2 && iR == iRow - 1)
				tMineNum[i] = 1;
			else if (iLine == 3 && (iR == iRow - 1 || iR == iRow - 2))
				tMineNum[i] = 1;
			else if (iLine == 4 && (iR == iRow - 2 || iR == iRow - 3))
				tMineNum[i] = 1;
			else if (iLine == 5 && (iR == iRow - 3 || iR == iRow - 4))
				tMineNum[i] = 1;
			else if (iLine == 6 && (iR == iRow - 4 || iR == iRow - 5))
				tMineNum[i] = 1;
			else if (iLine == 7 && (iR == iRow - 5 || iR == iRow - 6))
				tMineNum[i] = 1;
			else if (iLine == 8 && (iR == iRow - 6 || iR == iRow - 7))
				tMineNum[i] = 1;
			else if (iLine == 9 && (iR == iRow - 7 || iR == iRow - 8))
				tMineNum[i] = 1;
			else if (iLine == 10 && (iR == iRow - 8 || iR == iRow - 9))
				tMineNum[i] = 1;
			else if (iLine == 11 && iR == iRow - 9)
				tMineNum[i] = 1;
			else
				tMineNum[i] = 0;
		};
		return tMineNum;
	}

	int[] getHMine(){
		var iTotal = _iTotal;
		var iRow = _iRow;
		var tMineNum = new int[iTotal];
		for (var i = 0; i < iTotal; i++) {
			var iLine = (int)Mathf.Floor (i / iRow) + 1;
			var iR = i % iRow;
			if (iR == 0 || iLine == 6 || iR == 8)
				tMineNum [i] = 1;
			else
				tMineNum[i] = 0;
		};
		return tMineNum;
	}

	int[] getIMine(){
		var iTotal = _iTotal;
		var iRow = _iRow;
		var tMineNum = new int[iTotal];
		for (var i = 0; i < iTotal; i++) {
			var iLine = (int)Mathf.Floor (i / iRow) + 1;
			var iR = i % iRow;
			if (iR == 4)
				tMineNum [i] = 1;
			else if ((iLine == 1 || iLine == 12) && (iR > 1 && iR < 7))
				tMineNum [i] = 1;
			else
				tMineNum[i] = 0;
		};
		return tMineNum;
	}

	int[] getCMine(){
		var iTotal = _iTotal;
		var iRow = _iRow;
		var tMineNum = new int[iTotal];
		for (var i = 0; i < iTotal; i++) {
			var iLine = (int)Mathf.Floor (i / iRow) + 1;
			var iR = i % iRow;
			if ((iLine == 1 || iLine == 12) && iR > 4)
				tMineNum[i] = 1;
			else if ((iLine == 2 || iLine == 11) && (iR == 4 || iR == 3))
				tMineNum[i] = 1;
			else if ((iLine == 3 || iLine == 10) && (iR == 2 || iR == 1))
				tMineNum[i] = 1;
			else if ((iLine == 4 || iLine == 9) && iR == 1)
				tMineNum[i] = 1;
			else if ((iLine == 5 || iLine == 8) && iR == 0)
				tMineNum[i] = 1;
			else if ((iLine == 6 || iLine == 7) && iR == 0)
				tMineNum[i] = 1;
			else
				tMineNum[i] = 0;
		};
		return tMineNum;
	}

	int[] getUMine(){
		var iTotal = _iTotal;
		var iRow = _iRow;
		var tMineNum = new int[iTotal];
		for (var i = 0; i < iTotal; i++) {
			var iLine = (int)Mathf.Floor (i / iRow) + 1;
			var iR = i % iRow; 
			if (iR == 0 || iLine == 12 || iR == 8)
				tMineNum [i] = 1;
			else
				tMineNum[i] = 0;
		};
		return tMineNum;
	}

	int[] getLMine(){
		var iTotal = _iTotal;
		var iRow = _iRow;
		var tMineNum = new int[iTotal];
		for (var i = 0; i < iTotal; i++) {
			var iLine = (int)Mathf.Floor (i / iRow) + 1;
			var iR = i % iRow; 
			if (iR == 0 || iLine == 12)
				tMineNum [i] = 1;
			else
				tMineNum[i] = 0;
		};
		return tMineNum;
	}

	int[] getAMine(){
		var iTotal = _iTotal;
		var iRow = _iRow;
		var tMineNum = new int[iTotal];
		for (var i = 0; i < iTotal; i++) {
			var iLine = (int)Mathf.Floor (i / iRow) + 1;
			var iR = i % iRow; 
			if (iLine == 1 && iR == 4)
				tMineNum [i] = 1;
			else if (iLine == 2 && (iR == 3 || iR == 5))
				tMineNum[i] = 1;
			else if (iLine == 3 && (iR == 3 || iR == 5))
				tMineNum[i] = 1;
			else if (iLine == 4 && (iR == 3 || iR == 5))
				tMineNum[i] = 1;
			else if (iLine == 5 && (iR == 2 || iR == 6))
				tMineNum[i] = 1;
			else if (iLine == 6 && (iR == 2 || iR == 6))
				tMineNum[i] = 1;
			else if (iLine == 7 && (iR == 2 || iR == 6))
				tMineNum[i] = 1;
			else if (iLine == 8 && (iR >= 1 && iR <= 7))
				tMineNum[i] = 1;
			else if (iLine == 9 && (iR == 1 || iR == 7))
				tMineNum[i] = 1;
			else if (iLine == 10 && (iR == 0 || iR == 8))
				tMineNum[i] = 1;
			else if (iLine == 11 && (iR == 0 || iR == 8))
				tMineNum[i] = 1;
			else if (iLine == 12 && (iR == 0 || iR == 8))
				tMineNum[i] = 1;
			else
				tMineNum[i] = 0;
		};
		return tMineNum;
	}

	int[] getNMine(){
		var iTotal = _iTotal;
		var iRow = _iRow;
		var tMineNum = new int[iTotal];
		for (var i = 0; i < iTotal; i++) {
			var iLine = (int)Mathf.Floor (i / iRow) + 1;
			var iR = i % iRow; 
			if (iR == 0 || iR == 8)
				tMineNum [i] = 1;
			else if (iLine == 2 && iR == 1)
				tMineNum[i] = 1;
			else if (iLine == 3 && iR == 2)
				tMineNum[i] = 1;
			else if (iLine == 4 && iR == 3)
				tMineNum[i] = 1;
			else if (iLine == 5 && iR == 3)
				tMineNum[i] = 1;
			else if (iLine == 6 && iR == 4)
				tMineNum[i] = 1;
			else if (iLine == 7 && iR == 4)
				tMineNum[i] = 1;
			else if (iLine == 8 && iR == 5)
				tMineNum[i] = 1;
			else if (iLine == 9 && iR == 5)
				tMineNum[i] = 1;
			else if (iLine == 10 && iR == 6)
				tMineNum[i] = 1;
			else if (iLine == 11 && iR == 7)
				tMineNum[i] = 1;
			else
				tMineNum[i] = 0;
		};
		return tMineNum;
	}
}
