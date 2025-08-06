using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Form", menuName = "Form/ New Form")]
public class FormData : ScriptableObject
{
    public Texture itemPreview;
    public List<FormPart> forms;
}

[Serializable]
public struct FormPart
{
    public Vector3 position;
    public float radius;
    public Texture2D form;
}