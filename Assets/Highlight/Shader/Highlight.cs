using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    //we assign all the renderers here through the inspector
    [SerializeField]
    private Renderer renderer;
    [SerializeField]
    private Color color = Color.white;

    [SerializeField] Color wrongColor = Color.red;

    //helper list to cache all the materials ofd this object
    private Material material;

    //Gets all the materials from each renderer
    private void Awake()
    {
        //A single child-object might have mutliple materials on it
        //that is why we need to all materials with "s"
        material = renderer.material;
    }

    public void ToggleHighlight(HighlightState state)
    {
        switch(state)
        {
            case HighlightState.Interactable :
                //We need to enable the EMISSION
                material.EnableKeyword("_EMISSION");
                //before we can set the color
                material.SetColor("_EmissionColor", color);
                break;

            case HighlightState.NotInteractable :
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", wrongColor);
                break;

            default:
                //we can just disable the EMISSION
                //if we don't use emission color anywhere else
                material.DisableKeyword("_EMISSION");
                break;
        }
    }

    public enum HighlightState
    {
        Disabled, Interactable, NotInteractable
    }
}
