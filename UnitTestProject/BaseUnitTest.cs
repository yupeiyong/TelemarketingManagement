using Autofac;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TelemarketingManagement.App_Start.Base;
using TelemarketingManagement.Base;


namespace UnitTestProject
{
    [TestClass]
    public class BaseUnitTest
    {
        protected  IContainer Container = ContainerFactory.GetContainer();

        public BaseUnitTest()
        {
            AutoMapperInitial.RegisterMapperType();
            DataDbContext = Container.Resolve<DataDbContext>();
        }
        ~BaseUnitTest()
        {
            if (DataDbContext != null)
            {
                DataDbContext.Dispose();
            }
        }
        protected TService GetService<TService>()
        {
            return Container.Resolve<TService>();
        }

        protected DataDbContext DataDbContext { get;private set; }
    }
}
