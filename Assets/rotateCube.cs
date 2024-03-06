using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCube : MonoBehaviour
{
    private int x_speed;
    private int y_speed;
    private int z_speed;

    double _currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        x_speed = Random.Range(0, 100);
        y_speed = Random.Range(0, 100);
        z_speed = Random.Range(0, 100);

        Debug.Log((x_speed, y_speed, z_speed));

    }

    // Update is called once per frame
    void Update()
    {
        _currentTime = (double) Time.deltaTime;
        transform.Rotate((float) (x_speed * _currentTime / 20), (float) (y_speed* _currentTime / 20),(float) (z_speed* _currentTime / 20));
    }
}
