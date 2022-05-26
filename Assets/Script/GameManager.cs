using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C100;
public class GameManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] int frameRate;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = frameRate;
        Ditector.Init();
    }

    // Update is called once per frame
    void Update()
    {
        Ditector.Update();
        UpdateCamera();
    }
    void UpdateCamera()
    {
        Vector3 temp = player.position;
        temp.z = -10;

        transform.position = Vector3.Lerp(transform.position, temp, 12.0f * Time.deltaTime);
    }
}
