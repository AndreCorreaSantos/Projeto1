using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SceneMng : MonoBehaviour
{
    List<InputDevice> inputDevices = new List<InputDevice>();

    public bool detectedTriggers = false;
    public FadeScreen fadeScreen;
    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    {
        bool rightTrigger = false;
        bool leftTrigger = false;

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDevices);
        foreach (InputDevice device in inputDevices)
        {
            device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            Debug.Log(device.name + " " + triggerValue);
            if (triggerValue > 0.9) {
                rightTrigger = true;
            }
        }
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, inputDevices);
        foreach (InputDevice device in inputDevices)
        {
            device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            Debug.Log(device.name + " " + triggerValue);
            if (triggerValue > 0.9)
            {
                leftTrigger = true;
            }
        }


        detectedTriggers = leftTrigger && rightTrigger;
        int sceneIndex = 1;
        if (detectedTriggers)
        {
            StartCoroutine(GoToSceneRoutine(sceneIndex));
        }
    }
    public IEnumerator GoToSceneRoutine(int sceneIndex) { 
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
