using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoMonitoringIS.Models
{
    public partial class Loss
    {
        public int idLoss { get; private set; }
        public double LossUAH { get; private set; }
        public double Mi { get; private set; }
        public double P { get; private set; }
        public double Ai { get; private set; }
        public double Kpop { get; private set; }
        public double Kf { get; private set; }
        public double Kzi { get; private set; }
        public double Qv { get; private set; }

        public double T { get; private set; }
        public double POP_ { get; private set; }

        public int pollution_id { get; private set; }
        public virtual Pollution Pollution { get; set; } = null!;

        public Loss()
        { }
        public Loss(Pollution Pollution)
        {
            this.Pollution = Pollution;
            this.pollution_id = Pollution.Idpollution;
            Qv = 100.35446;
            T = 24 * 365;
            P = 6700;
            POP_ = 10000;
            SetKpop();
            Kf = 1;
            CalculateLossUAH();

        }
        private void CalculateLossUAH()
        {
            using (var context = new EcomonitoringdbContext())
            {
                Pollution pollution = context.Pollutions.FirstOrDefault(p => p.Idpollution == pollution_id);
                //Pollution pollution = context.Pollutions.Find(pollution_id);
                Pollutant pollutant = context.Pollutants.Find(pollution.PollutantId);
                if(pollution.Concentration - pollutant.Gdk >0)
                {
                    Mi = 0.0000036 * (pollution.Concentration - pollutant.Gdk) * Qv * T;
                }
                else
                {
                    Mi = 0;
                }
                Ai = 1/ pollutant.Gdk;
                Kzi = pollution.HQ;
                LossUAH = Mi * 1.1 * P * Ai * Kpop * Kf * Kzi;
                context.SaveChanges();
            }
        }

        private void SetKpop()
        {
            if(POP_<100000)
            {
                Kpop = 1.0;
            }
            else if (POP_>100000 && POP_ < 250000)
            {
                Kpop = 1.20;

            }
            else if (POP_ > 250000 && POP_ < 500000)
            {
                Kpop = 1.35;

            }
            else if (POP_ > 500000 && POP_ < 1000000)
            {
                Kpop = 1.55;

            }
            else
            {
                Kpop = 1.80;

            }

        }
    }
}
