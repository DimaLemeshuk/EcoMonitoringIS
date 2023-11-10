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

    public virtual Enterprise Enterprise { get; set; } = null!;

    public virtual Pollutant Pollutant { get; set; } = null!;

    public virtual Result? Result { get; set; }

    public Pollution()
    { }

    public void Initialize(string Enterprise, string Pollutant, double valueMfr, double? percent, int year)
    {
        using (var context = new EcomonitoringdbContext())
        {
            //try
            //{
                var e = context.Enterprises.FirstOrDefault(e => e.Name == Enterprise);
                var p = context.Pollutants.FirstOrDefault(p => p.Name == Pollutant);
                EnterpriseId = e.Identerprise;
                PollutantId = p.Idpollutant;
                ValueMfr = valueMfr;
                Percent = percent;
                Year = year;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Сталася помилка: " + ex.Message);
            //}
        }
    }
}
