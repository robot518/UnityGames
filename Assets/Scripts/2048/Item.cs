using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    bool _bPlay = false;
	Text lab;
    Image img;
    Smile2048 _delt;

    void Awake(){
		initParas ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_bPlay)
        {
            transform.localScale += Vector3.one / 20;
            if (transform.localScale == Vector3.one)
                _bPlay = false;
        }
    }

    public void init(Smile2048 delt)
    {
        _delt = delt;
    }

    void initParas(){
        img = transform.GetComponent<Image>();
        lab = transform.GetChild (0).GetComponent<Text> ();
	}

	public void showLab(int iNum){
        var idx = _delt.getIdx(iNum);
        //lab.text = "<color=#" + _delt.numColors[idx] + ">" + iNum + " </color>";
        lab.text = "<color=#" + "000" + ">" + iNum + "</color>";
        //lab.fontSize = _delt.sizes[idx];
        img.color = FuncMgr.getColorFromHex(_delt.colors[idx]);
    }

    public void play()
    {
        //_bPlay = true;
        transform.gameObject.SetActive(true);
        //transform.localScale = Vector3.one / 10;
    }
}
