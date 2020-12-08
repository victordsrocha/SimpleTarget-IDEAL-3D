using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    public string Label { get; set; }
    public Experiment Experiment { get; set; }
    public int valence { get; set; }
    public Interaction PreInteraction { get; set; }
    public Interaction PostInteraction { get; set; }
    public float Weight { get; set; }

    public Interaction(string label)
    {
        this.Label = label;
        Experiment = null;
        valence = 0;
        PreInteraction = null;
        PostInteraction = null;
        Weight = 0;
    }

    public bool is_primitive()
    {
        return (PreInteraction == null);
    }

    public override string ToString()
    {
        if (is_primitive())
        {
            return this.Label + " valence " + this.valence;
        }

        return this.Label + " valence " + this.valence + " weight " + Weight;
    }
}