using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;

public class pf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        string path = Application.persistentDataPath + "/example.xml";
        TestClass testClass = new TestClass();
        
        // StreamWriter 写入一个文件流，如果有则直接打开，如果没有就新建这个文件
        // using功能，当代码块结束时会自动调用括号中的对象streamWriter的Dispose()方法，释放资源
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestClass));
            // 文件流对象    需要序列化的具体对象，需要和上面创建时指定的类一致
            serializer.Serialize(streamWriter, testClass);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class TestClass
{
    [XmlElement("A")]
    public int a;
    public string b;
    
    [XmlArrayItem("Item")]
    public List<int> c;

    [XmlAttribute("Attribute1")] 
    private int d;
    
    public TestClass()
    {
        a = 0;
        b = "default";
        c = new List<int>();
        d = 3;
    }
}