namespace LiveArt.Data.Json.Design
{
    public class DesignDescriptor
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Date { get; set; } // TODO: reqrite to DateTime type
        public string Email { get; set; }
    }
}