using UnityEngine;

public class anim : MonoBehaviour
{
    //コンポーネントを取得するためのもの
    SpriteRenderer spriteRenderer;
    //アニメーション画像の配列宣言
    [SerializeField] Sprite[] Enemyanim;
    //1コマが表示されるFrame
    int animFrame = 10;
    //切り替え用のカウンター
    int frameTimer = 0;
    //アニメーションの最大数
    int animMax;
    //現在のコマ数
    int animIdx = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネントでGetできる
        spriteRenderer = GetComponent<SpriteRenderer>();
        //animMaxで配列の長さを代入
        animMax = Enemyanim.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        frameTimer++;
        //アニメーションを切り替える時の処理
        if (frameTimer >= animFrame)
        {
            frameTimer = 0;
            animIdx++;
            //アニメーションが最大数に達したらコマ数を0に戻す処理
            if (animIdx >= animMax)
            {
                animIdx = 0;
            }
        }
        //見た目をコマ数に応じて変更する処理
        spriteRenderer.sprite = Enemyanim[animIdx];
    }
}
