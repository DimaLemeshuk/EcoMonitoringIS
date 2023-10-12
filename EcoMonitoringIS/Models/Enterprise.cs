using System;
using System.Collections.Generic;

namespace EcoMonitoringIS.Models;

public partial class Enterprise
{
    public int Identerprise { get; set; }

    public string Name { get; set; } = null!;

    public string Activity { get; set; } = null!;

    public int BelongingId { get; set; }

    public string Addres { get; set; } = null!;

    public virtual Belonging Belonging { get; set; } = null!;

    public virtual ICollection<Pollution> Pollutions { get; set; } = new List<Pollution>();
}
