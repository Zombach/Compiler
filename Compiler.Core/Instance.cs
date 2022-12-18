using Compiler.Core.Configs;

namespace Compiler.Core;

public class Instance
{
    private static Instance? _instance;
    private AppConfig? _appConfig;
    public static Instance App => _instance ??= new();

    public AppConfig? Config
    {
        get => _appConfig ?? throw new Exception("Настройки не определены");
        set => _appConfig = value;
    }

    private Instance() { }
}