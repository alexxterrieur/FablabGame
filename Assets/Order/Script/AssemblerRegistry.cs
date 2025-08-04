using System.Collections.Generic;

public static class AssemblerRegistry
{
    private static Dictionary<Materials, AssemblerInteraction> assemblersByMaterial = new();

    public static void Register(Materials material, AssemblerInteraction assembler)
    {
        if (!assemblersByMaterial.ContainsKey(material))
            assemblersByMaterial[material] = assembler;
    }

    public static void Unregister(Materials material)
    {
        if (assemblersByMaterial.ContainsKey(material))
            assemblersByMaterial.Remove(material);
    }

    public static AssemblerInteraction GetAssembler(Materials material)
    {
        return assemblersByMaterial.TryGetValue(material, out var assembler) ? assembler : null;
    }

    public static void Clear()
    {
        assemblersByMaterial.Clear();
    }
}
