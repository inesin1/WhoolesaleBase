using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WholesaleBase
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public enum TableType
    {
        Products, Units, Category, Managers, Buyers, Orders, Sales
    }

    public partial class MainWindow : Window
    {
        DbService db; //Для работы с базой
        TableType currentTableType; //Хранит текущую открытую таблицу

        public MainWindow()
        {
            //new LoginWindow().ShowDialog();

            InitializeComponent();

            db = new DbService();
            currentTableType = TableType.Products;
            RefreshTable(currentTableType);
        }

        //Методы
        //Обновление таблиц
        private void RefreshTable(TableType tt)
        {
            db = new DbService();
            CollectionViewSource vs = new CollectionViewSource();
            switch (tt)
            {
                case TableType.Products:
                    db.products.Load();

                    vs.Source = db.products.Local;
                    this.productsTable.ItemsSource = vs.View;
                    this.productsTable.AddingNewItem += (sender, e) => e.NewItem = new product() { Name = "<новый>", Unit = 0, Category = 0, UnitPrice = 0 };
                    
                    this.colCategory.ItemsSource = db.categories.ToArray();
                    this.colUnit.ItemsSource = db.units.ToArray();

                    Views.ProductsView = vs;
                    break;
                case TableType.Units:
                    db.units.Load();

                    vs.Source = db.units.Local;
                    this.unitsTable.ItemsSource = vs.View;
                    this.unitsTable.AddingNewItem += (sender, e) => e.NewItem = new unit() { Name = "<новый>"};

                    Views.UnitsView = vs;
                    break;
                case TableType.Category:
                    db.categories.Load();

                    vs.Source = db.categories.Local;
                    this.categoryTable.ItemsSource = vs.View;
                    this.categoryTable.AddingNewItem += (sender, e) => e.NewItem = new category() { Name = "<новый>" };

                    Views.CategoryView = vs;
                    break;
                case TableType.Managers:
                    db.managers.Load();

                    vs.Source = db.managers.Local;
                    this.managersTable.ItemsSource = vs.View;
                    this.managersTable.AddingNewItem += (sender, e) => e.NewItem = new manager() { Name = "<новый>", Surname = "", MiddleName = "" };

                    Views.ManagersView = vs;
                    break;
                case TableType.Buyers:
                    db.buyers.Load();

                    vs.Source = db.buyers.Local;
                    this.buyersTable.ItemsSource = vs.View;
                    this.buyersTable.AddingNewItem += (sender, e) => e.NewItem = new buyer() { Name = "<новый>", Surname = "", MiddleName = "" };

                    Views.BuyersView = vs;
                    break;
                case TableType.Orders:
                    db.orders.Load();

                    vs.Source = db.orders.Local;
                    this.orderTable.ItemsSource = vs.View;
                    this.orderTable.AddingNewItem += (sender, e) => e.NewItem = new order() { Date = DateTime.Now.Date, Buyer = 0, ProductName = 0, ProductAmount = 0 };

                    this.colBuyer.ItemsSource = db.buyers.ToArray();
                    this.colProductName.ItemsSource = db.products.ToArray();

                    Views.OrdersView = vs;
                    break;
                case TableType.Sales:
                    db.sales_invoice.Load();

                    vs.Source = db.sales_invoice.Local;
                    this.salesTable.ItemsSource = vs.View;
                    this.salesTable.AddingNewItem += (sender, e) => e.NewItem = new sales_invoice() { Date = DateTime.Now.Date, Buyer = "<Новый>", Manager = 0, ProductName = "<Новый>", ProductUnitPrice = 1, ProductAmount = 1, ProductCost = 1, TotalCost = 1 };

                    this.colManager.ItemsSource = db.managers.ToArray();
                    this.colOrderNum.ItemsSource = db.orders.ToArray();

                    Views.SalesView = vs;
                    break;
            }
        }

        //Сохранение изменений
        private void SaveChanges(TableType tt)
        {
            db.SaveChanges();

            DataGrid currTable = null;
            switch (tt)
            {
                case TableType.Products:
                    currTable = productsTable;

                    break;
                case TableType.Category:
                    currTable = categoryTable;
                    break;
                case TableType.Units:
                    currTable = unitsTable;
                    break;
                case TableType.Managers:
                    currTable = managersTable;
                    break;
                case TableType.Buyers:
                    currTable = buyersTable;
                    break;
                case TableType.Orders:
                    currTable = orderTable;
                    break;
                case TableType.Sales:
                    currTable = salesTable;
                    break;
            }

            int si = currTable.SelectedIndex;
            RefreshTable(tt);
            currTable.SelectedIndex = si;
        }

        //Удаление записи
        private void DeleteRecord(TableType tt)
        {
            switch (tt)
            {
                case TableType.Category:
                    if (categoryTable.SelectedItem is category v && v.products.Count == 0)
                        db.categories.Local.Remove(v);
                    else
                        MessageBox.Show("Данное комплектующее уже содержится в сборках. Удаление невозможно!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case TableType.Units:
                    if (unitsTable.SelectedItem is unit p && p.products.Count == 0)
                        db.units.Local.Remove(p);
                    else
                        MessageBox.Show("Данное комплектующее уже содержится в сборках. Удаление невозможно!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;

                case TableType.Products:
                    if (productsTable.SelectedItem is product b)
                        db.products.Local.Remove(b);
                    else
                        MessageBox.Show("Данное комплектующее уже содержится в сборках. Удаление невозможно!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case TableType.Managers:
                    if (managersTable.SelectedItem is manager c)
                        db.managers.Local.Remove(c);
                    else
                        MessageBox.Show("Данное комплектующее уже содержится в сборках. Удаление невозможно!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case TableType.Buyers:
                    if (buyersTable.SelectedItem is buyer d)
                        db.buyers.Local.Remove(d);
                    else
                        MessageBox.Show("Данное комплектующее уже содержится в сборках. Удаление невозможно!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;

                case TableType.Orders:
                    if (orderTable.SelectedItem is order e)
                        db.orders.Local.Remove(e);
                    else
                        MessageBox.Show("Данное комплектующее уже содержится в сборках. Удаление невозможно!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case TableType.Sales:
                    if (salesTable.SelectedItem is sales_invoice f)
                        db.sales_invoice.Local.Remove(f);
                    else
                        MessageBox.Show("Данное комплектующее уже содержится в сборках. Удаление невозможно!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;

            }
        }


        //Кнопки
        private void RefreshTable_Button(object sender, RoutedEventArgs e)
        {
            RefreshTable(currentTableType);
        }

        private void SaveChanges_Button(object sender, RoutedEventArgs e)
        {
            SaveChanges(currentTableType);
        }

        private void DeleteRecord_Button(object sender, RoutedEventArgs e)
        {
            DeleteRecord(currentTableType);
        }

        private void UpdateTablesButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (order order in db.orders.Local)
            {
                order.ProductUnitPrice = order.product.UnitPrice;
            }

            foreach (sales_invoice sales in db.sales_invoice.Local)
            {
                sales.Buyer = Convert.ToString(sales.order.buyer1.Name);
                sales.ProductName = sales.order.product.Name;
                sales.ProductAmount = sales.order.ProductAmount;
                sales.ProductUnitPrice = sales.order.ProductUnitPrice;
                sales.ProductCost = sales.ProductUnitPrice * sales.ProductAmount;
            }


            //Сохраняем на сервер
            SaveChanges(currentTableType);
        }





        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TabItem ti)
            {
                TableType old = currentTableType;

                string header = ti.Header.ToString();
                if (header == "Товары")
                    currentTableType = TableType.Products;
                else if (header == "Категории")
                    currentTableType = TableType.Category;
                else if (header == "Единицы измерения")
                    currentTableType = TableType.Units;
                else if (header == "Менеджеры")
                    currentTableType = TableType.Managers;
                else if (header == "Покупатели")
                    currentTableType = TableType.Buyers;
                else if (header == "Заказ Покупателя")
                    currentTableType = TableType.Orders;
                else if (header == "Расходная Накладная")
                    currentTableType = TableType.Sales;

                if (currentTableType != old)
                    RefreshTable(currentTableType);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            switch (currentTableType)
            {
                case TableType.Category:
                    Views.CategoryView.Filter += (o, ea) =>
                    {
                        if (ea.Item is category v)
                        {
                            string name = v.Name.ToLower();

                            if (name.Contains(categorySearchName.Text.ToLower()))
                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
                case TableType.Units:
                    Views.UnitsView.Filter += (o, ea) =>
                    {
                        if (ea.Item is unit p)
                        {
                            string name = p.Name.ToLower();

                            if (name.Contains(unitSearchName.Text.ToLower()))
                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
                case TableType.Products:
                    Views.ProductsView.Filter += (o, ea) =>
                    {
                        if (ea.Item is product p)
                        {
                            string name = p.Name.ToLower();
                            string category = p.category1.Name.ToLower();
                            string unit = p.unit1.Name.ToLower();
                            decimal price = p.UnitPrice;
                            decimal price1 = decimal.Parse(productSearchPrice1.Text);
                            decimal price2 = decimal.Parse(productSearchPrice2.Text);

                            price2 = (price2 == 0 ? 1000000 : price2);

                            if (name.Contains(productSearchName.Text.ToLower()) &&
                                category.Contains(productSearchCategory.Text.ToLower()) &&
                                unit.Contains(productSearchUnits.Text.ToLower()) &&
                                price.CompareTo(price1) >= 0 && price.CompareTo(price2) <= 0)
                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
                case TableType.Buyers:
                    Views.BuyersView.Filter += (o, ea) =>
                    {
                        if (ea.Item is buyer p)
                        {
                            string name = p.Name.ToLower();
                            string surname = p.Surname.ToLower();
                            string middleName = p.MiddleName.ToLower();

                            if (name.Contains(buyerSearchName.Text.ToLower()) &&
                                surname.Contains(buyerSearchSurname.Text.ToLower()) &&
                                middleName.Contains(buyerSearchMiddleName.Text.ToLower())
                                )
                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
                case TableType.Managers:
                    Views.ManagersView.Filter += (o, ea) =>
                    {
                        if (ea.Item is manager p)
                        {
                            string name = p.Name.ToLower();
                            string surname = p.Surname.ToLower();
                            string middleName = p.MiddleName.ToLower();

                            if (name.Contains(managerSearchName.Text.ToLower()) &&
                                surname.Contains(managerSearchSurname.Text.ToLower()) &&
                                middleName.Contains(managerSearchMiddleName.Text.ToLower())
                                )
                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
                case TableType.Orders:
                    Views.OrdersView.Filter += (o, ea) =>
                    {
                        if (ea.Item is order p)
                        {
                            string id = p.ID.ToString();
                            DateTime date = p.Date;
                            string buyer = p.Buyer.ToString();
                            string productName = p.ProductName.ToString();

                            if (id.Contains(orderSearchID.Text.ToLower()) &&
                                buyer.Contains(orderSearchBuyer.Text.ToLower()) &&
                                productName.Contains(orderSearchName.Text.ToLower()))
                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
                case TableType.Sales:
                    Views.SalesView.Filter += (o, ea) =>
                    {
                        if (ea.Item is sales_invoice p)
                        {
                            string id = p.ID.ToString();
                            DateTime date = p.Date;
                            string buyer = p.Buyer.ToString();
                            string manager = p.Manager.ToString();
                            string productName = p.ProductName.ToString();
                            decimal price = p.ProductCost;
                            decimal price1 = decimal.Parse(saleSearchPrice1.Text);
                            decimal price2 = decimal.Parse(saleSearchPrice2.Text);

                            price2 = (price2 == 0 ? 1000000 : price2);

                            if (id.Contains(saleSearchID.Text.ToLower()) &&
                                buyer.Contains(saleSearchBuyer.Text.ToLower()) &&
                                manager.Contains(saleSearchManager.Text.ToLower()) &&
                                productName.Contains(saleSearchName.Text.ToLower()) &&
                                price.CompareTo(price1) >= 0 && price.CompareTo(price2) <= 0)
                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
            }
        }

        //Кнопка очистки полей
        private void CancelSearchButton_Click(object sender, RoutedEventArgs e)
        {
            switch (currentTableType)
            {
                case TableType.Category:
                    Views.CategoryView.Filter += (o, ea) => ea.Accepted = true;

                    categorySearchName.Text = "";
                    break;
                case TableType.Units:
                    Views.UnitsView.Filter += (o, ea) => ea.Accepted = true;

                    unitSearchName.Text = "";
                    break;
                case TableType.Products:
                    Views.ProductsView.Filter += (o, ea) => ea.Accepted = true;

                    productSearchName.Text = "";
                    productSearchCategory.Text = "";
                    productSearchUnits.Text = "";
                    productSearchPrice1.Text = "0";
                    productSearchPrice2.Text = "0";
                    break;
                case TableType.Buyers:
                    Views.BuyersView.Filter += (o, ea) => ea.Accepted = true;

                    buyerSearchName.Text = "";
                    buyerSearchSurname.Text = "";
                    buyerSearchMiddleName.Text = "";
                    break;
                case TableType.Managers:
                    Views.ManagersView.Filter += (o, ea) => ea.Accepted = true;

                    buyerSearchName.Text = "";
                    buyerSearchSurname.Text = "";
                    buyerSearchMiddleName.Text = "";
                    break;
                case TableType.Orders:
                    Views.OrdersView.Filter += (o, ea) => ea.Accepted = true;

                    orderSearchID.Text = "";
                    orderSearchDate.Text = "";
                    orderSearchBuyer.Text = "";
                    orderSearchName.Text = "";
                    break;
                case TableType.Sales:
                    Views.SalesView.Filter += (o, ea) => ea.Accepted = true;

                    saleSearchID.Text = "";
                    saleSearchDate.Text = "";
                    saleSearchBuyer.Text = "";
                    saleSearchManager.Text = "";
                    saleSearchName.Text = "0";
                    saleSearchPrice2.Text = "0";
                    saleSearchPrice2.Text = "0";
                    break;
            }
        }

        private void SearchOnlyDigits_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void SearchPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text.Trim() == "") tb.Text = "0";
        }

        //Создание отчета
        Report report;
        private void SalesReportButton_Click(object sender, RoutedEventArgs e)
        {
            report = new Report();
            report.SalesForMonthGen(Views.SalesView.Source as IList<sales_invoice>);
        }

        private void efficiencyReportButton_Click(object sender, RoutedEventArgs e)
        {
            report = new Report();
            report.SalesForMonthGen(Views.SalesView.Source as IList<sales_invoice>);
        }
    }
}
