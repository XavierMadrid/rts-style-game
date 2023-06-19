using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class HexVectorExtensions 
{

    public static Vector2 WorldToPlanar(this Vector3 world) 
    {
        return new Vector2(world.x, world.y);
    }

    public static Vector3 PlanarToWorld(this Vector2 planar, float z = 0f) 
    {
        return new Vector3(planar.x, planar.y, 0);
    }

    public static Hex ToHex(this Vector3 world) 
    {
        return Hex.FromWorld(world);
    }

    public static Hex ToHex(this Vector2 planar) 
    {
        return Hex.FromPlanar(planar);
    }

    public static Vector3 SnapWorld(this Vector3 world)
    {
        return world.ToHex().ToWorld();
    }

}

[System.Serializable]
public struct Hex 
{

    public static float RADIUS = 0.5f;
    public static Vector2 Q_BASIS = new Vector2(2f, 0) * RADIUS;
    public static Vector2 R_BASIS = new Vector2(1f, Mathf.Sqrt(3)) * RADIUS;
    public static Vector2 Q_INV = new Vector2(1f / 2, - Mathf.Sqrt(3) / 6);
    public static Vector2 R_INV = new Vector2(0, Mathf.Sqrt(3) / 3);

    public static Hex[] AXIAL_DIRECTIONS = new Hex[] 
    {
        new Hex(3, 3),      // 0
        new Hex(6, -3),     // 1
        new Hex(3, -6),     // 2
        new Hex(-3, -3),    // 3
        new Hex(-6, 3),     // 4
        new Hex(-3, 6)      // 5
    };

    public static Hex FromPlanar(Vector2 planar) 
    {
        float q = Vector2.Dot(planar, Q_INV) / RADIUS;
        float r = Vector2.Dot(planar, R_INV) / RADIUS;
        return new Hex(q, r);
    }

    public static Hex FromWorld(Vector3 world) 
    {
        return FromPlanar(world.WorldToPlanar());
    }

    public static Hex operator +(Hex a, Hex b) 
    {
        return new Hex(a.q + b.q, a.r + b.r);
    }

    public static Hex operator -(Hex a, Hex b) 
    {
        return new Hex(a.q - b.q, a.r - b.r);
    }

    public static Hex zero = new Hex(0, 0);

    public static IEnumerable<Hex> Ring(Hex center, int radius) 
    {
        Hex current = center + new Hex(0, -radius);
        foreach (Hex dir in AXIAL_DIRECTIONS) 
        {
            for (int i = 0; i < radius; i++) 
            {
                yield return current;
                current = current + dir;
            }
        }
    }

    public static IEnumerable<Hex> Spiral(Hex center, int minRadius, int maxRadius) 
    {
        if (minRadius == 0) 
        {
            yield return center;
            minRadius += 1;
        }
        for (int r = minRadius; r <= maxRadius; r++) 
        {
            var ring = Ring(center, r);
            foreach (Hex hex in ring) 
            {
                yield return hex;
            }
        }
    }

    public static IEnumerable<Hex> FloodFill(IEnumerable<Hex> startFrom) 
    {
        HashSet<Hex> visited = new HashSet<Hex>();
        Queue<Hex> frontier = new Queue<Hex>(startFrom);
        while (frontier.Count > 0) {
            Hex current = frontier.Dequeue();
            yield return current;
            foreach (Hex next in current.Neighbours()) 
            {
                if (visited.Contains(next)) 
                {
                    continue;
                }
                visited.Add(next);
                frontier.Enqueue(next);
            }
        }
    }

    public int q;
    public int r;

    public Hex(float q, float r) // : this(Mathf.RoundToInt(q), Mathf.RoundToInt(r))
    {
        int qint = Mathf.RoundToInt(q);
        int rint = Mathf.RoundToInt(r);
        int qmod3 = qint % 3; //make q multiple of 3
        if (qmod3 >= 1.5f) qint += 3 - qmod3; 
        else qint -= qmod3;
        int rmod3 = rint % 3; //make r multiple of 3
        if (rmod3 >= 1.5f) rint += 3 - rmod3;
        else rint -= rmod3;
        int rmod9 = (rint - qint % 9) % 9; // make r a multiple of 9 with q offset
        if (rmod9 >= 4.5f) rint += 9 - rmod9;
        else rint -= rmod9;
        this.q = qint;
        this.r = rint;
    }

    public Hex(int q, int r)
    {
        int qmod3 = q % 3; //make q multiple of 3
        if (qmod3 >= 1.5f) q += qmod3;
        else q -= qmod3;
        int rmod3 = r % 3; //make r multiple of 3
        if (rmod3 >= 1.5f) r += rmod3;
        else r -= rmod3;
        int rmod9 = (r - q % 9) % 9; // make r a multiple of 9 with q offset
        if (rmod9 >= 4.5f) r += 9 - rmod9;
        else r -= rmod9;
        this.q = q;
        this.r = r;
    }

    public Vector2 ToPlanar() 
    {
        return Q_BASIS * q + R_BASIS * r;
    }

    public Vector3 ToWorld(float z = 0f) 
    {
        return ToPlanar().PlanarToWorld(z);
    }

    public IEnumerable<Hex> Neighbours() 
    {
        foreach (Hex dir in AXIAL_DIRECTIONS) 
        {
            yield return this + dir;
        }
    }

    public Hex GetNeighbour(int dir) 
    {
        Hex incr = AXIAL_DIRECTIONS[dir % AXIAL_DIRECTIONS.Length];
        return this + incr;
    }

    public int DistanceTo(Hex to) 
    {
        return (Mathf.Abs(q - to.q)
          + Mathf.Abs(q + r - to.q - to.r)
          + Mathf.Abs(r - to.r)) / 2;
    }

    public override bool Equals(System.Object obj) 
    {
        Hex hex = (Hex)obj;
        return (q == hex.q) && (r == hex.r);
    }

    public override int GetHashCode() 
    {
        return 23 + 31 * q + 37 * r;
    }

    public override string ToString() 
    {
        return "(" + q + ";" + r + ")";
    }

}