using AutoMapper;
using Compiler.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Compiler.API.Controllers;

public class TestController : BaseController<ITestService>
{
    public TestController(ITestService testService, IMapper mapper) : base(testService, mapper) { }

    [HttpGet("test-connect")]
    [Description("Тестовый endpoint для подключения с клиента")]
    public string ConnectUnity() => "Есть коннект"[..5];
}