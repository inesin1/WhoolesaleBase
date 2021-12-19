using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace WholesaleBase
{
    class Report
    {
        Word.Application app = new Word.Application();
        Word.Document doc;

        private string[] month = new string[12] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль",
            "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

        ~Report()
        {
            doc.Saved = true;
            try { app.Quit(); }
            catch { }
        }


        public void SalesForMonthGen(IList<sales_invoice> sales)
        {
            if (sales != null)
            {
                doc = app.Documents.Add(Template: $@"{Environment.CurrentDirectory}\Templates\ПродажиЗаМесяц.docx", Visible: true);

                Word.Range dateTime = doc.Bookmarks["DateTime"].Range;
                dateTime.Text = DateTime.Now.ToString();

                Word.Table table = doc.Bookmarks["Table"].Range.Tables[1];
                int currPage = 1;
                foreach (var item in sales)
                {
                    int page = doc.ComputeStatistics(Word.WdStatistic.wdStatisticPages);

                    Word.Row row = table.Rows.Add();
                    if (page > currPage) //Если запись не влезает на текущую страницу
                    {
                        row.Range.InsertBreak();
                        table = doc.Tables[doc.Tables.Count];

                        doc.Tables[1].Rows[1].Range.Copy();
                        row.Range.Paste();
                        table.Rows[2].Delete(); //Удаляем пустую строку после заголовка

                        currPage = page;
                        row = table.Rows.Add();
                    }

                    bool isRepeat = false;
                    int rowRepeat = 1;
                    for (int i = 1; i <= table.Rows.Count; i++)
                    {
                        //Проверяем, повторяется ли товар и месяц
                        if ((item.ProductName + "\r\a").Equals(table.Rows[i].Cells[2].Range.Text) && (month[item.Date.Month - 1] + "\r\a").Equals(table.Rows[i].Cells[1].Range.Text))
                        {
                            isRepeat = true;
                            rowRepeat = i; //Сохраняем индекс его строки
                        }
                    }

                    if (!isRepeat)
                    {
                        //Если товар не повторяется, то выводим его в новую строку
                        row.Cells[1].Range.Text = month[item.Date.Month - 1]; //Месяц берем из массива
                        row.Cells[2].Range.Text = item.ProductName;
                        row.Cells[3].Range.Text = item.ProductAmount.ToString();
                        row.Cells[4].Range.Text = item.ProductUnitPrice.ToString();
                        row.Cells[5].Range.Text = item.ProductCost.ToString();
                    }
                    else {
                        //Если товар повторился, то пересчитываем его количество в той строке, в которой он уже существует, дабы не повторяться
                        decimal amount = Convert.ToDecimal(table.Rows[rowRepeat].Cells[3].Range.Text.Substring(0, table.Rows[rowRepeat].Cells[3].Range.Text.Length - 2)) + Convert.ToDecimal(item.ProductAmount);
                        table.Rows[rowRepeat].Cells[3].Range.Text = amount.ToString();

                        //То же самое, но с пересчетом стоимости
                        decimal cost = Convert.ToDecimal(table.Rows[rowRepeat].Cells[3].Range.Text.Substring(0, table.Rows[rowRepeat].Cells[3].Range.Text.Length - 2)) * Convert.ToDecimal(table.Rows[rowRepeat].Cells[4].Range.Text.Substring(0, table.Rows[rowRepeat].Cells[4].Range.Text.Length - 2));
                        table.Rows[rowRepeat].Cells[5].Range.Text = cost.ToString();
                    }

                    //Удаляем пустые строки в конце
                    if (table.Rows[table.Rows.Count].Cells[1].Range.Text == "\r\a") table.Range.Tables[1].Rows[table.Rows.Count].Delete();
                }
                doc.Bookmarks["Table"].Range.Tables[1].Rows[2].Delete(); //Удаляем строку [текст] [текст] [текст] [текст] в таблице

                app.Visible = true;
            }
        }

        public void efficiencyGen(IList<sales_invoice> sales)
        {
            if (sales != null)
            {
                doc = app.Documents.Add(Template: $@"{Environment.CurrentDirectory}\Templates\ЭффективностьПредприятия.docx", Visible: true);

                Word.Range dateTime = doc.Bookmarks["DateTime"].Range;
                dateTime.Text = DateTime.Now.ToString();

                Word.Table table = doc.Bookmarks["Table"].Range.Tables[1];
                int currPage = 1;
                int amount = 1; //Кол-во накладных
                foreach (var item in sales)
                {
                    int page = doc.ComputeStatistics(Word.WdStatistic.wdStatisticPages);

                    Word.Row row = table.Rows.Add();
                    if (page > currPage) //Если запись не влазеет на текущею страницу
                    {
                        row.Range.InsertBreak();
                        table = doc.Tables[doc.Tables.Count];

                        doc.Tables[1].Rows[1].Range.Copy();
                        row.Range.Paste();
                        table.Rows[2].Delete(); //Удаляем пустую строку после заголовка

                        currPage = page;
                        row = table.Rows.Add();
                    }

                    bool isRepeat = false;
                    int rowRepeat = 1;
                    for (int i = 1; i <= table.Rows.Count; i++)
                    {
                        //Проверяем, повторяется ли месяц
                        if ((month[item.Date.Month - 1] + "\r\a").Equals(table.Rows[i].Cells[1].Range.Text))
                        {
                            isRepeat = true;
                            rowRepeat = i; //Сохраняем индекс его строки
                        }
                    }

                    if (isRepeat)
                    {
                        //Если месяц повторился
                        amount++; //Увеличиваем кол-во накладных
                        table.Rows[rowRepeat].Cells[2].Range.Text = amount.ToString(); //Приравниваем новое кол-во накладных в повторяющийся месяц
                    }
                    else
                    {
                        //Если это новый месяц
                        row.Cells[1].Range.Text = month[item.Date.Month - 1]; //Месяц берем из массива
                        row.Cells[2].Range.Text = "1";
                        amount = 1; //Сбрасываем кол-во накладных
                    }

                    //Удаляем пустые строки в конце
                    if (table.Rows[table.Rows.Count].Cells[1].Range.Text == "\r\a") table.Range.Tables[1].Rows[table.Rows.Count].Delete();
                }

                doc.Bookmarks["Table"].Range.Tables[1].Rows[2].Delete(); //Удаляем строку [текст] [текст] [текст] [текст] в таблице

                app.Visible = true;
            }
        }
    }
}
