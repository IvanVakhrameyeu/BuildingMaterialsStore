using BuildingMaterialsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Word = Microsoft.Office.Interop.Word;
namespace BuildingMaterialsStore.ViewModels.WordReports
{
    class EmpReport
    {
        private static void ReplaceWordStub(string stubToReplace, string text, Word._Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }
        static public void writeClass(DateTime dateFrom, DateTime dateTo, string nameFile,string Who, string Name)
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
                ReplaceWordStub("{Who}", Who, wordDocument);
                ReplaceWordStub("{Date}", DateTime.Now.ToString(), wordDocument);
                ReplaceWordStub("{DateFrom}", dateFrom.ToString(), wordDocument);
                ReplaceWordStub("{DateTo}", dateTo.ToString(), wordDocument);
                ReplaceWordStub("{Name}", Name, wordDocument);
                wordDocument.SaveAs("Buf" + templatePathObj);
                wordApplication.Visible = true;
            }
            catch { }

            wordApplication.Selection.Find.Execute("{Table}");
            Word.Range wordRange = wordApplication.Selection.Range;

            var wordTable = wordDocument.Tables.Add(wordRange,
                ds.Count + 1, 6);

            wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
            wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;

            wordTable.Cell(1, 1).Range.Text = "Название товара";
            wordTable.Cell(1, 2).Range.Text = "Количество";
            wordTable.Cell(1, 3).Range.Text = "Ед измерения";
            wordTable.Cell(1, 4).Range.Text = "Цена за ед";
            wordTable.Cell(1, 5).Range.Text = "Общая цена";
            wordTable.Cell(1, 6).Range.Text = "Описание";

            for (int i = 2; i < ds.Count + 2; i++)
            {
                wordTable.Cell(i, 1).Range.Text = ds[i - 2].storage.Name;
                wordTable.Cell(i, 2).Range.Text = ds[i - 2].Count.ToString();
                wordTable.Cell(i, 3).Range.Text = ds[i - 2].storage.UnitName;
                wordTable.Cell(i, 4).Range.Text = ds[i - 2].storage.Price.ToString();
                wordTable.Cell(i, 5).Range.Text = ds[i - 2].Total.ToString();
                wordTable.Cell(i, 6).Range.Text = ds[i - 2].storage.Description;
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
