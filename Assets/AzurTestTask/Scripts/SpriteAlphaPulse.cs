using UnityEngine;

public class SpriteAlphaPulse : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_spriteRenderer != null)
        {
            float alpha = Mathf.PingPong(Time.time, 1f);
            
            Color newColor = _spriteRenderer.color;
            newColor.a = alpha;
            _spriteRenderer.color = newColor;
        }
    }
}