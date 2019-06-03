using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    bool _bPlayScale = false, _bPlayMove = false, _bMerge = false;
    int _num, _my = -1, _mx;
    float _y, _x, _t;
    Text lab;
    Image img;
    Smile2048 _delt;
    Item mergeItem;

    void Awake(){
		initParas ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_bPlayScale)
        {
            var dt = Time.deltaTime;
            transform.localScale += new Vector3(5 * dt, 5 * dt, 0);
            if (transform.localScale.x >= 1)
            {
                transform.localScale = Vector3.one;
                _bPlayScale = false;
            }
        } else if (_bPlayMove)
        {
            var dt = Time.deltaTime;
            _t += dt;
            transform.Translate(5*_mx*dt, 5*_my * dt, 0);
            if (_t >= 0.2f)
            {
                GetComponent<RectTransform>().anchoredPosition = new Vector2(_x, _y);
                _bPlayMove = false;
                if (_bMerge)
                {
                    gameObject.SetActive(false);
                    mergeItem.showLab(mergeItem.getNum() * 2);
                    _bMerge = false;
                }
            }
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
        _num = iNum;
        gameObject.SetActive(true);
        var idx = _delt.getIdx(iNum);
        //lab.text = "<color=#" + _delt.numColors[idx] + ">" + iNum + " </color>";
        lab.text = "<color=#" + "000" + ">" + iNum + "</color>";
        //lab.fontSize = _delt.sizes[idx];
        img.color = FuncMgr.getColorFromHex(_delt.colors[idx]);
    }

    public int getNum()
    {
        return _num;
    }

    public void playScale()
    {
        _bPlayScale = true;
        transform.localScale = Vector3.one / 10;
    }

    public void playMove(float x, float y, int mx, int my, bool bMerge, Item item)
    {
        _y = y;
        _x = x;
        _my = my;
        _mx = mx;
        _t = 0;
        _bPlayMove = true;
        _bMerge = bMerge;
        mergeItem = item;
    }
}
