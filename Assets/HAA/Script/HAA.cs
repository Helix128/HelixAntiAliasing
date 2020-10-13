using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[ImageEffectAllowedInSceneView]
[ExecuteAlways]
public class HAA : MonoBehaviour
{
    ComputeShader compute;
    RenderTexture csoutput;
    RenderTexture output;
    RenderTexture depth;
    [Range(0, 4)]
    public float _Strength = 1f;
    [Range(0.2f, 4)]
    public float _Threshold = 1f;
    [Range(0,1)]
    public float _Sharpen = 0.0f;

    bool useJitter = false;
    float jitterX;
    float jitterY;

    Material depthMat;
    public enum Supersampling
    {   
        Native = 1,
        x2 = 2,
        x4 = 4,
        x8 = 8
    }
    
    Supersampling _Supersampling = Supersampling.Native;
    private void Start()
    {
        compute = (ComputeShader)Resources.Load("HAA");
        output = new RenderTexture(Screen.width*(int)_Supersampling, Screen.height * (int)_Supersampling, 24);
        output.Create();
        csoutput = new RenderTexture(Screen.width * (int)_Supersampling, Screen.height * (int)_Supersampling, 24);
        csoutput.enableRandomWrite = true;
        csoutput.Create();
        depth = new RenderTexture(Screen.width * (int)_Supersampling, Screen.height * (int)_Supersampling, 24);
        depth.Create();
        output.filterMode = FilterMode.Point;
     
       
        depthMat = new Material(Shader.Find("Hidden/DepthTexture"));
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (output.width != Screen.width * (int)_Supersampling || output.height != Screen.height * (int)_Supersampling)
        { 
            output = new RenderTexture(Screen.width * (int)_Supersampling, Screen.height * (int)_Supersampling, 24);
          
            output.Create();
            csoutput = new RenderTexture(Screen.width * (int)_Supersampling, Screen.height * (int)_Supersampling, 24);
            csoutput.enableRandomWrite = true; 
            csoutput.Create();
            depth = new RenderTexture(Screen.width * (int)_Supersampling, Screen.height * (int)_Supersampling, 24);
            depth.Create();
        }
        if (depthMat == null)
        {
            depthMat = new Material(Shader.Find("Hidden/DepthTexture"));
        }
        compute.SetBool("jitter", useJitter);
        jitterX = Random.Range(-1, 1);
        jitterY = Random.Range(-1, 1);
        compute.SetFloat("jitX", jitterX);
        compute.SetFloat("jitY", jitterY);
        compute.SetTexture(0, "_Input", output);
        compute.SetTexture(0, "Result", csoutput);
        compute.SetTexture(0, "_Depth", depth);
        compute.SetInt("width", Screen.width);
        compute.SetInt("height", Screen.height);
        compute.SetFloat("_Sharpen", _Sharpen);
        compute.SetFloat("_Strength", _Strength);
        compute.SetFloat("_Threshold", _Threshold);
        compute.SetInt("_Supersampling", 1);
        Graphics.Blit(source, output);
        Graphics.Blit(source, depth, depthMat);
       
        compute.Dispatch(0, Screen.width/8, Screen.width/8, 1);
        Graphics.Blit(csoutput, destination);
    }
}
