using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        PlayerParametersEnum playerParametersInstance = (PlayerParametersEnum)SingletonManager.Instance;
        Dictionary<PlayerParameter, float> playerParameters = playerParametersInstance.GetAllParameters();

        float swimForce = playerParameters[PlayerParameter.SwimForce];
        float currentForce = playerParameters[PlayerParameter.CurrentForce];
        float moveSpeed = playerParameters[PlayerParameter.MoveSpeed];
        */

        float swimForce = playerData.swimForce;
        float currentForce = playerData.currentForce;
        float moveSpeed = playerData.moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) || (!GameManager.isPlaying && transform.position.x < -5))
        {
            float move = Input.GetAxis("Vertical");
            Vector2 origineVector = new Vector2(swimForce, move * moveSpeed);
            rb.velocity = origineVector.normalized * swimForce;
        }

        // 常に左に動く力を加える
        rb.AddForce(new Vector2(-currentForce, 0));

        // 移動の制限
        ClampPosition();
    }

    void ClampPosition()
    {
        // 画面左下のワールド座標をビューポイントから取得
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        // 画面右上のワールド座標をビューポイントから取得
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector2 pos = transform.position;

        // プレイヤーの位置が画面内に収まるように制限をかける
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }
}

