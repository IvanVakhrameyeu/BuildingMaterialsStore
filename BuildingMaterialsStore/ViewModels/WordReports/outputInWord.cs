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

        public static void ReplaceWordStub(string stubToReplace, string text, Word._Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }
        public static void outPutDataSet(out DataSet ds, string sql)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlDataAdapter adapter;

            con = new SqlConnection(AuthorizationSettings.connectionString);
            con.Open();
            cmd = new SqlCommand(sql, con);
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "Storedb");


            adapter.Dispose();
            con.Close();
            con.Dispose();
        }
    }
}
