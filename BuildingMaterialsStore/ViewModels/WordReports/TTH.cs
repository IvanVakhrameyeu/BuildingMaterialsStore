using BuildingMaterialsStore.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using Word = Microsoft.Office.Interop.Word;
namespace BuildingMaterialsStore.ViewModels.WordReports
{
    class TTH: outputInWord
    {
        public void writeClass(string nameFile, string sql,int id, DateTime day)
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

            wordApplication.Selection.Find.Execute("{Table}");
            Word.Range wordRange = wordApplication.Selection.Range;
            
            DataSet ds;
            outPutDataSet(out ds, sql);
            
            int rows = ds.Tables[0].Rows.Count + 2 + 1;
            int columns = 9;
            var wordTable = wordDocument.Tables.Add(wordRange,
                rows, columns);

            wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;


            wordTable.Cell(1, 1).Range.Text = "Наименование товара";
            wordTable.Cell(1, 2).Range.Text = "Единица измерения";
            wordTable.Cell(1, 3).Range.Text = "Количество";
            wordTable.Cell(1, 4).Range.Text = "Цена";
            wordTable.Cell(1, 5).Range.Text = "Стоимость";
            wordTable.Cell(1, 6).Range.Text = "Ставка НДС, %";
            wordTable.Cell(1, 7).Range.Text = "Сумма НДС, руб. коп.";
            wordTable.Cell(1, 8).Range.Text = "Стоимость с НДС";
            wordTable.Cell(1, 9).Range.Text = "Примечание";

            wordTable.Cell(2, 1).Range.Text = "1";
            wordTable.Cell(2, 2).Range.Text = "2";
            wordTable.Cell(2, 3).Range.Text = "3";
            wordTable.Cell(2, 4).Range.Text = "4";
            wordTable.Cell(2, 5).Range.Text = "5";
            wordTable.Cell(2, 6).Range.Text = "6";
            wordTable.Cell(2, 7).Range.Text = "7";
            wordTable.Cell(2, 8).Range.Text = "8";
            wordTable.Cell(2, 9).Range.Text = "9";

            int Count = 0;
            double Price = 0;
            double SumPrice = 0;
            double SumHDSPrice = 0;

            for (int i = 3; i < rows; i++)
            {
                double Discount = Convert.ToDouble(ds.Tables[0].Rows[i - 3][5])/100;
                double CurrentPrice = Convert.ToDouble(ds.Tables[0].Rows[i - 3][4])-(Convert.ToDouble(ds.Tables[0].Rows[i - 3][4])* Discount);
                wordTable.Cell(i, 1).Range.Text = ds.Tables[0].Rows[i - 3][0].ToString();
                wordTable.Cell(i, 2).Range.Text = ds.Tables[0].Rows[i - 3][1].ToString();
                wordTable.Cell(i, 3).Range.Text = ds.Tables[0].Rows[i - 3][2].ToString();
                wordTable.Cell(i, 4).Range.Text = (Math.Round(CurrentPrice - (CurrentPrice * 20 / 100),2)).ToString();
                wordTable.Cell(i, 5).Range.Text = ((Math.Round(CurrentPrice - (CurrentPrice * 20 / 100), 2))*Convert.ToInt32(ds.Tables[0].Rows[i - 3][2])).ToString();
                wordTable.Cell(i, 6).Range.Text = "20%";
                wordTable.Cell(i, 7).Range.Text = (Math.Round((CurrentPrice * 20 / 100),2)*Convert.ToInt32(ds.Tables[0].Rows[i - 3][2])).ToString();
                wordTable.Cell(i, 8).Range.Text = Math.Round(CurrentPrice * Convert.ToInt32(ds.Tables[0].Rows[i - 3][2]), 2).ToString();

                Count += Convert.ToInt32(ds.Tables[0].Rows[i - 3][2]);
                Price += ((Math.Round(CurrentPrice - (CurrentPrice * 20 / 100), 2)) * Convert.ToInt32(ds.Tables[0].Rows[i - 3][2]));
                SumPrice += (Math.Round((CurrentPrice * 20 / 100), 2) * Convert.ToInt32(ds.Tables[0].Rows[i - 3][2]));
                SumHDSPrice += Math.Round(CurrentPrice * Convert.ToInt32(ds.Tables[0].Rows[i - 3][2]), 2);                
            }
            wordTable.Cell(rows, 1).Range.Text = "ИТОГО";
            wordTable.Cell(rows, 3).Range.Text = Count.ToString();
            wordTable.Cell(rows, 5).Range.Text = Price.ToString();
            wordTable.Cell(rows, 7).Range.Text = SumPrice.ToString();
            wordTable.Cell(rows, 8).Range.Text = SumHDSPrice.ToString();

            sql = "select FirmName, UNP, FirmLegalAddress, FirmAccountNumber,FirmBankDetails, FirmPhoneNumber from Firms where FirmID="+id.ToString();
            outPutDataSet(out ds, sql);

            try
            {
                СуммаПрописью.Валюта.Рубли.Пропись(Price * 20 / 100);

                ReplaceWordStub("{YNH}", ds.Tables[0].Rows[0][1].ToString(), wordDocument);
                ReplaceWordStub("{DD}", day.ToString("d").Split('.')[0], wordDocument);
                ReplaceWordStub("{MM}", day.ToString("d").Split('.')[1], wordDocument);
                ReplaceWordStub("{YY}", day.ToString("d").Split('.')[2], wordDocument);
                ReplaceWordStub("{adress}", ds.Tables[0].Rows[0][0].ToString()+ ds.Tables[0].Rows[0][2].ToString(), wordDocument);
                ReplaceWordStub("{SumHdsP}", СуммаПрописью.Валюта.Рубли.Пропись(SumPrice), wordDocument);
             //  ReplaceWordStub("{SumHdsC}", СуммаПрописью.Валюта.Рубли.Пропись(Price * 20 / 100).Split(' ')[1], wordDocument);
                ReplaceWordStub("{TotalSumP}", СуммаПрописью.Валюта.Рубли.Пропись(SumHDSPrice), wordDocument);
              //  ReplaceWordStub("{TotalSumC}", СуммаПрописью.Валюта.Рубли.Пропись(Price), wordDocument);
            }
            catch { }

            wordApplication.Visible = true;
        }
    }
}
