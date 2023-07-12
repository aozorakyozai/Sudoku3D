/**********************************************************************
 * 最終更新：2023/04/24
 * 環境：Unity 2023.3.13f1 LTS
 * 環境：Unity C# 
 * 機能：ボタンの登録
**********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSettingScreen : MonoBehaviour
{
    SettingManager SettingManager = new SettingManager();

    public void OnClick()
    {
        string _buttonName = this.gameObject.name;

        switch (_buttonName)
        {
            case "Level":
                break;
            case "Next":
                break;
            case "Previous":
                break;
            case "Score":
                break;
            case "Reset":
                SettingManager.ResetCubeNumber();
                break;
            case "setting":
                // LoadSceneで実行
                break;
            default:
                break;
        }
    }
}
