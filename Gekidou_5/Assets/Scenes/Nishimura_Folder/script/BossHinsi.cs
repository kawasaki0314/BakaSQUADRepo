//using UnityEngine;

//public class BossHinsi : MonoBehaviour
//{
//    //コンポーネントを取得するためのもの
//    SpriteRenderer spriteRenderer;
//    //アニメーション画像の配列宣言
//    [SerializeField] Sprite[] bossHinsi;
//    //1コマが表示されるFrame
//    int animFrame = 10;
//    //切り替え用のカウンター
//    int frameTimer = 0;
//    //アニメーションの最大数
//    int animMax;
//    //現在のコマ数
//    int animIdx = 0;
//    BossHp bossHp;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        bossHp = GetComponent<BossHp>();
//        //コンポーネントでGetできる
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        //animMaxで配列の長さを代入
//        animMax = bossHinsi.Length;
//    }

//    // Update is called once per frame
//    private void FixedUpdate()
//    {
        

//        frameTimer++;
       
        

        
//        //見た目をコマ数に応じて変更する処理
//        spriteRenderer.sprite = bossHinsi[animIdx];
//    }
//}
