using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Manage : MonoBehaviour
{
    public Button m_btn;
    public GameObject[] m_objs;
    public Text m_text;
    int m_index;
    int frameCount;
    void Awake()
    {
        frameCount = 0;
        m_index = 0;
        m_objs[m_index].SetActive(true);
        m_text.text = $"Next {m_index}";
        m_btn.onClick.AddListener(() =>
        {
            frameCount = 0;

            m_objs[m_index].SetActive(false);
            m_index = (m_index + 1) % m_objs.Length;
            m_objs[m_index].SetActive(true);
            //UpdateIndex(m_index);
            m_text.text = $"Next {m_index}";
        });
    }
    float oriX, oriY;
    void Update()
    {
        Material mt = m_objs[m_index].GetComponent<MeshRenderer>().material;

        float x = .0f, y = .0f;
        if (Input.GetMouseButton(0))
        {
            x = Input.GetAxis("Mouse X");
            y = Input.GetAxis("Mouse Y");
        }

        try
        {
            mt.SetInt("_iFrame", frameCount / 100);

            if (mt.HasVector("_iMouse"))
            {
                Vector4 v4 = mt.GetVector("_iMouse");
                mt.SetVector("_iMouse", new Vector4(x,y) * 5 + v4);
                Debug.Log(x + " " + y);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        frameCount++;
    }
    private void OnMouseDrag()
    {
        //Vector2 startPoint = Input.mousePosition;
        //Vector2 objectPosition = Camera.main.ScreenToWorldPoint(startPoint);
        //transform.position = objectPosition;
    }
}
