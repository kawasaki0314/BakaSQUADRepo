using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFlashEffect : MonoBehaviour
{
    [Header("フェードアウト設定")]
    [Tooltip("完全に透明になるまでの時間（秒）")]
    [SerializeField] private float fadeDuration = 1.5f;

    // --- 【追加】SE設定 ---
    [Header("SE Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip flashSE; // フラッシュ時の音（ドン！やピカッ！など）

    private Image flashImage;

    void Awake()
    {
        flashImage = GetComponent<Image>();

        // 自動でAudioSourceを取得する保険
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        Color startColor = flashImage.color;
        startColor.a = 0f;
        flashImage.color = startColor;
    }

    public void PlayFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // 1. パッと真っ白にする
        Color tempColor = flashImage.color;
        tempColor.a = 1f;
        flashImage.color = tempColor;

        // --- 【追加】真っ白になった瞬間にフラッシュSEを再生 ---
        if (audioSource != null && flashSE != null)
        {
            audioSource.PlayOneShot(flashSE);
        }

        // 2. だんだん透明度を減らしていく
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / fadeDuration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            tempColor.a = Mathf.Lerp(1f, 0f, t);
            flashImage.color = tempColor;

            yield return null;
        }

        tempColor.a = 0f;
        flashImage.color = tempColor;
    }
}