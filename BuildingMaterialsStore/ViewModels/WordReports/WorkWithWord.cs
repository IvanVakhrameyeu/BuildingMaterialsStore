using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.ViewModels;
using BuildingMaterialsStore.ViewModels.WordReports;
using System;
using System.Collections.Generic;
using System.Data;
using Word = Microsoft.Office.Interop.Word;
namespace Wpf_журнал_учащихся_школы
{
    class WorkWithWord : outputInWord
    {
        /// <summary>
        /// замена меток в ворде
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="wordDocument"></param>
        /// <param name="templatePathObj"></param>
        private void wordsWhoReplase(List<Purchases> ds, Word._Document wordDocument, string templatePathObj)
        {
            try
            {
                DataSet dss;
                outPutDataSet(out dss, "select UNP, FirmLegalAddress, FirmAccountNumber, FirmBankDetails from Firms where FirmName like '%" + ds[0].FirmName + "'");


                ReplaceWordStub("{Date}", DateTime.Now.ToString("d"), wordDocument);
                ReplaceWordStub("{FirmName}", ds[0].FirmName, wordDocument);

                ReplaceWordStub("{UNP}", (dss.Tables[0].Rows[0][0]).ToString(), wordDocument);
                ReplaceWordStub("{FirmLegalAddress}", (dss.Tables[0].Rows[0][1]).ToString(), wordDocument);
                ReplaceWordStub("{FirmBankDetails}", (dss.Tables[0].Rows[0][3]).ToString() + " " + (dss.Tables[0].Rows[0][2]).ToString(), wordDocument);
                wordDocument.SaveAs("Buf" + templatePathObj);
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// создание таблицы
        /// </summary>
        /// <param name="wordApplication"></param>
        /// <param name="wordDocument"></param>
        /// <param name="ds">лист товаров</param>
        /// <param name="TotalSum">сумма без ндс</param>
        /// <param name="TotalSumHDS">сумма с ндс</param>
        private void createTable(Word._Application wordApplication, Word._Document wordDocument, List<Purchases> ds, out double TotalSum, out double TotalSumHDS)
        {
            wordApplication.Selection.Find.Execute("{Table}");
            Word.Range wordRange = wordApplication.Selection.Range;
            int rows = ds.Count + 3;
            var wordTable = wordDocument.Tables.Add(wordRange,
                ds.Count + 3, 6);
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
            TotalSum = 0;
            TotalSumHDS = 0;
            int Count = 0;
            for (int i = 3; i < ds.Count + 3; i++)
            {
                wordTable.Cell(i, 1).Range.Text = ds[i - 3].storage.Name;
                wordTable.Cell(i, 2).Range.Text = ds[i - 3].Count.ToString();
                wordTable.Cell(i, 3).Range.Text = Math.Round(((ds[i - 3].Total) - (ds[i - 3].Total * 20 / 100)), 2).ToString("0.00");
                wordTable.Cell(i, 4).Range.Text = "20%";
                wordTable.Cell(i, 5).Range.Text = Math.Round((ds[i - 3].Total * 20 / 100), 2).ToString("0.00");
                wordTable.Cell(i, 6).Range.Text = Math.Round(ds[i - 3].Total, 2).ToString("0.00");
                TotalSum += ((ds[i - 3].Total) - (ds[i - 3].Total * 20 / 100));
                TotalSumHDS += ds[i - 3].Total;
                Count += ds[i - 3].Count;
            }
            wordTable.Cell(rows, 1).Range.Text = "Всего";
            wordTable.Cell(rows, 2).Range.Text = Count.ToString();
            wordTable.Cell(rows, 3).Range.Text = TotalSum.ToString("0.00");
            wordTable.Cell(rows, 5).Range.Text = Math.Round((TotalSumHDS * 20 / 100), 2).ToString();
            wordTable.Cell(rows, 6).Range.Text = Math.Round(TotalSumHDS, 2).ToString();
        }

        public void writeClass(List<Purchases> ds, string nameFile)
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

            wordsWhoReplase(ds, wordDocument, templatePathObj);

            double TotalSum = 0;
            double TotalSumHDS = 0;
            createTable(wordApplication, wordDocument, ds, out TotalSum, out TotalSumHDS);

            ReplaceWordStub("{P}", СуммаПрописью.Валюта.Рубли.Пропись(Math.Round(Convert.ToDecimal(TotalSum), 2)), wordDocument);
            ReplaceWordStub("{HP}", СуммаПрописью.Валюта.Рубли.Пропись(Math.Round(Convert.ToDecimal(TotalSumHDS), 2)), wordDocument);

            wordApplication.Visible = true;

        }
    }
}
