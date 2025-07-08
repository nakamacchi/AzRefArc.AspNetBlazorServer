using System.Net;

namespace AzRefArc.AspNetBlazorServer.Tests.IntegrationTests
{
    /// <summary>
    /// 基本的なページアクセスの統合テスト
    /// </summary>
    [TestClass]
    public class BasicPagesTests
    {
        private static CustomWebApplicationFactory<Program>? _factory;
        private static HttpClient? _client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [TestMethod]
        [DataRow("/")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Act
            var response = await _client!.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual("text/html; charset=utf-8", 
                response.Content.Headers.ContentType?.ToString());
        }

        [TestMethod]
        public async Task Get_HomePageReturnsSuccess()
        {
            // Act
            var response = await _client!.GetAsync("/");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(content);
            Assert.IsTrue(content.Contains("<!DOCTYPE html>"));
        }

        [TestMethod]
        public async Task Get_NonExistentPageReturnsNotFound()
        {
            // Act
            var response = await _client!.GetAsync("/NonExistentPage");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
