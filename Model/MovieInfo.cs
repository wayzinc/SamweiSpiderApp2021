namespace SamweiSpiderApp2021.Model
{
    public class MovieInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FaceUrl { get; set; }
        public string? Url { get; set; }
        public int SourceId { get; set; }
        public string? Quality { get; set; }
        public decimal Score { get; set; }
        public List<MovieInfo_Tag>? Tags { get; set; }
    }

    public class MovieInfo_Tag
    {
        public string? Name { get; set; }
        public string? Short { get; set; }
    }
}
