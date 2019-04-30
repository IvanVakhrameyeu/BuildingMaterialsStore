using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;
namespace Wpf_журнал_учащихся_школы
{
    class WorkWithWord
    {
        private static void ReplaceWordStub(string stubToReplace, string text, Word._Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }
        static public void writeClass(List<Purchases> ds, string nameFile)
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
                SqlConnection con;
                SqlCommand cmd;
                SqlDataAdapter adapter;
                DataSet dss;
                //try
                //{
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("select UNP, FirmLegalAddress, FirmAccountNumber, FirmBankDetails from Firms where FirmName like '%"+ ds[0].FirmName + "'", con);
                adapter = new SqlDataAdapter(cmd);
                dss = new DataSet();
                adapter.Fill(dss, "Storedb");


                ReplaceWordStub("{Date}", DateTime.Now.ToString("d"), wordDocument);
                ReplaceWordStub("{FirmName}", ds[0].FirmName, wordDocument);

                ReplaceWordStub("{UNP}", (dss.Tables[0].Rows[0][0]).ToString(), wordDocument);
                ReplaceWordStub("{FirmLegalAddress}", (dss.Tables[0].Rows[0][1]).ToString(), wordDocument);
                ReplaceWordStub("{FirmBankDetails}", (dss.Tables[0].Rows[0][3]).ToString()+" " +(dss.Tables[0].Rows[0][2]).ToString(), wordDocument);
                wordDocument.SaveAs("Buf"+templatePathObj);
            }
            catch (Exception ex)
            { 
                //MessageBox.Show(ex.Message);
            }
            wordApplication.Selection.Find.Execute("{Table}");            
            Word.Range wordRange = wordApplication.Selection.Range;

            var wordTable = wordDocument.Tables.Add(wordRange,
                ds.Count + 2, 6);
            
            wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;

            wordTable.Cell(1, 1).Range.Text = "Наименование выполненных работ";
            wordTable.Cell(1, 2).Range.Text = "Кол-во услуг";
            wordTable.Cell(1, 3).Range.Text = "Стоимость услуг Без НДС, руб.коп.";
            wordTable.Cell(1, 4).Range.Text = "Ставка НДС, %";
            wordTable.Cell(1, 5).Range.Text = "Сумма НДС, руб.коп.";
            wordTable.Cell(1, 6).Range.Text = "Стоимость работ с НДС, руб.коп.";
            
            wordTable.Cell(2, 1).Range.Text = "1";
            wordTable.Cell(2, 2).Range.Text = "2";
            wordTable.Cell(2, 3).Range.Text = "3";
            wordTable.Cell(2, 4).Range.Text = "4";
            wordTable.Cell(2, 5).Range.Text = "5";
            wordTable.Cell(2, 6).Range.Text = "6";

            double TotalSum = 0;
            double TotalSumHDS = 0;
            for (int i = 3; i < ds.Count+3; i++)
            {
                wordTable.Cell(i, 1).Range.Text = ds[i - 3].storage.Name;
                wordTable.Cell(i, 2).Range.Text = ds[i - 3].Count.ToString();
                wordTable.Cell(i, 3).Range.Text = ((ds[i - 3].Total) -(ds[i - 3].Total * 20/100)).ToString();
                wordTable.Cell(i, 4).Range.Text = "20%";//ds[i - 2].storage.Price.ToString("0.00");
                wordTable.Cell(i, 5).Range.Text = (ds[i - 3].Total * 20 / 100).ToString();
                wordTable.Cell(i, 6).Range.Text = ds[i - 3].Total.ToString("0.00");
                TotalSum += ((ds[i - 3].Total) - (ds[i - 3].Total * 20 / 100));
                TotalSumHDS += ds[i - 3].Total;
            }



            ReplaceWordStub("{P}", СуммаПрописью.Валюта.Рубли.Пропись(Convert.ToDecimal(TotalSum)), wordDocument);
            ReplaceWordStub("{HP}", СуммаПрописью.Валюта.Рубли.Пропись(Convert.ToDecimal(TotalSumHDS)), wordDocument);

            wordApplication.Visible = true;

        }
        private void ConvertNumbersToWords()
        {

        }
    }
}
