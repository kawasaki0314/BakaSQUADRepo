using UnityEngine;
using TMPro; // TextMesh Proを使用するために必要

public class BlinkingText : MonoBehaviour
{
    private TextMeshProUGUI textComponent;

    [Header("点滅の設定")]
    [SerializeField] private float blinkSpeed = 2.0f;     // 点滅の速さ
    [SerializeField] private float minAlpha = 0.5f;       // 最も薄いときの透明度 (0.0 ～ 1.0)
    [SerializeField] private float maxAlpha = 1.0f;       // 最も濃いときの透明度 (0.0 ～ 1.0)

    void Start()
    {
        // 同じオブジェクトからTextMesh Proコンポーネントを取得
        textComponent = GetComponent<TextMeshProUGUI>();

        if (textComponent == null)
        {
            Debug.LogError("BlinkingText: TextMeshProUGUIコンポーネントが見つかりません！");
        }
    }

    void Update()
    {
        if (textComponent == null) return;

        // Mathf.PingPongを使って0～1の間を行ったり来たりさせる
        float lerpValue = Mathf.PingPong(Time.unscaledTime * blinkSpeed, 1.0f);

        // minAlpha と maxAlpha の間で補間する
        float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, lerpValue);

        // テキストの色を取得し、アルファ値だけを書き換えて再設定
        Color textColor = textComponent.color;
        textColor.a = currentAlpha;
        textComponent.color = textColor;
    }
}