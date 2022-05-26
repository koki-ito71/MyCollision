using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace C100 {
    public class Line : Physics
    {
        public Vector3 end { get; private set; }
        public bool isPlatform { private set; get; }
        public Vector3 to => position + end;

        private Physics beforeHit;

        public Line(Vector3 _from,Vector3 _to,bool _isPlatform):base()
        {
            position = _from;
            end = _to-_from;
            isPlatform = _isPlatform;
            isStatic = true;
            gravity = 0;//重力
        }
        public override void CollisionBase(Physics opponent)
        {
            Type type =opponent.GetType();
            if (type.IsSubclassOf(typeof(Square)) || type.Equals(typeof(Square)))
            {
                CollisionLinetoSquare(this, (Square)opponent);
            }
            else if (type.IsSubclassOf(typeof(Line)) || type.Equals(typeof(Line)))
            {
                CollisionEach(this, (Line)opponent);
            }
        }
        //線同士の当たり判定
        public static void CollisionEach(Line a, Line b)
        {
            //無し
        }
        //線同士の交点を返す
        public static Vector3? GetIntersection(Vector3 startA,Vector3 endA,Vector3 startB,Vector3 endB)
        {
            float tilt_a = endA.y;
            if (Mathf.Abs(endA.x) < 1e-4f)
                tilt_a = 1e+4f;
            else
                tilt_a /= endA.x;

            float tilt_b = endB.y;
            if (Mathf.Abs(endB.x) < 1e-4f)
                tilt_b = 1e+4f;
            else
                tilt_b /= endB.x;

            //Debug.Log(tilt_a);
           // Debug.Log(tilt_b);
            if (Mathf.Abs(tilt_a - tilt_b) < 1e-4f) return null;
            float x = (startB.y - startA.y
                        + tilt_a * startA.x - tilt_b * startB.x)
                            / (tilt_a - tilt_b);
            float y = tilt_a * (x - startA.x) + startA.y;
Vector3 point = new Vector3(x, y, 0);
            //Debug.Log(point);
            float r1 = tilt_a>1? (y - startA.y) / endA.y : (x - startA.x) / endA.x;
            float r2 = tilt_b > 1 ? (y - startB.y) / endB.y : (x - startB.x) / endB.x;
            bool hit = 0 <= r1 && r1 <= 1 && 0 <= r2 && r2 <= 1;
            if (!hit) return null;
            
            return point;
        }
        //線同士の当たり判定
        public static Vector3? GetIntersection2(Vector3 startA,Vector3 endA,Vector3 startB,Vector3 endB)
        {
            Vector3 v = startB - startA;
            float cross = OuterCloss(endA,endB);
            if (cross == 0f) return null;

            float vA = OuterCloss(v, endA);
            float vB = OuterCloss(v, endB);

            float tA = vB / cross;
            float tB = vA / cross;

            if (tA < 0 || 1 < tA || tB < 0 || 1 < tB) return null;
            return startA + endA * tA;
        }

        

    }
}
