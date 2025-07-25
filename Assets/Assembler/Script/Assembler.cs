using System;
using UnityEngine;

public abstract class Assembler : MonoBehaviour
{
    public Action<bool, Assembler> OnAssembleurActivityEnd;
    public Action<bool> OnAssembleurActivityExit;
    public abstract void Activate();
    public abstract void UnActivate();

    [SerializeField] private Animator animator;

    private void Start()
    {
        OnAssembleurActivityEnd += HandleAssemblerActivityEnd;
        animator.GetComponent<CraftAnim>().OnAssembleurAnimExit = () => OnAssembleurActivityExit.Invoke(true);
    }

    private void HandleAssemblerActivityEnd(bool isSuccessful, Assembler assembler)
    {
        if (isSuccessful)
        {
            animator.SetTrigger("PlayAnim");
        }
    }
}
