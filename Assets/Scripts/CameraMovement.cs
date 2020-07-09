using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 3f;

    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = Settings.musicVolume;
        audioSource.enabled = Settings.enableMusicAndSounds;        
    }

    private void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * speed);
    }
}
