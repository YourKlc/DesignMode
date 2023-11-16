using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class buildinPostProcess : MonoBehaviour
{
    public Material grayMaterial;
    [Range(0f, 1f)]
    public float grayScaleAmount = 1.0f;

    void Start()
    {
        // ≈–∂œ÷’∂À «∑Ò÷ß≥÷
        if (grayMaterial != null && grayMaterial.shader.isSupported == false)
        {
            enabled = false;
        }
    }

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (grayMaterial != null)
        {
            grayMaterial.SetFloat("_Color", grayScaleAmount);
            Graphics.Blit(sourceTexture, destTexture, grayMaterial);
        }
        else
        {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }
}
