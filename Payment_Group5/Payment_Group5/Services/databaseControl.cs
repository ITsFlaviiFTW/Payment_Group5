using System;
using System.Data.SqlClient;
using System.IO;

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
                        string customerID = reader.GetString(1);
                        int numberOfItems = reader.GetInt32(2);
                        decimal totalBeforeTax = reader.GetDecimal(3);
                        decimal totalAfterTax = reader.GetDecimal(4);
                        string paymentMethod = reader.GetString(5);
                        DateTime purchaseDateTime = reader.GetDateTime(6);
                        decimal averagePrice = reader.GetDecimal(7);

                        // Perform operations with the fetched data
                        // Console.WriteLine($"OrderID: {orderID}, CustomerID: {customerID}, Items: {numberOfItems}, TotalBeforeTax: {totalBeforeTax}," +
                        //    $" TotalAfterTax: {totalAfterTax}, PaymentMethod: {paymentMethod}, PurchaseDateTime: {purchaseDateTime}, AveragePrice: {averagePrice}");
                    }
                }
            }
        }

        public static void InsertOrder(SqlConnection connection, int orderID, string customerID, int numberOfItems, decimal totalBeforeTax, decimal totalAfterTax, string paymentMethod, DateTime purchaseDateTime, decimal averagePrice)
        {
            // Example query to insert data into the Orders table
            string insertQuery = "INSERT INTO group5DB (OrderID, CustomerID, NumberOfItems, TotalBeforeTax, TotalAfterTax, PaymentMethod, PurchaseDateTime, AveragePrice) " +
                                "VALUES (@OrderID, @CustomerID, @NumberOfItems, @TotalBeforeTax, @TotalAfterTax, @PaymentMethod, @PurchaseDateTime, @AveragePrice)";

            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                // Add parameters to the query to prevent SQL injection
                command.Parameters.AddWithValue("@OrderID", orderID);
                command.Parameters.AddWithValue("@CustomerID", customerID);
                command.Parameters.AddWithValue("@NumberOfItems", numberOfItems);
                command.Parameters.AddWithValue("@TotalBeforeTax", totalBeforeTax);
                command.Parameters.AddWithValue("@TotalAfterTax", totalAfterTax);
                command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                command.Parameters.AddWithValue("@PurchaseDateTime", purchaseDateTime);
                command.Parameters.AddWithValue("@AveragePrice", averagePrice);

                // Execute the INSERT INTO query
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Order inserted successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to insert order.");
                }
            }
        }

        public static void ExportOrdersToTxt(SqlConnection connection, string outputFile)
        {
            // Example query to fetch data from the Orders table
            string query = "SELECT OrderID, CustomerID, NumberOfItems, TotalBeforeTax, TotalAfterTax, PaymentMethod, PurchaseDateTime, AveragePrice FROM group5DB";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    using (StreamWriter writer = new StreamWriter(outputFile))
                    {
                        // Write header row
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            writer.Write($"{reader.GetName(i)}\t");
                        }
                        writer.WriteLine();

                        // Write data rows
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                writer.Write($"{reader[i]}\t");
                            }
                            writer.WriteLine();
                        }
                    }
                }
            }

            Console.WriteLine($"Exported data to {outputFile} successfully.");
        }

        public static int GetHighestOrderID(SqlConnection connection)
        {
            int highestOrderID = 0;

            // Example query to get the highest OrderID
            string query = "SELECT MAX(orderID) FROM group5DB;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Execute the SELECT query and get the result
                object result = command.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    highestOrderID = Convert.ToInt32(result);
                }
            }

            return highestOrderID;
        }
    }
}
