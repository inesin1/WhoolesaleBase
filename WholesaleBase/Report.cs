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
                        if ((item.ProductName + "\r\a").Equals(table.Rows[i].Cells[2].Range.Text))
                        {
                            isRepeat = true;
                            rowRepeat = i;
                        }
                    }

                    if (!isRepeat)
                    {
                        row.Cells[1].Range.Text = month[item.Date.Month - 1];
                        row.Cells[2].Range.Text = item.ProductName;
                        row.Cells[3].Range.Text = item.ProductAmount.ToString();
                        row.Cells[4].Range.Text = item.ProductUnitPrice.ToString();
                        row.Cells[5].Range.Text = item.ProductCost.ToString();
                    }
                    else {
                        decimal amount = Convert.ToDecimal(table.Rows[rowRepeat].Cells[3].Range.Text.Substring(0, table.Rows[rowRepeat].Cells[3].Range.Text.Length - 2)) + Convert.ToDecimal(item.ProductAmount);
                        table.Rows[rowRepeat].Cells[3].Range.Text = amount.ToString();

                        decimal cost = Convert.ToDecimal(table.Rows[rowRepeat].Cells[3].Range.Text.Substring(0, table.Rows[rowRepeat].Cells[3].Range.Text.Length - 2)) * Convert.ToDecimal(table.Rows[rowRepeat].Cells[4].Range.Text.Substring(0, table.Rows[rowRepeat].Cells[4].Range.Text.Length - 2));
                        table.Rows[rowRepeat].Cells[5].Range.Text = cost.ToString();
                    }

                    if (table.Rows[table.Rows.Count].Cells[1].Range.Text == "\r\a") table.Range.Tables[1].Rows[table.Rows.Count].Delete();
                }
                doc.Bookmarks["Table"].Range.Tables[1].Rows[2].Delete(); //Удаляем строку [текст] [текст] [текст] [текст] в таблице

                app.Visible = true;
            }
        }

/*        public void efficiencyGen(IList<sales_invoice> sales)
        {
            if (processors != null)
            {
                doc = app.Documents.Add(Template: $@"{Environment.CurrentDirectory}\Templates\Процессоры.docx", Visible: true);

                Word.Range dateTime = doc.Bookmarks["DateTime"].Range;
                dateTime.Text = DateTime.Now.ToString();

                Word.Table table = doc.Bookmarks["Table"].Range.Tables[1];
                int currPage = 1;
                foreach (var item in processors)
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

                    row.Cells[1].Range.Text = item.Name;
                    row.Cells[2].Range.Text = item.Frequrency;
                    row.Cells[3].Range.Text = item.Socket;
                    row.Cells[4].Range.Text = item.Price.ToString();
                }
                doc.Bookmarks["Table"].Range.Tables[1].Rows[2].Delete(); //Удаляем строку [текст] [текст] [текст] [текст] в таблице

                app.Visible = true;
            }
        }*/
    }
}
