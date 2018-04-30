using AutoMapper;
using DataTransferObjects;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelemarketingManagement.App_Start.Base
{
    public class AutoMapperInitial
    {
        public static void RegisterMapperType()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap(typeof(UserUpdateDto), typeof(User));
                //cfg.CreateMap(typeof(ChongQinSscPlan), typeof(ChongQinSSCPlanViewModel));
                //cfg.CreateMap(typeof(Pk10Plan), typeof(PK10PlanViewModel));
                //cfg.CreateMap(typeof(BetSettingEditDto), typeof(BetSetting));
                //cfg.CreateMap(typeof(BetUrlSettingEditDto), typeof(BetUrlSetting));
                //cfg.CreateMap(typeof(ChongQinSscPlanEditDto), typeof(ChongQinSscPlan));
                //cfg.CreateMap(typeof(PK10PlanEditDto), typeof(Pk10Plan));
            });
        }
    }
}