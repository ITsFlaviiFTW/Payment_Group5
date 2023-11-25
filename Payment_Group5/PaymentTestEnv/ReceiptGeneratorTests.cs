namespace PaymentTestEnv
{
    [TestClass]
    public class ReceiptGeneratorTests
    {
        private Mock<IProductService>? productServiceMock;
        private ReceiptGenerator? receiptGenerator;

        [TestInitialize]
        public void SetUp()
        {
            // Arrange
            productServiceMock = new Mock<IProductService>();
            receiptGenerator = new ReceiptGenerator(productServiceMock.Object);
        }

        [TestMethod]
        public void GenerateReceipt_ReturnsValidReceipt()
        {
            // Arrange
            var user = new User { UserId = 1, Name = "John Doe", Email = "john@example.com" };
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                UserId = user.UserId,
                ProductIds = new List<int> { 1, 2 },
                Amount = 100,
                Tax = 10,
                Total = 110,
                TransactionDate = DateTime.Now
            };

            productServiceMock.Setup(s => s.GetProductById(1)).Returns(new ProductDetail { ProductId = 1, Name = "Product 1", Price = 50 });
            productServiceMock.Setup(s => s.GetProductById(2)).Returns(new ProductDetail { ProductId = 2, Name = "Product 2", Price = 50 });

            // Expected receipt content (simplified for the example)
            var expectedReceipt = $"Transaction ID: {transaction.TransactionId}";

            // Act
            var receipt = receiptGenerator.GenerateReceipt(user, transaction);

            // Assert
            Assert.IsNotNull(receipt);
            Assert.IsTrue(receipt.Contains(expectedReceipt));
            Assert.IsTrue(receipt.Contains("Product 1"));
            Assert.IsTrue(receipt.Contains("Product 2"));
            // Add additional assertions for other parts of the receipt as needed
        }
    }
}
