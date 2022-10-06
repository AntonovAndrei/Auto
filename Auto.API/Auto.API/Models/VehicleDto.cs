using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Auto.API.Models;

public class VehicleDto {

    [HiddenInput(DisplayValue = false)]
    public string ModelCode { get; set; }

    private string registration;

    private static string NormalizeRegistration(string reg) {
        return reg == null ? reg : Regex.Replace(reg.ToUpperInvariant(), "[^A-Z0-9]", "");
    }

    [Required]
    [DisplayName("Registration Plate")]
    public string Registration {
        get => NormalizeRegistration(registration);
        set => registration = value;
    }

    [Required]
    [DisplayName("Year of first registration")]
    [Range(1950, 2022)]
    public int Year { get; set; }

    [Required]
    [DisplayName("Colour")]
    public string Color { get; set; }

    private static readonly string[] colors = new[] {
        "Black", "Blue", "Gold", "Green", "Grey", "Orange",
        "Purple", "Red", "Silver", "Turquoise", "White", "Yellow"
    };

    private static readonly SelectListItem blankSelectListItem = new SelectListItem("select...", String.Empty);
    public static IEnumerable<SelectListItem> ListColors(string selectedColor) {
        var items = new List<SelectListItem> { blankSelectListItem };
        items.AddRange(colors.Select(c => new SelectListItem(c, c, c == selectedColor)));
        return items;
    }
}