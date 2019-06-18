using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hoodle : MonoBehaviour
{
    bool _bGameOver = false;
    Transform goTips;
    AudioMgr adMgr;
    // Start is called before the first frame update
    void Start()
    {
        initParas();
        initEvent();
        initShow();
        Invoke("onStart", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initParas()
    {
        adMgr = AudioMgr.getInstance();
    }
    void initEvent()
    {
        transform.Find("top/back").GetComponent<Button>().onClick.AddListener(delegate
        {
            SceneManager.LoadScene("Lobby");
        });
        var btnStart = transform.Find("down/start").gameObject.GetComponent<Button>();
        btnStart.onClick.AddListener(onStart);
    }
    void initShow()
    {
        
    }
    bool checkWin()
    {
        return true;
    }
    void onStart()
    {
        _bGameOver = false;
        goTips.gameObject.SetActive(false);
    }

    void showWin()
    {
        goTips.gameObject.SetActive(true);
    }
}
