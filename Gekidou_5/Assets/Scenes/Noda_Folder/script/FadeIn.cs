using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 5.0f;

    private Coroutine fadeCoroutine;
    private bool isFading = false;

    void Start()
    {
        // フェードインのコルーチンを記憶しつつ開始
        fadeCoroutine = StartCoroutine(DoFadeIn());
    }

    void Update()
    {
        // フェード中にマウスの左クリック（またはスマホ画面タップ）を検知
        if (isFading && Input.GetMouseButtonDown(0))
        {
            SkipFadeIn();
        }
    }

    private IEnumerator DoFadeIn()
    {
        isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1.0f - (elapsedTime / fadeDuration));
            yield return null;
        }

        EndFade();
    }

    // スキップされた時の処理
    private void SkipFadeIn()
    {
        // 実行中のフェードコルーチンを強制停止
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        canvasGroup.alpha = 0f; // 一瞬で透明にする
        EndFade();
    }

    // フェード終了時の共通処理
    private void EndFade()
    {
        canvasGroup.blocksRaycasts = false; // 背後のUIを触れるようにする
        isFading = false;
        Debug.Log("フェードインが完了しました");
    }
}