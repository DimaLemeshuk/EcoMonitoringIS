using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EcoMonitoringIS.Models;

public partial class Pollution
{
    public int Idpollution { get; set; }

    public int EnterpriseId { get; set; }

    public int PollutantId { get; set; }

    public double ValueMfr { get; set; }

    public double? Percent { get; set; }

    public int Year { get; set; }
    public double Concentration { get; set; }
    public double HQ { get; private set; }
    public string? rating { get; set; } = null!;


    public virtual Enterprise Enterprise { get; set; } = null!;

    public virtual Pollutant Pollutant { get; set; } = null!;

    public virtual Result? Result { get; set; }

    public ICollection<Loss> Losses { get; set; } = null!;
    public ICollection<Tax> Taxes { get; set; } = null!;

    public Pollution()
    { }

    public void Initialize(string Enterprise, string Pollutant, double valueMfr, double? percent, int year, double concentration)
    {
        using (var context = new EcomonitoringdbContext())
        {

                var e = context.Enterprises.FirstOrDefault(e => e.Name == Enterprise);
                var p = context.Pollutants.FirstOrDefault(p => p.Name == Pollutant);
                EnterpriseId = e.Identerprise;
                PollutantId = p.Idpollutant;
                ValueMfr = valueMfr;
                Percent = percent;
                Year = year;
                Concentration = concentration;
                HQ = concentration / p.Gdk;
                rating = GetRating(HQ);
        }
    }

    public string GetRating(double HQ)
    {
        string rating;
        if (HQ < 1)
        {
            rating = "Зневажливо малий";
        }
        else if (HQ > 01)
        {
            rating = "Високий";

        }
        else
        {
            rating = "Гранична величина";
        }
        return rating;
    }
}
