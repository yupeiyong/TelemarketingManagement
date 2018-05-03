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
    public class TelephoneRecordingService
    {
        private readonly string _modelDescription = typeof(TelephoneRecording).GetDescription();

        public DataDbContext DataDbContext { get; set; }

        ~TelephoneRecordingService()
        {
            if (DataDbContext != null)
                DataDbContext.Dispose();
        }

        public List<TelephoneRecording> Search(TelephoneRecordingSearchDto dto)
        {
            var dataSource = DataDbContext.Set<TelephoneRecording>().AsQueryable();

            if(dto.CustomerId>0)
                dataSource = dataSource.Where(c => c.CustomerId==dto.CustomerId);

            if(dto.VisitorId>0)
                dataSource = dataSource.Where(c => c.VisitorId == dto.VisitorId);

            dataSource = dataSource.WhereDateTime(nameof(TelephoneRecording.CreatorTime), dto.StartCreatorTime, dto.EndCreatorTime);

            if (!string.IsNullOrWhiteSpace(dto.Keywords))
                dataSource = dataSource.Where(c => c.AudioFileName != null && c.AudioFileName.Contains(dto.Keywords) || c.CustomerRealName != null && c.CustomerRealName.Contains(dto.Keywords) || c.VisitorNickName != null && c.VisitorNickName.Contains(dto.Keywords));

            dataSource = dataSource.OrderByDescending(a => a.LastModifyTime);

            if (dto.IsGetTotalCount)
                dto.TotalCount = dataSource.Count();

            return dataSource.Skip(dto.StartIndex).Take(dto.PageSize).ToList();
        }


        public void Save(TelephoneRecordingEditDto dto)
        {
            if (dto.UpdateId > 0)
                Update(dto);
            else
                Add(dto);
        }


        private void ValidateEditDto(TelephoneRecordingEditDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AudioFileName))
                throw new Exception("错误，录音文件不能为空！");

            if(dto.VisitorId<=0)
                throw new Exception($"错误，采访记录人不能为空（Id={dto.VisitorId})！");

        }


        private void Add(TelephoneRecordingEditDto dto)
        {
            ValidateEditDto(dto);

            if (DataDbContext.Set<TelephoneRecording>().Any(tr=>tr.AudioFileName!=null && tr.AudioFileName==dto.AudioFileName))
                throw new Exception($"错误，录音文件:{dto.AudioFileName}已经存在，请检查后重试！");

            Customer customer = null;
            if (dto.CustomerId > 0)
            {
                customer = DataDbContext.Set<Customer>().FirstOrDefault(c => c.Id == dto.CustomerId);
                if(customer==null)
                    throw new Exception($"错误，Id={dto.CustomerId}的客户不存在，请检查后重试！");
            }

            var telephoneRecording = new TelephoneRecording { AudioFileName = dto.AudioFileName,Description=dto.Description };
            if (customer != null)
            {
                telephoneRecording.CustomerId = customer.Id;
                telephoneRecording.CustomerRealName = customer.RealName;
            }

            User visitor = null;
            if (dto.VisitorId > 0)
            {
                visitor = DataDbContext.Set<User>().FirstOrDefault(c => c.Id == dto.VisitorId);
                if (visitor == null)
                    throw new Exception($"错误，Id={dto.VisitorId}的采访人不存在，请检查后重试！");
            }
            if (visitor != null)
            {
                telephoneRecording.VisitorId = visitor.Id;
                telephoneRecording.VisitorNickName = visitor.NickName;
            }

            telephoneRecording.CreatorTime = DateTime.Now;
            telephoneRecording.LastModifyTime = DateTime.Now;
            DataDbContext.Set<TelephoneRecording>().Add(telephoneRecording);
            DataDbContext.SaveChanges();
        }


        public void Update(TelephoneRecordingEditDto dto)
        {
            var telephoneRecording = DataDbContext.Set<TelephoneRecording>().FirstOrDefault(tr => tr.Id == dto.UpdateId);
            if(telephoneRecording==null)
                throw new Exception($"错误，Id={dto.UpdateId}的采访记录不存在，修改记录失败！");

            Customer customer = null;
            if (dto.CustomerId > 0)
            {
                customer = DataDbContext.Set<Customer>().FirstOrDefault(c => c.Id == dto.CustomerId);
                if (customer == null)
                    throw new Exception($"错误，Id={dto.CustomerId}的客户不存在，请检查后重试！");
            }

            
            if (customer != null)
            {
                telephoneRecording.CustomerId = customer.Id;
                telephoneRecording.CustomerRealName = customer.RealName;
            }

            User visitor = null;
            if (dto.VisitorId > 0)
            {
                visitor = DataDbContext.Set<User>().FirstOrDefault(c => c.Id == dto.VisitorId);
                if (visitor == null)
                    throw new Exception($"错误，Id={dto.VisitorId}的采访人不存在，请检查后重试！");
            }
            if (visitor != null)
            {
                telephoneRecording.VisitorId = visitor.Id;
                telephoneRecording.VisitorNickName = visitor.NickName;
            }

            telephoneRecording.Description = dto.Description;
            telephoneRecording.LastModifyTime = DateTime.Now;
            DataDbContext.SaveChanges();

        }



        public void Remove(params long[] ids)
        {
            if (ids == null || ids.Length == 0)
                throw new Exception("错误，删除的序号为空！");
            foreach (var id in ids)
            {
                var data = DataDbContext.Set<TelephoneRecording>().FirstOrDefault(b => b.Id == id);
                if (data == null)
                    throw new Exception($"错误，{_modelDescription}不存在！(Id:{id})");

                DataDbContext.Set<TelephoneRecording>().Remove(data);
            }
            DataDbContext.SaveChanges();
        }


        public TelephoneRecording GetDataById(long id)
        {
            return DataDbContext.Set<TelephoneRecording>().FirstOrDefault(b => b.Id == id);
        }

    }
}
