using AutoMapper;
using DataTransferObjects;
using Models;


namespace TelemarketingManagement.Base
{

    public class AutoMapperInitial
    {

        public static void RegisterMapperType()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap(typeof (UserEditDto), typeof (User));
                cfg.CreateMap(typeof (CustomerEditDto), typeof (Customer));
                cfg.CreateMap(typeof (CustomerCategoryEditDto), typeof (CustomerCategory));
            });
        }

    }

}