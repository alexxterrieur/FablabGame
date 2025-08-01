using System;
using UnityEngine;

public abstract class Assembler : MonoBehaviour
{
    public Action<bool, Assembler> OnAssembleurActivityEnd;
    public Action<bool> OnAssembleurActivityExit;

    public PlayerInteraction playerInteraction;

    public abstract void Activate();
    public abstract void UnActivate();

    [SerializeField] private Animator animator;

    protected void Start()
    {
        OnAssembleurActivityEnd += HandleAssemblerActivityEnd;
        animator.GetComponent<CraftAnim>().OnAssembleurAnimExit = () => OnAssembleurActivityExit.Invoke(true);
        OnAssembleurActivityExit += playerInteraction.BreakPiece;
    }

    private void HandleAssemblerActivityEnd(bool isSuccessful, Assembler assembler)
    {
        if (isSuccessful)
        {
            animator.SetTrigger("PlayAnim");
        }
    }
}
