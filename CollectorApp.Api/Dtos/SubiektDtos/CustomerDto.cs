namespace CollectorApp.Api.Dtos.SubiektDtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}