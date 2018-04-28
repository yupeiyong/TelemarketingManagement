using System.ComponentModel;
using Models.DataBase;


namespace Models
{

    /// <summary>
    ///     客户分类
    /// </summary>
    [Description("客户分类")]
    public class CustomerCategory : BaseEntity
    {

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        ///     自定义顺序
        /// </summary>
        public int CustomOrder { get; set; }

    }

}