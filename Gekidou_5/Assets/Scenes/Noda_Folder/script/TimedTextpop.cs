using System.Collections;
using UnityEngine;
using TMPro; // TextMeshProを使うために必要（通常のTextなら using UnityEngine.UI;）

public class TimedTextPop : MonoBehaviour
{
    [SerializeField] private GameObject textObject; // 表示させたいTextオブジェクト
    [SerializeField] private float delaySeconds = 3.0f; // 何秒後に表示するか
    [SerializeField] private bool useFadeIn = true;    // じわっと表示させるかどうか
    [SerializeField] private float fadeDuration = 1.0f; // フェードインにかかる時間

    void Start()
    {
        // シーン開始時にコルーチンをスタート
        StartCoroutine(ShowTextAfterDelay());
    }

    private IEnumerator ShowTextAfterDelay()
    {
        // 指定された秒数だけ待つ
        yield return new WaitForSeconds(delaySeconds);

        // オブジェクトをアクティブ（表示状態）にする
        textObject.SetActive(true);

        // フェードイン演出をする場合
        if (useFadeIn)
        {
            TextMeshProUGUI tmpText = textObject.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                float timer = 0f;
                Color color = tmpText.color;

                // 最初は透明
                color.a = 0f;
                tmpText.color = color;

                // 徐々に不透明にしていく
                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                    tmpText.color = color;
                    yield return null;
                }
            }
        }
    }
}