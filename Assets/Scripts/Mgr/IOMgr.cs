//public static readonly string PathURL =  
//#if UNITY_ANDROID   //安卓  
//	"jar:file://" + Application.dataPath + "!/assets/";  
//#elif UNITY_IPHONE  //iPhone  
//	Application.dataPath + "/Raw/";  
//#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台  
//	"file://" + Application.dataPath + "/StreamingAssets/";  
//#else  
//	string.Empty;  
//#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class IOMgr {
	static IOMgr mgr;
	string filePath = Application.persistentDataPath + "//" + "mine.txt";
	string filePathSP = Application.persistentDataPath + "//" + "mineSP.txt";
	string filePathSound = Application.persistentDataPath + "//" + "sound.txt";

	public static IOMgr getInstance() {
		if (mgr == null)
			mgr = new IOMgr ();
		return mgr;
	}
//	public IOMgr()
//	{    
//	  
//	}
	public void CreateFile(){
		CreateFile (filePath);
		CreateFile (filePathSP);
		CreateFile (filePathSound);
	}
	public void CreateFile(string str) {
		//文件流信息
		StreamWriter sw;
		FileInfo t = new FileInfo(str);
		if(!t.Exists)
		{
			//如果此文件不存在则创建
			sw = t.CreateText();
			//以行的形式写入信息
			if (str == filePathSound)
				sw.WriteLine (0);
			else {
				for (int i = 0; i < 9; i++) {
					if (i == 2)
						sw.WriteLine(3601);
					else
						sw.WriteLine(0);
				}
			}
		}
		else
		{
			//如果此文件存在则打开
			sw = t.AppendText();
		}
		//关闭流
		sw.Close();
		//销毁流
		sw.Dispose();
	}  
	public List<string> LoadFile(int idx) {
		//使用流的形式读取
		var str = "";
		if (idx == -1)
			str = filePathSound;
		else
			str = idx == 0 ? filePath : filePathSP;
		StreamReader sr =null;
		try{
			sr = File.OpenText(str);
		}catch(Exception e)
		{
			//路径与名称未找到文件则直接返回空
			return null;
		}
		string line;
		List<string> lLines = new List<string>();
		while ((line = sr.ReadLine()) != null)
		{
			//一行一行的读取
			//将每一行的内容存入数组链表容器中
			lLines.Add(line);
		}
		//关闭流
		sr.Close();
		//销毁流
		sr.Dispose();
		//将数组链表容器返回
		return lLines;
	}   
	//胜利次数0、失败次数1、最短用时2、平均用时3、胜率4、总次数5、最大连胜6、当前连胜7、总用时8
	public void WriteWin(int iCost){
		var lStr = File.ReadAllLines (filePath);
		var iWinCount = Convert.ToInt32 (lStr [0]) + 1;
		lStr [0] = iWinCount.ToString ();
		var iPreCost = Convert.ToInt32 (lStr [2]);
		if (iCost < iPreCost)
			lStr [2] = iCost.ToString ();
		var iCount = Convert.ToInt32 (lStr [5]) + 1;
		lStr [5] = iCount.ToString ();
		var iCurWinSteak = Convert.ToInt32 (lStr [7]) + 1;
		var iMaxWinSteak = Convert.ToInt32 (lStr [6]);
		if (iCurWinSteak > iMaxWinSteak)
			lStr [6] = iCurWinSteak.ToString ();
		lStr [7] = iCurWinSteak.ToString ();
		var iCostCount = Convert.ToInt32 (lStr [8]) + iCost;
		lStr [8] = iCostCount.ToString ();
		lStr [3] = (iCostCount * 1.0 / iWinCount).ToString ("#0.00");
		lStr [4] = (iWinCount * 1.0 / iCount).ToString ("#0.00");
		File.WriteAllLines (filePath, lStr);
	}
	public void WriteLose(){
		var lStr = File.ReadAllLines (filePath);
		lStr [1] = (Convert.ToInt32 (lStr [1]) + 1).ToString ();
		var iCount = Convert.ToInt32 (lStr [5]) + 1;
		lStr [5] = iCount.ToString ();
		var iWinCount = Convert.ToInt32 (lStr [0]);
		lStr [4] = (iWinCount * 1.0 / iCount).ToString ("#0.00");
		lStr [7] = 0.ToString ();
		File.WriteAllLines (filePath, lStr);
	}

	public void WriteWinSP(int iCost){
		var lStr = File.ReadAllLines (filePathSP);
		var iWinCount = Convert.ToInt32 (lStr [0]) + 1;
		lStr [0] = iWinCount.ToString ();
		var iPreCost = Convert.ToInt32 (lStr [2]);
		if (iCost < iPreCost)
			lStr [2] = iCost.ToString ();
		var iCount = Convert.ToInt32 (lStr [5]) + 1;
		lStr [5] = iCount.ToString ();
		var iCurWinSteak = Convert.ToInt32 (lStr [7]) + 1;
		var iMaxWinSteak = Convert.ToInt32 (lStr [6]);
		if (iCurWinSteak > iMaxWinSteak)
			lStr [6] = iCurWinSteak.ToString ();
		lStr [7] = iCurWinSteak.ToString ();
		var iCostCount = Convert.ToInt32 (lStr [8]) + iCost;
		lStr [8] = iCostCount.ToString ();
		lStr [3] = (iCostCount * 1.0 / iWinCount).ToString ("#0.00");
		lStr [4] = (iWinCount * 1.0 / iCount).ToString ("#0.00");
		File.WriteAllLines (filePathSP, lStr);
	}
	public void WriteLoseSP(){
		var lStr = File.ReadAllLines (filePathSP);
		lStr [1] = (Convert.ToInt32 (lStr [1]) + 1).ToString ();
		var iCount = Convert.ToInt32 (lStr [5]) + 1;
		lStr [5] = iCount.ToString ();
		var iWinCount = Convert.ToInt32 (lStr [0]);
		lStr [4] = (iWinCount * 1.0 / iCount).ToString ("#0.00");
		lStr [7] = 0.ToString ();
		File.WriteAllLines (filePathSP, lStr);
	}
	public void SetSound(int idx){
		var lStr = File.ReadAllLines (filePathSound);
		lStr [0] = idx.ToString ();
		File.WriteAllLines (filePathSound, lStr);
	}
}
