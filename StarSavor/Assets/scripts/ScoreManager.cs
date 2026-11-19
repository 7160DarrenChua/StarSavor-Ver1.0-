//  成就系統
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    //四個場景獨立加分變數
    public int addScore1 = 0;
    public int addScore2 = 0;
    public int addScore3 = 0;
    public int addScore4 = 0;

    public int nowPoint = 0;//現在的分數
    public int totalScore = 0;//總分


    //宣告玩家碰病人成就標記，（對應三個病人場景）
    public bool achieve_FirstScare = false;     //scene01病人01 → 初次驚慌
    public bool achieve_NoIdea = false;         //scene02病人02 → 我還沒想到
    public bool achieve_UsedToIt = false;       //scene02pro病人02 → 習以為常

    void Awake()//優先
    {
        if (instance == null)
        {
            instance = this;//只怪這隻程式碼的物件
            DontDestroyOnLoad(gameObject);//不會因為場景切換被銷毀
          
            SceneManager.sceneLoaded += OnSceneLoaded; //綁定場景載入事件，切場景自動執行判斷
        }
        else
        {
            Destroy(gameObject);//刪除重複的
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentScene = scene.name;

        //進主選單(cover)歸零所有分數
        if (currentScene == "cover")//如果進到cover場景
        {
            ResetScore();
            Debug.Log("刪除前面遊玩分數");
        }

        //進勝利or失敗場景，四個分數相加給totalScore
        if (currentScene == "Win" || currentScene == "Lose")
        {
            totalScore = addScore1 + addScore2 + addScore3 + addScore4;//總分等於四個變數相加

            RefreshScoreUI();
            Debug.Log("已進入結局場景，四場分數相加：" + addScore1 + "+" + addScore2 + "+" + addScore3 + "+" + addScore4 + " = " + totalScore);
        }
    }


    //各場景直接呼叫
    public void AddScore1()
    {
        addScore1 = 25;
        Debug.Log("場景1分數設定25");
        TextMeshProUGUI addText = GameObject.Find("Canvas/加分")?.GetComponent<TextMeshProUGUI>();
        if (addText != null) addText.text = "加25分";
    }
    public void AddScore2()
    {
        addScore2 = 25;
        Debug.Log("場景2分數設定25");
        TextMeshProUGUI addText = GameObject.Find("Canvas/加分")?.GetComponent<TextMeshProUGUI>();
        if (addText != null) addText.text = "加25分";
    }
    public void AddScore3()
    {
        addScore3 = 25;
        Debug.Log("場景3分數設定25");
        TextMeshProUGUI addText = GameObject.Find("Canvas/加分")?.GetComponent<TextMeshProUGUI>();
        if (addText != null) addText.text = "加25分";
    }
    public void AddScore4()
    {
        addScore4 = 25;
        Debug.Log("場景4分數設定25");
        TextMeshProUGUI addText = GameObject.Find("Canvas/加分")?.GetComponent<TextMeshProUGUI>();
        if (addText != null) addText.text = "加25分";
    }


    public void ResetScore()
    {
        addScore1 = 0;
        addScore2 = 0;
        addScore3 = 0;
        addScore4 = 0;
        nowPoint = 0;
        totalScore = 0;
        achieve_FirstScare = false;
        achieve_NoIdea = false;
        achieve_UsedToIt = false;

        RefreshScoreUI();
    }

   
    void RefreshScoreUI()
    {
        TextMeshProUGUI scoreText = GameObject.Find("Canvas/劇情探索進度")?.GetComponent<TextMeshProUGUI>();
        if (scoreText != null)
        {
            if (totalScore == 0)
            {
                scoreText.text = $"劇情探索進度:0%";
            }
            else
            {
                scoreText.text = $"劇情探索進度:{totalScore}%";
            }
            Debug.Log("成功更新分數UI：" + scoreText.text);
        }
        else
        {
            Debug.Log("找不到路徑 Canvas/劇情探索進度 的TMP文字！");
        }

        //成就
        TextMeshProUGUI achieveText = GameObject.Find("Canvas/成就")?.GetComponent<TextMeshProUGUI>();
        if (achieveText != null)
        {
            string allAchieveText = "成就:\n";
            
            if (achieve_FirstScare)
                allAchieveText += "【初次驚慌】\n";

            if (achieve_NoIdea)
                allAchieveText += "【我還沒想到】\n";

            if (achieve_UsedToIt)
                allAchieveText += "【習以為常】";

            achieveText.text = allAchieveText;
            Debug.Log("更新成就列表：" + allAchieveText);
        }
        else
        {
            Debug.Log("找不到 Canvas/成就 TMP文字物件");
        }
    }
    //成就呼叫函式
    public void CallAchieve_FirstScare()//呼叫成就FirstScare
    {
        if (achieve_FirstScare == false)
        {
            achieve_FirstScare = true;//布林變數（bool），記錄該成就是否已經解鎖
            Debug.Log("解鎖成就：初次驚慌");
            RefreshScoreUI();//呼叫更新介面的函式
        }
    }

    public void CallAchieve_NoIdea()
    {
        if (achieve_NoIdea == false)
        {
            achieve_NoIdea = true;
            Debug.Log("解鎖成就：我還沒想到");
            RefreshScoreUI();
        }
    }

    public void CallAchieve_UsedToIt()
    {
        if (achieve_UsedToIt == false)
        {
            achieve_UsedToIt = true;
            Debug.Log("解鎖成就：習以為常");
            RefreshScoreUI();
        }
    }
    // 銷毀時解除事件綁定，避免記憶體洩漏
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
/*參考 原本怪物呼叫計分
 private void OnCollisionEnter(Collision collision)//如果碰撞器跟碰撞器碰到 怪物計分
    {
        if (collision.gameObject.name == "Spherebb(Clone)")//BB子彈
        {
            if (gameObject.name == "enemyA(Clone)")//碰到怪物1
            {
                Debug.Log("分數加25");//通知加25
                addScore = 25;//真的加25
            }
            ScoreManager.instance.AddScore(addScore);
        }
    }
*/

/*參考 原本怪物接收計分
public void AddScore(int score)
{
    nowPoint += score;
    totalScore = nowPoint;

    //更新結局畫面總分文字
    RefreshScoreUI();

    //加分彈窗修正寫法
    TextMeshProUGUI addText = GameObject.Find("Canvas/加分")?.GetComponent<TextMeshProUGUI>();
    if (addText != null)
    {
        addText.text = $"加{score}分";
    }
}*/