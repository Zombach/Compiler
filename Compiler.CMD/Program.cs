using Compiler.DLL;
using Microsoft.CodeAnalysis;

namespace Compiler.CMD;

public class Program
{
    public static void Main(string[] args)
    {
        CompilationCSharp compilationCSharp = new(OutputKind.ConsoleApplication);
        compilationCSharp.AddSyntaxTree(Source.Test);
        compilationCSharp.AddSyntaxTree(Source.Test2);
        compilationCSharp.AddGlobalReference();
        compilationCSharp.AddAdditionalReferences();
        //compilationCSharp.AddReference("System.Console.dll");
        compilationCSharp.Building("Memory");
    }
}