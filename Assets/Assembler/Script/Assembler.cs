using System;
using UnityEngine;

public abstract class Assembler : MonoBehaviour
{
    public Action<bool> OnAssembleurActivityEnd;
    public abstract void Activate();
    public abstract void UnActivate();
}
