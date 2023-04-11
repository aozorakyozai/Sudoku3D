using System;
using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Pool;
//using UnityEngine.InputSystem.Controls;
//using Unity.VisualScripting;

//using UnityEditor.EditorTools; ここが「ファイルが見つかりません」の原因では

/********************************************************************
 * 最終更新：2023/04/07
 * メソッド：数字を生成、挿入、削除、表示
 * 数字プレファブを生成
 * キューブに数字を入れる
 * キューブ内の数字を削除
 * 数字プレファブの使いまわし
 * textHeadにCube内の子オブジェクト(Number)を表示
*********************************************************************/

// スクリプトの実行順序を設定した　2023/03/14　検証未済

// 数字が反対向き　2023/03/15
// 回転させる
// 問題の数字は固定　配列から分岐
// バイブを外す

public class NumberDriver : MonoBehaviour
{
    /// <summary>Cube + xyz</summary>
    public static string cubeAdress;
    /// <summary>押されたボタン番号</summary>
    public static string buttonName;
    

    /// <summary>1~9の定数</summary>
    private const int numberCount = 9;
    /// <summary>Add 9コ以上入らない定数</summary>
    private const int makeNumberCount = 9;
    //
    private const int maxCubeNumber = 8;
    GameObject _tradeCubeAdress = GameManager.tradeCubeAdress;
    // numberPrefabの親
    [SerializeField] public GameObject PooledGameObject;
    // Number(1~9)のPrefabのリスト
    [SerializeField] public List<GameObject> numberPrefab;
    /// <summary>numberObjectを格納するList</summary>
    public static List<GameObject>[] listOfPooledObjects = new List<GameObject>[9];
    
    //List<GameObject> listOfPooledObject; //　コメントアウトしてみた → 不要かも　2023/03/18

    // 画面上下のテキスト
    ScreenManagaer txtMeshPro;

    private void Awake()
    {
        //Debug.Log("numberDriver");
        // テキストを表示
        txtMeshPro = FindObjectOfType<ScreenManagaer>();

        txtMeshPro.textBottom.text = "NumberDriver test ";

        // (1~9)x9コを作成
        for (int i = 0; i < numberCount; i++)
        {
            listOfPooledObjects[i] = new List<GameObject>();

            for (int j = 0; j < makeNumberCount; j++)
            {
                // Prefab配列から生成
                GameObject numberObject = Instantiate(numberPrefab[i]);
                // 非Active
                numberObject.SetActive(false);
                // PooledGameObjectに格納
                numberObject.transform.SetParent(PooledGameObject.transform);
                // [3][3]配列に追加
                listOfPooledObjects[i].Add(numberObject);
            }
        }
    }
    
    /// <summary>
    /// 選択された数字をキューブに入れる
    /// </summary>
    /// <param name="押されたボタン番号"></param>
    public void SetNumber(string buttonName)
    {
        //Debug.Log("numberのテスト : " + tradeCubeAdress + ":");
        if (_tradeCubeAdress != null)
        {
            // セルが選択されている場所を親オブジェクトにする
            GameObject parentGameObject = _tradeCubeAdress;
            // ParentObjectのTransformを取得する
            Transform parentTransform = parentGameObject.transform;
            // 数字変換できるかを判定する
            if (int.TryParse(buttonName, out int num))
            {
                num = num - 1;

                try
                {
                    // 同じ数字を弾く予定

                    // 出題キューブ以外 && 数字は8コまで入る
                    if (_tradeCubeAdress.GetComponent<Renderer>().material.color != GameManager.QuestionColor && _tradeCubeAdress.transform.childCount <= maxCubeNumber)
                    {
                        // 非表示NumberObjectを探す
                        GameObject activeNumberObject = this.GetPooledObject(num);

                        Transform NumberObjectObjTransform = activeNumberObject.transform;

                        // objTransformをParentObjectの子オブジェクトにする
                        
                        NumberObjectObjTransform.SetParent(parentTransform);

                        // キューブの中央に表示
                        NumberObjectObjTransform.localPosition = Vector3.zero;
                        // 子オブジェクトの回転を親オブジェクトと同じにする
                        NumberObjectObjTransform.localRotation = Quaternion.Euler(0, 180, 0);

                        NumberObjectObjTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        // 非表示を表示させる
                        activeNumberObject.SetActive(true);
                    }
                    else
                    {
                        Handheld.Vibrate();
                        throw new Exception("Err");

                        return;
                    }
                }
                catch
                {
                    // 振動
                    Handheld.Vibrate();
                }
            }
            else
            {
                // 数字以外のボタンの場合(ボタン側で制御しているからこれはないはず)
                return;
            }
        }
        else
        {
            // キューブ選択がない状態 → あとからでも選択できるようにする
            Handheld.Vibrate();
        }
    }

    /// <summary>
    /// キューブに数字がある場合に削除する
    /// </summary>
    public void DeleteNumber()
    {
        // 出題キューブを除外する
        if (_tradeCubeAdress.GetComponent<Renderer>().material.color != GameManager.QuestionColor)
        {
            int childCount = _tradeCubeAdress.transform.childCount;

            if (childCount > 0)
            {
                // 親オブジェクトから子オブジェクトを取得する
                Transform childTransform = _tradeCubeAdress.transform.GetChild(childCount - 1);
                // 子オブジェクトからGameObjectを求める
                GameObject childGameObject = childTransform.gameObject;
                // 非アクティブ化
                childGameObject.SetActive(false);
                // 親オブジェクトを変更する
                try
                {
                    GameObject PooledGameObject = GameObject.Find("PooledGameObject");
                    // 子オブジェクトを親オブジェクトの中に入れる
                    // Nullが返るのでエラーにならない
                    childTransform.parent = PooledGameObject?.transform;
                }
                catch
                {
                    Handheld.Vibrate();
                    Debug.Log("Err 2");
                    //throw new Exception("Err");
                }
            }
            else
            {
                // 子オブジェクトがない場合
                Handheld.Vibrate();
                return;
            }
        }
        else
        {
            Handheld.Vibrate();
        }
    }
    
    /// <summary>
    /// リストから非アクティブな数字オブジェクトを探す <br/>
    /// 数字プレファブの使い回し
    /// </summary>
    /// <returns>非アクティブな数字オブジェクトを返す</returns>
    public GameObject GetPooledObject(int num)
    {
        // listOfPooledObjects[num].Countから変更　2023/03/18
        for (int i = 0; i < makeNumberCount; i++)
        {
            if (listOfPooledObjects[num][i].activeInHierarchy == false)
            {
                return listOfPooledObjects[num][i];
            }
        }
        Handheld.Vibrate();
        return null;
    }
    /// <summary>
    /// textHeadにCube内のNumberを表示
    /// </summary>
    /// <param name="parentGameObject"></param>
    public void GetNumberObjectName()
    {
        if (_tradeCubeAdress != null)
        {
            //Debug.Log("GetNumberObjectName" + tradeCubeAdress.name);
            // 子オブジェクトの個数
            int childCount = _tradeCubeAdress.transform.childCount;
            Char numChar;
            string numStr = "";

            if (childCount >= 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    // [3]は数字の位置
                    numChar = _tradeCubeAdress.transform.GetChild(i).name[3];
                    numStr += " " + numChar;
                }
                // テキストオブジェクトを探す
                txtMeshPro = FindObjectOfType<ScreenManagaer>();
                // テキストを表示
                txtMeshPro.textHead.text = _tradeCubeAdress.name + " " + numStr;
            }
        }
    }
}
