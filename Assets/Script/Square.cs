using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace C100
{
    public class Square : Physics
    {
        public Vector2 size { get; private set; }

        public Square(Vector2 _size, bool _isStatic) : base()
        {
            if (_size.x <= 0 || _size.y <= 0) Debug.LogWarning("面積が0以下です" + _size);
            this.size = _size;
            isStatic = _isStatic;
            gravity = isStatic ? 0 : 80;//重力調節
        }

        
        public bool Inclusive(Vector3 pos)
        {
            Vector3 d = pos - position;
            return (size.x / 2 >= Mathf.Abs(d.x)) && (size.y / 2 >= Mathf.Abs(d.y));
        }
        public override void CollisionBase(Physics opponent)
        {
            Type type = opponent.GetType();
            if (type.IsSubclassOf(typeof(Square)) || type.Equals(typeof(Square)))
            {
                CollisionEach(this, (Square)opponent);
            }
            else if (type.IsSubclassOf(typeof(Line)) || type.Equals(typeof(Line)))
            {
                Line.CollisionLinetoSquare((Line)opponent, this);
            }
        }
        public static void CollisionEach(Square a, Square b)
        {
            if (a.isStatic && b.isStatic) return;//両方静的なら何もしない
            Vector2 delta = Vector2.zero;
            delta.x = (a.size.x + b.size.x) / 2 - Mathf.Abs(a.position.x - b.position.x);//deltaは共通範囲の矩形の大きさ
            delta.y = (a.size.y + b.size.y) / 2 - Mathf.Abs(a.position.y - b.position.y);
            if (delta.x <= 0 || delta.y <= 0) return;//共通範囲のdeltaが無いなら何もしない
            Vector3 coef = Vector3.zero;
            coef.x = delta.x < delta.y ? delta.x : 0;
            coef.y = delta.x >= delta.y ? delta.y : 0;
            coef.x *= a.position.x > b.position.x ? 1 : -1;
            coef.y *= a.position.y > b.position.y ? 1 : -1;
            if (!a.isStatic && !b.isStatic)
            {
                a.position += coef / 2;
                b.position -= coef / 2;
            }
            else if (a.isStatic)
            {
                b.position -= coef;
            }
            else
            {
                a.position += coef;
            }

            //衝突方向の速度0
            if (coef.x * a.velocity.x < 0) a.velocity.x = 0;
            if (coef.x * b.velocity.x > 0) b.velocity.x = 0;
            if (coef.y * a.velocity.y < 0) a.velocity.y = 0;
            if (coef.y * b.velocity.y > 0) b.velocity.y = 0;

            //コールバック
            b.CollisionTrigger?.Invoke(b);
            a.CollisionTrigger?.Invoke(a);
        }
    }
}