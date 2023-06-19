using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct IntRange
{
    public int Min;
    public int Max;
    public float Weight;
}
 
public struct FloatRange
{
    public float Min;
    public float Max;
    public float Weight;
}
 
public static class WeightedRandom
{

    public static CelBody ChooseCelBody(params CelBody[] celBodies)
    {
        IntRange[] ranges = new IntRange[celBodies.Length];
        for (int i = 0; i < celBodies.Length; i++)
        {
            ranges[i] = new IntRange()
                { Min = celBodies[i].ID, Max = celBodies[i].ID + 1, Weight = celBodies[i].SpawningWeight };
        }

        if (ranges.Length == 0) throw new System.ArgumentException("At least one range must be included.");
        if (ranges.Length == 1) return celBodies[0];
        float total = 0f;
        for (int i = 0; i < ranges.Length; i++) total += ranges[i].Weight;

        float rand = Random.value;
        float selection = 0f;

        int count = ranges.Length - 1;
        for (int i = 0; i < count; i++)
        {
            selection += ranges[i].Weight / total;
            if (selection >= rand)
            {
                return celBodies[i];
            }
        }

        return celBodies[count];
    }

    public static int Range(params CelBody[] celBodies)
    {
        IntRange[] ranges = new IntRange[celBodies.Length];
        for (int i = 0; i < celBodies.Length; i++)
        {
            ranges[i] = new IntRange() {Min = celBodies[i].ID, Max = celBodies[i].ID + 1, Weight = celBodies[i].SpawningWeight};
        }
        
        if (ranges.Length == 0) throw new System.ArgumentException("At least one range must be included.");
        if (ranges.Length == 1) return Random.Range(ranges[0].Max, ranges[0].Min);
 
        float total = 0f;
        for (int i = 0; i < ranges.Length; i++) total += ranges[i].Weight;
 
        float rand = Random.value;
        float selection = 0f;
 
        int count = ranges.Length - 1;
        for (int i = 0; i < count; i++)
        {
            selection += ranges[i].Weight / total;
            if (selection >= rand)
            {
                return Random.Range(ranges[i].Max, ranges[i].Min);
            }
        }
 
        return Random.Range(ranges[count].Max, ranges[count].Min);
    }
    
    public static int Range(params IntRange[] ranges)
    {
        if (ranges.Length == 0) throw new System.ArgumentException("At least one range must be included.");
        if (ranges.Length == 1) return Random.Range(ranges[0].Max, ranges[0].Min);
 
        float total = 0f;
        for (int i = 0; i < ranges.Length; i++) total += ranges[i].Weight;
 
        float rand = Random.value;
        float selection = 0f;
 
        int count = ranges.Length - 1;
        for (int i = 0; i < count; i++)
        {
            selection += ranges[i].Weight / total;
            if (selection >= rand)
            {
                return Random.Range(ranges[i].Max, ranges[i].Min);
            }
        }
 
        return Random.Range(ranges[count].Max, ranges[count].Min);
    }
 
    public static float Range(params FloatRange[] ranges)
    {
        if (ranges.Length == 0) throw new System.ArgumentException("At least one range must be included.");
        if (ranges.Length == 1) return Random.Range(ranges[0].Max, ranges[0].Min);
 
        float total = 0f;
        for (int i = 0; i < ranges.Length; i++) total += ranges[i].Weight;
 
        float r = Random.value;
        float s = 0f;
 
        int cnt = ranges.Length - 1;
        for (int i = 0; i < cnt; i++)
        {
            s += ranges[i].Weight / total;
            if (s >= r)
            {
                return Random.Range(ranges[i].Max, ranges[i].Min);
            }
        }
 
        return Random.Range(ranges[cnt].Max, ranges[cnt].Min);
    }
 
}
