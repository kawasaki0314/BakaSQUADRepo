using UnityEngine;

public class chicken : MonoBehaviour
{
    private Vector2 pos;
    public int num = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        //マイナスをかけることで逆方向にいどうする
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        if(pos.x > -2)
        {
            num = -1;
        }
        if(pos.x < -11)
        {
            num = 11;
        }
    }
}
