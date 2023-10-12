using System;
using System.Collections.Generic;

namespace EcoMonitoringIS.Models;

public partial class Pollution
{
    public int Idpollution { get; set; }

    public int EnterpriseId { get; set; }

    public int PollutantId { get; set; }

    public double ValueMfr { get; set; }

    public double? Percent { get; set; }

    public DateTime Date { get; set; }

    public virtual Enterprise Enterprise { get; set; } = null!;

    public virtual Pollutant Pollutant { get; set; } = null!;

    public virtual Result? Result { get; set; }
}
