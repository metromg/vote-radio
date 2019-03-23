using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Radio.Infrastructure.Api.Extensions;

namespace Radio.Infrastructure.Api.Pipeline
{
    public class UnitOfWorkControllerActivator : IControllerActivator
    {
        public object Create(ControllerContext context)
        {
            return context.HttpContext.GetRequestUnitOfWork().Dependent.Resolve(context.ActionDescriptor.ControllerTypeInfo);
        }

        public void Release(ControllerContext context, object controller)
        {
        }
    }
}
