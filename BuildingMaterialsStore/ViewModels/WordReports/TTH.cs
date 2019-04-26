using BuildingMaterialsStore.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;
namespace BuildingMaterialsStore.ViewModels.WordReports
{
    class TTH
    {
        private static void ReplaceWordStub(string stubToReplace, string text, Word._Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }
        static public void writeClass(string nameFile, string NameReport, string sql)
        {
            Word._Application wordApplication = new Word.Application();
            Word._Document wordDocument = null;
            var templatePathObj = Environment.CurrentDirectory + "\\" + nameFile + ".docx";
            try
            {
                wordDocument = wordApplication.Documents.Add(templatePathObj);
            }
            catch
            {
                if (wordDocument != null)
                {
                    wordDocument.Close(false);
                    wordDocument = null;
                }
                wordApplication.Quit();
                wordApplication = null;
                throw;
            }
            wordApplication.Visible = false;
            try
            {
                ReplaceWordStub("{Who}", NameReport, wordDocument);
                ReplaceWordStub("{Date}", DateTime.Now.Date.ToString("d"), wordDocument);
                //ReplaceWordStub("{DateFrom}", dateFrom.Date.ToString("d"), wordDocument);
                //ReplaceWordStub("{DateTo}", dateTo.Date.ToString("d"), wordDocument);
                //ReplaceWordStub("{NameCategory}", NameCategory, wordDocument);
                //ReplaceWordStub("{Name}", Name, wordDocument);
                //ReplaceWordStub("{Price}", Price.ToString("0.00"), wordDocument);
                //ReplaceWordStub("{AmountStore}", AmountStore.ToString(), wordDocument);
                //ReplaceWordStub("{Description}", Description, wordDocument);
                wordDocument.SaveAs("Buf" + templatePathObj);
                wordApplication.Visible = true;
            }
            catch { }

            wordApplication.Selection.Find.Execute("{Table}");
            Word.Range wordRange = wordApplication.Selection.Range;

            SqlConnection con;
            SqlCommand cmd;
            SqlDataAdapter adapter;
            DataSet ds;
            //try
            //{
            con = new SqlConnection(AuthorizationSettings.connectionString);
            con.Open();
            cmd = new SqlCommand(sql, con);
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "Storedb");

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            adapter.Dispose();
            con.Close();
            con.Dispose();
            //}
            var wordTable = wordDocument.Tables.Add(wordRange,
                ds.Tables[0].Rows.Count + 1, 6);

            wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;


            wordTable.Cell(1, 1).Range.Text = "Фамилия";
            wordTable.Cell(1, 2).Range.Text = "Имя";
            wordTable.Cell(1, 3).Range.Text = "Кол-во";
            wordTable.Cell(1, 4).Range.Text = "Скидка";
            wordTable.Cell(1, 5).Range.Text = "Общая цена";
            wordTable.Cell(1, 6).Range.Text = "Дата покупки";

            for (int i = 2; i < ds.Tables[0].Rows.Count + 2; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //if(j==4)
                    //    wordTable.Cell(i, j + 1).Range.Text = ds.Tables[0].Rows[i - 2][j].ToString("0.00");
                    //  else
                    if (j == 5)
                        wordTable.Cell(i, j + 1).Range.Text = Convert.ToDateTime(ds.Tables[0].Rows[i - 2][j]).Date.ToString("d");
                    else
                        wordTable.Cell(i, j + 1).Range.Text = ds.Tables[0].Rows[i - 2][j].ToString();
                }
            }
            wordApplication.Visible = true;
        }
    
}
}
