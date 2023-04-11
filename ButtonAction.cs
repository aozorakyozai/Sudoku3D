using System;
using System.Collections;
using System.Collections.Generic;
//using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.SocialPlatforms.Impl;

/********************************************************************
 * 最終更新：2023/04/07
 * 機能：ボタンの操作内容を設定
 * 数字の挿入、削除、設定画面
*********************************************************************/

public class ButtonAction : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI textHead;
    //[SerializeField] TextMeshProUGUI textBottom;

    NumberDriver numberDriver = new NumberDriver();
    GameManager gameManager = new GameManager();

    string _cubeAdress = NumberDriver.cubeAdress;

    string _buttonName = NumberDriver.buttonName;

    // 画面上下のテキスト
    ScreenManagaer txtMeshPro;

    // ボタンの画面サイズで位置を均等配置
    　
    private void Start()
    {
        gameManager.SetCubeAdress();

        // テキストを表示
        txtMeshPro = FindObjectOfType<ScreenManagaer>();// 現在未使用
    }
    
    /// <summary>
    /// 選択されたセルに数字を入れる
    /// </summary>
    public void OnClick()
    {
        //Debug.Log("ButtonAction2");
        // 自分のボタン名
        _buttonName = this.gameObject.name;

        //txtMeshPro.textBottom.text = "Cube Adress : " + _cubeAdress + "::" + _buttonName;
        // ボタン名によってbuttonNameで場合分け
        // get or delete
        //_buttonName = (int.Parse(_buttonName) >= 1 && int.Parse(_buttonName) <= 9) ? "number" : _buttonName;

        switch (_buttonName)
        {
            case "1":
                numberDriver.SetNumber(_buttonName);
                numberDriver.GetNumberObjectName();
                break;
            case "2":
                numberDriver.SetNumber(_buttonName);
                numberDriver.GetNumberObjectName();
                break;
            case "3":
                numberDriver.SetNumber(_buttonName);
                numberDriver.GetNumberObjectName();
                break;
            case "4":
                numberDriver.SetNumber(_buttonName);
                numberDriver.GetNumberObjectName();
                break;
            case "5":
                numberDriver.SetNumber(_buttonName);
                numberDriver.GetNumberObjectName();
                break;
            case "6":
                numberDriver.SetNumber(_buttonName);
                numberDriver.GetNumberObjectName();
                break;
            case "7":
                numberDriver.SetNumber(_buttonName);
                numberDriver.GetNumberObjectName();
                break;
            case "8":
                numberDriver.SetNumber(_buttonName);
                numberDriver.GetNumberObjectName();
                break;
            case "9":
                numberDriver.SetNumber(_buttonName);
                numberDriver.GetNumberObjectName();
                break;
            case "C":
                numberDriver.DeleteNumber();
                numberDriver.GetNumberObjectName();
                break;
            case "AC":
                gameManager.ClearCube();
                break;
            case "setting":
                break;
            default:
                break;
        }
    }
}
