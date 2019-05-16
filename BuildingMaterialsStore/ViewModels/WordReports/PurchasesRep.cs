using System;
using System.Data;
using Word = Microsoft.Office.Interop.Word;
namespace BuildingMaterialsStore.ViewModels.WordReports
{
    class PurchasesRep : outputInWord
    {
        private void wordsWhoReplase(string NameReport, string Name, Word._Document wordDocument, DateTime dateFrom, DateTime dateTo,
            string templatePathObj, string NameCategory, double Price, double AmountStore, string Description)
        {
            try
            {
                ReplaceWordStub("{Who}", NameReport, wordDocument);
                ReplaceWordStub("{Date}", DateTime.Now.Date.ToString("d"), wordDocument);
                ReplaceWordStub("{DateFrom}", dateFrom.Date.ToString("d"), wordDocument);
                ReplaceWordStub("{DateTo}", dateTo.Date.ToString("d"), wordDocument);
                ReplaceWordStub("{NameCategory}", NameCategory, wordDocument);
                ReplaceWordStub("{Name}", Name, wordDocument);
                ReplaceWordStub("{Price}", Price.ToString("0.00"), wordDocument);
                ReplaceWordStub("{AmountStore}", AmountStore.ToString(), wordDocument);
                ReplaceWordStub("{Description}", Description, wordDocument);
                wordDocument.SaveAs("Buf" + templatePathObj);
                wordApplication.Visible = true;
            }
            catch { }
        }

        public void writeClass(DateTime dateFrom, DateTime dateTo, string nameFile, string NameReport,
            string Name, double Price, string NameCategory, string Description, int AmountStore, string sql)
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

            wordsWhoReplase(NameReport, Name, wordDocument, dateFrom, dateTo,
           templatePathObj, NameCategory, Price, AmountStore, Description);



            wordApplication.Selection.Find.Execute("{Table}");
            Word.Range wordRange = wordApplication.Selection.Range;

            DataSet ds;
            outPutDataSet(out ds, sql);

            var wordTable = wordDocument.Tables.Add(wordRange,
                ds.Tables[0].Rows.Count + 1, 6);

            wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;


            wordTable.Cell(1, 1).Range.Text = "Название";
            wordTable.Cell(1, 2).Range.Text = "УПН";
            wordTable.Cell(1, 3).Range.Text = "Кол-во";
            wordTable.Cell(1, 4).Range.Text = "Скидка";
            wordTable.Cell(1, 5).Range.Text = "Общая цена";
            wordTable.Cell(1, 6).Range.Text = "Дата покупки";

            for (int i = 2; i < ds.Tables[0].Rows.Count + 2; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (j == 5)
                        wordTable.Cell(i, j + 1).Range.Text = Convert.ToDateTime(ds.Tables[0].Rows[i - 2][j]).Date.ToString("d");
                    else
                       if (j == 4) wordTable.Cell(i, j + 1).Range.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i - 2][j])).ToString();
                    else
                        wordTable.Cell(i, j + 1).Range.Text = ds.Tables[0].Rows[i - 2][j].ToString();
                }
            }
            wordApplication.Visible = true;
        }
    }
}
