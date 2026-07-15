using UnityEngine;
using UnityEngine.UI; // UIコンポーネントを扱うために必要！

public class KeyColorChangerUI : MonoBehaviour
{
    [Header("キーに対応するUIオブジェクト")]
    public Image imageW;
    public Image imageA;
    public Image imageS;
    public Image imageD;
    public Image imageP;

    [Header("マウスクリックに対応するUIオブジェクト")]
    public Image imageLeftClick;  // 左クリック用
    public Image imageRightClick; // 右クリック用

    [Header("色の設定")]
    public Color normalColor = Color.white; // 通常時の色
    public Color pressedColor = Color.red;  // 押した時の色（赤）

    void Update()
    {
        // --- キーボードの判定 ---
        ChangeColorIfPressedKey(KeyCode.W, imageW);
        ChangeColorIfPressedKey(KeyCode.A, imageA);
        ChangeColorIfPressedKey(KeyCode.S, imageS);
        ChangeColorIfPressedKey(KeyCode.D, imageD);
        ChangeColorIfPressedKey(KeyCode.P, imageP);

        // --- マウスクリックの判定 ---
        // 0 = 左クリック、1 = 右クリック
        ChangeColorIfPressedMouse(0, imageLeftClick);
        ChangeColorIfPressedMouse(1, imageRightClick);
    }

    // キーボード用の色変更メソッド
    void ChangeColorIfPressedKey(KeyCode key, Image targetImage)
    {
        if (targetImage == null) return;

        if (Input.GetKey(key))
        {
            targetImage.color = pressedColor;
        }
        else
        {
            targetImage.color = normalColor;
        }
    }

    // マウス用の色変更メソッド
    void ChangeColorIfPressedMouse(int mouseButton, Image targetImage)
    {
        if (targetImage == null) return;

        if (Input.GetMouseButton(mouseButton))
        {
            targetImage.color = pressedColor;
        }
        else
        {
            targetImage.color = normalColor;
        }
    }
}