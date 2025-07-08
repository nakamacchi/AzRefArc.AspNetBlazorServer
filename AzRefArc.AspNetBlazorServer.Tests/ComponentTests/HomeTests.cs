using Bunit;
using Microsoft.Extensions.DependencyInjection;
using AzRefArc.AspNetBlazorServer.Components.Pages;

namespace AzRefArc.AspNetBlazorServer.Tests.ComponentTests
{
    /// <summary>
    /// Homeページコンポーネントのテスト
    /// </summary>
    [TestClass]
    public class HomeTests : Bunit.TestContext
    {
        [TestMethod]
        public void Home_RendersCorrectly()
        {
            // Arrange & Act
            var component = RenderComponent<Home>();

            // Assert
            Assert.IsNotNull(component);
            Assert.IsNotNull(component.Markup);
        }

        [TestMethod]
        public void Home_ContainsBusinessApplicationContent()
        {
            // Arrange & Act
            var component = RenderComponent<Home>();

            // Assert
            // 実際のHomeコンポーネントの内容に応じてアサーションを調整
            Assert.IsTrue(component.Markup.Contains("ASP.NET Core Blazor Server による業務サンプルアプリ"));
        }

        [TestMethod]
        public void Home_ContainsBizGroupASection()
        {
            // Arrange & Act
            var component = RenderComponent<Home>();

            // Assert
            // BizGroupAセクションが含まれているかチェック
            Assert.IsTrue(component.Markup.Contains("Server 参照系アプリ"));
        }
    }
}
