namespace PaymentTestEnv
{
    [TestClass]
    public class PaymentControllerTests
    {
        private Mock<IReceiptGenerator>? receiptGeneratorMock;
        private PaymentController? controller;

        [TestInitialize]
        public void SetUp()
        {
            // Arrange
            receiptGeneratorMock = new Mock<IReceiptGenerator>();
            controller = new PaymentController(receiptGeneratorMock.Object);
        }

        [TestMethod]
        public void ProcessPayment_ReturnsOkResult_WithReceipt()
        {
            // Arrange
            var paymentInfo = new PaymentInfo
            {
                CustomerID = 1,
                Total = 100,
                Products = new List<int> { 1, 2, 3 } // Example product IDs
            };

            var expectedReceipt = "Receipt Content";
            receiptGeneratorMock.Setup(r => r.GenerateReceipt(It.IsAny<User>(), It.IsAny<Transaction>()))
                                .Returns(expectedReceipt);

            // Act
            var result = controller.ProcessPayment(paymentInfo);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(object));
            Assert.AreEqual(expectedReceipt, ((dynamic)okResult.Value).Receipt);
        }

        // Additional tests could be written to check for different outcomes of ProcessPayment,
        // such as handling invalid input, failed transactions, etc.
    }
}