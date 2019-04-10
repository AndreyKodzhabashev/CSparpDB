namespace CarDealer.DTO
{
    using System.Collections.Generic;

    public class InsertCarDto
    {
        public string Make { get; set; }
        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public int[] PartsId { get; set; } = new int[0];
    }
}