using AutoMapper;
using Compiler.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Compiler.API.Controllers;

public class TemplatesController : BaseController<ITemplatesService>
{
    public TemplatesController(ITemplatesService templatesController, IMapper mapper) : base(templatesController, mapper) { }

    [HttpGet("get-class")]
    [Description("Получение шаблона \"класс\"")]
    public string ConnectUnity() => "Есть коннект"[..5];
}