using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fadeout_Warning : MonoBehaviour
{
    [SerializeField] private Image fadeImage;      // 白いイメージ
    [SerializeField] private float fadeDuration = 1.0f; // フェードにかかる時間（秒）
    [SerializeField] private string nextSceneName;  // 遷移先のシーン名

    private bool isFading = false;

    void Update()
    {
        // 画面のどこかが左クリックされ、かつフェード中でなければ開始
        if (Input.GetMouseButtonDown(0) && !isFading)
        {
            StartCoroutine(FadeOutAndChangeScene());
        }
    }

    private IEnumerator FadeOutAndChangeScene()
    {
        isFading = true;
        float timer = 0f;
        Color color = fadeImage.color;

        // 徐々に白くしていく（アルファ値を0から1へ）
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // 完全に白くなったらシーン遷移
        SceneManager.LoadScene(nextSceneName);
    }
}