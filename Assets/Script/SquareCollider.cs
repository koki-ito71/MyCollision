using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C100;

public class SquareCollider : MonoBehaviour
{
    [SerializeField] Vector2 size;
    [SerializeField] bool isStatic;
    void Start()
    {
        Vector3 v = (Vector3)size;
        v.Scale(transform.localScale);
        C100.Physics s = new TransformSquare(v, isStatic, this.transform);
        Ditector.Add(s);


    }

    private void OnDrawGizmos()
    {
        Vector3 v = (Vector3)size;
        v.Scale(transform.localScale);
        Gizmos.DrawWireCube(transform.position, v);
    }

}
