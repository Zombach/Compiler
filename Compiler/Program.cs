using Compiler.DLL;
using Microsoft.CodeAnalysis;

namespace Compiler;

public class Program
{
    public static void Main(string[] args)
    {
        string source = "using Test.Test2;\r\nnamespace Test;\r\n\r\npublic class Program   \r\n{\r\n   public static void Main(string[] args){ Pr pr = new();\r\n pr.Write(); }\r\n\r\n}";
        string source2 = "using System;\r\nnamespace Test.Test2;\r\n\r\npublic class Pr \r\n{\r\n   public void Write(){ string line = Console.ReadLine(); Console.WriteLine($\"Вы ввели: {line}\"); }\r\n\r\n}";

        CompilationCSharp compilationCSharp = new(OutputKind.ConsoleApplication);
        compilationCSharp.AddSyntaxTree(source);
        compilationCSharp.AddSyntaxTree(source2);
        compilationCSharp.AddGlobalReference();
        compilationCSharp.AddAdditionalReferences();
        //compilationCSharp.AddReference("System.Console.dll");
        compilationCSharp.Building("Memory");
    }
}