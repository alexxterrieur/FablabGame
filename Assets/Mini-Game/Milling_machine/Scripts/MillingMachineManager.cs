using UnityEngine;
using UnityEngine.InputSystem;

public class MillingMachineManager : Assembler
{
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera millingCamera;

    [SerializeField] private GameObject miniGame;

    private void Awake()
    {
        millingCamera.enabled = false;
    }

    public override void Activate()
    {
        millingCamera.enabled = true;
        mainCamera.enabled = false;
        miniGame.SetActive(true);
    }

    public override void UnActivate()
    {
        millingCamera.enabled = false;
        mainCamera.enabled = true;
        miniGame.SetActive(false);
    }
}
