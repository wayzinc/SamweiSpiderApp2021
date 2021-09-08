namespace SamweiSpiderApp2021
{
    using AppCode;

    /// <summary>
    /// 程序入口
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            //for (var i = 1; i <= W91MJWHelper.GetPageTotal(); i++)
            for (var i = 1; i <= W91MJWHelper.GetPageTotal(); i++)
            {
                Console.WriteLine($"Page: {i}");

                var Results = W91MJWHelper.GetList(i);
                //Console.WriteLine(JsonHelper.GetJson(Results));
                new SQLiteHelper().Save(Results);

                Thread.Sleep(1000 * new Random().Next(3, 6));
            }
        }
    }
}