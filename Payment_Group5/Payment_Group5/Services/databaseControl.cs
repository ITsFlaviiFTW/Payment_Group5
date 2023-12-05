using System;
using System.Data.SqlClient;

namespace Payment_Group5.Services
{
    public class DatabaseControl
    {
        public static void DatabaseConnection(SqlConnection connection)
        {
            // Example query to fetch data from the Orders table
            string query = "SELECT OrderID, CustomerID, NumberOfItems, TotalBeforeTax, TotalAfterTax, PaymentMethod, PurchaseDateTime, AveragePrice FROM group5DB";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Access data using column names or indices
                        int orderID = reader.GetInt32(0);
                        int customerID = reader.GetInt32(1);
                        int numberOfItems = reader.GetInt32(2);
                        decimal totalBeforeTax = reader.GetDecimal(3);
                        decimal totalAfterTax = reader.GetDecimal(4);
                        string paymentMethod = reader.GetString(5);
                        DateTime purchaseDateTime = reader.GetDateTime(6);
                        decimal averagePrice = reader.GetDecimal(7);

                        // Perform operations with the fetched data (e.g., display, process, etc.)
                        /* Console.WriteLine($"OrderID: {orderID}, CustomerID: {customerID}, Items: {numberOfItems}, TotalBeforeTax: {totalBeforeTax}," +
                            $" TotalAfterTax: {totalAfterTax}, PaymentMethod: {paymentMethod}, PurchaseDateTime: {purchaseDateTime}, AveragePrice: {averagePrice}"); */
                    }
                }
            }
        }
    }
}
