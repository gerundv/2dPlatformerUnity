using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    private float positionZ = -10f; 

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, positionZ);
    }
}
