﻿namespace AzRefArc.AspNetBlazorServer.Components.Pages.Shared
{
    public class TitleOverview
    {
        public string TitleId { get; set; } = String.Empty;
        public string TitleName { get; set; } = String.Empty;
        public decimal? Price { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ImageUrl { get; set; } = String.Empty;
        public string ImageThumbnailUrl { get; set; } = String.Empty;
    }
}
