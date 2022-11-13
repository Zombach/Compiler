using AutoMapper;
using Compiler.Business.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Compiler.API.Controllers;

[ApiController]
[EnableCors("CorsApi")]
[Route("[controller]")]
public abstract class BaseController<T> : Controller where T : IGeneralService
{
    protected IMapper Mapper { get; }
    protected T Service { get; }

    protected BaseController(IMapper mapper, IGeneralService generalService)
    {
        Mapper = mapper;
        Service = (T)generalService;
    }
}