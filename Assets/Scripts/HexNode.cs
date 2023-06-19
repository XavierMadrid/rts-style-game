using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class HexNode : MonoBehaviour 
{

    [Range(0, 5)]
    public int dir;
    public bool randomizeDir = false;
    public bool lockZ = false;

    public Hex hex 
    {
        get 
        {
            return transform.position.ToHex();
        }
    }

    public Hex localHex 
    {
        get 
        {
            return transform.localPosition.ToHex();
        }
    }

    public void ApplyTransform() 
    {
        if (randomizeDir) 
        {
            Hex hex = this.hex;
            int i = hex.q * 100 + hex.r;
            dir = ((i % 6) + 6) % 6;
        }
        float z = lockZ ? 0f : transform.localPosition.z;
        Vector3 newPos = this.localHex.ToWorld(z);
        transform.localPosition = newPos;
        transform.localRotation = Quaternion.Euler(0, 0, -60f * dir);
    }

#if UNITY_EDITOR
    protected virtual void Update() 
    {
        if (!Application.isPlaying) 
        {
            ApplyTransform();
            // Hack to never re-apply dir to instances
            this.dir += 1;
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            this.dir = (dir - 1) % 6;
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }
    }

    void OnDrawGizmosSelected() 
    {
        UnityEditor.Handles.Label(transform.position, hex.ToString());
    }
#endif

}