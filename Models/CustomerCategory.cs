using System.ComponentModel;
using Models.DataBase;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{

    /// <summary>
    ///     客户分类
    /// </summary>
    [Description("客户分类")]
    public class CustomerCategory : BaseEntity
    {
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Name { get; set; }


        [Index]
        [MaxLength(150)]
        public string Description { get; set; }

        /// <summary>
        ///     自定义顺序
        /// </summary>
        [Index]
        public int CustomOrder { get; set; }

    }

}