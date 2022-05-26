using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using C100;
[RequireComponent(typeof(LineRenderer))]

public class LineCollider : MonoBehaviour
{
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 to;
    [SerializeField] private bool isPlatform=false;
    private void Start()
    {
        Line line = new Line(from+transform.position, to + transform.position,isPlatform);
        Ditector.Add(line);
    }
    private void OnValidate()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from + transform.position);
        lineRenderer.SetPosition(1, to + transform.position);
    }
}
