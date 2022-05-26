using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C100;

//プレーヤーの操作
public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float Verocity;
    [SerializeField] Vector2 size;
    [SerializeField] bool isStatic;
    private Square s;

    private bool canJump;

    // Start is called before the first frame update
    void Start()
    {
        s = new TransformSquare(size, isStatic, this.transform);
        Ditector.Add(s);
        s.SetTrigger((C100.Physics s) => { canJump = true; });
        // s.SetTrigger(CallBack);
        canJump = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 1));
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 vector = Vector3.zero;
        if (transform.position.y <= -20)
        {
            s.position = Vector3.zero;
            s.velocity = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.D))
        {
            vector.x += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            vector.x -= 1;

        }
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        vector = vector * Verocity;
            s.velocity.x = vector.x;
    }

    public void Jump()
    {
        if (canJump)
        {
            s.velocity.y = 25;
            canJump = false;
        }
    }

}
