using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MTouch : MonoBehaviour, IPointerClickHandler
{
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
        var pos = eventData.position;
        var iLen = 60;
        if (pos.x >= 90 && pos.x < 630 && pos.y >= 380 && pos.y < 920)
        {
            var col = Mathf.Floor((pos.x - 90) / iLen);
            var row = 8 - Mathf.Floor((pos.y - 380) / iLen);
            Debug.Log(col + "," + row);
        }
    }
}
