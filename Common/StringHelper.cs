namespace Common
{

    public static class StringHelper
    {

        /// <summary>
        ///     全角符号转半角
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string AllCornersOfTheCorner(this string source)
        {
            return source.Replace("：", ":").Replace("、", ",").Replace("（", "(").Replace("）", ")");
            ;
        }

    }

}