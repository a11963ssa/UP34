using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//引用場景管理API

public class GameManager : MonoBehaviour
{
    public Text textload;
    public Image imageload;

    /// <summary>
    /// 重啟遊戲
    /// </summary>
    /// <param name="secn"></param>
    public void Replay(string secn)
    {
        SceneManager.LoadScene(secn);

    }
    /// <summary>
    /// 結束遊戲
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    public void startload()
    {
        StartCoroutine(LOADING());
    }

    public IEnumerator LOADING()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("關卡1");     //取得場景資料
        ao.allowSceneActivation = false;                              //取消載入

        while (ao.isDone == false)
        {
            textload.text = ao.progress / 0.9f * 100 + " /100";
            imageload.fillAmount = ao.progress / 0.9f;
            yield return null;
            if (ao.progress == 0.9f && Input.anyKey)///進度完成+按任意建
            {
                ao.allowSceneActivation = true;
            }
        }

    }

}
