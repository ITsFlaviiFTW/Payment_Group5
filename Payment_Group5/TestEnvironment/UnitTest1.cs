using Payment_Group5.Models;
using PaymentModuleDemo;
using PaymentModuleDemo.Models;

namespace TestEnvironment
{
    [TestClass]
    public class ReceiptGeneratorTests
    {
        [TestMethod]
        public void GenerateReceipt_ValidData_ReturnsFormattedReceipt()
        {
            // Arrange
            var receiptGenerator = new ReceiptGenerator();
            var testData = new dataBaseData
            {
                orderId = 12345,
                customerId = "Cust100",
                subtotal = 100m,
                total = 113m,
                date = new DateTime(2023, 1, 1)
            };

            // Act
            var receipt = receiptGenerator.GenerateReceipt(testData);

            // Assert
            Assert.IsTrue(receipt.Contains("Payment Receipt"));
            Assert.IsTrue(receipt.Contains($"Transaction ID: {testData.orderId + testData.date.Minute}"));
            Assert.IsTrue(receipt.Contains($"CustomerId: {testData.customerId}"));
            Assert.IsTrue(receipt.Contains($"Subtotal: {testData.subtotal:C}"));
            Assert.IsTrue(receipt.Contains($"Tax: {testData.total * 0.13m:C}"));
            Assert.IsTrue(receipt.Contains($"Total: {testData.total:C}"));
            Assert.IsTrue(receipt.Contains($"Date: {testData.date}"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateReceipt_NullData_ThrowsArgumentNullException()
        {
            // Arrange
            var receiptGenerator = new ReceiptGenerator();

            // Act
            receiptGenerator.GenerateReceipt(null);
        }

        [TestMethod]
        public void GenerateReceipt_ZeroTotal_ReturnsReceiptWithZeroTotal()
        {
            // Arrange
            var receiptGenerator = new ReceiptGenerator();
            var testData = new dataBaseData
            {
                total = 0m
                // Initialize other required properties
            };

            // Act
            var receipt = receiptGenerator.GenerateReceipt(testData);

            // Assert
            Assert.IsTrue(receipt.Contains("Total: $0.00"));
        }

        [TestMethod]
        public void GenerateReceipt_NegativeTotal_ReturnsReceiptWithNegativeTotal()
        {
            // Arrange
            var receiptGenerator = new ReceiptGenerator();
            var testData = new dataBaseData
            {
                total = -50m
                // Initialize other required properties
            };

            // Act
            var receipt = receiptGenerator.GenerateReceipt(testData);

            // Assert
            Assert.IsTrue(receipt.Contains("Total: -$50.00"));
        }

        [TestMethod]
        public void GenerateReceipt_EmptyData_ReturnsEmptyReceipt()
        {
            // Arrange
            var receiptGenerator = new ReceiptGenerator();
            var testData = new dataBaseData(); // Assuming default constructor creates an empty object

            // Act
            var receipt = receiptGenerator.GenerateReceipt(testData);

            // Assert
            // Assuming that an empty receipt still contains the header
            Assert.IsTrue(receipt.Contains("Payment Receipt"));
            Assert.IsTrue(receipt.Contains("---------------"));
        }

    }

    [TestClass]
    public class TransactionsTests
    {
        [TestMethod]
        public void Transaction_Constructor_InitializesProductIdsList()
        {
            // Act
            var transaction = new Transaction();

            // Assert
            Assert.IsNotNull(transaction.ProductIds);
            Assert.IsInstanceOfType(transaction.ProductIds, typeof(List<string>));
            Assert.AreEqual(0, transaction.ProductIds.Count);
        }

        [TestMethod]
        public void Transaction_ValidData_CalculatesTotalCorrectly()
        {
            // Arrange
            var transaction = new Transaction
            {
                Amount = 100m,
                Tax = 15m
            };

            // Act
            transaction.Total = transaction.Amount + transaction.Tax;

            // Assert
            Assert.AreEqual(115m, transaction.Total);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transaction_TaxGreaterThanAmount_ThrowsInvalidOperationException()
        {
            // Arrange & Act
            var transaction = new Transaction
            {
                Amount = 50m,
                Tax = 60m
            };

            // Additional logic is required to throw InvalidOperationException in the setter of Tax or when calculating Total
            transaction.Total = transaction.Amount + transaction.Tax;
        }

        [TestMethod]
        public void Transaction_TransactionDate_DefaultsToCurrentDateTime()
        {
            // Arrange
            var now = DateTime.Now;

            // Act
            var transaction = new Transaction();

            // Assert
            Assert.IsTrue(transaction.TransactionDate >= now && transaction.TransactionDate <= DateTime.Now);
        }

        [TestMethod]
        public void Transaction_ProductIds_AddProductIdsToList()
        {
            // Arrange
            var transaction = new Transaction();
            var productId = "123";

            // Act
            transaction.ProductIds.Add(productId);

            // Assert
            Assert.IsTrue(transaction.ProductIds.Contains(productId));
        }

        [TestMethod]
        public void Transaction_UserId_SetUserIdProperty()
        {
            // Arrange
            var transaction = new Transaction();
            var userId = "User123";

            // Act
            transaction.UserId = userId;

            // Assert
            Assert.AreEqual(userId, transaction.UserId);
        }
    }

    [TestClass]
    public class ProductInfoTests
    {
        [TestMethod]
        public void ProductInfo_SetProductId_ValidProductId()
        {
            // Arrange
            var productInfo = new ProductInfo();
            var expectedProductId = 123;

            // Act
            productInfo.ProductId = expectedProductId;

            // Assert
            Assert.AreEqual(expectedProductId, productInfo.ProductId);
        }

        [TestMethod]
        public void ProductInfo_SetQuantity_ValidQuantity()
        {
            // Arrange
            var productInfo = new ProductInfo();
            var expectedQuantity = 10;

            // Act
            productInfo.Quantity = expectedQuantity;

            // Assert
            Assert.AreEqual(expectedQuantity, productInfo.Quantity);
        }

        [TestMethod]
        public void ProductInfo_SetPrice_ValidPrice()
        {
            // Arrange
            var productInfo = new ProductInfo();
            var expectedPrice = 99.99m;

            // Act
            productInfo.Price = expectedPrice;

            // Assert
            Assert.AreEqual(expectedPrice, productInfo.Price);
        }

        [TestMethod]
        public void ProductInfo_SetTransaction_ValidTransaction()
        {
            // Arrange
            var productInfo = new ProductInfo();
            var transaction = new Transaction();

            // Act
            productInfo.Transaction = transaction;

            // Assert
            Assert.AreEqual(transaction, productInfo.Transaction);
        }

        [TestMethod]
        public void ProductInfo_SetTransactionId_ValidTransactionId()
        {
            // Arrange
            var productInfo = new ProductInfo();
            var expectedTransactionId = 100;

            // Act
            productInfo.TransactionId = expectedTransactionId;

            // Assert
            Assert.AreEqual(expectedTransactionId, productInfo.TransactionId);
        }

        [TestMethod]
        public void ProductInfo_SetTransactionId_NegativeTransactionId()
        {
            // Arrange
            var productInfo = new ProductInfo();
            var negativeTransactionId = -1;

            // Act & Assert
            // You need to add logic in ProductInfo to throw an exception for negative IDs
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => productInfo.TransactionId = negativeTransactionId);
        }

        [TestMethod]
        public void ProductInfo_SetTransactionId_ZeroTransactionId()
        {
            // Arrange
            var productInfo = new ProductInfo();
            var zeroTransactionId = 0;

            // Act
            productInfo.TransactionId = zeroTransactionId;

            // Assert
            Assert.AreEqual(zeroTransactionId, productInfo.TransactionId);
        }

        [TestMethod]
        public void ProductInfo_SetTransactionId_NullTransaction_ThrowsArgumentNullException()
        {
            // Arrange
            var productInfo = new ProductInfo();
            productInfo.Transaction = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => productInfo.TransactionId = 1);
        }
    }

    [TestClass]
    public class ErrorViewModelTests
    {
        [TestMethod]
        public void ErrorViewModel_SetRequestId_ValidRequestId()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel();
            var validRequestId = "12345";

            // Act
            errorViewModel.RequestId = validRequestId;

            // Assert
            Assert.AreEqual(validRequestId, errorViewModel.RequestId);
        }

        [TestMethod]
        public void ErrorViewModel_SetRequestId_NullRequestId()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel();

            // Act
            errorViewModel.RequestId = null;

            // Assert
            Assert.IsNull(errorViewModel.RequestId);
            Assert.IsFalse(errorViewModel.ShowRequestId);
        }

        [TestMethod]
        public void ErrorViewModel_SetRequestId_EmptyRequestId()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel();
            var emptyRequestId = "";

            // Act
            errorViewModel.RequestId = emptyRequestId;

            // Assert
            Assert.AreEqual(emptyRequestId, errorViewModel.RequestId);
            Assert.IsFalse(errorViewModel.ShowRequestId);
        }

        [TestMethod]
        public void ErrorViewModel_ShowRequestId_RequestIdNotNull()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel();
            var nonEmptyRequestId = "12345";

            // Act
            errorViewModel.RequestId = nonEmptyRequestId;

            // Assert
            Assert.IsTrue(errorViewModel.ShowRequestId);
        }

        [TestMethod]
        public void ErrorViewModel_ShowRequestId_NullRequestId()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel();

            // Act
            errorViewModel.RequestId = null;

            // Assert
            Assert.IsFalse(errorViewModel.ShowRequestId);
        }
    }

    [TestClass]
    public class dataBaseDataTests
    {
        [TestMethod]
        public void dataBaseData_SetOrderId_ValidOrderId()
        {
            // Arrange
            var data = new dataBaseData();
            var validOrderId = 123;

            // Act
            data.orderId = validOrderId;

            // Assert
            Assert.AreEqual(validOrderId, data.orderId);
        }

        [TestMethod]
        public void dataBaseData_SetCustomerId_ValidCustomerId()
        {
            // Arrange
            var data = new dataBaseData();
            var validCustomerId = "Cust123";

            // Act
            data.customerId = validCustomerId;

            // Assert
            Assert.AreEqual(validCustomerId, data.customerId);
        }

        [TestMethod]
        public void dataBaseData_SetNumOfItems_ValidNumOfItems()
        {
            // Arrange
            var data = new dataBaseData();
            var validNumOfItems = 5;

            // Act
            data.numOfItems = validNumOfItems;

            // Assert
            Assert.AreEqual(validNumOfItems, data.numOfItems);
        }

        [TestMethod]
        public void dataBaseData_SetSubtotal_ValidSubtotal()
        {
            // Arrange
            var data = new dataBaseData();
            var validSubtotal = 200.50m;

            // Act
            data.subtotal = validSubtotal;

            // Assert
            Assert.AreEqual(validSubtotal, data.subtotal);
        }

        [TestMethod]
        public void dataBaseData_SetTotal_ValidTotal()
        {
            // Arrange
            var data = new dataBaseData();
            var validTotal = 250.75m;

            // Act
            data.total = validTotal;

            // Assert
            Assert.AreEqual(validTotal, data.total);
        }

        [TestMethod]
        public void dataBaseData_SetPaymentMethod_ValidPaymentMethod()
        {
            // Arrange
            var data = new dataBaseData();
            var validPaymentMethod = "Credit Card";

            // Act
            data.paymentMethod = validPaymentMethod;

            // Assert
            Assert.AreEqual(validPaymentMethod, data.paymentMethod);
        }

        [TestMethod]
        public void dataBaseData_SetDate_ValidDate()
        {
            // Arrange
            var data = new dataBaseData();
            var validDate = new DateTime(2023, 4, 1);

            // Act
            data.date = validDate;

            // Assert
            Assert.AreEqual(validDate, data.date);
        }

        [TestMethod]
        public void dataBaseData_SetAvg_ValidAvg()
        {
            // Arrange
            var data = new dataBaseData();
            var validAvg = 123.45m;

            // Act
            data.avg = validAvg;

            // Assert
            Assert.AreEqual(validAvg, data.avg);
        }

        [TestMethod]
        public void dataBaseData_SetDate_DefaultsToCurrentDateTime()
        {
            // Arrange
            var data = new dataBaseData();
            var now = DateTime.Now;

            // Act
            var date = data.date;

            // Assert
            Assert.IsTrue(date >= now && date <= DateTime.Now);
        }
    }

    [TestClass]
    public class CartModelTests
    {
        [TestMethod]
        public void CartModel_AddProduct_ValidProduct()
        {
            // Arrange
            var cart = new CartModel();
            var product = "Product123";

            // Act
            cart.products.Add(product);

            // Assert
            Assert.IsTrue(cart.products.Contains(product));
            Assert.AreEqual(1, cart.products.Count);
        }

        [TestMethod]
        public void CartModel_RemoveProduct_ValidProduct()
        {
            // Arrange
            var cart = new CartModel();
            var product = "Product123";
            cart.products.Add(product);

            // Act
            cart.products.Remove(product);

            // Assert
            Assert.IsFalse(cart.products.Contains(product));
        }

        [TestMethod]
        public void CartModel_RemoveProduct_ProductNotInCart_NoChange()
        {
            // Arrange
            var cart = new CartModel();
            var product = "Product123";
            var initialCount = cart.products.Count;

            // Act
            cart.products.Remove(product);

            // Assert
            Assert.AreEqual(initialCount, cart.products.Count);
        }

        [TestMethod]
        public void CartModel_ClearCart_EmptyProductsList()
        {
            // Arrange
            var cart = new CartModel();
            cart.products.Add("Product123");

            // Act
            cart.products.Clear();

            // Assert
            Assert.AreEqual(0, cart.products.Count);
        }

        [TestMethod]
        public void CartModel_SetCustomerID_ValidCustomerID()
        {
            // Arrange
            var cart = new CartModel();
            var customerID = "Cust100";

            // Act
            cart.customerID = customerID;

            // Assert
            Assert.AreEqual(customerID, cart.customerID);
        }

        [TestMethod]
        public void CartModel_SetTotal_ValidTotal()
        {
            // Arrange
            var cart = new CartModel();
            var total = 100.50m;

            // Act
            cart.total = total;

            // Assert
            Assert.AreEqual(total, cart.total);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CartModel_SetTotal_NegativeTotal_ThrowsArgumentException()
        {
            // Arrange
            var cart = new CartModel();
            var negativeTotal = -100m;

            // Act
            cart.total = negativeTotal;
        }

        [TestMethod]
        public void CartModel_SetTotal_ZeroTotal()
        {
            // Arrange
            var cart = new CartModel();
            var zeroTotal = 0m;

            // Act
            cart.total = zeroTotal;

            // Assert
            Assert.AreEqual(zeroTotal, cart.total);
        }
    }

/*
    [TestClass]
    public class DatabaseControlTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
*/

    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void User_SetUserId_ValidUserId()
        {
            // Arrange
            var user = new User();
            var validUserId = "User123";

            // Act
            user.UserId = validUserId;

            // Assert
            Assert.AreEqual(validUserId, user.UserId);
        }

        [TestMethod]
        public void User_SetName_ValidName()
        {
            // Arrange
            var user = new User();
            var validName = "John Doe";

            // Act
            user.Name = validName;

            // Assert
            Assert.AreEqual(validName, user.Name);
        }

        [TestMethod]
        public void User_SetEmail_ValidEmail()
        {
            // Arrange
            var user = new User();
            var validEmail = "johndoe@example.com";

            // Act
            user.Email = validEmail;

            // Assert
            Assert.AreEqual(validEmail, user.Email);
        }

        [TestMethod]
        public void User_SetBillingAddress_ValidBillingAddress()
        {
            // Arrange
            var user = new User();
            var validBillingAddress = "123 Main St";

            // Act
            user.BillingAddress = validBillingAddress;

            // Assert
            Assert.AreEqual(validBillingAddress, user.BillingAddress);
        }

        [TestMethod]
        public void User_SetShippingAddress_ValidShippingAddress()
        {
            // Arrange
            var user = new User();
            var validShippingAddress = "456 Elm St";

            // Act
            user.ShippingAddress = validShippingAddress;

            // Assert
            Assert.AreEqual(validShippingAddress, user.ShippingAddress);
        }

        [TestMethod]
        public void User_SetPaymentMethod_ValidPaymentMethod()
        {
            // Arrange
            var user = new User();
            var validPaymentMethod = "Credit Card";

            // Act
            user.PaymentMethod = validPaymentMethod;

            // Assert
            Assert.AreEqual(validPaymentMethod, user.PaymentMethod);
        }

        [TestMethod]
        public void User_SetUserId_NullUserId()
        {
            // Arrange
            var user = new User();

            // Act
            user.UserId = null;

            // Assert
            Assert.IsNull(user.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void User_SetEmail_InvalidEmailFormat()
        {
            // Arrange
            var user = new User();
            var invalidEmail = "invalid-email";

            // Act
            user.Email = invalidEmail;
        }
    }

    [TestClass]
    public class PaymentInfoTests
    {
        [TestMethod]
        public void PaymentInfo_SetProducts_ValidProducts()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            var validProducts = new List<string> { "Product1", "Product2" };

            // Act
            paymentInfo.Products = validProducts;

            // Assert
            CollectionAssert.AreEqual(validProducts, paymentInfo.Products);
        }

        [TestMethod]
        public void PaymentInfo_SetCustomerID_ValidCustomerID()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            var validCustomerID = "Cust123";

            // Act
            paymentInfo.CustomerID = validCustomerID;

            // Assert
            Assert.AreEqual(validCustomerID, paymentInfo.CustomerID);
        }

        [TestMethod]
        public void PaymentInfo_SetTotal_ValidTotal()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            var validTotal = 100.50m;

            // Act
            paymentInfo.Total = validTotal;

            // Assert
            Assert.AreEqual(validTotal, paymentInfo.Total);
        }

        [TestMethod]
        public void PaymentInfo_SetCardNumber_ValidCardNumber()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            var validCardNumber = "1234567890123456";

            // Act
            paymentInfo.CardNumber = validCardNumber;

            // Assert
            Assert.AreEqual(validCardNumber, paymentInfo.CardNumber);
        }

        [TestMethod]
        public void PaymentInfo_SetExpiration_ValidExpiration()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            var validExpiration = "12/25";

            // Act
            paymentInfo.Expiration = validExpiration;

            // Assert
            Assert.AreEqual(validExpiration, paymentInfo.Expiration);
        }

        [TestMethod]
        public void PaymentInfo_SetCVV_ValidCVV()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            var validCVV = "123";

            // Act
            paymentInfo.CVV = validCVV;

            // Assert
            Assert.AreEqual(validCVV, paymentInfo.CVV);
        }

        [TestMethod]
        public void PaymentInfo_SetBillingAddress_ValidBillingAddress()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            var validBillingAddress = new BillingAddressModel { /* Set valid properties */ };

            // Act
            paymentInfo.BillingAddress = validBillingAddress;

            // Assert
            Assert.AreEqual(validBillingAddress, paymentInfo.BillingAddress);
        }

        [TestMethod]
        public void PaymentInfo_SetShipping_ValidShipping()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            var validShipping = new ShippingModel { /* Set valid properties */ };

            // Act
            paymentInfo.Shipping = validShipping;

            // Assert
            Assert.AreEqual(validShipping, paymentInfo.Shipping);
        }

        [TestMethod]
        public void PaymentInfo_SetPaymentMethod_ValidPaymentMethod()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            var validPaymentMethod = "CreditCard";

            // Act
            paymentInfo.PaymentMethod = validPaymentMethod;

            // Assert
            Assert.AreEqual(validPaymentMethod, paymentInfo.PaymentMethod);
        }

        [TestMethod]
        public void PaymentInfo_ShippingCost_ExpressShippingOption_CalculatesShippingCost()
        {
            // Arrange
            var paymentInfo = new PaymentInfo();
            paymentInfo.Shipping = new ShippingModel { ShippingOption = "Express" };

            // Act
            var shippingCost = paymentInfo.ShippingCost;

            // Assert
            Assert.AreEqual(9.99m, shippingCost);
        }
    }

    [TestClass]
    public class BillingAddressModelTests
    {
        [TestMethod]
        public void BillingAddressModel_SetName_ValidName()
        {
            var model = new BillingAddressModel();
            var validName = "John Doe";

            model.Name = validName;

            Assert.AreEqual(validName, model.Name);
        }

        [TestMethod]
        public void BillingAddressModel_SetName_NullName()
        {
            var model = new BillingAddressModel();

            model.Name = null;

            Assert.IsNull(model.Name);
        }

    }
}



