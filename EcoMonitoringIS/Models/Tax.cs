using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Windows;

namespace EcoMonitoringIS.Models
{
    public partial class Tax
    {
        public int Idtax { get; set; }
        public int PollutionId { get; set; }
        public double Rate { get; set; }
        public double TaxUAH { get; set; }
        public virtual Pollution Pollution { get; set; } = null!;

        public Tax()
        {
        }

        public Tax(int Idpollution)
        {
            this.PollutionId = Idpollution;
            CalculateCR(Idpollution);
        }

        public void CalculateCR(int id_Pollution)
        {
            using (var context = new EcomonitoringdbContext())
            {
                Pollution poll = context.Pollutions.Find(id_Pollution); 

                double MRF = poll.ValueMfr;
                //this.Pollution = poll;
                this.Rate = context.Pollutants.Find(poll.PollutantId).TaxRate;
                this.TaxUAH = MRF * Rate;
            }
        }

        public static void reload()
        {
            using (var context = new EcomonitoringdbContext())
            {
                var allRecords = context.Taxes.ToList();

                // Видалити всі записи з таблиці
                context.Taxes.RemoveRange(allRecords);

                // Зберегти зміни в базі даних
                context.SaveChanges();

                foreach (var pollution in context.Pollutions.AsNoTracking().ToList())
                {
                    context.Taxes.Add(new Tax(pollution.Idpollution));
                }

                context.SaveChanges();
            }
        }
    }
}

