using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
