using UnityEngine;


public class Bullethan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Bullethan: Start");
    }

    //Playerに当たったら自分を消す
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Bullethan: OnTriggerEnter2D");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame

}