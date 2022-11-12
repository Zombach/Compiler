using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;

namespace Compiler;

public class Program
{
    public static void Main(string[] args)
    {
        string source = "using System;\r\nnamespace Test;\r\n\r\npublic class Program\r\n{\r\n    public static void Main(string[] args) => Console.WriteLine(\"Тест\");\r\n    \r\n}";
        SourceText sourceText = SourceText.From(source);
        CSharpParseOptions option = new(LanguageVersion.CSharp11, preprocessorSymbols: new List<string> { "Debug" });
        SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceText, option);
        Debug.Assert(tree.GetDiagnostics().ToList().Count == 0);

        CSharpCompilationOptions compileOption = new(OutputKind.ConsoleApplication, optimizationLevel: OptimizationLevel.Debug, allowUnsafe: true);
        CSharpCompilation compilation = CSharpCompilation.Create("Test", options: compileOption);
        compilation = compilation.AddSyntaxTrees(tree);

        IEnumerable<MetadataReference> references = GetGlobalReferences();
        AddReference(ref references, "System.Console.dll");
        AddReference(ref references, "System.Runtime.dll");
        compilation.SyntaxTrees.Select(node => node.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>())
        .SelectMany(s => s).ToList()
        .ForEach(usingDirectiveSyntax => AddReference(ref references, $"{usingDirectiveSyntax.Name}.dll"));
        compilation = compilation.AddReferences(references.ToList());
        Debug.Assert(tree.GetDiagnostics().ToList().Count == 0);
        
        BuildInDirectory(ref compilation);
        BuildInMemory(ref compilation);
    }

    private static void BuildInDirectory(ref CSharpCompilation compilation)
    {
        string assemblyPath = @"..\Test.dll";
        string pdbPath = @"..\Test.pdb";
        EmitResult result = compilation.Emit(assemblyPath, pdbPath);
        Console.WriteLine($"Статус: {result.Success}");
    }

    private static void BuildInMemory(ref CSharpCompilation compilation)
    {
        using MemoryStream ms = new();
        EmitResult result = compilation.Emit(ms);

        if (!result.Success)
        {
            result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error)
            .ToList().ForEach(diagnostic => Console.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()}, {diagnostic.Location}"));
        }
        else
        {
            ms.Seek(0, SeekOrigin.Begin);
            AssemblyLoadContext context = AssemblyLoadContext.Default;
            Assembly assembly = context.LoadFromStream(ms);
            assembly.EntryPoint.Invoke(null, new object[] { new string[] { } });
        }
    }

    private static IEnumerable<MetadataReference> GetGlobalReferences()
    {
        string assemblyDirectory = typeof(int).Assembly.Location;
        List<MetadataReference> referencer = new()
        {
            MetadataReference.CreateFromFile(assemblyDirectory)
        };
        return referencer;
    }

    private static void AddReference(ref IEnumerable<MetadataReference> references, string dllName)
    {
        string? assemblyDirectory = Path.GetDirectoryName(typeof(object).Assembly.Location);
        if (assemblyDirectory is not null && Directory.Exists(assemblyDirectory) && File.Exists($"{assemblyDirectory}\\{dllName}"))
        {
            references = references.Append(MetadataReference.CreateFromFile($"{assemblyDirectory}\\{dllName}"));
        }
    }
}