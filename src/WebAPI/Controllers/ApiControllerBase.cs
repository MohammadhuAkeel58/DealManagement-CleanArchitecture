using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}