using Bunit;
using Microsoft.Extensions.DependencyInjection;
using AzRefArc.AspNetBlazorServer.Components.Pages;

namespace AzRefArc.AspNetBlazorServer.Tests.ComponentTests
{
    /// <summary>
    /// Homeページコンポーネントのテスト
    /// </summary>
    public class HomeTests : TestContext
    {
        [Fact]
        public void Home_RendersCorrectly()
        {
            // Arrange & Act
            var component = RenderComponent<Home>();

            // Assert
            Assert.NotNull(component);
            Assert.NotNull(component.Markup);
        }

        [Fact]
        public void Home_ContainsBusinessApplicationContent()
        {
            // Arrange & Act
            var component = RenderComponent<Home>();

            // Assert
            // 実際のHomeコンポーネントの内容に応じてアサーションを調整
            Assert.Contains("ASP.NET Core Blazor Server による業務サンプルアプリ", component.Markup);
        }

        [Fact]
        public void Home_ContainsBizGroupASection()
        {
            // Arrange & Act
            var component = RenderComponent<Home>();

            // Assert
            // BizGroupAセクションが含まれているかチェック
            Assert.Contains("Server 参照系アプリ", component.Markup);
        }
    }
}
