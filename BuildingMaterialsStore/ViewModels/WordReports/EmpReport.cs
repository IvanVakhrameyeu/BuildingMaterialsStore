using System;
using System.Data;
using Word = Microsoft.Office.Interop.Word;

namespace BuildingMaterialsStore.ViewModels.WordReports
{
    class EmpReport: outputInWord
    {       
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
        private void createTable(Word._Application wordApplication, Word._Document wordDocument, DataSet ds, string Column1, string Column2)
        {
            try
            {
                wordApplication.Selection.Find.Execute("{Table}");
                Word.Range wordRange = wordApplication.Selection.Range;

                var wordTable = wordDocument.Tables.Add(wordRange,
                   ds.Tables[0].Rows.Count + 1, 9);

                wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;


                wordTable.Cell(1, 1).Range.Text = Column1;
                wordTable.Cell(1, 2).Range.Text = Column2;
                wordTable.Cell(1, 3).Range.Text = "Категория";
                wordTable.Cell(1, 4).Range.Text = "Название";
                wordTable.Cell(1, 5).Range.Text = "Цена за ед";
                wordTable.Cell(1, 6).Range.Text = "Кол-во";
                wordTable.Cell(1, 7).Range.Text = "Общая цена";
                wordTable.Cell(1, 8).Range.Text = "Дата покупки";
                wordTable.Cell(1, 9).Range.Text = "Описание";

                for (int i = 2; i < ds.Tables[0].Rows.Count + 2; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (j == 7)
                            wordTable.Cell(i, j + 1).Range.Text = Convert.ToDateTime(ds.Tables[0].Rows[i - 2][j]).Date.ToString("d");
                        else
                            wordTable.Cell(i, j + 1).Range.Text = ds.Tables[0].Rows[i - 2][j].ToString();
                    }
                }
            }
            catch { }
        }
        public void writeClass(DateTime dateFrom, DateTime dateTo, string nameFile, string NameReport, string Name, string sql, string Column1, string Column2)
        {
            try {
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
                if (NameReport == "работнику") {
                    DataSet ds0;
                    outPutDataSet(out ds0, "select sum(TotalPrice) from Store " +
                        "where EmployeeID = (select EmployeeID from Employee where EmpLastName='" + Name.Split()[0] + "' and EmpFirstName = '" + Name.Split()[1] + "')");

                    Total = Math.Round(Convert.ToDouble(ds0.Tables[0].Rows[0][0]), 2).ToString();
                }
                else
                {

                }
                wordsWhoReplase(NameReport, Name, Total, wordDocument, dateFrom, dateTo, templatePathObj);

                DataSet ds;
                outPutDataSet(out ds, sql);
                createTable(wordApplication, wordDocument, ds, Column1, Column2);

                wordApplication.Visible = true;
            }
            catch { }
            }
        
    }
}
