using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anticipation
{
    public Experiment Experiment;
    public float Proclivity;

    public Anticipation(Experiment experiment, float proclivity)
    {
        this.Experiment = experiment;
        this.Proclivity = proclivity;
    }

    public void AddProclivity(float incProclivity)
    {
        this.Proclivity += incProclivity;
    }

    public override string ToString()
    {
        return Experiment.Label + " proclivity " + Proclivity;
    }
}