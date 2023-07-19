using System.Collections.Generic;
using UnityEngine;

public class NavyTracker : MonoBehaviour // I might just make the army tracker also track the navies. But for now this if good for clarity
{
    public Dictionary<NavyProps, Vector2> navyPositions = new Dictionary<NavyProps, Vector2>();

    public void AddNavy(NavyProps navy, Vector2 position)
    {
        if (!navyPositions.ContainsKey(navy))
        {
            navyPositions.Add(navy, position);
        }
    }

    public void RemoveNavy(NavyProps navy)
    {
        if (navyPositions.ContainsKey(navy))
        {
            navyPositions.Remove(navy);
        }
    }

    public void UpdateNavyPosition(NavyProps navy, Vector2 newPosition) //I can optimise the fuck out of this shit
    {
        if (navyPositions.ContainsKey(navy))
        {
            navyPositions[navy] = newPosition;
        }
    }
}