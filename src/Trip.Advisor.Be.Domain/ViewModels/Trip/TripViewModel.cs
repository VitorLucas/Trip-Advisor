using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Trip.Advisor.Be.Domain.ViewModels.Finder;

public class TripViewModel
{
    [SwaggerSchema(Description = "Days planned to travel.")]
    [Required(ErrorMessage = "The days to travel is required")]
    public int DaysToTravel { get; set; }

    [Required(ErrorMessage = "The country destiny is required")]
    [Description("Country you would like to go.")]
    [MaxLength(30)]
    public string CountryDestiny { get; set; }

    [Required(ErrorMessage = "The Host amout per night is required")]
    [Description("Host amount per night in euro.")]
    public decimal HostAmoutPerNight { get; set; }

    [Required(ErrorMessage = "The Restaurants price per day is required")]
    [Description("Restaurant price per day in euro.")]
    public decimal RestaurantsPricePerDay { get; set; }
}
