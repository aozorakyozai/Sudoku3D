/**********************************************************************
 * 最終更新：2023/04/11
 * 環境：Unity 2023.3.13f1 LTS
 * 環境：Unity C# 
 * 
**********************************************************************/

/**************************** シングルトン *****************************
 * シングルトンの実装
 * 機能：クラスのインスタンスが一つしか存在しないことを保証するデザインパターン
 * 実装：staticで実装、
 * オブジェクトの場合：破壊されないようにする → DontDestroyOnLoad
 * DATAだけの場合は空のオブジェクトにアタッチすれば保持できる
 * MusicManager(音の管理)など
 * データ取得が先になるように設定する場合：プロジェクト設定 > スクリプト実行順
**********************************************************************/

/************************* リファクタリング ****************************
 * static変数をここに集める予定
 * 問題
 * 回答
 * 
 * 2023/04/12
 * 
 * クリアが一つおき
 * 
**********************************************************************/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DataSingleton : MonoBehaviour
{
    // シングルトンの実装 1
    public static DataSingleton instance;

    private void Awake()
    {
        // シングルトンの実装 2
        if (instance == null)
        {
            instance = this;
            // 破壊されないオブジェクト
            DontDestroyOnLoad(this.gameObject);

            
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private static int _gameData;
    public static int gameData { get { return _gameData; } set { _gameData = (value < 0) ? 0 : value; } }

    // シングルトンクラスのmethodを記述

    public void TestMethod()
    {
        
    }

}
