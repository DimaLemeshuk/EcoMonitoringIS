using System;
using System.Collections.Generic;

namespace EcoMonitoringIS.Models;

public partial class Belonging
{
    public int Idbelonging { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Enterprise> Enterprises { get; set; } = new List<Enterprise>();

    public Belonging(int idbelonging, string name)
    {
        Idbelonging = idbelonging;
        Name = name;
    }
}
