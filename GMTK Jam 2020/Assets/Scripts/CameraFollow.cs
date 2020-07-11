using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform playerTransform;
    float cameraBoundX = 10f;
    float cameraBoundY = 7f;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        // Get the current camera position
        Vector3 cameraPosition = transform.position;

        // Set position
        if (cameraPosition.x > playerTransform.position.x + cameraBoundX) {
            cameraPosition.x = playerTransform.position.x + cameraBoundX;
        } else if (cameraPosition.x < playerTransform.position.x - cameraBoundX) {
            cameraPosition.x = playerTransform.position.x - cameraBoundX;
        }
        if (cameraPosition.y > playerTransform.position.y + cameraBoundY) {
            cameraPosition.y = playerTransform.position.y + cameraBoundY;
        }
        else if (cameraPosition.y < playerTransform.position.y - cameraBoundY) {
            cameraPosition.y = playerTransform.position.y - cameraBoundY;
        }

        // Update camera position
        transform.position = cameraPosition;
    }
}
