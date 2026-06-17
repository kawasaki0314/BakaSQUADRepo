using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private float normalSize = 30f;
    [SerializeField] private float hoverSize = 38f;

    // アニメーションのスピード（値を大きくするほど速く変化します）
    [SerializeField] private float changeSpeed = 120f;

    private float targetSize; // 目標にする文字サイズ

    void Start()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        // 最初は通常サイズを目標にする
        targetSize = normalSize;
        if (buttonText != null)
        {
            buttonText.fontSize = normalSize;
        }
    }

    void Update()
    {
        if (buttonText == null) return;

        // 現在のサイズが目標サイズと違う場合、滑らかに近づける
        if (!Mathf.Approximately(buttonText.fontSize, targetSize))
        {
            buttonText.fontSize = Mathf.MoveTowards(
                buttonText.fontSize,
                targetSize,
                changeSpeed * Time.unscaledDeltaTime
            );
        }
    }

    // カーソルがボタンの上に乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetSize = hoverSize; // 目標サイズを大きくする
    }

    // カーソルがボタンから離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        targetSize = normalSize; // 目標サイズを元に戻す
    }
}