

using DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Enum;
using Service;
using Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace UnitTestProject.Service
{
    [TestClass]
    public class UnitTestCustomerService:BaseUnitTest
    {
        [TestMethod]
        public void Test_Add()
        {
            using(var tran=new TransactionScope())
            {
                var beforeCount = DataDbContext.Set<Customer>().Count();

                var customerEditDto = new CustomerEditDto
                {
                    RealName="李强",
                    Gender=Gender.Female,
                    Birthday=new System.DateTime(1980,12,5),
                    Wechat="wwwweweqw",
                    Qq="5678",
                    NickName="小李",
                    Address="合肥四中",
                    MobilePhoneNumber="13990182231"
                };
                var service = GetService<CustomerService>();
                service.Save(customerEditDto);

                var afterCount = DataDbContext.Set<Customer>().Count();
                Assert.IsTrue(beforeCount + 1 == afterCount);

            }
        }

        [TestMethod]
        public void Test_Update()
        {
            using (var tran = new TransactionScope())
            {

                var customerEditDto = new CustomerEditDto
                {
                    RealName = "李强",
                    Gender = Gender.Female,
                    Birthday = new System.DateTime(1980, 12, 5),
                    Wechat = "wwwweweqw",
                    Qq = "5678",
                    NickName = "小李",
                    Address = "合肥四中",
                    MobilePhoneNumber = "13990182231"
                };
                var service = GetService<CustomerService>();
                service.Save(customerEditDto);

                var customer = DataDbContext.Set<Customer>().OrderByDescending(c=>c.Id).FirstOrDefault();
                Assert.IsNotNull(customer);

                customerEditDto.UpdateId = customer.Id;
                customerEditDto.RealName = "修改名字";
                service.Save(customerEditDto);
                customer = DataDbContext.Set<Customer>().AsNoTracking().FirstOrDefault(c=>c.Id==customer.Id);
                Assert.IsNotNull(customer);
                Assert.IsTrue(customer.RealName == customerEditDto.RealName);
            }
        }


        [TestMethod]
        public void Test_Search_Category()
        {
            using (var tran = new TransactionScope())
            {
                var category = new CustomerCategory { Name = "客户分类N" };
                DataDbContext.Set<CustomerCategory>().Add(category);
                DataDbContext.SaveChanges();

                var customerEditDto = new CustomerEditDto
                {
                    RealName = "李强",
                    Gender = Gender.Female,
                    Birthday = new System.DateTime(1980, 12, 5),
                    Wechat = "wwwweweqw",
                    Qq = "5678",
                    NickName = "小李",
                    Address = "合肥四中",
                    MobilePhoneNumber = "13990182231",
                    CustomerCategoryId=category.Id
                };
                var service = GetService<CustomerService>();
                service.Save(customerEditDto);

                var customer = DataDbContext.Set<Customer>().OrderByDescending(c => c.Id).FirstOrDefault();
                Assert.IsNotNull(customer);

                var rows=service.Search(new CustomerSearchDto { StartIndex = 0, PageSize = 20, CustomerCategoryId = category.Id });
                Assert.IsTrue(rows.Count == 1);
            }

        }

        [TestMethod]
        public void Test_Search_Last_Modify_Time()
        {
            using (var tran = new TransactionScope())
            {
                var customers = new List<Customer>();
                customers.Add(new Customer
                {
                    RealName = $"客户",
                    Gender = Gender.Male,
                    Birthday = new System.DateTime(1980, 12, 5),
                    Wechat = "wwwweweqw",
                    Qq = $"5678",
                    NickName = "小李",
                    Address = "合肥四中",
                    MobilePhoneNumber = "13990182231",
                    LastModifyTime = DateTime.Now.AddDays(10)
                });

                customers.Add(new Customer
                {
                    RealName = $"客户",
                    Gender = Gender.Male,
                    Birthday = new System.DateTime(1980, 12, 5),
                    Wechat = "wwwweweqw",
                    Qq = $"5678",
                    NickName = "小李",
                    Address = "合肥四中",
                    MobilePhoneNumber = "13990182231",
                    LastModifyTime = DateTime.Now.AddDays(11)
                });
                customers.Add(new Customer
                {
                    RealName = $"客户",
                    Gender = Gender.Male,
                    Birthday = new System.DateTime(1980, 12, 5),
                    Wechat = "wwwweweqw",
                    Qq = $"5678",
                    NickName = "小李",
                    Address = "合肥四中",
                    MobilePhoneNumber = "13990182231",
                    LastModifyTime = DateTime.Now.AddDays(12)
                });

                DataDbContext.Set<Customer>().AddRange(customers);

                DataDbContext.SaveChanges();
                var service = GetService<CustomerService>();
                var rows = service.Search(new CustomerSearchDto { StartIndex = 0, PageSize = 20, StartCreatorTime= DateTime.Now.AddDays(10).Date,EndCreatorTime= DateTime.Now.AddDays(12).Date });
                Assert.IsTrue(rows.Count == 3);
            }

        }

    }
}
