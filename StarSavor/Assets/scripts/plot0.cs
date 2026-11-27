//excel文本抓取器讀資料，然後依照讀到的資料去替換
//文本劇情
//立會頭像顯示替換

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class plot0 : MonoBehaviour
{
    public Text SpeakerName, SpeakeringContent;
    public plot0Reader novelReader;//使程式碼可以引用excel抓取器程式碼的參數
    private int currentId;//紀錄現在使用到字典中的 整數 編號

    private string[] CharacterNameList = { "PlayerCharacter", "npcCharacter" };
    private string[] AvatarNameList = { "PlayerAvatar", "npcAvatar" };
    private string[] ALLBackgroundList = { "場景1", "黑幕", "駕駛艙" };
    private string[] ALLDialogueBoxList = { "DialogueBox", "對話框隱藏控制器" };

    void Start()
    {
        currentId = 1;
        ShowAllContent();
    }

    void Update()
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

        // 按下 Enter 鍵直接跳轉到下一個場景
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("劇情結束，跳轉場景");
            SceneManager.LoadScene("plot1");
            return;
        }
    }

    void TriggerInfoA()
    {
        Debug.LogError("資訊A被呼叫，目前ID=" + currentId);
    }

    void ShowAllContent()
    {
        if (novelReader?.NovelLines?.ContainsKey(currentId) != true) return;

        string[] data = novelReader.NovelLines[currentId];

        //data[0]   編號
        //data[1]   名字
        //data[2]   對話內容
        //data[3]   接續編號
        //data[4]   立繪編號
        //data[5]   頭像編號
        //data[6]   背景
        //data[7]   對話框

        //名字
        GameObject.Find("Canvas/ALLDialogueBox/DialogueBox/SpeakerName").GetComponent<Text>().text = data[1];
        //對話內容
        GameObject.Find("Canvas/ALLDialogueBox/DialogueBox/SpeakeringContent").GetComponent<Text>().text = data[2];

        for (int i = 0; i < CharacterNameList.Length; i++)
        {
            string name = CharacterNameList[i];
            if (data[4].Trim() == name)
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
                GameObject.Find("Canvas/ALLDialogueBox/DialogueBox/ALL avatar/" + name).SetActive(true);
            }
            else
            {
                GameObject.Find("Canvas/ALLDialogueBox/DialogueBox/ALL avatar/" + name).SetActive(false);
            }
        }

        for (int i = 0; i < ALLBackgroundList.Length; i++)
        {
            string name = ALLBackgroundList[i];
            if (data[6].Trim() == name)
            {
                GameObject.Find("Canvas/ALLBackground/" + name).SetActive(true);
            }
            else
            {
                GameObject.Find("Canvas/ALLBackground/" + name).SetActive(false);
            }
        }

        for (int i = 0; i < ALLDialogueBoxList.Length; i++)
        {
            string name = ALLDialogueBoxList[i];
            if (data[7].Trim() == name)
            {
                GameObject.Find("Canvas/ALLDialogueBox/" + name).SetActive(true);
            }
            else
            {
                GameObject.Find("Canvas/ALLDialogueBox/" + name).SetActive(false);
            }
        }
    }
}
