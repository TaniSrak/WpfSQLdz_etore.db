using System;
using System.Data;
using System.Windows;
using System.Data.SQLite;
using Microsoft.Win32;


namespace WpfSQLdz
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=store.db;Version=3;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPurchases();
        }

        //загрузка покупок
        private void LoadPurchases()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Purchase"; //загрузили
                SQLiteDataAdapter da = new SQLiteDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridPurchases.ItemsSource = dt.DefaultView; //отобразили
            }

        }

        private void LoadPurchases1()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Product (ProductName, Price, CategoryId) VALUES ('Watch', 199.99, 1)";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
        }

        //пример
        private void ExecuteQuery1_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(DISTINCT CustomerID) " +
                    "FROM Purchase" +
                    "JOIN Product ON Purchase.ProductID = Product.ProductID" +
                    "WHERE Product.ProductName = 'Часы'" +
                    "AND strftime('%Y', Purchase.PurchaseDate) = '2023';";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                var result = command.ExecuteScalar();
                MessageBox.Show($"Количество покупателей, купивших часы в 2023 году: {result}");

            }
        }

        private void ExecuteQuery2_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT AVG(Age)" +
                    "FROM Customer" +
                    "JOIN Purchase ON Customer.CustomerID = Purchase.CustomerID" +
                    "JOIN Product ON Purchase.ProductID = Product.ProductID" +
                    "WHERE Product.ProductName = 'Кулон'" +
                    "AND PurchaseDate = '2023-02-14';";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                var result = command.ExecuteScalar();
                MessageBox.Show($"Средний возраст покупателей купивших кулон на 14 февраля: {result}");
            }
        }

        private void ExecuteQuery3_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT AVG(TotalAmount) FROM Purchase WHERE strftime('%m-%d', PurchaseDate) = '12-31';";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                var result = command.ExecuteScalar();
                MessageBox.Show($"Средний чек покупок 31 декабря: {result}");
            }
        }

    }
}