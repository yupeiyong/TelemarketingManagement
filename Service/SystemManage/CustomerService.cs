using Common;
using Data;
using DataTransferObjects;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.SystemManage
{
    public class CustomerService
    {
        private readonly string _modelDescription = typeof(Customer).GetDescription();

        public DataDbContext DataDbContext { get; set; }

        ~CustomerService()
        {
            if (DataDbContext != null)
                DataDbContext.Dispose();
        }

        public List<Customer> Search(CustomerSearchDto dto)
        {
            var dataSource = DataDbContext.Set<Customer>().AsQueryable();

            if (dto.Gender.HasValue)
                dataSource = dataSource.Where(c => c.Gender != null && c.Gender.Value == dto.Gender.Value);

            if (dto.CustomerCategoryId > 0)
                dataSource = dataSource.Where(c => c.CustomerCategory != null && c.CustomerCategory.Id == dto.CustomerCategoryId);

            dataSource = dataSource.WhereDateTime(nameof(Customer.CreatorTime), dto.StartCreatorTime, dto.EndCreatorTime);

            if(!string.IsNullOrWhiteSpace(dto.Keywords))
                dataSource = dataSource.Where(c => c.RealName!=null && c.RealName.Contains(dto.Keywords) || c.NickName != null && c.NickName.Contains(dto.Keywords)|| c.MobilePhoneNumber != null && c.MobilePhoneNumber.Contains(dto.Keywords));

            dataSource = dataSource.OrderByDescending(a => a.LastModifyTime);

            if (dto.IsGetTotalCount)
                dto.TotalCount = dataSource.Count();

            return dataSource.Skip(dto.StartIndex).Take(dto.PageSize).ToList();
        }


        public void Save(CustomerEditDto dto)
        {
            if (dto.UpdateId > 0)
                Update(dto);
            else
                Add(dto);
        }


        private void ValidateEditDto(CustomerEditDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RealName))
                throw new Exception("错误，客户真实姓名不能为空！");

            if (!string.IsNullOrWhiteSpace(dto.MobilePhoneNumber))
            {
                if (!Validator.IsMobile(dto.MobilePhoneNumber))
                    throw new Exception($"错误，{dto.MobilePhoneNumber} 不是正确的手机号！");
            }
        }


        private void Add(CustomerEditDto dto)
        {
            ValidateEditDto(dto);

            if (DataDbContext.Set<Customer>().Any(c => c.MobilePhoneNumber != null && c.MobilePhoneNumber == dto.MobilePhoneNumber && c.RealName == dto.RealName))
                throw new Exception($"错误，用户名:{dto.RealName},手机号:{dto.MobilePhoneNumber}的用户已经存在，请检查后重试！");

            var customer = dto.MapTo<Customer>();
            if (dto.CustomerCategoryId > 0)
            {
                var category = DataDbContext.Set<CustomerCategory>().FirstOrDefault(cc => cc.Id == dto.CustomerCategoryId);
                if (category == null)
                    throw new Exception($"错误，Id={dto.CustomerCategoryId} 的客户分类不存在！");

                customer.CustomerCategory = category;
            }
            customer.CreatorTime = DateTime.Now;
            customer.LastModifyTime = DateTime.Now;
            DataDbContext.Set<Customer>().Add(customer);
            DataDbContext.SaveChanges();
        }


        public void Update(CustomerEditDto dto)
        {
            ValidateEditDto(dto);

            var customer = DataDbContext.Set<Customer>().FirstOrDefault(c => c.Id == dto.UpdateId);
            if (customer == null)
                throw new Exception($"错误，Id={dto.UpdateId} 的客户不存在！");

            if (DataDbContext.Set<Customer>().Any(c => c.Id != customer.Id && c.MobilePhoneNumber != null && c.MobilePhoneNumber == dto.MobilePhoneNumber && c.RealName == dto.RealName))
                throw new Exception($"错误，用户名:{dto.RealName},手机号:{dto.MobilePhoneNumber}的用户已经存在，请检查后重试！");

            dto.MapTo<Customer>(customer);
            if (dto.CustomerCategoryId > 0)
            {
                var category = DataDbContext.Set<CustomerCategory>().FirstOrDefault(cc => cc.Id == dto.CustomerCategoryId);
                if (category == null)
                    throw new Exception($"错误，Id={dto.CustomerCategoryId} 的客户分类不存在！");

                customer.CustomerCategory = category;
            }
            customer.LastModifyTime = DateTime.Now;
            DataDbContext.SaveChanges();
        }



        public void Remove(params long[] ids)
        {
            if (ids == null || ids.Length == 0)
                throw new Exception("错误，删除的序号为空！");
            foreach (var id in ids)
            {
                var data = DataDbContext.Set<Customer>().FirstOrDefault(b => b.Id == id);
                if (data == null)
                    throw new Exception($"错误，{_modelDescription}不存在！(Id:{id})");

                DataDbContext.Set<Customer>().Remove(data);
            }
            DataDbContext.SaveChanges();
        }


        public Customer GetDataById(long id)
        {
            return DataDbContext.Set<Customer>().FirstOrDefault(b => b.Id == id);
        }
    }
}
