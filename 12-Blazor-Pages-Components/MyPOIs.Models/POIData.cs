namespace MyPOIs.Models
{
    public class POIData
    {
        public Guid	Id {get; set; } = Guid.NewGuid();
		public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUri { get; set; }
        public DateTime Date { get; set; }
    }
}