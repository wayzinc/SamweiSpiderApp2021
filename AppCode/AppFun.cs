namespace SamweiSpiderApp2021.AppCode
{
    #region _Spaces
    using System.Text.RegularExpressions;
    #endregion

    public class AppFun
    {
        #region String
        /// <summary>
        /// 判断输出string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="IsTrim"></param>
        /// <param name="IsReplaceSW">是否替换特殊字符</param>
        /// <returns></returns>
        public static string GetString(object? obj, bool IsTrim)
        {
            return obj != null ? (IsTrim ? obj.ToString().Trim() : obj.ToString()) : "";
        }
        #endregion

        #region Int
        /// <summary>
        /// Sbyte:代表有符号的8位整数，数值范围从-128 ～ 127
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte GetInt8(object obj)
        {
            _ = byte.TryParse(GetString(obj, true), out byte Result);
            return Result;
        }

        /// <summary>
        /// Short:代表有符号的16位整数，范围从-32768 ～ 32767
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static short GetInt16(object obj)
        {
            _ = short.TryParse(GetString(obj, true), out short Result);
            return Result;
        }

        /// <summary>
        /// Int:代表有符号的32位整数，范围从-2147483648 ～ 2147483648
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetInt32(object obj)
        {
            _ = int.TryParse(GetString(obj, true), out int Result);
            return Result;
        }

        /// <summary>
        /// Long:代表有符号的64位整数，范围从-9223372036854775808 ～ 9223372036854775808
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetInt64(object obj)
        {
            _ = long.TryParse(GetString(obj, true), out long Result);
            return Result;
        }

        /// <summary>
        /// 提取字符串中的数字（纯数字）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetNumbericInString(string str)
        {
            return Regex.Replace(str, @"[^0-9]+", "");
        }

        /// <summary>
        /// 提取字符串中的数字（浮点型）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="IsFloat"></param>
        /// <returns></returns>
        public static decimal GetNumbericInString(string str, bool _)
        {
            str = Regex.Replace(str, @"[^\d.\d]", "");
            return Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$") ? GetDecimal(str) : 0;
        }
        #endregion

        #region Float
        /// <summary>
        /// Double
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetDouble(object obj)
        {
            _ = double.TryParse(GetString(obj, true), out double Result);
            return Result;
        }

        /// <summary>
        /// Decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object obj)
        {
            _ = decimal.TryParse(GetString(obj, true), out decimal Result);
            return Result;
        }
        #endregion
    }
}
