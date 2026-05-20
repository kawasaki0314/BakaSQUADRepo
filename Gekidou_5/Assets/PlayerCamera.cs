using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public GameObject player;

    Vector3 prePlayerPos; //前フレームでのプレイヤーの座標位置
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     

    // Update is called once per frame

    void Update()

    {
      if(player.transform.position != prePlayerPos)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            prePlayerPos = player.transform.position;
        }
    }
}
