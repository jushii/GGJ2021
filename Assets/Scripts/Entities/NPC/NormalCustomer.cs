using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalCustomer : NPC
{
    private Animator _animator;
    private AnimatorControllerParameter tmpParameter;
    private List<int> _cachedParameterIds = new List<int>();

    public bool stunned;
    public Animator Animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }

    public override void Start()
    {
        base.Start();

        _animator = GetComponent<Animator>();
        CacheAnimatorParameterIds();

        stunned = false;
    }

    public override void OnPunch()
    {
       aiManager.ChangeState(this, typeof(NormalCustomer_Fly));
    }

    private void CacheAnimatorParameterIds()
    {
        for (int i = 0; i < _animator.parameters.Length; i++)
        {
            tmpParameter = _animator.parameters[i];
            if (tmpParameter.type == AnimatorControllerParameterType.Trigger)
            {
                _cachedParameterIds.Add(Animator.StringToHash(tmpParameter.name));
            }
        }
    }
    public void ResetAnimatorTriggers()
    {
        foreach (int id in _cachedParameterIds)
        {
            _animator.ResetTrigger(id);
        }
    }
}