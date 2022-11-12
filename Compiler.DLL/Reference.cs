using Microsoft.CodeAnalysis;

namespace Compiler.DLL;

public class Reference
{
    private IEnumerable<MetadataReference> _references;
    private IEnumerable<string> _ignoreDll;
    public IEnumerable<MetadataReference> References
    {
        get => _references;
        private set => _references = value is null || value == default ? new List<MetadataReference>() : value;
    }

    public Reference()
    {
        _references = new List<MetadataReference>();
        _ignoreDll = GetIgnoreDll();
    }

    public void AddGlobalReferences()
    {
        if (References.Any()) { References = default!; }
        string? assemblyDirectory = Path.GetDirectoryName(typeof(object).Assembly.Location);
        if (assemblyDirectory is null) { return; }

        IEnumerable<string> directories = Directory.EnumerateFiles(assemblyDirectory, "*.dll");
        foreach (string path in directories)
        {
            FileInfo info = new(path);
            if (_ignoreDll.Contains(info.Name)) { continue; }
            PortableExecutableReference executableReference = MetadataReference.CreateFromFile(path);
            References = References.Append(executableReference);
        }
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

    private IEnumerable<string> GetIgnoreDll()
    => new List<string>
    {
        "System.IO.Compression.Native.dll",
        "msquic.dll",
        "mscorrc.dll",
        "mscordbi.dll",
        "mscordaccore_amd64_amd64_7.0.22.51805.dll",
        "mscordaccore.dll",
        "Microsoft.DiaSymReader.Native.amd64.dll",
        "hostpolicy.dll",
        "coreclr.dll",
        "clrjit.dll",
        "clrgc.dll",
        "clretwrc.dll",
    };
}