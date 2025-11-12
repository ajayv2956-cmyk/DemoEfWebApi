using System;
using System.Web.Http.Dependencies;
using DemoEfWebApi.Services;
using DemoEfWebApi.Services.Interfaces;

namespace DemoEfWebApi.App_Start
{
    public class SimpleResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IProductService))
                return new ProductService(() => new EFEntities());
            if (serviceType == typeof(IAuthService))
                return new AuthService(() => new EFEntities());

            // controllers
            if (serviceType == typeof(Controllers.ProductsController))
                return new Controllers.ProductsController((IProductService)GetService(typeof(IProductService)));
            if (serviceType == typeof(Controllers.AuthController))
                return new Controllers.AuthController((IAuthService)GetService(typeof(IAuthService)));

            return null;
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType) => new object[0];
        public IDependencyScope BeginScope() => this;
        public void Dispose() { }
    }
}
