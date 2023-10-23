using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

    //public Enterprise(string Name, string Activity, string BelongingName, string Addres)
    //{
    //    using (var context = new EcomonitoringdbContext())
    //    {
    //        var b = context.Belongings.FirstOrDefault(e => e.Name == BelongingName);
    //        this.Name = Name;
    //        this.Activity = Activity;   
    //        this.BelongingId = b.Idbelonging; 
    //        this.Addres = Addres;   
    //        Belonging = b;
    //    }
    //}

    public Enterprise()
    {
        // Конструктор залишається пустим
    }

    public Enterprise(string Name, string Activity, int BelongingId, string Addres)
    {
        this.Name = Name;
        this.Activity = Activity;
        this.BelongingId = BelongingId;
        this.Addres = Addres;
    }

    public void Initialize(string Name, string Activity, string BelongingName, string Addres)
    {
        using (var context = new EcomonitoringdbContext())
        {
            var b = context.Belongings.FirstOrDefault(e => e.Name == BelongingName);
            this.Name = Name;
            this.Activity = Activity;
            this.BelongingId = b.Idbelonging;
            this.Addres = Addres;
            //Belonging = b;
        }
    }
}
