using Common;
using Data;
using DataTransferObjects;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enum;
using NPOI.HSSF.Record.Chart;


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

            if (!string.IsNullOrWhiteSpace(dto.Keywords))
                dataSource = dataSource.Where(c => c.RealName != null && c.RealName.Contains(dto.Keywords) || c.NickName != null && c.NickName.Contains(dto.Keywords) || c.MobilePhoneNumber != null && c.MobilePhoneNumber.Contains(dto.Keywords));

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


        /// <summary>
        /// 导入客户资料
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns>返回导入条数</returns>
        public ImportedResultDto Import(List<CustomerImportDto> dtos)
        {
            var message = new StringBuilder();
            var count = 0;
            for (int i = 0, len = dtos.Count; i < len; i++)
            {
                var dto = dtos[i];


                if (string.IsNullOrWhiteSpace(dto.RealName))
                {
                    message.AppendLine($"第{i + 1}条导入资料，客户姓名为空！");
                    continue;
                }
                var customer = new Customer { RealName = dto.RealName };

                if (!string.IsNullOrWhiteSpace(dto.CustomerCategoryName))
                {
                    var category = DataDbContext.Set<CustomerCategory>().FirstOrDefault(cc => cc.Name != null && cc.Name == dto.CustomerCategoryName);
                    if (category == null)
                    {
                        message.AppendLine($"第{i + 1}条导入资料，{dto.CustomerCategoryName}的客户分类不存在！");
                        continue;
                    }
                    customer.CustomerCategory = category;
                }
                customer.NickName = dto.NickName;
                if (!string.IsNullOrWhiteSpace(dto.GenderDescription))
                {
                    var genderDescriptions = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToDictionary(g => g.GetEnumDescription(), v => v);
                    if (genderDescriptions.Keys.All(desc => desc != dto.GenderDescription))
                    {
                        message.AppendLine($"第{i + 1}条导入资料，{dto.GenderDescription}不是正确的性别，正确为{string.Join(",", genderDescriptions)}！");
                        continue;
                    }
                    var gender = genderDescriptions[dto.GenderDescription];
                    customer.Gender = gender;
                }

                if (!string.IsNullOrWhiteSpace(dto.MobilePhoneNumber))
                {
                    if (!Validator.IsMobile(dto.MobilePhoneNumber))
                    {
                        message.AppendLine($"第{i + 1}条导入资料，{dto.MobilePhoneNumber} 不是正确的手机号！");
                        continue;
                    }
                    customer.MobilePhoneNumber = dto.MobilePhoneNumber;
                }

                customer.Qq = dto.Qq;
                customer.Wechat = dto.Wechat;
                customer.Address = dto.Address;

                if (!string.IsNullOrWhiteSpace(dto.BirthdayDescription))
                {
                    DateTime birthday;
                    if (!DateTime.TryParse(dto.BirthdayDescription, out birthday))
                    {
                        message.AppendLine($"第{i + 1}条导入资料，{dto.BirthdayDescription} 不是正确的日期格式！");
                        continue;
                    }
                    customer.Birthday = birthday;
                }

                var originalCustomer = DataDbContext.Set<Customer>()
                    .FirstOrDefault(c => c.MobilePhoneNumber == null && c.MobilePhoneNumber != dto.MobilePhoneNumber && c.RealName == dto.RealName);

                if (originalCustomer != null)
                {
                    originalCustomer.RealName = customer.RealName;
                    originalCustomer.NickName = customer.NickName;
                    originalCustomer.Gender = customer.Gender;
                    originalCustomer.Birthday = customer.Birthday;
                    originalCustomer.MobilePhoneNumber = customer.MobilePhoneNumber;
                    originalCustomer.Qq = customer.Qq;
                    originalCustomer.Wechat = customer.Wechat;
                    originalCustomer.Address = customer.Address;
                    originalCustomer.CustomerCategory = customer.CustomerCategory;
                    originalCustomer.LastModifyTime = DateTime.Now;
                }
                else
                {
                    customer.CreatorTime = DateTime.Now;
                    customer.LastModifyTime = DateTime.Now;
                    DataDbContext.Set<Customer>().Add(customer);
                }
                DataDbContext.SaveChanges();
                count++;
            }
            var resultMessage = message.Length > 0 ? $"成功导入{count},其中失败的：{message.ToString()}" : $"成功导入{count}";
            return new ImportedResultDto { Message = resultMessage, Title = "", Count = count, Success = true };
        }
    }
}
