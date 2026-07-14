using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; } = false;

    [SerializeField] private CanvasGroup pauseCanvasGroup;
    [SerializeField] private Button firstSelectedButton;
    [SerializeField] private float fadeDuration = 0.05f;

    private bool isPaused = false;
    private Coroutine fadeCoroutine;

    // ★【追加】ポーズが許可されているかどうかのフラグ
    private bool isPauseAllowed = false;

    void Awake()
    {
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    void Start()
    {
        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.alpha = 0f;
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;
        }

        // ★【追加】演出時間を読み取って、その間ポーズを禁止するコルーチンを開始
        StartCoroutine(WaitForProductionRoutine());
    }

    // ★【追加】演出時間を待つためのコルーチン
    private IEnumerator WaitForProductionRoutine()
    {
        isPauseAllowed = false; // 最初はポーズ禁止

        // シーン内の GameStartDirector を探す
        GameStartDirector startDirector = Object.FindFirstObjectByType<GameStartDirector>();

        if (startDirector != null)
        {
            // 演出の合計時間（秒）を取得
            float waitTime = startDirector.TotalProductionDuration;

            // Time.timeScale = 0 なので Realtime（現実の時間）で待つ
            yield return new WaitForSecondsRealtime(waitTime);
        }

        isPauseAllowed = true; // 演出時間を過ぎたのでポーズを解禁！
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // ★【修正】まだポーズが許可されていなければ、入力を完全に無視
            if (!isPauseAllowed)
            {
                return;
            }

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
        Time.timeScale = 1f;
        StartFade(0f, false);
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        IsGamePaused = true;
        Time.timeScale = 0f;
        StartFade(1f, true);

        EventSystem.current.SetSelectedGameObject(null);
        if (firstSelectedButton != null)
        {
            firstSelectedButton.Select();
        }
    }

    private void StartFade(float targetAlpha, bool isInteractive)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, isInteractive));
    }

    private IEnumerator FadeRoutine(float targetAlpha, bool isInteractive)
    {
        if (!isInteractive)
        {
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;
        }

        float startAlpha = pauseCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / fadeDuration;
            float smoothedT = Mathf.SmoothStep(0f, 1f, t);
            pauseCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, smoothedT);
            yield return null;
        }

        pauseCanvasGroup.alpha = targetAlpha;

        if (isInteractive)
        {
            pauseCanvasGroup.interactable = true;
            pauseCanvasGroup.blocksRaycasts = true;
        }
    }
}