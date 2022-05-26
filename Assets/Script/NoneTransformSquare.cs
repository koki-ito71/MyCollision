using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C100;
public class NoneTransformSquare : Square
{
    public NoneTransformSquare(Vector2 bottomLeft, Vector2 upRight)
            : base(upRight - bottomLeft, true)
    {
        position = (upRight + bottomLeft) / 2f;
    }
}
