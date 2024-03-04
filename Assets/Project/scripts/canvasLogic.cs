using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Barracuda;
using UnityEngine.Events;

public class canvasLogic : MonoBehaviour
{
    public GameObject marker;
    public Transform markerSpawnPoint;

    public NNModel _model;

    public RenderTexture renderTexture;

    public int targetNumber;

    public AudioSource source;

    public AudioClip passSound;

    public AudioClip failSound;
    public void spawnMarker()
    {
        Instantiate(marker, markerSpawnPoint.position, Quaternion.identity);
    }

    public void clearCanvas()
    {
        Texture2D capturedTexture = GetTexture2DFromRenderTexture(renderTexture);

        // Do something with capturedTexture here (e.g., save it, use it in UI, etc.)
        predict(capturedTexture);

        // Clear the canvas
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null; // Reset active RenderTexture
    }
    public Texture2D GetTexture2DFromRenderTexture(RenderTexture rTex)
    {
        // Save the current RenderTexture so we can restore it later
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set the provided RenderTexture as active
        RenderTexture.active = rTex;

        // Create a new Texture2D and read the RenderTexture content into it
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        // Restore the previously active RenderTexture
        RenderTexture.active = currentActiveRT;

        return tex;
    }

    public void predict(Texture2D _image)
    {
        // Convert the input image into a 1x28x28x1 tensor.
        using var input = new Tensor(1, 28, 28, 1);

        for (var y = 0; y < 28; y++)
        {
            for (var x = 0; x < 28; x++)
            {
                var tx = x * _image.width  / 28;
                var ty = y * _image.height / 28;
                input[0, 27 - y, x, 0] = _image.GetPixel(tx, ty).grayscale;
            }
        }

        // Run the MNIST model.
        using var worker = ModelLoader.Load(_model).CreateWorker(WorkerFactory.Device.CPU);

        worker.Execute(input);

        // Inspect the output tensor.
        var output = worker.PeekOutput();

        var scores = Enumerable.Range(0, 10).
                     Select(i => output[0, 0, 0, i]).SoftMax().ToArray();

        int maxScoreIndex = scores
            .Select((score, index) => new { Index = index, Score = score }) // Project each score with its index
            .Aggregate((a, b) => (a.Score > b.Score) ? a : b) // Aggregate to find the maximum score
            .Index;

        if (maxScoreIndex == targetNumber)
        {
            Pass();
        }
        else
        {
            Fail();
        }
    }

    public void Pass()
    {
        source.PlayOneShot(passSound,0.5f);
    }

    public void Fail()
    {
        source.PlayOneShot(failSound,0.5f);
    }
}
