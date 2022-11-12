using Microsoft.CodeAnalysis;

namespace Compiler.DLL;

public class Reference
{
    private IEnumerable<MetadataReference> _references;
    public IEnumerable<MetadataReference> References
    {
        get => _references;
        private set => _references = value is null || value == default ? new List<MetadataReference>() : value;
    }

    public Reference() => _references = new List<MetadataReference>();

    public void AddGlobalReferences()
    {
        if (References.Any()) { References = default!; }
        string assemblyDirectory = typeof(int).Assembly.Location;
        References = References.Append(MetadataReference.CreateFromFile(assemblyDirectory));
        AddReference("System.dll");
        AddReference("System.IO.dll");
        AddReference("System.Console.dll");
        AddReference("System.Runtime.dll");
    }

    public void AddReference(string dllName)
    {
        string? assemblyDirectory = Path.GetDirectoryName(typeof(object).Assembly.Location);
        if (assemblyDirectory is null || !Directory.Exists(assemblyDirectory)) { return; }

        string filePath = Path.Combine(assemblyDirectory, dllName);
        if (!File.Exists(filePath)) { return; }

        PortableExecutableReference executableReference = MetadataReference.CreateFromFile(filePath);
        if (!References.Contains(executableReference)) { References = References.Append(executableReference); }
    }
}