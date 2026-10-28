using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Constants;
using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(r => r.Trim())
                          .ToArray();
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var httpContext = _httpContextAccessor?.HttpContext;
            var roleClaims = httpContext?.User?.ClaimRoles();
            if (roleClaims != null)
            {
                foreach (var role in _roles)
                {
                    if (roleClaims.Any(c => string.Equals(c?.Trim(), role, StringComparison.OrdinalIgnoreCase)))
                    {
                        return;
                    }
                }
            }
            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}
