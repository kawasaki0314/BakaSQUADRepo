using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttack : MonoBehaviour
{

    public GameObject attack1Prefab;  // 剣の当たり判定
    public float attackOffset = 1.0f;   // 攻撃をどれだけ前に出すか

    public PlayerMove playerMove;
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //  左クリックを押したら攻撃
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    void Attack()
    {

        Vector2 dir = playerMove.GetLastDir();
       
        // 攻撃位置
        Vector3 spawnPos = transform.position + (Vector3) (dir * attackOffset);

        // y座標補正
        spawnPos.y += 0.2f;

        // 回転
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        // 攻撃生成
        GameObject attack = Instantiate(attack1Prefab, spawnPos, rot);

        Attack1 atk = attack.GetComponent<Attack1>();
        atk.SetPlayer(playerMove);
    }

}
