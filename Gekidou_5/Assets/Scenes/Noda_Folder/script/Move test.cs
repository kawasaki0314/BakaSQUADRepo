using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveTest : MonoBehaviour
{
    [SerializeField] private float speed = 0.02f; // フェードスピード

    private Image fadeImage;
    private float currentAlpha = 0f;
    private bool isFadingOut = false;
    private string nextSceneName = "TestScene"; 

    void Start()
    {
        // 自分自身についているImageを取得
        fadeImage = GetComponent<Image>();
        
        if (fadeImage == null)
        {
            Debug.LogError("【エラー】このオブジェクトにImageコンポーネントがついていません！");
            return;
        }

        // 開始時は透明にして、クリックの邪魔をさせない
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
        fadeImage.enabled = false;
    }

    void Update()
    {
        if (isFadingOut)
        {
            currentAlpha += speed;
            currentAlpha = Mathf.Clamp01(currentAlpha);
            fadeImage.color = new Color(0f, 0f, 0f, currentAlpha);

            if (currentAlpha >= 1f)
            {
                isFadingOut = false;
                Debug.Log("【完了】画面が真っ黒になりました。シーンを切り替えます！");
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    // ★ボタンから呼び出す関数
    public void ClickToFadeOut()
    {
        Debug.Log("★ボタンがクリックされました！フェードアウトを開始します。");
        
        if (fadeImage == null)
        {
            Debug.LogError("【エラー】fadeImageが空っぽなので処理を中断します。");
            return;
        }

        currentAlpha = 0f;
        fadeImage.enabled = true; 
        isFadingOut = true;      
    }
}