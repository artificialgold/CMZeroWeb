using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.Windsor;
using Castle.Windsor.Installer;

namespace CMZeroWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrapper.Init(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

    public interface IBootstrapperTask
    {
        void Execute();
    }
    public class Bootstrapper
    {
        public static readonly IWindsorContainer Container = new WindsorContainer();

        public static void Init(Assembly applicationAssembly)
        {
            InitContainer(applicationAssembly);
            InitTasks();
        }

        private static void InitContainer(Assembly applicationAssembly)
        {
            Container.Install(FromAssembly.Instance(applicationAssembly));

            DependencyResolver.SetResolver(new WindsorDependencyResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(Container);
        }

        private static void InitTasks()
        {
            var bootstrapperTasks = Container.ResolveAll<IBootstrapperTask>();

            foreach (IBootstrapperTask bootstrapperTask in bootstrapperTasks)
            {
                bootstrapperTask.Execute();
            }
        }
    }
    public class WindsorDependencyResolver : System.Web.Mvc.IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IWindsorContainer _container;

        public WindsorDependencyResolver(IWindsorContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (_container.Kernel.HasComponent(serviceType))
            {
                return _container.Resolve(serviceType);
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(_container, this);
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
    public class WindsorDependencyScope : IDependencyScope
    {
        private readonly IWindsorContainer _container;
        private readonly IDependencyScope _scope;
        private readonly List<object> _instances = new List<object>();

        public WindsorDependencyScope(IWindsorContainer container, IDependencyScope scope)
        {
            _container = container;
            _scope = scope;
        }

        public object GetService(Type serviceType)
        {
            object service = _scope.GetService(serviceType);

            AddToScope(service);

            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var services = _scope.GetServices(serviceType);

            AddToScope(services);

            return services;
        }

        public void Dispose()
        {
            foreach (object instance in _instances)
            {
                _container.Release(instance);
            }

            _instances.Clear();
        }

        private void AddToScope(params object[] services)
        {
            if (services.Any())
            {
                _instances.AddRange(services);
            }
        }
    }
}