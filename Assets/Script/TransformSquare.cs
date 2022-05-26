using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C100;

public class TransformSquare : Square
{
    public override Vector3 position
    {
        get => transform.position;
        set => transform.position = value;
    }
    private Transform transform;
    public TransformSquare(Vector2 _size, bool _isStatic,Transform _transform) :base(_size,_isStatic)
    {
        transform = _transform;
    }
}
