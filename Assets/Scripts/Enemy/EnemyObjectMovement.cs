using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectMovement : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    [SerializeField] bool loop = false;
    private float warpThreshold;
    private Vector2 warpPosition = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        warpThreshold = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x - 10;
        warpPosition.x = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x + 10;
        warpPosition.y = transform.position.y;
        Debug.Log(warpThreshold);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if(transform.position.x < warpThreshold)
        {
            if(loop)
            {
                transform.position = warpPosition;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
