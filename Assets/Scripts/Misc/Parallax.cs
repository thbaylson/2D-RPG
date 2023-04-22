using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // The "speed" at which the parallax object will move
    [SerializeField] private float parallaxOffset = -0.15f;

    private Camera cam;
    private Vector2 startPos;
    // Distance between the camera's current position and the start position of this object
    private Vector2 travel => (Vector2)cam.transform.position - startPos;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        // Parallax equation that will move the object based on the following parameters
        transform.position = startPos + travel * parallaxOffset;
    }
}
