using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineResult<T> : CustomYieldInstruction
{
    public T value;
    public bool hasValue = false;
    public override bool keepWaiting => !hasValue;

    private IEnumerator _target;

    public CoroutineResult(IEnumerator target_)
    {
        _target = target_;
        value = default(T);
        Coroutiner.Instance.StartCoroutine(Run());
    }

    public CoroutineResult(IEnumerator target_, MonoBehaviour owner)
    {
        _target = target_;
        value = default(T);
        owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (_target.MoveNext())
        {
            yield return _target.Current;
        }

        value = (T)_target.Current;
        hasValue = true;
    }
}
