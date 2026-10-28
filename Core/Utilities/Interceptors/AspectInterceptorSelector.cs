using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();
            var paramTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
            var targetMethod = type.GetMethod(method.Name, paramTypes)
                               ?? type.GetMethods().FirstOrDefault(m => m.Name == method.Name);

            if (targetMethod != null)
            {
                var methodAttributes = targetMethod.GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
                classAttributes.AddRange(methodAttributes);
            }

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
