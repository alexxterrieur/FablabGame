using UnityEngine;

public class CraftAnim : MonoBehaviour
{
    [SerializeField] ParticleSystem craftParticle;
    [SerializeField] Animator playerAnimator; //play show item
    [SerializeField] CameraCraftMovement cameraMovement; //play movement

    public void PlayParticleSystem()
    {
        craftParticle.Play();
        cameraMovement.MoveToCraftView();
        //bloquer movement joeuur
    }

    public void EndCrraft()
    {
        //player anim, regarde la cam et montre lobjet
        //equip object
        //rétablir mouvements joueur
    }

    public void MoveBackCamera()
    {
        cameraMovement.ReturnToOriginalView();
    }
}