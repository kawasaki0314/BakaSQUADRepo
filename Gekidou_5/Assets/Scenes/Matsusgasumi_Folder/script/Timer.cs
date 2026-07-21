using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; // コルーチンを使うために必要！

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private float countTime; // インスペクター用

    [Header("Warning Settings")]
    [SerializeField] private float warningTime = 60f; // 赤色・点滅を開始する残り時間（秒）
    [SerializeField] private Color defaultColor = Color.white; // 通常時の文字色
    [SerializeField] private Color warningColor = Color.red;   // 警告時の文字色
    [SerializeField] private float blinkSpeed = 5f;          // 点滅の速さ

    [Header("Scene Transition Settings")]
    [SerializeField] private string nextSceneName; // 遷移先のシーン名
    [SerializeField] private Image fadeImage;      // フェード用の Image
    [SerializeField] private float fadeDuration = 1.0f; // 暗くなるまでの時間（秒）

    private float remainingTime; // 残り時間を保持する変数
    private bool isTimerRunning = true; // タイマーが動いているかどうかのフラグ

    private void Start()
    {
        // 初期化
        remainingTime = countTime;
        uiText.color = defaultColor;

        // フェード用の画像を最初は完全に透明（アルファ 0）にし、非表示にする
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // タイマーが動いていない場合は処理を抜ける
        if (!isTimerRunning) return;

        // 毎フレームの経過時間を引いていく
        remainingTime -= Time.deltaTime;

        // 0秒以下になったらタイマーを止めてフェードアウト開始
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            isTimerRunning = false;

            // 時間の表示を 00:00 に固定
            uiText.text = "00:00";
            uiText.color = warningColor;

            // タイムアップ処理（フェード付きシーン遷移）を開始
            StartCoroutine(TimeUpRoutine());
            return;
        }

        // 時間の表示更新
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        uiText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        // 60秒未満の演出処理
        if (isTimerRunning && remainingTime < warningTime)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 0.8f) + 0.2f;

            Color currentColor = warningColor;
            currentColor.a = alpha;
            uiText.color = currentColor;
        }
    }

    /// <summary>
    /// タイムアップ時のフェードアウト＆シーン遷移コルーチン
    /// </summary>
    private IEnumerator TimeUpRoutine()
    {
        if (fadeImage != null)
        {
            // 画像を表示状態にする
            fadeImage.gameObject.SetActive(true);

            Color fadeColor = fadeImage.color;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                // 徐々にアルファ値を 1.0（不透明）に近づける
                fadeColor.a = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = fadeColor;
                yield return null; // 1フレーム待つ
            }

            fadeColor.a = 1f;
            fadeImage.color = fadeColor;
        }

        // 暗くなり切ったらシーン遷移
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("遷移先のシーン名（Next Scene Name）が設定されていません！");
        }
    }
}