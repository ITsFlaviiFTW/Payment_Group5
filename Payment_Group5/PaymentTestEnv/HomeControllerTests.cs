namespace PaymentTestEnv
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController? controller;
        private Mock<ILogger<HomeController>>? loggerMock;
        private Mock<IReceiptGenerator>? receiptGeneratorMock;

        [TestInitialize]
        public void Setup()
        {
            // Mock the ILogger
            loggerMock = new Mock<ILogger<HomeController>>();

            // Mock the IReceiptGenerator
            receiptGeneratorMock = new Mock<IReceiptGenerator>();

            // Set up your controller with the required mocks
            controller = new HomeController(loggerMock.Object, receiptGeneratorMock.Object);
        }

        [TestMethod]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Error_ReturnsViewResult_WithErrorViewModel()
        {
            // Simulate setting the TraceIdentifier
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(hc => hc.TraceIdentifier).Returns("TestTraceIdentifier");
            controller.ControllerContext = new ControllerContext() { HttpContext = httpContext.Object };

            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ErrorViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual("TestTraceIdentifier", model.RequestId);
        }

        [TestMethod]
        public void GenerateReceipt_ReturnsViewResult_WithReceipt()
        {
            // Arrange
            var user = new User() { Name = "John Doe", Email = "john@example.com" };
            var transaction = new PaymentModuleDemo.Transaction() { Amount = 250, TransactionDate = DateTime.Now };
            var expectedReceipt = "Expected Receipt Content";

            receiptGeneratorMock.Setup(r => r.GenerateReceipt(user, transaction)).Returns(expectedReceipt);

            // Act
            var result = controller.GenerateReceipt() as ViewResult;

            // Assert
            receiptGeneratorMock.Verify(r => r.GenerateReceipt(It.IsAny<User>(), It.IsAny<Transaction>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReceipt, result.ViewData.Model);
        }
    }
}