using System;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;


namespace UnitTestProject.Models
{
    [TestClass]
    public class UnitTestCustomer
    {
        [TestMethod]
        public void Test_Add()
        {
            using (var dao = new DataDbContext()) 
            {
                var customer=new Customer
                {
                    AccountName = "John.lee"
                };
                dao.Set<Customer>().Add(customer);
                dao.SaveChanges();
            }
        }
    }
}
