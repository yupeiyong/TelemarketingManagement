using System.ComponentModel;


namespace Models.Enum
{

    /// <summary>
    ///     性别
    /// </summary>
    [Description("性别")]
    public enum Gender
    {

        /// <summary>
        ///     男性
        /// </summary>
        [Description("男")] Male = 1,

        /// <summary>
        ///     女性
        /// </summary>
        [Description("女")] Female = 2

    }

}