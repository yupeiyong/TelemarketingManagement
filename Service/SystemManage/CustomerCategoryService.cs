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
    public class CustomerCategoryService
    {
        private readonly string _modelDescription = typeof(CustomerCategory).GetDescription();

        public DataDbContext DataDbContext { get; set; }

        ~CustomerCategoryService()
        {
            if (DataDbContext != null)
                DataDbContext.Dispose();
        }

        public List<CustomerCategory> Search(CustomerCategorySearchDto dto)
        {
            var dataSource = DataDbContext.Set<CustomerCategory>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Keywords))
                dataSource = dataSource.Where(cc=>cc.Name!=null && cc.Name.Contains(dto.Keywords) || cc.Description!=null && cc.Description.Contains(dto.Keywords));

            dataSource = dataSource.OrderByDescending(a => a.LastModifyTime);

            if (dto.IsGetTotalCount)
                dto.TotalCount = dataSource.Count();

            return dataSource.Skip(dto.StartIndex).Take(dto.PageSize).ToList();
        }


        public void Save(CustomerCategoryEditDto dto)
        {
            if (dto.UpdateId > 0)
                Update(dto);
            else
                Add(dto);
        }


        private void ValidateEditDto(CustomerCategoryEditDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception($"错误，{_modelDescription}的名称不能为空！");
        }


        private void Add(CustomerCategoryEditDto dto)
        {
            ValidateEditDto(dto);

            if (DataDbContext.Set<CustomerCategory>().Any(cc=>cc.Name!=null && cc.Name==dto.Name))
                throw new Exception($"错误，新增失败，名称：{dto.Name}的{_modelDescription}已经存在！");

            var CustomerCategory = dto.MapTo<CustomerCategory>();
            CustomerCategory.CreatorTime = DateTime.Now;
            CustomerCategory.LastModifyTime = DateTime.Now;
            DataDbContext.Set<CustomerCategory>().Add(CustomerCategory);
            DataDbContext.SaveChanges();
        }


        public void Update(CustomerCategoryEditDto dto)
        {
            ValidateEditDto(dto);

            var customerCategory = DataDbContext.Set<CustomerCategory>().FirstOrDefault(c => c.Id == dto.UpdateId);
            if (customerCategory == null)
                throw new Exception($"错误，Id={dto.UpdateId} 的{_modelDescription}不存在！");

            if (DataDbContext.Set<CustomerCategory>().Any(cc => cc.Id != customerCategory.Id && cc.Name != null && cc.Name == dto.Name))
                throw new Exception($"错误，修改失败，名称：{dto.Name}的{_modelDescription}已经存在！");

            dto.MapTo<CustomerCategory>(customerCategory);
            customerCategory.LastModifyTime = DateTime.Now;
            DataDbContext.SaveChanges();
        }



        public void Remove(params long[] ids)
        {
            if (ids == null || ids.Length == 0)
                throw new Exception("错误，删除的序号为空！");
            foreach (var id in ids)
            {
                var data = DataDbContext.Set<CustomerCategory>().FirstOrDefault(b => b.Id == id);
                if (data == null)
                    throw new Exception($"错误，{_modelDescription}不存在！(Id:{id})");

                DataDbContext.Set<CustomerCategory>().Remove(data);
            }
            DataDbContext.SaveChanges();
        }


        public CustomerCategory GetDataById(long id)
        {
            return DataDbContext.Set<CustomerCategory>().FirstOrDefault(b => b.Id == id);
        }
    }
}
