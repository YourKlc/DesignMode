using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class textBehaviourScript : MonoBehaviour
{
    public TMP_Text tmp;
    [Range(0.1f, 10f)]
    public float xScale;
    [Range(0.1f, 2f)]
    public float movingSpeed;
    float maxThre;
    [Range(0, 1f)]
    public float prcnt = 0.5f;

    float maxDis = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        tmp.ForceMeshUpdate();
        var textInfo = tmp.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] =
                    orig + new Vector3(Mathf.Sin(Time.time / movingSpeed + orig.y * 0.3f) * xScale, 0, 0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            tmp.UpdateGeometry(meshInfo.mesh, i);
        }

        //maxThre 
        Vector3[] arrPoint = new Vector3[4];
        Vector3 pos = transform.position;
        maxDis = 0;
        for (int i = 0; i < 4; i++)
        {
            maxDis = Mathf.Max(maxDis, Vector3.Distance(textInfo.meshInfo[0].vertices[i], pos));
        }
        for (int i = 0; i < 4; i++)
        {
            maxDis = Mathf.Max(maxDis, Vector3.Distance(textInfo.meshInfo[0].vertices[textInfo.meshInfo[0].vertexCount - i - 1], pos));
        }
        Material mt = GetComponent<TMP_Text>().fontMaterial;
        mt.SetFloat("_Thre", maxDis * prcnt);
    }
}
