using System;
using Microsoft.AspNetCore.Http;

namespace Radio.Infrastructure.Api.Services
{
    public interface IPrimitiveUserIdentificationService
    {
        Guid GetOrCreateUserId(HttpContext context);
    }
}
