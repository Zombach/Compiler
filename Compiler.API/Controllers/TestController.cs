using System.ComponentModel;
using AutoMapper;
using Compiler.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Compiler.API.Controllers;

public class TestController : BaseController<ITestService>
{
    public TestController(IMapper mapper, ITestService testService) : base(mapper, testService) { }

    [HttpGet("test-connect")]
    [Description("Тестовый endpoint для подключения с клиента")]
    public string ConnectUnity()
    {
        return "Есть коннект";
    }
}