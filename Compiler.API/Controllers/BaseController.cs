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
    protected T Service { get; }
    protected IMapper Mapper { get; }

    protected BaseController(IGeneralService generalService, IMapper mapper)
    {
        Service = (T)generalService;
        Mapper = mapper;
    }
}