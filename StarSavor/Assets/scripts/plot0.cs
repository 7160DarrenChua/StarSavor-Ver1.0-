//excel文本抓取器讀資料，然後依照讀到的資料去替換
//文本劇情
//立會頭像顯示替換

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class plot0Reader : MonoBehaviour
{
    public TextMeshProUGUI SpeakerName, SpeakeringContent;
    public plot0 novelReader;//使程式碼可以引用excel抓取器程式碼的參數                                                  //這一行的第二個要改成程式的名稱
    private int currentId;//紀錄現在使用到字典中的 整數 編號

    private string[] CharacterNameList = { "PlayerCharacter", "護士" };
    private string[] AvatarNameList = { "PlayerAvatar","護士" };
    private string[] QQpeopleList = { "Q人npc1", "Q人npc2" };



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentId = 1;
        ShowAllContent();
        GameObject.Find("Canvas/Q人/Q人npc1" ).SetActive(true);
        GameObject.Find("Canvas/Q人/Q人npc2").SetActive(false);
    }
    void Update()//放這邊會重複執行
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {


            if (novelReader?.NovelLines?.ContainsKey(currentId) != true)
            {
                return;
            }

            string[] data = novelReader.NovelLines[currentId];

            // 檢查接續編號是否為空
            if (string.IsNullOrWhiteSpace(data[3]))
            {
                Debug.Log("劇情結束，跳轉場景");
                SceneManager.LoadScene("plot1");
                return;
            }

            if (int.TryParse(data[3].Trim(), out int nextId))
            {
                currentId = nextId;

                if (novelReader.NovelLines.ContainsKey(currentId))
                {
                    ShowAllContent();
                }
            }

        }
        if (Input.GetKey(KeyCode.Return))
        {


            if (novelReader?.NovelLines?.ContainsKey(currentId) != true)
            {
                return;
            }

            string[] data = novelReader.NovelLines[currentId];

            // 檢查接續編號是否為空
            if (string.IsNullOrWhiteSpace(data[3]))
            {
                Debug.Log("劇情結束，跳轉場景");
                SceneManager.LoadScene("plot1");
                return;
            }

            if (int.TryParse(data[3].Trim(), out int nextId))
            {
                currentId = nextId;

                if (novelReader.NovelLines.ContainsKey(currentId))
                {
                    ShowAllContent();
                }
            }

        }
        /*
         if (Input.GetKeyDown(KeyCode.Space)) //GetKeyDown跟GetKey差別在「一個是按一次偵測一次」，「一個是一直按著可以一直偵測」
         {
             //Debug.Log("VNgametextscripts 啟動：" + gameObject.name);
             //Debug.Log("按下空白鍵");


             // 防錯：如果找不到當前編號代表該場景的劇本結束
             //重點拆解：為什麼要用問號？問號代表什麼意思？這個語法在 C# 叫作 「空值條件運算子 (Null-conditional Operator)」。它的用意就是「如果我是空的，請到此為止，不要再往後讀取，
             //免得當機！」我們把這一長行拆成三個部分來看：if ( novelReader? .NovelLines? .ContainsKey(currentId) != true )1. novelReader?（第一個問號）代表的意思：程式會先去問：
             //「畫面上到底有沒有掛載 novelReader 這個物件？」如果沒有拉物件（Null）：一般的程式碼寫 novelReader.NovelLines 會立刻紅色報錯當機。但加了問號，Unity 只要發現它是空的，
             //就會貼心地直接停住，回傳一個空值，不再往後讀取。2. .NovelLines?（第二個問號）代表的意思：通過第一關後，程式接著問：「novelReader 雖然在，但裡面的 Excel 字典資料夾 NovelLines
             //到底有沒有初始化、讀好檔案？」如果 Excel 還沒讀完（Null）：加了問號，它就會再次停住、回傳空值，絕對不會強行讀取而導致當機。3. != true代表的意思：「不等於 True」
             //（也就是 False 或者是空值）。整行合起來的邏輯：如果 novelReader 遺失了、或者 Excel 資料沒讀到、或者是字典裡「找不到（False）」這個 currentId。只要這三種狀況符合任何一種
             //（都不是標準的 True），就代表出事了或劇情結束了。

             //如果劇情結束
             if (isStoryEnd)
             {
                 SceneManager.LoadScene("scene01RUN");
                 return;
             }

             if (novelReader?.NovelLines?.ContainsKey(currentId) != true)
             {
                 //TriggerInfoA(); // 向上帝視角傳送資訊a(開啟 True/False 按鈕)
                 return;         // 提早結束，絕對不讓程式往下走到 data[3] 導致當機！
             }
             string[] data = novelReader.NovelLines[currentId];

             // 【防呆】：利用 int.TryParse 防止企劃在接續編號欄位不小心填寫了非數字或留白
             if (int.TryParse(data[3].Trim(), out int nextId))
             {
                 currentId = nextId; // 成功換成下一個編號

                 // 檢查即將要顯示的下一個編號是否存在
                 if (novelReader.NovelLines.ContainsKey(currentId))
                 {
                     ShowAllContent();
                 }

                 else
                 {

                 }
             }

         }
         else
         {
             TriggerInfoA(); S
         }
         }*/
    }



    // 【全新加入：當文本編號停止後，傳輸資訊 a 的上帝視角接口】
    void TriggerInfoA()
    {

        Debug.LogError(
        "資訊A被呼叫，目前ID=" + currentId
                        );
        //Debug.Log("【資訊 a】Excel 文本編號已停止或結束。");
        // 這裡之後用來放：顯示 True / False 兩個 Button 按鈕的程式碼
    }
    void ShowAllContent()
    {

        // 【雙重保險】：進入此功能前再次確保防錯，如果沒有這個編號就直接跳出，防止下面 data[1]~data[5] 噴出 4 個紅色錯誤
        if (novelReader?.NovelLines?.ContainsKey(currentId) != true) return;

        string[] data = novelReader.NovelLines[currentId];//取出excel中需要的編號所對應的一整行欄位

        //data[0]   編號
        //data[1]   名字
        //data[2]   對話內容
        //data[3]   接續編號
        //data[4]   立繪編號
        //data[5]   頭像編號
        //data[6]   正確按鈕
        //data[7]   錯誤按鈕

        //名字
        GameObject.Find("Canvas/DialogueBox/SpeakerName").GetComponent<TextMeshProUGUI>().text = data[1];
        //對話內容
        GameObject.Find("Canvas/DialogueBox/SpeakeringContent").GetComponent<TextMeshProUGUI>().text = data[2];





        for (int i = 0; i < CharacterNameList.Length; i++)
        {
            string name = CharacterNameList[i];
            //if (data[4] == name)
            if (data[4].Trim() == name)//加入.Trim()，可以去除掉excel中的空格
            {
                GameObject.Find("Canvas/ALL character/" + name).SetActive(true);
            }
            else
            {
                GameObject.Find("Canvas/ALL character/" + name).SetActive(false);
            }
        }

        for (int i = 0; i < AvatarNameList.Length; i++)
        {
            string name = AvatarNameList[i];
            if (data[5].Trim() == name)
            {
                GameObject.Find("Canvas/DialogueBox/ALL avatar/" + name).SetActive(true);
            }
            else
            {
                GameObject.Find("Canvas/DialogueBox/ALL avatar/" + name).SetActive(false);
            }
            
        }
            for (int i = 0; i < QQpeopleList.Length; i++)
            {
                string name = QQpeopleList[i];
                if (data[6].Trim() == name)
                {
                    GameObject.Find("Canvas/Q人/" + name).SetActive(true);
                }
                else
                {
                    GameObject.Find("Canvas/Q人/" + name).SetActive(false);
                }
            }
    }
}
