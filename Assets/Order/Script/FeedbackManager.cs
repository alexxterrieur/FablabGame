using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [System.Serializable]
    public class ShelfGroup
    {
        public SO_CollectableItem item;
        public List<Shelf> shelves;
    }

    [System.Serializable]
    public class AssemblerGroup
    {
        public Materials material;
        public AssemblerInteraction assembler;
    }

    [Header("Manual References")]
    public List<ShelfGroup> shelfGroups = new();
    public List<AssemblerGroup> assemblerGroups = new();

    // === SHELVES ===

    public void ActivateShelfFeedback(SO_CollectableItem item, bool isActive)
    {
        ShelfGroup group = shelfGroups.Find(g => g.item == item);
        if (group == null) return;

        foreach (var shelf in group.shelves)
        {
            shelf.ToggleFeedback(isActive);
        }
    }

    public void UpdateShelfFeedbackColor(SO_CollectableItem item, Highlight.HighlightState state)
    {
        ShelfGroup group = shelfGroups.Find(g => g.item == item);
        if (group == null) return;

        foreach (var shelf in group.shelves)
        {
            shelf.UpdateFeedbackColor(state);
        }
    }

    // === ASSEMBLERS ===

    public void ActivateAssemblerFeedback(Materials material, bool isActive)
    {
        AssemblerInteraction assembler = GetAssembler(material);
        if (assembler != null)
        {
            assembler.ToggleFeedback(isActive);
        }
    }

    public void UpdateAssemblerFeedbackColor(Materials material, Highlight.HighlightState state)
    {
        AssemblerInteraction assembler = GetAssembler(material);
        if (assembler != null)
        {
            assembler.UpdateFeedbackColor(state);
        }
    }

    public AssemblerInteraction GetAssembler(Materials material)
    {
        var group = assemblerGroups.Find(g => g.material == material);
        return group != null ? group.assembler : null;
    }
}
