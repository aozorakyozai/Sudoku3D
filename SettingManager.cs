using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    GameManager gameManager = new GameManager();

    public void ResetCubeNumber()
    {
        // Clear → 問題のみ表示 // 配列を文字列にする
        string answerArrayToString = string.Join("|", gameManager.questionArray[0]);
        // PlayerPrefsにキューブの角度を0°で保存

        //floatに変更予定

        PlayerPrefs.SetString("savedRotation", "0,0,0");
        // 変更
        //string answerArrayToString = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
        PlayerPrefs.SetString("answerArrayToString", answerArrayToString);
        // 保存
        PlayerPrefs.Save();
    }
}
