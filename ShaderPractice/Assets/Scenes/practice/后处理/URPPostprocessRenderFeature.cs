using UnityEngine;
using UnityEngine.Rendering.Universal;

public class URPPostprocessRenderFeature : ScriptableRendererFeature
{
    //设置一个RendererFeature的配置
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public LayerMask layerMask = -1;
        public Material blurMat;
        public string textureName = "";
        public string cmdName = "";
        public string passName = "";
        //目标RenderTexture 
        public RenderTexture blurRt = null;
    }

    URPPostprocessRenderPass m_ScriptablePass;
    private RenderTargetHandle dest;
    public Settings settings;
    public override void Create()
    {
        m_ScriptablePass = new URPPostprocessRenderPass(settings);
        m_ScriptablePass.renderPassEvent = settings.renderEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var src = renderer.cameraColorTarget;
        dest = RenderTargetHandle.CameraTarget;
        //把camera渲染到的画面src 传入GlassBlurRenderPass里。
        m_ScriptablePass.Setup(src, this.dest);
        //注入队列
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
