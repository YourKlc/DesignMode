using UnityEngine;
using UnityEngine.Rendering.Universal;

public class URPPostprocessRenderFeature : ScriptableRendererFeature
{
    //����һ��RendererFeature������
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public LayerMask layerMask = -1;
        public Material blurMat;
        public string textureName = "";
        public string cmdName = "";
        public string passName = "";
        //Ŀ��RenderTexture 
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
        //��camera��Ⱦ���Ļ���src ����GlassBlurRenderPass�
        m_ScriptablePass.Setup(src, this.dest);
        //ע�����
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
