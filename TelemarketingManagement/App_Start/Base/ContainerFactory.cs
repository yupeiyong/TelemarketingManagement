using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;


namespace TelemarketingManagement.Base
{
    public class ContainerFactory
    {
        private static readonly IContainer AutofacContainer;


        static ContainerFactory()
        {
            var builder = new ContainerBuilder();

            var assemblyNames = new[] { "Common", "Data", "Service", "TelemarketingManagement" };
            var assemblies = assemblyNames.Select(a => Assembly.Load(a)).ToList();
            //var types = AppDomain.CurrentDomain
            //    .GetAssemblies()
            //    .Where(a => assemblyNames.Any(n => a.FullName.StartsWith(n)))
            //    .SelectMany(a => a.GetTypes().Where(t => t.IsClass && t.IsPublic))
            //    .ToArray();

            var types = assemblies
                .SelectMany(a => a.GetTypes().Where(t => t.IsClass && t.IsPublic))
                .ToArray();

            builder.RegisterTypes(types).AsSelf().InstancePerDependency().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            //通过容器配置生成容器. 
            AutofacContainer = builder.Build();

            //提供给MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(AutofacContainer));
        }


        public static IContainer GetContainer()
        {
            return AutofacContainer;
        }
    }
}