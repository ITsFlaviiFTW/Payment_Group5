using Payment_Group5.Services;
using PaymentModuleDemo;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IReceiptGenerator, ReceiptGenerator>();

// Establish the database connection
string connectionString = "Server=tcp:group5-payment-server.database.windows.net,1433;Initial Catalog=Group5-Payment-SQLDatabase;Persist Security Info=False;User ID=ktargosz;Password=paymentGr0up;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    // Pass the connection to the DatabaseControl class
    DatabaseControl.DatabaseConnection(connection);
    DatabaseControl.ExportOrdersToTxt(connection, "DataBase.txt");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();

