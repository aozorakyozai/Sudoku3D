using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

/********************************************************************
 * 最終更新：2023/04/07
 * 機能：Load,Save,clearなど
 * awake(出題を選択)
 * Clear 出題数字,回答数字,キューブカラー
 * Save(出題,キューブに数字を入れる)
 * Save回答番号の保存
 * Load回答番号を再現する：前回の続きからスタートするため
 * Saveキューブの向き
 * Loadキューブの向き
*********************************************************************/

public class GameManager : MonoBehaviour
{
    // DBから取得するテーブル　とりあえず仮置　playfabに移動予定　2023/04/06
    public List<List<string>> dbQuestionList;
    /// <summary>[27] : 3x3x3 Cubeの定数</summary>
    public const int cubeCount = 27;
    /// <summary>出題の配列</summary>
    public string[] questionArray = new string[cubeCount];
    /// <summary>回答の配列</summary>
    public string[] answerArray = new string[cubeCount];
    /// <summary>答え合わせの配列</summary>
    public int[] checkAnswerArray = new int[cubeCount];
    /// <summary>現在の出題</summary>
    public int[] puzzleQuestion;
    /// <summary>answerArrayに格納するための変数</summary>
    string puzzleQuestionNumbeToString;
    /// <summary>１文字を格納</summary>
    Char numChar;
    /// <summary>数字オブジェクト名から数字を取得する場所(ex.num1Prefab)</summary>
    const int numberPrefabToNumber = 3;

    // 読まない
    /// <summary>問題番号</summary>
    int questionNo = 8;
    /// <summary>回答中を文字列で保存</summary>
    string answerArrayToString;

    // 画面上下のテキスト
    ScreenManagaer txtMeshPro;

    NumberDriver numberDriver = new NumberDriver();

    /// <summary>選択されたキューブ</summary>
    public static GameObject tradeCubeAdress;

    // cubeAdressから配列を作成し、Findメソッドを回避する
    public GameObject[] cubeGameObjects = new GameObject[cubeCount];

    //CubeDriver cubeDriver = new CubeDriver();

    //ScoreManager scoreManager = new ScoreManager();

    /// <summary>Cubeのアドレス(Cube123)</summary>
    string cubeNumber;
    /// <summary>キューブに入れる出題用の数字</summary>
    int puzzleQuestionNumber;
    /// <summary>キューブに入れる回答の数字</summary>
    int puzzleAnswerNumber;
    // cubeのフレームのマテリアルを指定するための変数
    private Material myMaterial_0;
    // cubeのガラスのマテリアルを指定するための変数
    private Material myMaterial_1;
    /// <summary>出題キューブのフレームの色(書き換え不可)</summary>
    public static Color QuestionColor { get; } = Color.blue;
    // 相対位置のオフセット
    public Vector3 positionOffset;
    // 相対回転(向き)のオフセット
    public Quaternion rotationOffset;
    // スケール
    public Vector3 scale;
    

    private void Awake()
    {
        // キューブ配列の作成 Find回避
        SetCubeAdress();

        // Start画面の表示

        // 出題配列をインストール

        // 難易度設定　 → Setting

        // 正解ならシーンを変更　アニメーション？

        // DBにあるリストから出題する問題を取得
        dbQuestionList = new List<List<string>>{new List<string> { "0", "0", "3", "8", "5", "0", "0", "0", "0", "4", "0", "0", "0", "0", "9", "7", "6", "0", "0", "0", "1", "2", "0", "0", "0", "4", "8" } };

        // リストから出題を引き出す
        // 現在はリストは[0]一つのみ出題があるから　2023/04/07
        questionArray = dbQuestionList[0].ToArray();

        //テキストを表示
        txtMeshPro = FindObjectOfType<ScreenManagaer>();

        txtMeshPro.textBottom.text = "GameManager test";
    }

    private void Start()
    {
        // 出題
        LoadQuestion();

        /**** 前回の状態を再現する ****/
        // 前回までの回答を再現する
        LoadNumberAnswer();
        // キューブの向き
        LoadCubeAngle();
    }

    // どこに設置するかあとから検討予定
    private void OnApplicationQuit()
    {
        // 回答の保存
        SaveNumberAnswer();
        // キューブの向き
        SaveCubeAngle();
    }

    // NEXT

    // 問題レベル
    // 広告
    // 履歴
    // 問題点：キューブにマテリアルが反映されず、個別に指定した場合は、色が少し違う　2023/04/11

    /// <summary>
    /// キューブナンバーから配列を作成する
    /// </summary>
    public void SetCubeAdress()
    {
        int l = 0;
        for (int i = 1; i < 4; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                for (int k = 1; k < 4; k++)
                {
                    // 3桁のCubNumberを生成
                    cubeNumber = "Cube" + (i * 100 + j * 10 + k).ToString();
                    cubeGameObjects[l++] = GameObject.Find(cubeNumber);
                }
            }
        }
    }

    /// <summary>
    /// cubeマテリアルの初期化 <br/>
    /// cube子オブジェクトのDelete <br/>
    /// </summary>
    public void ClearCube()
    {
        for (int i = 0; i < cubeGameObjects.Length; i++)
        {

            // MaterialIndex(1：セルキューブのガラス面のマテリアルインデックス)
            Transform cubeNumberTransform = cubeGameObjects[i].transform;

            // **** フレームとガラスのマテリアル ****
            Material material_0 = cubeNumberTransform.GetComponent<Renderer>().materials[0];
            Material material_1 = cubeNumberTransform.GetComponent<Renderer>().materials[1];

            // フレーム
            material_0.color = Color.white;
            material_0.SetFloat("_Metallic", 0.5f);
            material_0.SetFloat("_Smoothness", 0.5f);
            // ガラス
            material_1.color = new Color(255f, 255f, 255f, 0.05f);
            material_1.SetFloat("_Metallic", 1.0f);
            material_1.SetFloat("_Smoothness", 1.0f);

            // **** 子オブジェクトを外す ****


            //問題 → 1つおきに削除される


            int childCount = cubeNumberTransform.childCount;
            Debug.Log(childCount);
            if (childCount > 0)
            {
                // 親オブジェクトから子オブジェクトを取得する
                for (int l = 0; l < childCount; l++)
                {
                    try
                    {
                        Transform childTransform = cubeNumberTransform.GetChild(l);
                        // 子オブジェクトからGameObjectを求める
                        GameObject childGameObject = childTransform.gameObject;
                        // 親オブジェクトを変更する
                        // NumberDriverからもらってくる予定　2023/04/11
                        GameObject PooledGameObject = GameObject.Find("PooledGameObject");
                        // 子オブジェクトを親オブジェクトの中に入れる
                        childTransform.parent = PooledGameObject?.transform;
                        // 非アクティブ化
                        childGameObject.SetActive(false);
                    }
                    catch
                    {
                        //continue;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 出題
    /// </summary>
    public void LoadQuestion()
    {
        // 出題用のint型の配列　( DBはstring型のため)
        puzzleQuestion = new int[cubeCount];

        // 出題用int型の配列を取得
        for (int i = 0; i < questionArray.Length; i++)
        {
            // 文字配列を数字変換　失敗時は、０を代入
            puzzleQuestion[i] = int.TryParse(questionArray[i], out int result) ? result : 0;
        }
        
        int l = 0;
        for (int i = 1; i < cubeGameObjects.Length; i++)
        {
            // MaterialIndex(1：セルキューブのガラス面のマテリアルインデックス)
            Transform cubeNumberTransform = cubeGameObjects[i].transform;

            Material material_0 = cubeNumberTransform.GetComponent<Renderer>().materials[0];
            Material material_1 = cubeNumberTransform.GetComponent<Renderer>().materials[1];

            puzzleQuestionNumber = puzzleQuestion[l++];

            if (puzzleQuestionNumber != 0)
            {
                // キューブ　色と透明度
                material_0.color = QuestionColor;
                material_1.color = new Color(0f, 0f, 1f, 0.1f);
                // キューブ　メタリック
                material_1.SetFloat("_Metallic", 0.5f);
                // キューブ　スムースネス
                material_1.SetFloat("_Smoothness", 0.5f);
                // 数字オブジェクト　非表示オブジェクトを探すして該当キューブに入れる
                GameObject numberGameObject = numberDriver.GetPooledObject(puzzleQuestionNumber - 1);
                // 数字　子オブジェクトにする
                numberGameObject.transform.SetParent(cubeNumberTransform);
                // 数字　表示
                numberGameObject.SetActive(true);
                // 数字　ポジション
                numberGameObject.transform.localPosition = new Vector3(0, 0, 0);
                // 数字　向き
                numberGameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
                // 数字　スケール
                numberGameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
            else
            {
                // ALL Clearへ移植予定
                // Assetsフォルダ内にある"MyMaterial"という名前のマテリアルを取得する
                try
                {
                    myMaterial_0 = Resources.Load<Material>("Cube_White");
                    myMaterial_1 = Resources.Load<Material>("Cube_Transparent");
                            
                }
                catch
                {
                    return;
                }
                // フレーム
                material_0 = myMaterial_0;
                // ガラス
                material_1 = myMaterial_1;
            }
        }
    }

    /// <summary>
    /// 保存用配列の作成
    /// </summary>
    public void SaveNumberAnswer()
    {
        // 出題
        string[] puzzleQuestion = answerArray;

        int l = 0;
        for (int i = 1; i < cubeGameObjects.Length; i++)
        {

            // 全検索
            Transform cubeNumberTransform = cubeGameObjects[i].transform;

            // キューブ内の子オブジェクト情報の文字列を作る
            if (cubeNumberTransform.childCount == 0)
            {
                // キューブに数字がない場合
                puzzleQuestionNumbeToString = "0";
            }
            else if (cubeNumberTransform.childCount == 1)
            {
                // キューブに数字が１つある場合
                numChar = cubeNumberTransform.GetChild(0).name[numberPrefabToNumber];
                puzzleQuestionNumbeToString = numChar.ToString();
            }
            else
            {
                // 複数ある場合
                string numStr = "";
                for (int m = 0; m < cubeNumberTransform.childCount; m++)
                {
                    numChar = cubeNumberTransform.GetChild(m).name[numberPrefabToNumber];
                    numStr += numChar;
                    // 最後の","は入れない
                    numStr += (m < cubeNumberTransform.childCount - 1) ? "," : "";
                }

                puzzleQuestionNumbeToString = numStr;
            }
            // 配列に書き出し
            answerArray[l++] = puzzleQuestionNumbeToString;
        }
        
        // 配列を文字列にする
        answerArrayToString = string.Join("|", answerArray);
        // 問題番号
        PlayerPrefs.SetInt("questionNumber", questionNo);
        // 変更
        PlayerPrefs.SetString("answerArrayToString", answerArrayToString);
        // 保存
        PlayerPrefs.Save();
    }

    /// <summary>再起動用の出題</summary>
    public void LoadNumberAnswer()
    {
        int j = 0;
        int k = 0;
        for (int i = 1; i < cubeGameObjects.Length; i++)
        {
            // MaterialIndex(1：セルキューブのガラス面のマテリアルインデックス)
            Transform cubeNumberTransform = cubeGameObjects[i].transform;
            // ２つの配列を同時に検索する
            puzzleQuestionNumber = puzzleQuestion[j++];
            string numString = ImportPuzzleAnswer().ansArray[k++];
            // 要素の長さ
            int numLength = numString.Length;

            if (puzzleQuestionNumber == 0)
            {
                if (numLength == 1)
                {
                    puzzleAnswerNumber = int.TryParse(numString, out int result) ? result : 0;
                    if (puzzleAnswerNumber != 0)
                    {
                        SetNumberInCube(cubeNumberTransform, puzzleAnswerNumber);
                    }
                }
                else if (numLength > 1)
                {
                    // 数字が複数の場合
                    string[] strAry = numString.Split(",");

                    for (int n = 0; n < strAry.Length; n++)
                    {
                        puzzleAnswerNumber = int.TryParse(strAry[n], out int result) ? result : 0;

                        if (puzzleAnswerNumber != 0)
                        {
                            SetNumberInCube(cubeNumberTransform, puzzleAnswerNumber);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 前回の続きを取り出す
    /// </summary>
    /// <returns>tuple:(問題番号, 回答の配列)</returns>
    public (int qNo, string[] ansArray) ImportPuzzleAnswer()
    {
        // 問題番号
        questionNo = PlayerPrefs.GetInt("questionNumber");
        // スコアのロード
        answerArrayToString = PlayerPrefs.GetString("answerArrayToString");
        // 文字列を配列に変更
        answerArray = answerArrayToString.Split('|');

        return (questionNo, answerArray);
    }

    /// <summary>
    /// 指定された数字を指定されたCubeにセットする
    /// </summary>
    /// <param name="Transform"></param>
    /// <param name="int"></param>
    public void SetNumberInCube(Transform t, int num)
    {
        // 数字オブジェクト　非表示オブジェクトを探すして該当キューブに入れる
        GameObject numberGameObject = numberDriver.GetPooledObject(num - 1);
        // 数字　子オブジェクトにする
        numberGameObject.transform.SetParent(t);
        // 数字　表示
        numberGameObject.SetActive(true);
        // 数字　ポジション
        numberGameObject.transform.localPosition = new Vector3(0, 0, 0);
        // 数字　向き
        numberGameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        // 数字　スケール
        numberGameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    /// <summary>
    /// キューブの回転の向きを取得する <br/>
    /// PlayerPrefsにデータを送る
    /// </summary>
    public void SaveCubeAngle()
    {
        Transform transform = GameObject.Find("Cube_333").transform;

        Vector3 savedRotation = transform.rotation.eulerAngles;
        // ｘｙｚの値を文字列に変換
        string stringAngle = savedRotation.x.ToString() + "," + savedRotation.y.ToString() + "," + savedRotation.z.ToString();
        // PlayerPrefsに保存
        PlayerPrefs.SetString("savedRotation", stringAngle);
    }

    /// <summary>
    /// PlayerPrefsからデータを受け取る <br/>
    /// キューブの回転の向きを再現する
    /// </summary>
    public void LoadCubeAngle()
    {
        // 
        Transform transform = GameObject.Find("Cube_333").transform;
        // PlayerPrefsからインポート
        string savedRotationString = PlayerPrefs.GetString("savedRotation");

        Debug.Log(savedRotationString);

        if (!string.IsNullOrEmpty(savedRotationString))
        {
            string[] split = savedRotationString.Split(',');
            if (split.Length == 3)
            {
                float x, y, z;
                if (float.TryParse(split[0], out x) && float.TryParse(split[1], out y) && float.TryParse(split[2], out z))
                {
                    transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
                }
                else
                {
                    return;
                }
            }
        }
    }
}
