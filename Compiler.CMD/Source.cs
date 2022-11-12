namespace Compiler.CMD;

public struct Source
{
    public const string Test =
    "using Test.Test2;"
    +
    "namespace Test;"
    +
    "public class Program" +
    "{" +
        "public static void Main(string[] args)" +
        "{" +
            "Writer writer = new();" +
            "writer.Write();" +
        "}" +
    "}";

    public const string Test2 =
    "using System;" +
    "using System.Text;"
    +
    "namespace Test.Test2;"
    +
    "public class Writer" +
    "{" +
        "public void Write()" +
        "{" +
            "Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);" +
            "Encoding enc1251 = Encoding.GetEncoding(1251);" +
            "System.Console.InputEncoding = enc1251;" +
            "System.Console.OutputEncoding = System.Text.Encoding.UTF8;" +
            "string line = Console.ReadLine();" +
            "Console.WriteLine($\"Вы ввели: {line}\");" +
        "}" +
    "}";

    public Source() { }
}