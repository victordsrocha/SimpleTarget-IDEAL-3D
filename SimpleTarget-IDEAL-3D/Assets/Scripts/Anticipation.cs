using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anticipation
{
    public Experiment Experiment;
    public int Proclivity;

    public Anticipation(Experiment experiment, int proclivity)
    {
        this.Experiment = experiment;
        this.Proclivity = proclivity;
    }

    public void AddProclivity(int incProclivity)
    {
        this.Proclivity += incProclivity;
    }

    public override string ToString()
    {
        return Experiment.Label + " proclivity " + Proclivity;
    }
}