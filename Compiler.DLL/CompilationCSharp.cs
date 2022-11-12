using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace Compiler.DLL;

public class CompilationCSharp
{
    private CSharpCompilation _cSharpCompilation;
    private readonly Tree _tree;
    private readonly Reference _reference;
    private readonly Builder _builder;

    public CompilationCSharp(OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary, LanguageVersion languageVersion = LanguageVersion.CSharp11, OptimizationLevel optimizationLevel = OptimizationLevel.Debug, bool allowUnsafe = true)
    {
        CSharpCompilationOptions compileOption = new(outputKind, optimizationLevel: optimizationLevel, allowUnsafe: allowUnsafe);
        _cSharpCompilation = CSharpCompilation.Create("Test", options: compileOption);
        _tree = new Tree(languageVersion, new List<string> { $"{optimizationLevel}" });
        _reference = new();
        _builder = new();
    }

    public void AddSyntaxTree(string source)
    {
        SyntaxTree syntaxTree = _tree.CreateSyntaxTree(source);
        _cSharpCompilation = _cSharpCompilation.AddSyntaxTrees(syntaxTree);
    }

    public void AddGlobalReference() => _reference.AddGlobalReferences();

    public void AddAdditionalReferences()
    {
        _cSharpCompilation.SyntaxTrees
        .Select(syntaxTree => syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>())
        .SelectMany(usingDirectiveSyntax => usingDirectiveSyntax)
        .ToList().ForEach(usingDirectiveSyntax => _reference.AddReference($"{usingDirectiveSyntax.Name}.dll"));
        CheckTrees();
    }

    public void AddReference(string reference) => _reference.AddReference(reference);

    public void Building(string name)
    {
        _cSharpCompilation = _cSharpCompilation.AddReferences(_reference.References);
        if (name is "Memory") { _builder.BuildInMemory(ref _cSharpCompilation); }
        else { _builder.BuildInDirectory(ref _cSharpCompilation); }
    }

    private void CheckTrees()
    => _cSharpCompilation.SyntaxTrees.Select(syntaxTree => syntaxTree)
    .ToList()
    .ForEach(syntaxTree => Debug.Assert(syntaxTree.GetDiagnostics().ToList().Count == 0));
}