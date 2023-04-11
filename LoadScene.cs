using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/********************************************************************
 * 最終更新：2023/04/07
 * 環境：Unity 2023.3.13f1 LTS
 * 環境：Unity C# 
 * 機能：シーンの変更
*********************************************************************/
public class LoadScene : MonoBehaviour
{
    GameManager gameManager = new GameManager();

    public void OnClickSetting()
    {
        SceneManager.LoadScene("3DSUDOKU");
    }
    public void OnClick3DSUDOKU()
    {
        // 保存
        gameManager.SaveNumberAnswer();

        gameManager.SaveCubeAngle();

        SceneManager.LoadScene("Setting");
    }
}
