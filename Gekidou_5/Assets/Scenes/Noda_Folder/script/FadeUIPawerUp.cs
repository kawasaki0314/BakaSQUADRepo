using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeText : MonoBehaviour
{
    [SerializeField] private float displayDuration = 2f;   // 表示し続ける時間
    [SerializeField] private float fadeDuration = 1f;      // フェードにかける時間

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        // 一定時間そのまま表示
        yield return new WaitForSeconds(displayDuration);

        // フェードアウト処理
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;

        // 必要なら非表示にする
        gameObject.SetActive(false);
    }
}