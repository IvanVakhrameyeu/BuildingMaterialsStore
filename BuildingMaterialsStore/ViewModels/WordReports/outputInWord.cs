using BuildingMaterialsStore.Models;
using System.Data;
using System.Data.SqlClient;
using Word = Microsoft.Office.Interop.Word;

namespace BuildingMaterialsStore.ViewModels.WordReports
{
    public class outputInWord
    {
       public Word._Application wordApplication = new Word.Application();
       public Word._Document wordDocument = null;

        /// <summary>
        /// замена слов в документе
        /// </summary>
        /// <param name="stubToReplace">что заменять</param>
        /// <param name="text">текст замены</param>
        /// <param name="wordDocument">документ</param>
        public static void ReplaceWordStub(string stubToReplace, string text, Word._Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }
        /// <summary>
        /// вывод из бд данных
        /// </summary>
        /// <param name="ds">сохранение результата</param>
        /// <param name="sql">скл запрос</param>
        public static void outPutDataSet(out DataSet ds, string sql)
        {
            SqlConnection con=null;
            SqlCommand cmd;
            SqlDataAdapter adapter=null;
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand(sql, con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "Storedb");
            }
            catch { ds = null; }
            finally
            {
                adapter.Dispose();
                con.Close();
                con.Dispose();
            }
        }
    }
}
