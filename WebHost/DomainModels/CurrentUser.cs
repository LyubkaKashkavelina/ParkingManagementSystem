namespace WebHost.DomainModels
{
    public class CurrentUser
    {
        public string Name { get; set; }

        public string ParkingSpaceNumber { get; set; }

        public bool IsAdmin { get; set; }

        public string PhoneNumber { get; set; }

        public string BookedParkingSpaceForToday { get; set; }
    }
}