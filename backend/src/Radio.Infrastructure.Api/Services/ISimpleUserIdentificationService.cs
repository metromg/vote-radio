using System;
using Microsoft.AspNetCore.Http;

namespace Radio.Infrastructure.Api.Services
{
    public interface ISimpleUserIdentificationService
    {
        Guid GetOrCreateUserId(HttpContext context);
    }
}
