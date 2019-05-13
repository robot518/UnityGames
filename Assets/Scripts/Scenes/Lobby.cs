using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        initEvent();
        initShow();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            Application.Quit();
        }
    }

    void initEvent()
    {
        var btns = transform.Find("btns");
        string[] tMenuItem = { "Minesweeper", "2048", "Tetris", "D3Minesweeper", "Marble"};
        for (int i = 0, iL = btns.childCount; i < iL; i++)
        {
            var item = btns.GetChild(i).GetComponent<Button>();
            var idx = i;
            item.onClick.AddListener(delegate {
                if (idx == 1 || idx == 4) return;
                SceneManager.LoadScene(tMenuItem[idx]);
            });
        }

        var sets = transform.Find("sets");
        for (int i = 0, iL = sets.childCount; i < iL; i++)
        {
            var item = sets.GetChild(i).GetComponent<Button>();
            //var idx = i;
            item.onClick.AddListener(delegate {
                Global.bVoice = !Global.bVoice;
                if (Global.bVoice == false)
                    item.GetComponent<Image>().color = Color.gray;
                else if (Global.bVoice == true)
                    item.GetComponent<Image>().color = Color.white;
            });
        }
    }

    void initShow()
    {
        var sets = transform.Find("sets");
        var voice = sets.GetChild(0).GetComponent<Image>();
        voice.color = Global.bVoice ? Color.white : Color.gray;
    }
}
