using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasLogic : MonoBehaviour
{
    public GameObject marker;
    public Transform markerSpawnPoint;

    public RenderTexture renderTexture;
    public void spawnMarker()
    {
        Instantiate(marker, markerSpawnPoint.position, Quaternion.identity);
    }

    public void clearCanvas()
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }
}
