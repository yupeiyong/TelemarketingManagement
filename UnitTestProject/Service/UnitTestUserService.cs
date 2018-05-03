using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataTransferObjects;
using Service.SystemManage;

namespace UnitTestProject.Service
{
    [TestClass]
    public class UnitTestUserService:BaseUnitTest
    {
        [TestMethod]
        public void Test_Add()
        {
            var updateDto = new UserEditDto { AccountName="admin",Password="admin" };
            var service = GetService<UserService>();
            service.Add(updateDto);
        }

        [TestMethod]
        public void Test_Login()
        {
            var service = GetService<UserService>();
            var user=service.CheckLogin("admin", "admin");
        }
    }
}
