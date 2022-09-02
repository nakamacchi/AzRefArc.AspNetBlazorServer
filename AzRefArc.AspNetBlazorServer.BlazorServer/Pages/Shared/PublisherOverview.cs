namespace AzRefArc.AspNetBlazorServer.BlazorServer.Pages.Shared
{
    public class PublisherOverview
    {
        public string PublisherId { get; set; }
        public string PublisherName { get; set; }
        public List<TitleOverview> TitleOverviews { get; set; }
    }
}
