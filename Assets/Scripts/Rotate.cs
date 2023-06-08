using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private AudioSource sawSound;
    private void Update()
    {
        sawSound.Play();
        transform.Rotate(0, 0, 360 * speed * Time.deltaTime);
    }
}
