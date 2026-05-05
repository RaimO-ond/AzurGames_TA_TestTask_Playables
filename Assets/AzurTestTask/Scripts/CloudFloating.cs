using UnityEngine;

public class CloudFloating : MonoBehaviour
{
    public float speed = 0.5f;
    public float range = 2f;
    public bool horizontal = true;
    
    private Vector3 _startPos;
    private float _offset;

    void Start()
    {
        _startPos = transform.localPosition;
        _offset = Random.value * 10f;
    }

    void Update()
    {
        float move = Mathf.Sin(Time.time * speed + _offset) * range;
        
        if (horizontal)
            transform.localPosition = _startPos + new Vector3(move, 0, 0);
        else
            transform.localPosition = _startPos + new Vector3(0, move, 0);
    }
}