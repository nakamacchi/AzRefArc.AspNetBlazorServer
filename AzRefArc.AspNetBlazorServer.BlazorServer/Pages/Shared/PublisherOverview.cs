namespace AzRefArc.AspNetBlazorServer.BlazorServer.Pages.Shared
{
    public class PublisherOverview
    {
        public string PublisherId { get; set; } = String.Empty;
        public string PublisherName { get; set; } = String.Empty;
        public List<TitleOverview> TitleOverviews { get; set; } = new List<TitleOverview>();
    }
}
