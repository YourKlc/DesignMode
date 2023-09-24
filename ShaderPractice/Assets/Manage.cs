using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manage : MonoBehaviour
{
    public Button m_btn;
    public GameObject[] m_objs;
    public Text m_text;
    int m_index;
    void Awake()
    {
        m_index = 0;
        m_objs[m_index].SetActive(true);
        m_text.text = $"Next {m_index}";
        m_btn.onClick.AddListener(() =>
        {
            m_objs[m_index].SetActive(false);
            m_index = (m_index + 1) % m_objs.Length;
            m_objs[m_index].SetActive(true);
            m_text.text = $"Next {m_index}";
        });
    }

}
