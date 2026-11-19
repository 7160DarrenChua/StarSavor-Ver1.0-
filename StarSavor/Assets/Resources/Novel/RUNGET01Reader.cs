//excel文本抓取器
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using TMPro;

public class RUNGET01Reader : MonoBehaviour
{
    /*   
       string 是字串 list是清單 以excel舉例 {編號1	npc1	「醫生你好。」	2	0	}這樣一串等於一個清單字串 整個excel有好幾個清單字串 所以文法會是 List<List<string>>
       如果excel中想要有換行的句子就直接\n隔中間
       「\」在程式語言中代表 我後面跟的字有一個特殊的功能
       \ + n =>換行
       \ + t =>tab
       \ + c =>半形逗點(英文)
       \ + \ =>「\」
       所以\\n在unity 看來等於\n
   */
    //List<List<string>> NovelLines = new List<List<string>>();//其中NovelLines是自己創，自己去定義的資料夾名稱
    //List<string[]> NovelLines = new List<string[]>();//lines清單是一個string[]
    //但因為在寫excel文本的石對應行數不一定對應到該編號 所以可以用dictionary (字典)的方式去撰寫
    //Dictionary<int, string[]> NovelLines = new Dictionary<int, string[]>();//也可以int,int 整數
    public Dictionary<int, string[]> NovelLines = new Dictionary<int, string[]>();//public全部程式碼都可以抓到，private只能支援寫的程式碼
    void Awake()//Awake比start更早執行
    {
        TextAsset novel = Resources.Load<TextAsset>("Novel/RUNGET01"); //讀取的檔案是 TextAsset (文字的文件)
                                                                      //沒告訴 Resources 要找的東西在哪個資料夾的話，她不會找的到
                                                                      //<TextAsset>已經告知要讀取的檔案類型 所以不用打上副檔名
                                                                      //Sprite 圖片；AudioClip 音檔
        
        string fullConrent = novel.text;   //讓程式抓到那裡有換行的句子

        string[] lines = fullConrent.Split('\n');  //Split 拆解 ，根據我告訴他要切什麼東西
        //截止至目前已經將一段話一段話拆出來了
        //但還需要將一個字一個字也拆出來
        //已知csv utf8的excel檔案會給換格子以,做紀錄，所以...

        for(int index = 0;index < lines.Length;index++)//for迴圈  index = 0代表從第0行開始讀取
        {
            if (lines.Length <= 1) continue;//如果格子長度小於等於一的時候跳過
           
            //string[] line = lines[0].Split(',');//如果這樣寫會只能讀取lines[0]
            string[] line = lines[index].Split(',');//這樣會只認英文逗號

            if (int.TryParse(line[0], out int result))//偵測是不是整數
            {//如果是才收錄 

                //NovelLines.Add( int.Parse (line[0]),line);                                        //讀取每一行的第一格作為編號，int整數.Parse解讀 (line[0]) 括號代表是一個字串，
                NovelLines.Add(result, line);                                                       //所以這句的意思是把字串解讀成整數
                                                                                                    //但第0行第一格是「編號」不能化成整數
            }
        }
        /*
        Debug.Log(novel.text);                  //確保程式有抓到文本
        Debug.Log(lines[0]);                    //卻保有抓到一句一句話
       */
        /*
        Debug.Log(NovelLines[2][1]);//拆分看有沒有每一格讀出來
        Debug.Log(NovelLines[2][2]);
        Debug.Log(NovelLines[2][3]);
        Debug.Log(NovelLines[2][4]);
        Debug.Log(NovelLines[2][5]);
    */
    }
}
