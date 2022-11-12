using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;

namespace Compiler.DLL;

public class Tree
{
    private readonly CSharpParseOptions _option;

    public Tree(IEnumerable<string> preprocessorSymbols, LanguageVersion languageVersion)
    {
        _option = new(languageVersion, preprocessorSymbols: preprocessorSymbols);
    }

    public SyntaxTree CreateSyntaxTree(string source)
    {
        SourceText sourceText = SourceText.From(source);
        SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceText, _option);
        Debug.Assert(tree.GetDiagnostics().ToList().Count == 0);
        return tree;
    }
}