using Autofac;
using Autofac.Integration.WebApi;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using UserCenter.IServices;

namespace UserCenter.OpenAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitAutoFac();
            GlobalConfiguration.Configure(WebApiConfig.Register);            
        }

        //http://www.cnblogs.com/yinrq/p/5383396.html
        //Install-Package Autofac.WebApi2
        //不要错误的安装Autofac.Mvc5
        //也不要错误的安装Autofac.WebApi，因为Autofac.WebApi是给webapi1的，否则会报错
        //重写成员“Autofac.Integration.WebApi.AutofacWebApiDependencyResolver.BeginScope()”时违反了继承安全性规则。重写方法的安全可访问性必须与所重写方法的安全可访问性匹配

        //using Autofac.Integration.Mvc;
        //不要先安装最新版Autofac，因此Install-Package Autofac.WebApi2自动安装的Autofac就行
        private void InitAutoFac()
        {
            var configuration = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();
            
            // Register API controllers using assembly scanning. 
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterWebApiFilterProvider(configuration);
            builder.RegisterType(typeof(AuthorizationFilter));

            var services = Assembly.Load("UserCenter.Services");
            builder.RegisterAssemblyTypes(services)
                .Where(type => !type.IsAbstract && typeof(IServiceTag).IsAssignableFrom(type))
                .AsImplementedInterfaces().SingleInstance();

            var container = builder.Build();
            // Set the WebApi dependency resolver.  
            var resolver = new AutofacWebApiDependencyResolver(container);
            configuration.DependencyResolver = resolver;
        }
    }
}
