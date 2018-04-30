using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TelemarketingManagement.App_Start.Base;

namespace UnitTestProject
{
    [TestClass]
    public class BaseUnitTest
    {
        public IContainer Container = ContainerFactory.GetContainer();

        public BaseUnitTest()
        {
            AutoMapperInitial.RegisterMapperType();
        }
        public TService GetService<TService>()
        {
            return Container.Resolve<TService>();
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
