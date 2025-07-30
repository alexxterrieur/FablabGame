using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class ObjectCapture : MonoBehaviour
{
    [Header("Capture Settings")]
    public Camera captureCamera;              // Une caméra dédiée, non activée dans l'inspecteur
    public RenderTexture renderTexture;       // Une RenderTexture (ex: 512x512)

    [Header("Memory")]
    public Texture2D capturedTexture;  // Liste des captures
    public List<Texture2D> allCreatedTextures;

    /// <summary>
    /// Capture l'objet visible par la caméra et stocke le résultat dans la liste.
    /// </summary>
    public Texture2D CaptureObjectImage()
    {
        // Affecte la RenderTexture à la caméra
        captureCamera.targetTexture = renderTexture;
        captureCamera.Render();

        // Copie le rendu dans une Texture2D
        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        // Nettoyage
        RenderTexture.active = null;
        captureCamera.targetTexture = null;

        // Stocke la texture dans la liste
        capturedTexture = tex;

        return tex;
    }

    private void OnDestroy()
    {
        ClearCapturedTextures();
    }

    /// <summary>
    /// Supprime toutes les textures enregistrées (libération mémoire).
    /// </summary>
    public void ClearCapturedTextures()
    {
        foreach (var tex in allCreatedTextures)
        {
            Destroy(tex);
        }
        allCreatedTextures.Clear();
    }
}

