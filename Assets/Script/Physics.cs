using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace C100
{
    public abstract class Physics
    {
        public virtual Vector3 position { get; set; }//今の位置
        public Vector3 velocity; //速度
        public Vector3 beforeVelocity { get; private set; }

        public bool isStatic { protected set; get; }//静的か動的なものか
        public virtual float gravity { get; protected set; }
        public Vector3 accelaration;//加速度

        protected Action<Physics> CollisionTrigger { get; private set; }

        public Physics()
        {
            velocity = Vector3.zero;
            accelaration = Vector3.zero;
        }
        public void SetTrigger(Action<Physics> action)
        {
            CollisionTrigger += action;
        }
        public void UpdateParametar()
        {
            beforeVelocity = velocity;
            velocity += (accelaration + Vector3.down * gravity) * Ditector.scaledDeltaTime;
            position += velocity * Ditector.scaledDeltaTime;
        }
        public abstract void CollisionBase(Physics opponent);

        //外積
        protected static float OuterCloss(Vector3 a,Vector3 b)
        {
            return a.x * b.y - a.y * b.x;
        }
        //長さ^2
        protected static float SquaredLength2D(Vector3 v)
        {
            return v.x * v.x + v.y * v.y;
        }
        //矩形と線の当たり判定
        public static void CollisionLinetoSquare(Line line,Square square)
        {
            if (square.isStatic) return;
            Vector3[] edges = new Vector3[4];
            Vector2 half = square.size/2;
            edges[0] = square.position + new Vector3(half.x, half.y, 0);
            edges[1] = square.position + new Vector3(-half.x, half.y, 0);
            edges[2] = square.position + new Vector3(-half.x, -half.y, 0);
            edges[3] = square.position + new Vector3(half.x, -half.y, 0);

            Vector3 point1=Vector3.zero;
            Vector3 point2 = Vector3.zero;
           
            int first = -1;
            int second = -1;

            for(int i = 0; i < 4; i++)
            {
                //Debug.Log(edges[i]);
                Vector3? v = Line.GetIntersection2(
                    edges[i],edges[(i+1)%4]- edges[i],
                    line.position,line.end
                );
                if (v != null)
                {
                    if (first<0)
                    {
                        point1 = (Vector3)v;
                        point2=point1;
                        first = i;
                    }
                    else
                    {
                        point2 = (Vector3)v;
                        second = i;
                        break;
                    }
                }
            }
            if (first < 0) return;
            Vector3 delta = Vector3.zero;
            int inclusive = 0;
            if (square.Inclusive(line.position))inclusive++;
            if (square.Inclusive(line.to))inclusive++;
            //Debug.Log(first + "  " + second + "  " + inclusive);

            //Debug.Log(square.velocity);
            if (second>-1&&((first+second)%2==1))//角
            {
                if (second == 3 && first == 0) second = 0;
                point1 -= edges[second];
                point2 -= edges[second];
                float r = SquaredLength2D(point1) / (SquaredLength2D(point1) + SquaredLength2D(point2));
                if (Double.IsNaN(r)) return;
                delta = point1 * (1 - r) + point2 * r;
                //Debug.Log(delta);
            }
            else if (inclusive==0&&(first + second) % 2 == 0)//線
            {
                if (first == 0)//f=0,s=2
                {
                    if ((point1.x + point2.x) / 2 > square.position.x)
                    {
                        if (point1.x < point2.x)
                        {
                            delta.x = point1.x - edges[0].x;
                        }
                        else
                        {
                            delta.x = point2.x - edges[0].x;
                        }
                    }
                    else
                    {
                        if (point1.x > point2.x)
                        {
                            delta.x = point1.x - edges[2].x;
                        }
                        else
                        {
                            delta.x = point2.x - edges[2].x;
                        }
                    }
                }
                else//f=1,s=3
                {
                    if ((point1.y + point2.y) / 2 > square.position.y)
                    {
                        if (point1.y < point2.y)
                        {
                            delta.y = point1.y - edges[1].y;
                        }
                        else
                        {
                            delta.y = point2.y - edges[1].y;
                        }
                    }
                    else
                    {
                        if (point1.y > point2.y)
                        {
                            delta.y = point1.y - edges[3].y;
                        }
                        else
                        {
                            delta.y = point2.y - edges[3].y;
                        }
                    }
                }
            }
            else if(inclusive==1)//点
            {
                Vector3 edge = line.position;
                if (square.Inclusive(line.to)) edge = line.to;
               
                Vector3 conf =(edge+point1)/2- square.position;
                conf.x *= square.size.y / square.size.x;

                if (Mathf.Abs(conf.x)> Mathf.Abs(conf.y))
                {
                    if ((point1.x + edge.x) / 2 > square.position.x)
                    {
                        if (point1.x < edge.x)
                        {
                            delta.x = point1.x - edges[0].x;
                        }
                        else
                        {
                            delta.x = edge.x - edges[0].x;
                        }
                    }
                    else
                    {
                        if (point1.x > edge.x)
                        {
                            delta.x = point1.x - edges[2].x;
                        }
                        else
                        {
                            delta.x = edge.x - edges[2].x;
                        }
                    }
                }
                else
                {
                    if ((point1.y + edge.y) / 2 > square.position.y)
                    {
                        if (point1.y < edge.y)
                        {
                            delta.y = point1.y - edges[1].y;
                        }
                        else
                        {
                            delta.y = edge.y - edges[1].y;
                        }
                    }
                    else
                    {
                        if (point1.y > edge.y)
                        {
                            delta.y = point1.y - edges[3].y;
                        }
                        else
                        {
                            delta.y = edge.y - edges[3].y;
                        }
                    }
                }

            }
            //platform
            if (line.isPlatform)
            {
                float closs = OuterCloss(line.end, delta);
                //Debug.Log(closs);
                if (closs <=0) return;
            }
            if (delta.x * square.velocity.x < 0) square.velocity.x = 0;
            if (delta.y * square.velocity.y < 0) square.velocity.y = 0;
            square.position += delta;

            square.CollisionTrigger?.Invoke(line);
            line.CollisionTrigger?.Invoke(square);
        }
    }
}
