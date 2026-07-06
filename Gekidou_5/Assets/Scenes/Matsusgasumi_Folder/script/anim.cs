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
    //プレイヤーのTransformを直接持つ
    Transform playerTr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネントでGetできる
        spriteRenderer = GetComponent<SpriteRenderer>();
        //animMaxで配列の長さを代入
        animMax = Enemyanim.Length;
        //プレイヤーを探して取得
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");
        if (playerobj != null) playerTr = playerobj.transform;
        
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

        //前フレームとの差分でX方向の移動を判定
       // float deltaX = transform.position.x - lastPosition.x;

        //プレイヤーが敵より右にいるか左にいるかで反転
        if(playerTr != null)
        {
            float delaX = playerTr.position.x - transform.position.x;
            
            //プレイヤー右にいる
            if(delaX > 0.0001f)
            {
                spriteRenderer.flipX = false;
            }
            else if(delaX < -0.0001f)
            {
                spriteRenderer.flipX = true;
            }   
        }
        
    }
}
    
