using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MTouch : MonoBehaviour, IPointerClickHandler
{
    Minesweeper _delt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(eventData);
        //世界坐标转局部坐标
        var pos = transform.parent.InverseTransformPoint(eventData.position);
        //Debug.Log(pos);
        var iLen = 600/9;
        int xmin = -300, xmax = 300;
        int ymin = -300, ymax = 300;
        if (pos.x >= xmin && pos.x < xmax && pos.y >= ymin && pos.y < ymax)
        {
            var col = Mathf.Floor((pos.x - xmin) / iLen);
            var row = 8 - Mathf.Floor((pos.y - ymin) / iLen);
            var idx = (int)(9 * row + col);
            _delt.onClick(idx);
            //Debug.Log(pos + " " + idx+" "+col+" "+row+" "+iLen);
        }
    }

    public void init(Minesweeper delt)
    {
        _delt = delt;
    }
}
