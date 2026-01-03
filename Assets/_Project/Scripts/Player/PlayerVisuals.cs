using System;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private Animator _animator;
    private static readonly int _isMovingParam = Animator.StringToHash("IsMoving");
    private Vector3 _initialLocalScale;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _initialLocalScale = transform.localScale;
    }

    public void SetIsMoving(bool isMoving)
    {
        _animator.SetBool(_isMovingParam, isMoving);
    }

    public void SetFacing(bool isRight)
    {
        transform.localScale = new Vector3(isRight ? _initialLocalScale.x : -_initialLocalScale.x, _initialLocalScale.y, _initialLocalScale.z);
    }
}
