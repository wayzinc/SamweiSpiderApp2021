namespace SamweiSpiderApp2021.AppCode
{
    #region _Spaces
    using System.Runtime.Serialization.Json;
    using System.Text;
    #endregion

    public class JsonHelper
    {
        #region GetJson
        /// <summary>
        /// 把对象序列化 JSON 字符串 
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实体</param>
        /// <returns>JSON字符串</returns>
        public static string GetJson<T>(T obj)
        {
            try
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                using var ms = new MemoryStream();
                json.WriteObject(ms, obj);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch { return ""; }
        }
        #endregion

        #region ParseJson
        /// <summary>
        /// 把JSON字符串还原为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="szJson">JSON字符串</param>
        /// <returns>对象实体</returns>
        public static T ParseJson<T>(string szJson)
        {
            try
            {
                T obj = Activator.CreateInstance<T>();
                using var ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson));
                DataContractJsonSerializer dcj = new DataContractJsonSerializer(typeof(T));
                return (T)dcj.ReadObject(ms);
            }
            catch { return default(T); }
        }
        #endregion
    }
}
