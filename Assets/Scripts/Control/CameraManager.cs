using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    // 放大
    public void ZoomIn()
    {
        mainCam.DOOrthoSize(13, 0.5f);
    }

    // 缩小
    public void ZoomOut()
    {
        mainCam.DOOrthoSize(20, 0.5f);
    }
}
