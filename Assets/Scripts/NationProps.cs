using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NationProps : MonoBehaviour
{
    public string nationName;
    //public bool AI;
    public List<TileProps> tiles;
    public int population;
    //public int troops;
    public int money;
    public int debt;

    public List<int> populationHistory = new List<int>();

    //MIGHT PUT THIS STUFF IN ANOTHER SCRIPT ##############################################################################
    public int Timber;

    //public int Lead;

    //public int Sulphur;

    //public int Copper;

    public int Iron;

    public int Coal;

    //public int Rubber;

    public int Gold;

    //public int Oil;

        //AGRICULTURE
    public int wheat;
    public int cotton;
    //#####################################################################################################################
}