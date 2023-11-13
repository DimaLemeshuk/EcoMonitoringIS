using System;
using System.Collections.Generic;

namespace EcoMonitoringIS.Models;

public partial class Result
{
    public int Idresults { get; set; }

    public double CR { get; set; }

    public int PollutionId { get; set; }

    public virtual Pollution Pollution { get; set; } = null!;
}
