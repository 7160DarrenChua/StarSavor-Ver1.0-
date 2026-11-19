//主畫面選單
//大聲公
//場景切換
//呼叫加分
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class cover : MonoBehaviour
{
    void Awake()//優先
    {
        PlayerPrefs.DeleteKey("totalScore"); // 當跳轉到這個場景時，會先將舊的分數紀錄清除
        //PlayerPrefs 是 Unity 內建的輕量級本地資料存儲類別，專門用來在玩家設備上儲存與讀取少量的偏好設定或遊戲存檔（例如：音量大小、最高得分、關卡進度）。
    }

    public int addScore;//加多少分
   
    public void Play_run()//按鈕按下去後跳轉場景的程式碼
    {
        SceneManager.LoadScene("scene01");
    }

    public void Quit_run()
    {
        Application.Quit();
    }
    public void GoBackTo_cover()
    {
        SceneManager.LoadScene("cover");
    }
    
    
    public void GoBackTo_scene01_2()
    {
        SceneManager.LoadScene("scene01_2");
    }
    public void GoBackTo_scene01_1()
    {
        SceneManager.LoadScene("scene01_1");
    }
    public void GoBackTo_scene02_2()
    {
        SceneManager.LoadScene("scene02_2");
    }
    public void GoBackTo_scene02_1()
    {
        SceneManager.LoadScene("scene02_1");
    }
    public void GoBackTo_scene02_1PRO()
    {
        SceneManager.LoadScene("scene02_1PRO");
    }
    public void GoBackTo_scene03_3()
    {
        SceneManager.LoadScene("scene03_3");
    }
    public void GoBackTo_scene03_2()
    {
        SceneManager.LoadScene("scene03_2");
    }
    public void GoBackTo_scene03_1()
    {
        SceneManager.LoadScene("scene03_1");
    }
    public void GoBackTo_scene02RUNPRO()
    {
        SceneManager.LoadScene("scene02RUNPRO");
    }
}