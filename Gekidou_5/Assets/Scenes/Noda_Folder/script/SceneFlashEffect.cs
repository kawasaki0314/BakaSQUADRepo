using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Imageを扱うために必要

public class SceneFlashEffect : MonoBehaviour
{
    [Header("フェードアウト設定")]
    [Tooltip("完全に透明になるまでの時間（秒）")]
    [SerializeField] private float fadeDuration = 1.5f;

    private Image flashImage;

    void Awake()
    {
        flashImage = GetComponent<Image>();

        // 開始時は完全に透明にしておく（保険）
        Color startColor = flashImage.color;
        startColor.a = 0f;
        flashImage.color = startColor;
    }

    /// <summary>
    //「ゲキドウ！」が揃った直後に外部から呼ばれる関数
    /// </summary>
    public void PlayFlash()
    {
        // 既に光っている場合は一度止める（保険）
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // 1. パッと真っ白にする
        Color tempColor = flashImage.color;
        tempColor.a = 1f; // 完全に不透明（真っ白）
        flashImage.color = tempColor;

        // 2. だんだん透明度（Alpha）を減らしていく（フェードアウト）
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;

            // 進捗の割合（0～1）
            float t = timeElapsed / fadeDuration;

            // イージング（滑らかなフェード）：数学的な調整
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            // Lerpを使って、1(不透明)から0(透明)へ変化させる
            tempColor.a = Mathf.Lerp(1f, 0f, t);
            flashImage.color = tempColor;

            yield return null; // 1フレーム待つ
        }

        // 3. 最後に確実に完全に透明にする
        tempColor.a = 0f;
        flashImage.color = tempColor;
    }
}