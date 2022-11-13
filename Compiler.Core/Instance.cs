using Compiler.Models.Settings;

namespace Compiler.Core;

public class Instance
{
    private static Instance? _instance;
    private AppConfig? _appConfig;
    public static Instance App => _instance ??= new Instance();

    public AppConfig? Config
    {
        get => _appConfig ?? throw new Exception("Настройки не определены");
        set => _appConfig = value;
    }

    private Instance() { }
}