using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Converts a Texture2D to a Sprite with center pivot.
    /// </summary>
    public static Sprite ConvertToSprite(Texture2D texture)
    {
        if (texture == null) return null;

        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
    }
}
