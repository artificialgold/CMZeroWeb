using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CMZeroWeb.Controllers
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<HomeController>().ImplementedBy<HomeController>().LifeStyle.Transient);
            container.Register(Component.For<StyleGuideController>().ImplementedBy<StyleGuideController>().LifeStyle.Transient);
        }
    }
}