using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Users.Application.Tests;

public class DummyHttpContextAccessor : IHttpContextAccessor
{
    public HttpContext? HttpContext { get; set; }
    
    public DummyHttpContextAccessor()
    {
        var services = new ServiceCollection().BuildServiceProvider();
        HttpContext = new DefaultHttpContext
        {
            RequestServices = services
        };
    }
}