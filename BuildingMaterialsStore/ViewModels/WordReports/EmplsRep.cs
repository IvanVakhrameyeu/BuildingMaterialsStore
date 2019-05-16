using System;
using System.Data;
using Word = Microsoft.Office.Interop.Word;

namespace BuildingMaterialsStore.ViewModels.WordReports
{
    public class EmplsRep : outputInWord
    {
        /// <summary>
        /// замена слов в документе
        /// </summary>
        /// <param name="NameReport"></param>
        /// <param name="Name"></param>
        /// <param name="Total"></param>
        /// <param name="wordDocument"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="templatePathObj"></param>
        private void wordsWhoReplase(string NameReport, string Name, string Total, Word._Document wordDocument, DateTime dateFrom, DateTime dateTo, string templatePathObj)
        {
            try
            {
                ReplaceWordStub("{Who}", NameReport, wordDocument);
                ReplaceWordStub("{Date}", DateTime.Now.Date.ToString("d"), wordDocument);
                ReplaceWordStub("{DateFrom}", dateFrom.Date.ToString("d"), wordDocument);
                ReplaceWordStub("{DateTo}", dateTo.Date.ToString("d"), wordDocument);
                ReplaceWordStub("{Name}", Name, wordDocument);
                ReplaceWordStub("{Total}", Total, wordDocument);
                wordDocument.SaveAs("Buf" + templatePathObj);
            }
            catch { }
        }
        /// <summary>
        /// создание и заполнение таблицы
        /// </summary>
        /// <param name="wordApplication"></param>
        /// <param name="wordDocument"></param>
        /// <param name="ds"></param>
        /// <param name="Column1"></param>
        /// <param name="Column2"></param>
        private void createTable(Word._Application wordApplication, Word._Document wordDocument, DataSet ds, string Column1, string Column2)
        {
            wordApplication.Selection.Find.Execute("{Table}");
            Word.Range wordRange = wordApplication.Selection.Range;
            var wordTable = wordDocument.Tables.Add(wordRange,
               ds.Tables[0].Rows.Count + 1, 3);
            wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;
            wordTable.Cell(1, 1).Range.Text = Column1;
            wordTable.Cell(1, 2).Range.Text = Column2;
            wordTable.Cell(1, 3).Range.Text = "Сумма продажи";
            for (int i = 2; i < ds.Tables[0].Rows.Count + 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    wordTable.Cell(i, j + 1).Range.Text = ds.Tables[0].Rows[i - 2][j].ToString();
                }
            }
        }
        /// <summary>
        /// старт класса
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="nameFile"></param>
        /// <param name="NameReport"></param>
        /// <param name="Name"></param>
        /// <param name="sql"></param>
        /// <param name="Column1"></param>
        /// <param name="Column2"></param>
        public void writeClass(DateTime dateFrom, DateTime dateTo, string nameFile, string NameReport, string Name, string sql, string Column1, string Column2)
        {
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
            string Total = "";

            DataSet ds0;
            outPutDataSet(out ds0, "select sum(TotalPrice) from Store ");

            Total = Math.Round(Convert.ToDouble(ds0.Tables[0].Rows[0][0]), 2).ToString();
            wordsWhoReplase(NameReport, Name, Total, wordDocument, dateFrom, dateTo, templatePathObj);

            DataSet ds;
            outPutDataSet(out ds, sql);
            createTable(wordApplication, wordDocument, ds, Column1, Column2);

            wordApplication.Visible = true;
        }
    }
}
