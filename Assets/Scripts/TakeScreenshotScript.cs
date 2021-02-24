using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshotScript : MonoBehaviour
{
    public Camera topViewCam;
    public Material mapMaterial;

    public static TakeScreenshotScript instance;

    private void Awake()
    {
        instance = this;
    }

    

    public IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        topViewCam.gameObject.SetActive(true);

        RenderTexture renderTexture = null;
        topViewCam.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        topViewCam.Render();

        Texture2D texture = new Texture2D(topViewCam.pixelWidth, topViewCam.pixelHeight, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0);
        texture.Apply();
        mapMaterial.SetTexture("_MainTex", texture);

        RenderTexture.active = null;
        topViewCam.targetTexture = null;

        topViewCam.gameObject.SetActive(false);

        yield return null;
    }
}
