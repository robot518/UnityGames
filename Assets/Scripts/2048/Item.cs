using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    bool _bPlayScale = false, _bPlayMove = false, _bMerge = false;
    const float SPEED = 4800.0f;
    int _num, _my = -1, _mx;
    float _y, _x, _t, _cost, _sm;
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
            transform.localScale += new Vector3(7 * dt, 7 * dt, 0);
            if (transform.localScale.x >= 1)
            {
                transform.localScale = Vector3.one;
                _bPlayScale = false;
            }
        } else if (_bPlayMove)
        {
            var dt = Time.deltaTime;
            _t += dt;
            if (_t >= _cost)
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
            else
            {
                //transform.Translate(_sm * _mx * dt, _sm * _my * dt, 0);
                GetComponent<RectTransform>().anchoredPosition += new Vector2(_sm * _mx * dt, _sm * _my * dt);
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
        var color = iNum <= 4 ? "000" : "fff";
        lab.text = "<color=#" + color + ">" + iNum + "</color>";
        lab.fontSize = iNum < 1024 ? 70 : 50;
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
        _cost = (_mx != 0 ? _mx : _my) / SPEED;
        if (_cost < 0) _cost = -_cost;
        _sm = 1 / _cost;
        _bPlayMove = true;
        _bMerge = bMerge;
        mergeItem = item;
    }
}
