using UnityEngine;

public class KeyColorChanger2D : MonoBehaviour
{
    [Header("キーに対応する2Dオブジェクト")]
    public SpriteRenderer spriteW;
    public SpriteRenderer spriteA;
    public SpriteRenderer spriteS;
    public SpriteRenderer spriteD;
    public SpriteRenderer spriteP;

    [Header("マウスクリックに対応する2Dオブジェクト")]
    public SpriteRenderer spriteLeftClick;  // 左クリック用
    public SpriteRenderer spriteRightClick; // 右クリック用

    [Header("色の設定")]
    public Color normalColor = Color.white; // 通常時の色
    public Color pressedColor = Color.red;  // 押した時の色（赤）

    void Update()
    {
        // --- キーボードの判定 ---
        ChangeColorIfPressedKey(KeyCode.W, spriteW);
        ChangeColorIfPressedKey(KeyCode.A, spriteA);
        ChangeColorIfPressedKey(KeyCode.S, spriteS);
        ChangeColorIfPressedKey(KeyCode.D, spriteD);
        ChangeColorIfPressedKey(KeyCode.P, spriteP);

        // --- マウスクリックの判定 ---
        // 0 = 左クリック、1 = 右クリック
        ChangeColorIfPressedMouse(0, spriteLeftClick);
        ChangeColorIfPressedMouse(1, spriteRightClick);
    }

    // キーボード用の色変更メソッド
    void ChangeColorIfPressedKey(KeyCode key, SpriteRenderer targetSprite)
    {
        if (targetSprite == null) return;

        if (Input.GetKey(key))
        {
            targetSprite.color = pressedColor;
        }
        else
        {
            targetSprite.color = normalColor;
        }
    }

    // マウス用の色変更メソッド
    void ChangeColorIfPressedMouse(int mouseButton, SpriteRenderer targetSprite)
    {
        if (targetSprite == null) return;

        if (Input.GetMouseButton(mouseButton))
        {
            targetSprite.color = pressedColor;
        }
        else
        {
            targetSprite.color = normalColor;
        }
    }
}