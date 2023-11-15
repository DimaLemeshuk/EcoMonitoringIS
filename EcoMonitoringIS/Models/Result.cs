using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;

namespace EcoMonitoringIS.Models;

public partial class Result
{
    public int Idresults { get; private set; }

    public double PCR { get; private set; }
    public double CR { get; private set; }
    public double LADD { get; private set; }
    public double POP { get; set; }
    public int _pollutionId;
    private double _ca;
    private double _ch;
    private double _tout;
    private double _tin;
    private double _vout;
    private double _vin;
    private double _ef;
    private double _ed;
    private double _bw;
    private double _at;
    private Pollution _pollution;

    public int PollutionId
    {
        get { return _pollutionId; }
        set
        {
            _pollutionId = value;
            //CalculateCR();
        }
    }

    public double Ca
    {
        get { return _ca; }
        set
        {
            _ca = value;
            //CalculateCR();
        }
    }

    public double Ch
    {
        get { return _ch; }
        set
        {
            _ch = value;
            //CalculateCR();
        }
    }

    public double Tout
    {
        get { return _tout; }
        set
        {
            _tout = value;
            //CalculateCR();
        }
    }

    public double Tin
    {
        get { return _tin; }
        set
        {
            _tin = value;
            //CalculateCR();
        }
    }

    public double Vout
    {
        get { return _vout; }
        set
        {
            _vout = value;
            //CalculateCR();
        }
    }

    public double Vin
    {
        get { return _vin; }
        set
        {
            _vin = value;
            //CalculateCR();
        }
    }

    public double EF
    {
        get { return _ef; }
        set
        {
            _ef = value;
            //CalculateCR();
        }
    }

    public double ED
    {
        get { return _ed; }
        set
        {
            _ed = value;
            //CalculateCR();
        }
    }

    public double BW
    {
        get { return _bw; }
        set
        {
            _bw = value;
           //alculateCR();
        }
    }

    public double AT
    {
        get { return _at; }
        set
        {
            _at = value;
            //CalculateCR();
        }
    }

    public Pollution Pollution
    {
        get { return _pollution; }
        set
        {
            //using( var contex = new EcomonitoringdbContext())
            //{
            //    var p = contex.Pollutions.Find(value.Idpollution);
            //    Pollution = p;

            //}
            _pollution = value;
            //CalculateCR();
        }
    }
    public Result()
    { }
    public Result(Pollution p)
    {
        this.Pollution = p;
        this.PollutionId = p.Idpollution;
        Ca = 0.00000095;
        Ch = 1.0 * 0.00000095;
        Tout = 8;
        Tin = 16;
        Vout = 1.4;
        Vin = 0.63;
        EF = 350;
        ED = 30;
        BW = 70;
        AT = 70;
        POP = 10000;
        this.CalculateCR(this.PollutionId);
    }

    public void CalculateCR(int id_Pollution)
    {
        using (var context = new EcomonitoringdbContext())
        {
            Pollution poll = context.Pollutions.Find(id_Pollution);
            LADD = ((poll.Concentration * Tout * Vout) + (Ch * Tin * Vin)) * EF * ED / (BW * AT * 365);
            double SF = context.Pollutants.Find(poll.PollutantId).SF;
            CR = LADD * SF;
            PCR = CR * POP;
            context.SaveChanges();
        }
    }
}
