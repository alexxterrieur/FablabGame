using System.Collections;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    //we assign all the renderers here through the inspector
    [SerializeField]
    private Renderer renderer;
    [SerializeField]
    public Color glowColor = Color.cyan;
    public Color noGlowColor = Color.black;
    public float glowSpeed = 2f;
    private Coroutine glowCoroutine;

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
        switch (state)
        {
            case HighlightState.Interactable:
                // Start glow animation
                if (glowCoroutine != null)
                    StopCoroutine(glowCoroutine);

                glowCoroutine = StartCoroutine(GlowPulseCoroutine());
                break;

            case HighlightState.NotInteractable:
                if (glowCoroutine != null)
                    StopCoroutine(glowCoroutine);

                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", wrongColor);
                break;

            default:
                if (glowCoroutine != null)
                    StopCoroutine(glowCoroutine);

                material.DisableKeyword("_EMISSION");
                break;
        }
    }

    private IEnumerator GlowPulseCoroutine()
    {
        material.EnableKeyword("_EMISSION");

        float t = 0f;
        while (true)
        {
            t += Time.deltaTime * glowSpeed;
            Color lerpedColor = Color.Lerp(noGlowColor, glowColor, (Mathf.Sin(t) + 1f) / 2f);
            material.SetColor("_EmissionColor", lerpedColor);
            yield return null;
        }
    }


    public enum HighlightState
    {
        Disabled, Interactable, NotInteractable
    }
}
