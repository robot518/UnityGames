using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGrid : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showDown(Sprite spt){
		var img = GetComponent<Image> ();
		img.sprite = spt;
		img.color = Color.black;
	}

	public void initShow(){
		var img = GetComponent<Image> ();
		img.sprite = null;
		img.color = new Color (1, 1, 1, 0);
	}
}
