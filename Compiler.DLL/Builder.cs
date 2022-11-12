using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Compiler.DLL;

public class Builder
{
    public void BuildInMemory(ref CSharpCompilation compilation)
    {
        using MemoryStream memoryStream = new();
        EmitResult result = compilation.Emit(memoryStream);

        if (!result.Success)
        {
            result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error)
            .ToList().ForEach(diagnostic => Console.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()}, {diagnostic.Location}"));
        }
        else
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            AssemblyLoadContext context = AssemblyLoadContext.Default;
            Assembly assembly = context.LoadFromStream(memoryStream);
            assembly.EntryPoint?.Invoke(null, new object[] { new string[] { } });
        }
    }

    public void BuildInDirectory(ref CSharpCompilation compilation)
    {
        string assemblyPath = @"..\Test.dll";
        string pdbPath = @"..\Test.pdb";
        EmitResult result = compilation.Emit(assemblyPath, pdbPath);
        Console.WriteLine($"Статус: {result.Success}");
    }
}