using Trip.Advisor.Be.Domain.ViewModels.Finder;

namespace  Trip.Advisor.Be.Domain.Arguments.Trip;

public class TripRequest
{
    public TripRequest(int daysToTravel,
                        string countryDestiny,
                        decimal hostAmoutPerNight,
                        decimal restaurantsPricePerDay)
    {
        DaysToTravel = daysToTravel;
        CountryDestiny = countryDestiny;
        HostAmoutPerNight = hostAmoutPerNight;
        RestaurantsPricePerDay = restaurantsPricePerDay;
    }

    public int DaysToTravel { get; set; }

    public string CountryDestiny { get; set; }

    public decimal HostAmoutPerNight { get; set; }

    public decimal RestaurantsPricePerDay { get; set; }

    public static explicit operator TripRequest(TripViewModel tripViewModel) 
    {
        return new TripRequest(tripViewModel.DaysToTravel, 
                                tripViewModel.CountryDestiny, 
                                tripViewModel.HostAmoutPerNight, 
                                tripViewModel.RestaurantsPricePerDay);
    }
}
