using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;  // 追加しましょう
using TMPro;

/********************************************************************
 * 最終更新：2023/04/08
 * 正解判定
 * 
*********************************************************************/

public class ScoreManager : MonoBehaviour
{
    //public TextMeshProUGUI score_text;
    ////public GameObject score_object = null; // Textオブジェクト
    //public int score_num = 0; // スコア変数

    //NumberDriver numberDriver = new NumberDriver();
    //GameManager gameManager = new GameManager();

    //string[] _answerArray = new GameManager().answerArray;
    
    ///// <summary>Cubeのアドレス(Cube123)</summary>
    //string cubeNumber;
    ///// <summary>キューブに入れる出題用の数字</summary>
    //int questionNumberArray;
    ///// <summary>キューブに入っている回答の数字</summary>
    //int answerNumberArray;
    
    //// カラーマテリアルを指定するための変数
    //private Material myMaterial;
    /// <summary>出題キューブのフレームの色</summary>
    //public static Color QuestionColor = Color.blue;
    

    private void Awake()
    {
        // 画面の向きを縦に固定する
        //Screen.orientation = ScreenOrientation.Portrait;
        // プロジェクト設定で画面回転を固定している
    }
    // 初期化時の処理
    void Start()
    {
        
    }

    // 更新
    void Update()
    {
        
    }
    // どこに設置するかあとから検討予定
    private void OnApplicationQuit()//OnDestroy
    {
        
    }
    // 終了時の処理
    void OnDestroy()
    {
        
    }
}
