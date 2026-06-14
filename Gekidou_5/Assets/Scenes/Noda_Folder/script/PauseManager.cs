using System.Collections; // コルーチンを使うために必要です
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; } = false;//どこからでも参照できるフラグ

    [SerializeField] private CanvasGroup pauseCanvasGroup; // GameObjectの代わりにCanvasGroupを使います
    [SerializeField] private Button firstSelectedButton;
    [SerializeField] private float fadeDuration = 0.05f; // フェードにかかる時間（秒）

    private bool isPaused = false;
    private Coroutine fadeCoroutine; // 実行中のフェード処理を管理する変数

    void Awake()
    {
        //時を動かす
        Time.timeScale = 1f;

        //シーン遷移に残ってしまったポーズフラグを強制リセットする
        IsGamePaused = false;
    }

    void Start()
    {
        // ゲーム開始時はポーズ画面を完全に隠しておく
        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.alpha = 0f;
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;
        IsGamePaused = false;

        // 時間の流れを元に戻す
        Time.timeScale = 1f;

        // フェードアウトを開始
        StartFade(0f, false);
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        IsGamePaused = true;

        //時を動かす
        Time.timeScale = 0f;

        // フェードインを開始
        StartFade(1f, true);

        // ボタンの選択
        EventSystem.current.SetSelectedGameObject(null);
        if (firstSelectedButton != null)
        {
            firstSelectedButton.Select();
        }
    }

    // フェード処理を安全に開始するためのメソッド
    private void StartFade(float targetAlpha, bool isInteractive)
    {
        // すでにフェード中なら、それを止めて新しいフェードを開始する
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, isInteractive));
    }

    // フェードの具体的な中身（コルーチン）
    private IEnumerator FadeRoutine(float targetAlpha, bool isInteractive)
    {
        // フェード中はボタンが誤反応しないように、一旦操作を無効化する（フェードイン時は最後に有効化）
        if (!isInteractive)
        {
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;
        }

        float startAlpha = pauseCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // ポーズ中も時間をカウントするために unscaled を使用
            float t = elapsedTime / fadeDuration;
            float smoothedT = Mathf.SmoothStep(0f, 1f, t);//SmoothStepで始まりと終わりを滑らかに
            pauseCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        pauseCanvasGroup.alpha = targetAlpha;

        // フェードイン完了時のみ、ボタンを触れるようにする
        if (isInteractive)
        {
            pauseCanvasGroup.interactable = true;
            pauseCanvasGroup.blocksRaycasts = true;
        }
    }
}