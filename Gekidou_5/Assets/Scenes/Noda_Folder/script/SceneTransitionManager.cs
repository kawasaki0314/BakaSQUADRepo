using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("フェード用UI")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f; // フェードにかかる時間（秒）

    [Header("遷移先シーン名")]
    [SerializeField] private string nextSceneName;

    private bool isTransitioning = false;

    void Start()
    {
        if (fadeImage != null)
        {
            // 開始時はフェード用画像を透明にして、クリックを邪魔しないように非アクティブにする
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(false);
        }
    }

    // ボタンから呼び出す関数
    public void OnButtonClick()
    {
        // 連打防止
        if (isTransitioning) return;

        StartCoroutine(FadeAndTransition());
    }

    private IEnumerator FadeAndTransition()
    {
        isTransitioning = true;

        if (fadeImage != null)
        {
            // フェード画像を表示し、最前面に持ってくる
            fadeImage.gameObject.SetActive(true);
            fadeImage.transform.SetAsLastSibling();

            float elapsedTime = 0f;
            Color color = fadeImage.color;

            // 徐々に不透明（アルファ値を1）にしていく
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime; // Time.timeScaleに依存しないように
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }

            // 完全に白にする
            color.a = 1f;
            fadeImage.color = color;
        }

        // シーンのロードを実行
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("遷移先のシーン名が設定されていません。");
        }
    }
}