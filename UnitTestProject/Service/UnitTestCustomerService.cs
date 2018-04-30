

using DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Enum;
using Service;
using Service.SystemManage;
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

    }
}
