namespace AzRefArc.AspNetBlazorServer.BlazorServer.Pages.Shared
{
    public class AuthorOverview
    {
        public string AuthorId { get; set; } = String.Empty;
        public string AuthorName { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string? State { get; set; }
        public bool Contract { get; set; }
    }
}
