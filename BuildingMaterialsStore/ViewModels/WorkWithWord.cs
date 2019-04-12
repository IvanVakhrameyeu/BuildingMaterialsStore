using BuildingMaterialsStore.Models;
using System;
using System.Collections.Generic;
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
                ReplaceWordStub("{Date}", DateTime.Now.ToString(), wordDocument);
                ReplaceWordStub("{Employee}", AuthorizationSettings.EmpFirstName+ " " + AuthorizationSettings.EmpPatronymic+ " " + AuthorizationSettings.EmpLastName, wordDocument);
                ReplaceWordStub("{Customer}", ds[0].CustFirstName + " " + ds[0].CustLastName, wordDocument);
                wordDocument.SaveAs("Buf"+templatePathObj);
                wordApplication.Visible = true;
            }
            catch { }

            wordApplication.Selection.Find.Execute("{Table}");            
            Word.Range wordRange = wordApplication.Selection.Range;

            var wordTable = wordDocument.Tables.Add(wordRange,
                ds.Count + 1, 7);
            
            wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;

            wordTable.Cell(1, 1).Range.Text = "Название товара";
            wordTable.Cell(1, 2).Range.Text = "Количество";
            wordTable.Cell(1, 3).Range.Text = "Ед измерения";
            wordTable.Cell(1, 4).Range.Text = "Цена за ед";
            wordTable.Cell(1, 5).Range.Text = "Скидка";
            wordTable.Cell(1, 6).Range.Text = "Общая цена";
            wordTable.Cell(1, 7).Range.Text = "Описание";

            for(int i = 2; i < ds.Count+2; i++)
            {
                wordTable.Cell(i, 1).Range.Text = ds[i - 2].storage.Name;
                wordTable.Cell(i, 2).Range.Text = ds[i - 2].Count.ToString();
                wordTable.Cell(i, 3).Range.Text = ds[i - 2].storage.UnitName;
                wordTable.Cell(i, 4).Range.Text = ds[i - 2].storage.Price.ToString("0.00");
                wordTable.Cell(i, 5).Range.Text = ds[i - 2].CurrentDiscountAmount.ToString("0.00 %");
                wordTable.Cell(i, 6).Range.Text = ds[i - 2].Total.ToString("0.00");
                wordTable.Cell(i, 7).Range.Text = ds[i - 2].storage.Description;             
            }
            try
            {
                ReplaceWordStub("{Total}", ds[0].Total.ToString(), wordDocument);
                wordApplication.Visible = true;
            }
            catch { }
        }

    }
}
