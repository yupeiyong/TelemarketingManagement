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


        public List<Customer> Search(CustomerSearchDto dto)
        {
            var dataSource = DataDbContext.Set<Customer>().AsQueryable();

            //if (dto.BudgetUnitIds.Count > 0)
            //{
            //    var units = DataDbContext.Set<BudgetUnit>().Where(bu => dto.BudgetUnitIds.Any(i => i == bu.Id));
            //    dataSource = dataSource.Where(m => m.BudgetUnit != null && units.Any(unit => unit.Id == m.BudgetUnit.Id || m.BudgetUnit.Level > unit.Level && m.BudgetUnit.CustomedNumber.StartsWith(unit.CustomedNumber)));
            //}

            //if (dto.MoneyNatureIds.Count > 0)
            //    dataSource = dataSource.Where(m => m.MoneyNature != null && dto.MoneyNatureIds.Any(i => i == m.MoneyNature.Id));

            //if (dto.FunctionSubjectIds.Count > 0)
            //{
            //    var subjects = DataDbContext.Set<FunctionSubject>().Where(fs => dto.FunctionSubjectIds.Any(i => i == fs.Id));
            //    dataSource = dataSource.Where(m => m.FunctionSubject != null && subjects.Any(subject => subject.Id == m.FunctionSubject.Id || m.FunctionSubject.Level > subject.Level && m.FunctionSubject.CustomedNumber.StartsWith(subject.CustomedNumber)));
            //}

            //if (dto.FundingSourcesIds.Count > 0)
            //    dataSource = dataSource.Where(m => m.FundingSources != null && dto.FundingSourcesIds.Any(i => i == m.FundingSources.Id));

            //if (dto.BudgetSourceIds.Count > 0)
            //    dataSource = dataSource.Where(m => m.BudgetSource != null && dto.BudgetSourceIds.Any(i => i == m.BudgetSource.Id));

            //if (dto.TargetManageDepartmentIds.Count > 0)
            //    dataSource = dataSource.Where(m => m.TargetManageDepartment != null && dto.TargetManageDepartmentIds.Any(i => i == m.TargetManageDepartment.Id));

            //if (!string.IsNullOrWhiteSpace(dto.Keywords))
            //    dataSource = dataSource.Where(m => m.DocumentNumber != null && m.DocumentNumber.Contains(dto.Keywords) || m.HigherSpecialFundDocumentNumber != null && m.HigherSpecialFundDocumentNumber.Contains(dto.Keywords) || m.Summary != null && m.Summary.Contains(dto.Keywords));

            //if (dto.StartDocumentNumberIndex.HasValue && dto.EndDocumentNumberIndex.HasValue)
            //    dataSource = dataSource.Where(m => m.DocumentNumberIndex >= dto.StartDocumentNumberIndex.Value && m.DocumentNumberIndex <= dto.EndDocumentNumberIndex.Value);
            //else if (dto.StartDocumentNumberIndex.HasValue)
            //    dataSource = dataSource.Where(m => m.DocumentNumberIndex >= dto.StartDocumentNumberIndex.Value);
            //else if (dto.EndDocumentNumberIndex.HasValue)
            //    dataSource = dataSource.Where(m => m.DocumentNumberIndex <= dto.EndDocumentNumberIndex.Value);

            dataSource = dataSource.WhereDateTime(nameof(Customer.LastModifyTime), dto.StartLastModifyTime, dto.EndLastModifyTime);

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
            if(string.IsNullOrWhiteSpace(dto.RealName))
                throw new Exception("错误，客户真实姓名不能为空！");

            if (!string.IsNullOrWhiteSpace(dto.MobilePhoneNumber))
            {
                if(!Validator.IsMobile(dto.MobilePhoneNumber))
                    throw new Exception($"错误，{dto.MobilePhoneNumber} 不是正确的手机号！");
            }
        }


        private void Add(CustomerEditDto dto)
        {
            ValidateEditDto(dto);

            if(DataDbContext.Set<Customer>().Any(c=>c.MobilePhoneNumber!=null && c.MobilePhoneNumber==dto.MobilePhoneNumber && c.RealName==dto.RealName))
                throw new Exception($"错误，用户名:{dto.RealName},手机号:{dto.MobilePhoneNumber}的用户已经存在，请检查后重试！");

            var customer = dto.MapTo<Customer>();
            if (dto.CustomerCategoryId > 0)
            {
                var category = DataDbContext.Set<CustomerCategory>().FirstOrDefault(cc=>cc.Id==dto.CustomerCategoryId);
                if(category == null)
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
            if(customer==null)
                throw new Exception($"错误，Id={dto.UpdateId} 的客户不存在！");

            if (DataDbContext.Set<Customer>().Any(c =>c.Id!=customer.Id && c.MobilePhoneNumber != null && c.MobilePhoneNumber == dto.MobilePhoneNumber && c.RealName == dto.RealName))
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
