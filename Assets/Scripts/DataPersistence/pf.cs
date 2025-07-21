using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class pf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        string path = Application.persistentDataPath + "/test.json";
        TestClass t = new TestClass();
        string str = JsonUtility.ToJson(t);
        File.WriteAllText(path, str);
        
        string jsonstr = File.ReadAllText(path);
        TestClass t2 = JsonUtility.FromJson<TestClass>(jsonstr);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class TestClass
{
    [XmlElement("A")]
    public int a;
    public string b;
    
    [XmlArrayItem("Item")]
    public List<int> c;

    [XmlAttribute("Attribute1")] 
    [SerializeField]
    private int d;
    
    public TestClass()
    {
        a = 0;
        b = "default";
        c = new List<int>();
        d = 3;
    }
}