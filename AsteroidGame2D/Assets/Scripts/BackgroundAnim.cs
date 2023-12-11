using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnim : MonoBehaviour
{
    [Range(0.1f, 3f)][SerializeField] private float speed;
    [SerializeField] private Vector3 movementSide;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private float _distance;
    void Start()
    {
        _endPos.x = GetComponent<SpriteRenderer>().sprite.rect.width / 100.0f;
        _endPos.y = GetComponent<SpriteRenderer>().sprite.rect.height / 100.0f;
        _startPos = transform.position;
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x) < Mathf.Abs(_endPos.x) && Mathf.Abs(transform.position.y) < Mathf.Abs(_endPos.y))
        {
            transform.position += movementSide * (speed * Time.deltaTime);
        }
        else
        {
            transform.position = _startPos;
        }
    }
}
