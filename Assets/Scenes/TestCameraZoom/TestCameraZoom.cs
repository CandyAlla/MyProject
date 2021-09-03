using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraZoom : MonoBehaviour
{
    public Camera _cam;
    void Start()
    {
        var recognizer = new TKPinchRecognizer();
        recognizer.gestureRecognizedEvent += ( r ) =>
        {
            Zoom(r);
        };
        TouchKit.addGestureRecognizer( recognizer );
    }

    private void Zoom(TKPinchRecognizer r)
    {
        Debug.Log(r.deltaScale);
        
        // _cam.fieldOfView += r.deltaScale;
    }
}
