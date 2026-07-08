using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ゲーム開始時にサウンド再生
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
