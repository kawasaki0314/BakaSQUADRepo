using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ColorChanger : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textMeshPro;

    // 状態管理用
    // 0: 白, 1: 黄色, 2: 虹色
    private int colorState = 0;

    // 虹色の変化スピード
    [SerializeField] private float rainbowSpeed = 2f;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        SetColorState(0); // 初期状態は白
    }

    void Update()
    {
        // 状態が2（虹色）の時は、時間経過で色を変化させる
        if (colorState == 2)
        {
            // 時間経過で色相（Hue）を0～1でループさせる
            float hue = (Time.time * rainbowSpeed) % 1f;
            textMeshPro.color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }

    // クリックされた時のイベント（EventSystemが必要です）
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            // ダブルクリック時
            SetColorState(2);
        }
        else if (eventData.clickCount == 1)
        {
            // シングルクリック時
            if (colorState == 0)
            {
                SetColorState(1); // 白なら黄色へ
            }
            else if (colorState == 1 || colorState == 2)
            {
                SetColorState(0); // 黄色か虹色なら白へ
            }
        }
    }

    // 状態に応じた色の切り替え
    private void SetColorState(int state)
    {
        colorState = state;

        switch (colorState)
        {
            case 0:
                textMeshPro.color = Color.white;
                break;
            case 1:
                textMeshPro.color = Color.yellow;
                break;
            case 2:
                // 虹色はUpdate内で更新するためここでは何もしない
                break;
        }
    }
}