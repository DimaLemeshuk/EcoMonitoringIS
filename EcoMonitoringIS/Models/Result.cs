using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;

namespace EcoMonitoringIS.Models;

public partial class Result
{
    public int Idresults { get; private set; }

    public double CR { get; private set; }
    public double LADD { get; private set; }
    public int _pollutionId;
    private double _ca = 0.00000095;
    private double _ch = 1.0 * 0.00000095;
    private double _tout = 8;
    private double _tin = 16;
    private double _vout = 1.4;
    private double _vin = 0.63;
    private double _ef = 350;
    private double _ed = 30;
    private double _bw = 70;
    private double _at = 70;
    private Pollution _pollution;

    public int PollutionId
    {
        get { return _pollutionId; }
        set
        {
            _pollutionId = value;
            CalculateCR();
        }
    }

    public double Ca
    {
        get { return _ca; }
        set
        {
            _ca = value;
            CalculateCR();
        }
    }

    public double Ch
    {
        get { return _ch; }
        set
        {
            _ch = value;
            CalculateCR();
        }
    }

    public double Tout
    {
        get { return _tout; }
        set
        {
            _tout = value;
            CalculateCR();
        }
    }

    public double Tin
    {
        get { return _tin; }
        set
        {
            _tin = value;
            CalculateCR();
        }
    }

    public double Vout
    {
        get { return _vout; }
        set
        {
            _vout = value;
            CalculateCR();
        }
    }

    public double Vin
    {
        get { return _vin; }
        set
        {
            _vin = value;
            CalculateCR();
        }
    }

    public double EF
    {
        get { return _ef; }
        set
        {
            _ef = value;
            CalculateCR();
        }
    }

    public double ED
    {
        get { return _ed; }
        set
        {
            _ed = value;
            CalculateCR();
        }
    }

    public double BW
    {
        get { return _bw; }
        set
        {
            _bw = value;
            CalculateCR();
        }
    }

    public double AT
    {
        get { return _at; }
        set
        {
            _at = value;
            CalculateCR();
        }
    }

    public Pollution Pollution
    {
        get { return _pollution; }
        set
        {
            Pollution = value;
            CalculateCR();
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
        this.CalculateCR();
    }

    private void CalculateCR()
    {
        using (var context = new EcomonitoringdbContext())
        {
            LADD = ((Pollution.Concentration * Tout * Vout) + (Ch * Tin * Vin)) * EF * ED / (BW * AT * 365);
            CR = LADD * Pollution.Pollutant.SF;
        }
    }
}
