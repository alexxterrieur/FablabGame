using System;
using System.Collections;
using UnityEngine;

public abstract class Assembler : MonoBehaviour
{
    public Action<bool, Assembler> OnAssembleurActivityEnd;
    public Action<bool> OnAssembleurActivityExit;

    public PlayerInteraction playerInteraction;

    public abstract void Activate();
    public abstract void UnActivate();

    [SerializeField] private Animator animator;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private GameObject failTextFeedback;

    protected void Start()
    {
        OnAssembleurActivityEnd += HandleAssemblerActivityEnd;
        animator.GetComponent<CraftAnim>().OnAssembleurAnimExit = () => OnAssembleurActivityExit.Invoke(true);
        OnAssembleurActivityExit += playerInteraction.BreakPiece;
        countdownTimer.onTimerFinished.AddListener(UnActivate);
    }

    private void HandleAssemblerActivityEnd(bool isSuccessful, Assembler assembler)
    {
        if (isSuccessful)
        {
            animator.SetTrigger("PlayAnim");
        }
        else
        {
            failTextFeedback.SetActive(true);
        }
    }
}
