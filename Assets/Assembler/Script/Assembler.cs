using System;
using UnityEngine;

public abstract class Assembler : MonoBehaviour
{
    public Action<bool> OnAssembleurActivityEnd;
    public abstract void Activate();
    public abstract void UnActivate();

    [SerializeField] private Animator animator;

    private void Start()
    {
        OnAssembleurActivityEnd += HandleAssemblerActivityEnd;
    }

    private void HandleAssemblerActivityEnd(bool isSuccessful)
    {
        if (isSuccessful)
        {
            animator.SetTrigger("PlayAnim");
        }
    }
}
