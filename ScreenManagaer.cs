using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/********************************************************************
 * 最終更新：2023/04/07
 * 機能：画面設定：いろいろなデバイスに対応させる
 * 画面サイズを取得
 * デバイスの向きを取得
 * テキストの大きさ
 * ボタン位置を相対的にする予定
 * 縦横変更 レイアウトも変更　 ← 画面は回転させない予定
*********************************************************************/

public class ScreenManagaer : MonoBehaviour
{
    /**** 左下テキスト ****/
    [SerializeField] public TextMeshProUGUI textHead;
    [SerializeField] public TextMeshProUGUI textBottom;

    private Camera mainCamera;
    private float StartScreenSize;
    private Rect viewportRect;
    // 画面サイズ
    public float screenHeight;
    public float screenWidth;

    public float textWidth;

    // 直前のディスプレイ向き
    public DeviceOrientation PrevOrientation;
    // 回転検知用
    public DeviceOrientation currentOrientation;
    // cubeのAjust機能に渡す
    float deviceRotate;

    private void Awake()
    {
        // スマホの画面サイズの取得
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        // スマホ画面の幅を取得する(小さい方をテキストの幅にする)
        textWidth = screenHeight > screenWidth ? screenWidth : screenHeight;

        /**** textHead ****/
        // テキストの幅をスマホ画面に合わせて変更する
        textHead.rectTransform.sizeDelta = new Vector2(textWidth, textHead.rectTransform.sizeDelta.y);
        textHead.alignment = TextAlignmentOptions.Left;
        // テキストを左詰め、上詰めにする
        textHead.alignment = TextAlignmentOptions.TopLeft;
        // フォントサイズを60に設定する
        textHead.fontSize = 60;
        // フォント色を白に設定する
        textHead.color = Color.white;

        //textHead.text = "test Head";

        /**** textBotton ****/
        // テキストの幅をスマホ画面に合わせて変更する
        textBottom.rectTransform.sizeDelta = new Vector2(textWidth, textBottom.rectTransform.sizeDelta.y);
        textBottom.alignment = TextAlignmentOptions.Left;
        // テキストを左詰め、上詰めにする
        textBottom.alignment = TextAlignmentOptions.TopLeft;
        // フォントサイズを60に設定する
        textBottom.fontSize = 60;
        // フォント色を白に設定する
        textBottom.color = Color.white;

        //textBottom.text = "test Bottom";
    }
    void Start()
    {
        textHead.text = viewportRect.ToString();
        PrevOrientation = GetOrientation(0);
    }

    void Update()
    {
        currentOrientation = GetOrientation(0);

        if (PrevOrientation != currentOrientation)
        {
            // 画面の向きが変わった場合の処理
            textHead.text = currentOrientation + " : " + " 向き：　" + GetOrientation(0);
            PrevOrientation = currentOrientation;

            // 画面の向きを取得　横or縦
        }
    }
    
    /// <summary>
    /// 端末の向きを取得するメソッド <br/>
    /// キャストして数値変換できる
    /// </summary>
    /// <param name="引い数はオーバーロードのため"0"を代入></param>
    /// <returns>string</returns>
    public DeviceOrientation GetOrientation(int rotateValue)
    {
        DeviceOrientation result = Input.deviceOrientation;

        // Unkownならピクセル数から判断
        if (result == DeviceOrientation.Unknown)
        {
            if (Screen.width < Screen.height)
            {
                result = DeviceOrientation.Portrait;
            }
            else
            {
                result = DeviceOrientation.LandscapeLeft;
            }
        }

        return result;
    }

    /// <summary>
    /// Cubeの回転を初期値に戻す <br/>
    /// デバイスの向きによるZ軸の調整値を渡す
    /// </summary>
    public int GetOrientation()
    {
        DeviceOrientation deviceRotate = Input.deviceOrientation;
        
        switch ((int)deviceRotate)
        {
            case 1:
                return 0;
            case 2:
                return 180;
            case 3:
                return -90;
            case 4:
                return 90;
            default:
                return 0;
        }
    }
}
