using System.Configuration;
using CMZero.API.ServiceAgent;
using CMZero.Web.Services;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Labels.Mappers;
using CMZero.Web.Services.Logging;
using CMZero.Web.Services.Login;
using CMZero.Web.Services.ViewModelGetters;
using CMZeroWeb.Controllers.Dashboard;
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
            container.Register(Component.For<LayoutController>().ImplementedBy<LayoutController>().LifeStyle.Transient);
            container.Register(Component.For<LoginController>().ImplementedBy<LoginController>().LifeStyle.Transient);
            container.Register(Component.For<DashboardController>().ImplementedBy<DashboardController>().LifeStyle.Transient);
            container.Register(Component.For<ApplicationController>().ImplementedBy<ApplicationController>().LifeStyle.Transient);
        }
    }

    public class ServicesWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ISystemSettings>().ImplementedBy<SystemSettings>().LifeStyle.Transient);
            container.Register(
                Component.For<ILabelCollectionRetriever>().ImplementedBy<LabelCollectionRetriever>().LifeStyle.Transient);
            container.Register(
                Component.For<IContentAreaMapper>().ImplementedBy<ContentAreaMapper>().LifeStyle.Transient);
            container.Register(
                Component.For<ILabelCollectionMapper>().ImplementedBy<LabelCollectionMapper>().LifeStyle.Transient);
            container.Register(Component.For<ILogger>().ImplementedBy<Logger>().LifeStyle.Transient);
            container.Register(
                Component.For<IFormsAuthenticationService>()
                         .ImplementedBy<FormsAuthenticationService>()
                         .LifeStyle.Transient);
            container.Register(
                Component.For<IDashboardViewModelGetter>().ImplementedBy<DashboardViewModelGetter>().LifeStyle.Transient);
            container.Register(Component.For<IApplicationService>().ImplementedBy<ApplicationService>().LifeStyle.Transient);
            container.Register(Component.For<IApplicationViewModelGetter>().ImplementedBy<ApplicationViewModelGetter>().LifeStyle.Transient);
            

            container.Register(
                Component.For<IContentAreasServiceAgent>().ImplementedBy<ContentAreasServiceAgent>()
                .DependsOn(Property.ForKey("baseUri").Eq(ConfigurationManager.AppSettings["BaseUri"])).LifestyleTransient());
            container.Register(
                            Component.For<IApplicationsServiceAgent>().ImplementedBy<ApplicationsServiceAgent>()
                            .DependsOn(Property.ForKey("baseUri").Eq(ConfigurationManager.AppSettings["BaseUri"])).LifestyleTransient());

        }
    }
}