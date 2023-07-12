/********************************************************************
 * 最終更新：2023/04/15
 * 正解判定
 * falseの場合に素早く抜けるか検討中
*********************************************************************/

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class ScoreManager : MonoBehaviour
{
    /// <summary>答え合わせの配列 27</summary>
    public int[] ans = new int[GameManager.cubeCount];
    public int[] checkAnswerArray = new int[9];
    public int[] correctAnswerArray = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    private void Start()
    {

    }

    public bool IsAnserArray()
    {
        ScreenManagaer screenManagaer = new ScreenManagaer();
        // テキストオブジェクトを探す
        screenManagaer = FindObjectOfType<ScreenManagaer>();

        int correctCount = 0;

        // すべて埋まり、子オブジェクトが２つ以上ないことを確認する
        for (int i = 0; i < GameManager.cubeGameObjects.Length; i++)
        {
            if (GameManager.cubeGameObjects[i].transform.childCount == 1)
            {
                char num = GameManager.cubeGameObjects[i].transform.GetChild(0).name[GameManager.numberPrefabToNumber];
                //screenManagaer.textHead.text = i +  " Anser : " + num.ToString();
                if (int.TryParse(num.ToString(), out int ansNum))
                {
                    ans[i] = ansNum;
                }
                else
                {
                    screenManagaer.textBottom.text = "Err Adress:" + GameManager.cubeGameObjects[i].name;
                    return false;
                }
            }
            else
            {
                screenManagaer.textBottom.text = "Err Adress:" + GameManager.cubeGameObjects[i].name;
                return false;
            }
        }
        // { 1, 2, 3, 4, 5, 6, 7, 8, 9 }と同じか判定
        checkAnswerArray = new int[] { ans[0], ans[1], ans[2], ans[3], ans[4], ans[5], ans[6], ans[7], ans[8] };
        if (checkAnswerArray.All(x => correctAnswerArray.Contains(x))) { correctCount++; } else { return false; }
        checkAnswerArray = new int[] { ans[9], ans[10], ans[11], ans[12], ans[13], ans[14], ans[15], ans[16], ans[17] };
        if (checkAnswerArray.All(x => correctAnswerArray.Contains(x))) { correctCount++; } else { return false; }
        checkAnswerArray = new int[] { ans[18], ans[19], ans[20], ans[21], ans[22], ans[23], ans[24], ans[25], ans[26] };
        if (checkAnswerArray.All(x => correctAnswerArray.Contains(x))) { correctCount++; } else { return false; }
        checkAnswerArray = new int[] { ans[0], ans[3], ans[6], ans[9], ans[12], ans[15], ans[18], ans[21], ans[24] };
        if (checkAnswerArray.All(x => correctAnswerArray.Contains(x))) { correctCount++; } else { return false; }
        checkAnswerArray = new int[] { ans[1], ans[4], ans[7], ans[10], ans[13], ans[16], ans[19], ans[22], ans[25] };
        if (checkAnswerArray.All(x => correctAnswerArray.Contains(x))) { correctCount++; } else { return false; }
        checkAnswerArray = new int[] { ans[2], ans[5], ans[8], ans[11], ans[14], ans[17], ans[20], ans[23], ans[26] };
        if (checkAnswerArray.All(x => correctAnswerArray.Contains(x))) { correctCount++; } else { return false; }
        // どっちかの判定にする
        checkAnswerArray = new int[] { ans[0], ans[1], ans[2], ans[9], ans[10], ans[11], ans[18], ans[19], ans[20] };
        Array.Sort(checkAnswerArray);
        if (checkAnswerArray.SequenceEqual(correctAnswerArray)) { correctCount++; } else { return false; }
        checkAnswerArray = new int[] { ans[3], ans[4], ans[5], ans[12], ans[13], ans[14], ans[21], ans[22], ans[23] };
        Array.Sort(checkAnswerArray);
        if (checkAnswerArray.SequenceEqual(correctAnswerArray)) { correctCount++; } else { return false; }
        checkAnswerArray = new int[] { ans[6], ans[7], ans[8], ans[15], ans[16], ans[17], ans[24], ans[25], ans[26] };
        Array.Sort(checkAnswerArray);
        if (checkAnswerArray.SequenceEqual(correctAnswerArray)) { correctCount++; } else { return false; }

        if (correctCount == 9)
        {
            screenManagaer.textHead.text = "SUCCESS!";
            return true;
        }
        else
        {
            screenManagaer.textBottom.text = "NO SUCCESSFULL";
            return false;
        }
    }
}
