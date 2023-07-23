public class BusArrival
{
    public int Number { get; set; }
    public string Destination { get; set; }
    public DateTime ArrivalTime { get; set; }

    public BusArrival(int number, string destination, DateTime arrivalTime)
    {
        Number = number;
        Destination = destination;
        ArrivalTime = arrivalTime;
    }
}