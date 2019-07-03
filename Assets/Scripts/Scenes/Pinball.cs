using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pinball : MonoBehaviour
{
    bool _bGameOver = false;
    public GameObject goTips;
    AudioMgr adMgr;
    // Start is called before the first frame update
    void Start()
    {
        initParas();
        initEvent();
        initShow();
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
    }
    void initShow()
    {
        
    }
    bool checkWin()
    {
        return true;
    }

    void showWin()
    {
        goTips.SetActive(true);
    }
}
