using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] float amplitude = 1.0f;  // U•
    [SerializeField] float frequency = 1.0f;  // ü”g”
    private Vector2 direction = Vector3.up;  // U“®‚Ì•ûŒü
    private Vector2 initialPosition;

    void Start()
    {
        initialPosition = transform.position;  // ‰ŠúˆÊ’u‚ğ•Û‘¶
    }

    void Update()
    {
        // ’PU“®‚ÌŒvZ
        float oscillation = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.time);
        transform.position = initialPosition + direction.normalized * oscillation;
    }
}
