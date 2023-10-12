using System;
using System.Collections.Generic;

namespace EcoMonitoringIS.Models;

public partial class Result
{
    public int Idresults { get; set; }

    public double Exceeding { get; set; }

    public int PollutionId { get; set; }

    public virtual Pollution Pollution { get; set; } = null!;
}
