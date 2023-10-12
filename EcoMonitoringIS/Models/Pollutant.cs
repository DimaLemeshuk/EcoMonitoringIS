using System;
using System.Collections.Generic;

namespace EcoMonitoringIS.Models;

public partial class Pollutant
{
    public int Idpollutant { get; set; }

    public string Name { get; set; } = null!;

    public int DangerClass { get; set; }

    public double Gdk { get; set; }

    public double Mfr { get; set; }

    public virtual ICollection<Pollution> Pollutions { get; set; } = new List<Pollution>();
}
