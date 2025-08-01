using System;
using UnityEngine;

public class CraftAnim : MonoBehaviour
{
    [SerializeField] ParticleSystem craftParticle;
    [SerializeField] Animator playerAnimator; //play show item
    [SerializeField] CameraCraftMovement cameraMovement; //play movement
    public Action OnAssembleurAnimExit;
    public void PlayParticleSystem()
    {
        craftParticle.Play();
        cameraMovement.ZoomToPlayer();
        //bloquer movement joeuur
    }

    public void EndCrraft()
    {
        //player anim, regarde la cam et montre lobjet
        //equip object
        OnAssembleurAnimExit.Invoke();
        //rétablir mouvements joueur
    }

    public void MoveBackCamera()
    {
        cameraMovement.ZoomOut();
    }
}