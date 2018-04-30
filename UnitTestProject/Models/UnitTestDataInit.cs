using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System.Data.Entity;
using Models;
using Models.Enum;
using System.Collections.Generic;
using System.Linq;
using Service.SystemManage;

namespace UnitTestProject.Models
{
    [TestClass]
    public class UnitTestDataInit:BaseUnitTest
    {
        [TestMethod]
        public void Test_Init()
        {
            RemoveAllData();
            UserInit();
            CustomerCategoryInit();
            CustomerInit();
        }

        [TestMethod]
        public void CustomerInit()
        {
            DataDbContext.Set<Customer>().RemoveRange(DataDbContext.Set<Customer>());
            DataDbContext.SaveChanges();
            var categories = DataDbContext.Set<CustomerCategory>().ToList();
            var customers = new List<Customer>();
            var rnd = new Random();
            for(var i = 0; i < 100; i++)
            {
                customers.Add(new Customer
                {
                    RealName = $"客户{i}",
                    Gender = i%2==2? Gender.Female:Gender.Male,
                    CustomerCategory=categories[rnd.Next(0,categories.Count-1)],
                    Birthday = new System.DateTime(1980, 12, 5),
                    Wechat = "wwwweweqw",
                    Qq = $"5678{i}",
                    NickName = "小李",
                    Address = "合肥四中",
                    MobilePhoneNumber = "13990182231"
                });
            }

            DataDbContext.Set<Customer>().AddRange(customers);
            DataDbContext.SaveChanges();
        }


        [TestMethod]
        public void CustomerCategoryInit()
        {
            DataDbContext.Set<Customer>().RemoveRange(DataDbContext.Set<Customer>());
            DataDbContext.SaveChanges();

            DataDbContext.Set<CustomerCategory>().RemoveRange(DataDbContext.Set<CustomerCategory>());
            DataDbContext.SaveChanges();
            var customers = new List<CustomerCategory>();
            for (var i = 0; i < 20; i++)
            {
                customers.Add(new CustomerCategory
                {
                    Name=$"客户分类{i}",
                    Description=$"客户分类描述{i}",
                    CustomOrder=i
                });
            }

            DataDbContext.Set<CustomerCategory>().AddRange(customers);
            DataDbContext.SaveChanges();
        }

        [TestMethod]
        public void UserInit()
        {
            DataDbContext.Set<User>().RemoveRange(DataDbContext.Set<User>());
            DataDbContext.SaveChanges();
            var users = new List<User>();
            var rnd = new Random();
            var defaultPassword =UserService.Encrypt("user");
            for (var i = 0; i < 100; i++)
            {
                users.Add(new User
                {
                    RealName = $"用户{i}",
                    Gender = i % 2 == 2 ? Gender.Female : Gender.Male,
                    Birthday = new System.DateTime(1980, 12, 5),
                    Wechat = "wwwweweqw",
                    Qq = $"5678{i}",
                    NickName = "小李",
                    UserState=UserStateEnum.Enable,
                    MobilePhoneNumber = "13990182231",
                    Password= defaultPassword
                });
            }

            DataDbContext.Set<User>().AddRange(users);
            DataDbContext.SaveChanges();
        }

        [TestMethod]
        public void AdminUserInit()
        {

        }


        [TestMethod]
        public void RemoveAllData()
        {
            try
            {
                if (!DataDbContext.Database.Exists())
                    return;
                if ((uint)DataDbContext.Database.Connection.State > 0U)
                    DataDbContext.Database.Connection.Close();
                Database.Delete(DataDbContext.Database.Connection);
                DataDbContext.Database.Initialize(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
