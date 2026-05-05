using UnityEngine;

public class OrcAnimationOpt : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        
        if (_animator != null)
        {
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
            _animator.Play(state.fullPathHash, 0, Random.value);
            _animator.speed = Random.Range(0.9f, 1.1f);
        }
        
        Destroy(this);
    }
}