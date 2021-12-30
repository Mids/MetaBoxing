using System.Collections.Generic;
using System.Linq;
using MetaBoxing;
using Photon.Pun;
using UnityEngine;

public class Measure : MonoBehaviour
{
    private List<ABController> _abList;

    public float stiffness = 0f;
    public float damping = 0f;

    public float MSESum = 0f;
    public float MSEMean = 0f;
    public float MSEMax = 0f;

    public float RMSESum = 0f;
    public float RMSEMean = 0f;
    public float RMSEMax = 0f;

    public int frameCount = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _abList = GetComponentsInChildren<ABController>().ToList();
    }

    // Update is called once per frame
    private void Update()
    {
        ++frameCount;

        if (frameCount <= 100)
            return;

        var frameMSESum = 0f;
        var frameRMSESum = 0f;
        foreach (var ab in _abList)
        {
            var mseDiff = (ab.transform.position - ab.kinematicBody.position).sqrMagnitude;

            frameMSESum += mseDiff;

            var rmseDiff = (ab.transform.position - ab.kinematicBody.position).magnitude;

            frameRMSESum += rmseDiff;
        }

        var frameMSE = frameMSESum / _abList.Count;
        var frameRMSE = frameRMSESum / _abList.Count;

        MSEMax = Mathf.Max(frameMSE, MSEMax);
        RMSEMax = Mathf.Max(frameRMSE, RMSEMax);

        MSESum += frameMSE;
        MSEMean = MSESum / frameCount;
        RMSESum += frameRMSE;

        if (frameCount % 1000 == 100)
        {
            RMSEMean = RMSESum / 1000;
            print($"{frameCount}\t{name}\t{RMSEMean}\t{RMSEMax}");
            RMSEMax = 0f;
            RMSESum = 0f;
        }
    }
}