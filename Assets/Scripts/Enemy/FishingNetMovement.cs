using UnityEngine;

public class FishingNetMovement : MonoBehaviour
{
    private Vector2 initialPosition;
    [SerializeField] float movementRange = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialPosition + GetRandomOffset();
    }

    // �����_���ȃI�t�Z�b�g�𐶐����郁�\�b�h
    Vector2 GetRandomOffset()
    {
        float offsetX = Random.Range(-movementRange, movementRange);
        float offsetY = Random.Range(-movementRange, movementRange);
        return new Vector2(offsetX, offsetY);
    }
}
