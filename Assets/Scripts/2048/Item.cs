using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    bool _bPlayScale = false;
    bool _bPlayMoveY = false;
    float _py, _px;
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
        if (_bPlayScale)
        {
            var dt = Time.deltaTime;
            transform.localScale += new Vector3(10 * dt, 10 * dt, 10 * dt);
            if (transform.localScale.x >= 1)
            {
                transform.localScale = Vector3.one;
                _bPlayScale = false;
            }
        } else if (_bPlayMoveY)
        {
            var dt = Time.deltaTime;
            transform.Translate(0, 400 * dt, 0);
            var rect = GetComponent<RectTransform>();
            if (rect.anchoredPosition.y >= _py)
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, _py);
                _bPlayMoveY = false;
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
        var idx = _delt.getIdx(iNum);
        //lab.text = "<color=#" + _delt.numColors[idx] + ">" + iNum + " </color>";
        lab.text = "<color=#" + "000" + ">" + iNum + "</color>";
        //lab.fontSize = _delt.sizes[idx];
        img.color = FuncMgr.getColorFromHex(_delt.colors[idx]);
    }

    public void playScale()
    {
        _bPlayScale = true;
        transform.gameObject.SetActive(true);
        transform.localScale = Vector3.one / 10;
    }

    public void playMoveY(float i)
    {
        _py = i;
        _bPlayMoveY = true;
    }
}
