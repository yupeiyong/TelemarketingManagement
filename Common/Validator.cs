using System;
using System.Text.RegularExpressions;


namespace Common
{

    public class Validator
    {

        private const string IsNumberRegexString = @"^([0]|([1-9]+\d{0,}?))(.[\d]+)?$";

        private const string IsDateTimeRegexString = @"[1-2]{1}[0-9]{3}((-|\/|\.){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|\/|\.){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";

        private const string IsMobileRegexString = @"^1[0123456789]\d{9}$";

        private const string IsPhoneRegexString = @"(^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$)|(^0?1[35]\d{9}$)";

        private const string IsEmailRegexString = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        private const string IsFaxRegexString = @"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$";

        private const string IsOnlyChineseRegexString = @"^[\u4e00-\u9fa5]+$";

        private const string IsImageRegexString = @"(?i).+?\.(gif|jpg|jpeg|png)";

        private const string IsChineseNumberLetterRegexString = @"^[0-90-9a-zA-Z\u4E00-\u9FA5]*$";

        /// <summary>
        ///     匹配decimal类型数字，1、符号 2、整数部分长度 3、小数部分长度 4、是否有小数部分
        /// </summary>
        private const string IsDecimalRegexString = @"^{0}(\d|[1-9]\d{0,{1}})(\.\d{1,{2}}){0,{3}}$";


        /// <summary>
        ///     验证输入字符串为数字
        /// </summary>
        /// <param name="strln">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsNumber(string strln)
        {
            return null != strln && Regex.IsMatch(strln, IsNumberRegexString);
        }


        /// <summary>
        ///     判断用户输入是否为日期
        /// </summary>
        /// <param name="strln"></param>
        /// <returns></returns>
        /// <remarks>
        ///     可判断格式如下（其中-可替换为/，不影响验证)
        ///     YYYY | YYYY-MM | YYYY-MM-DD | YYYY-MM-DD HH:MM:SS | YYYY-MM-DD HH:MM:SS.FFF
        /// </remarks>
        public static bool IsDateTime(string strln)
        {
            if (null == strln)
            {
                return false;
            }
            const string regexDate = IsDateTimeRegexString;
            if (!Regex.IsMatch(strln, regexDate)) return false;

            //以下各月份日期验证，保证验证的完整性
            var indexY = -1;
            var indexM = -1;
            var indexD = -1;
            if (-1 != (indexY = strln.IndexOf("-", StringComparison.Ordinal)))
            {
                indexM = strln.IndexOf("-", indexY + 1, StringComparison.Ordinal);
                indexD = strln.IndexOf(":", StringComparison.Ordinal);
            }
            else
            {
                indexY = strln.IndexOf("/", StringComparison.Ordinal);
                indexM = strln.IndexOf("/", indexY + 1, StringComparison.Ordinal);
                indexD = strln.IndexOf(":", StringComparison.Ordinal);
            }

            //不包含日期部分，直接返回true
            if (-1 == indexM)
                return true;
            if (-1 == indexD)
            {
                indexD = strln.Length + 3;
            }
            var iYear = Convert.ToInt32(strln.Substring(0, indexY));
            var iMonth = Convert.ToInt32(strln.Substring(indexY + 1, indexM - indexY - 1));
            var iDate = Convert.ToInt32(strln.Substring(indexM + 1, indexD - indexM - 4));

            //判断月份日期
            if ((iMonth < 8 && 1 == iMonth%2) || (iMonth > 8 && 0 == iMonth%2))
            {
                if (iDate < 32)
                    return true;
            }
            else
            {
                if (iMonth != 2)
                {
                    if (iDate < 31)
                        return true;
                }
                else
                {
                    //闰年
                    if ((0 == iYear%400) || (0 == iYear%4 && 0 < iYear%100))
                    {
                        if (iDate < 30)
                            return true;
                    }
                    else
                    {
                        if (iDate < 29)
                            return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        ///     验证输入字符串为11位的手机号码
        /// </summary>
        /// <param name="strln"></param>
        /// <returns></returns>
        public static bool IsMobile(string strln)
        {
            return null != strln && Regex.IsMatch(strln, IsMobileRegexString, RegexOptions.IgnoreCase);
        }


        /// <summary>
        ///     验证身份证是否有效
        /// </summary>
        /// <param name="strln"></param>
        /// <returns></returns>
        public static bool IsIdCard(string strln)
        {
            if (null == strln)
            {
                return false;
            }

            switch (strln.Length)
            {
                case 18:
                {
                    var check = IsIdCard18(strln);
                    return check;
                }
                case 15:
                {
                    var check = IsIdCard15(strln);
                    return check;
                }
                default:
                    return false;
            }
        }


        /// <summary>
        ///     验证输入字符串为18位的身份证号码
        /// </summary>
        /// <param name="strln">输入的字符</param>
        /// <returns></returns>
        public static bool IsIdCard18(string strln)
        {
            if (null == strln)
            {
                return false;
            }

            long n = 0;
            if (long.TryParse(strln.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(strln.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false; //数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(strln.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false; //省份验证
            }
            var birth = strln.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false; //生日验证
            }
            var arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            var wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            var ai = strln.Remove(17).ToCharArray();
            var sum = 0;
            for (var i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i])*int.Parse(ai[i].ToString());
            }
            var y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != strln.Substring(17, 1).ToLower())
            {
                return false; //校验码验证
            }
            return true; //符合GB11643-1999标准
        }


        /// <summary>
        ///     验证输入字符串为15位的身份证号码
        /// </summary>
        /// <param name="strln">输入的字符</param>
        /// <returns></returns>
        public static bool IsIdCard15(string strln)
        {
            if (null == strln)
            {
                return false;
            }

            long n = 0;
            if (long.TryParse(strln, out n) == false || n < Math.Pow(10, 14))
            {
                return false; //数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(strln.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false; //省份验证
            }
            var birth = strln.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false; //生日验证
            }
            return true; //符合15位身份证标准
        }


        /// <summary>
        ///     验证输入字符串为电话号码
        /// </summary>
        /// <param name="strln"></param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsPhone(string strln)
        {
            return null != strln && Regex.IsMatch(strln, IsPhoneRegexString);
        }


        /// <summary>
        ///     验证是否是有效邮箱地址
        /// </summary>
        /// <param name="strln">输入的字符</param>
        /// <returns></returns>
        public static bool IsEmail(string strln)
        {
            return null != strln && Regex.IsMatch(strln, IsEmailRegexString);
        }


        /// <summary>
        ///     验证是否是有效传真号码
        /// </summary>
        /// <param name="strln">输入的字符</param>
        /// <returns></returns>
        public static bool IsFax(string strln)
        {
            return null != strln && Regex.IsMatch(strln, IsFaxRegexString);
        }


        /// <summary>
        ///     验证是否只含有汉字
        /// </summary>
        /// <param name="strln">输入的字符</param>
        /// <returns></returns>
        public static bool IsOnlyChinese(string strln)
        {
            return null != strln && Regex.IsMatch(strln, IsOnlyChineseRegexString);
        }


        /// <summary>
        ///     验证是否为图片
        /// </summary>
        /// <param name="strln"></param>
        /// <returns></returns>
        public static bool IsIamge(string strln)
        {
            return null != strln && Regex.IsMatch(strln, IsImageRegexString);
        }


        /// <summary>
        ///     验证是否只含有汉字数字字母
        /// </summary>
        /// <param name="strln"></param>
        /// <returns></returns>
        public static bool IsChineseNumberLetter(string strln)
        {
            return null != strln && Regex.IsMatch(strln, IsChineseNumberLetterRegexString);
        }


        /// <summary>
        ///     是否为指定的Decimal类型
        /// </summary>
        /// <param name="strln"></param>
        /// <param name="totalLength"></param>
        /// <param name="maxDecimalLength"></param>
        /// <param name="isPositive">默认为正数</param>
        /// <returns></returns>
        public static bool IsDecimal(string strln, int totalLength, int maxDecimalLength, bool isPositive = true)
        {
            if (null == strln)
            {
                return false;
            }

            if (totalLength <= 0) return false;
            if (maxDecimalLength < 0) return false;
            if (maxDecimalLength >= totalLength) return false;

            var integratedLength = totalLength - maxDecimalLength;
            var regexString = IsDecimalRegexString;
            regexString = regexString.Replace("{0}", isPositive ? "" : "-").Replace("{1}", integratedLength.ToString()).Replace("{2}", maxDecimalLength.ToString()).Replace("{3}", maxDecimalLength == 0 ? "0" : "1");
            return Regex.IsMatch(strln, regexString);
        }

    }

}