using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Xeon.IO;

public class CsvParseTest
{
    private enum TestEnum
    {
        Zero,
        One,
        Two,
    }
    [Serializable]
    private class TestData : CsvData
    {
        [CsvColumn("int_value")]
        public int intValue;
        [CsvColumn("float_value")]
        public float floatValue;
        [CsvColumn("string_value")]
        public string stringValue;
        [CsvColumn("bool_value")]
        public bool boolValue;
        [CsvColumn("vector2_value")]
        public Vector2 vector2Value;
        [CsvColumn("vector2int_value")]
        public Vector2Int vector2IntValue;
        [CsvColumn("vector3_value")]
        public Vector3 vector3Value;
        [CsvColumn("vector3int_value")]
        public Vector3Int vector3IntValue;
        [CsvColumn("vector4_value")]
        public Vector4 vector4Value;
        [CsvColumn("enum_value")]
        public TestEnum enumValue;
        [CsvColumn("private_int_value")]
        private int privateValue;

        public TestData() { }
        public TestData(int privateValue)
            => this.privateValue = privateValue;
    }
    [Test]
    public void ToCsvTest()
    {
        var testData = new TestData(12)
        {
            intValue = 10,
            floatValue = 123.45f,
            stringValue = "abcdefg",
            boolValue = false,
            vector2Value = new Vector2(10f, 10.5f),
            vector2IntValue = Vector2Int.down,
            vector3Value = new Vector3(1.1f, 2.2f, 3.3f),
            vector3IntValue = Vector3Int.one,
            vector4Value = new Vector4(1.1f, 2.2f, 3.3f, 4),
            enumValue = TestEnum.Two
        };
        var csv = CsvParser.ToCSV(new List<TestData>(){ testData }, ",");
        var expect = @"int_value,float_value,string_value,bool_value,vector2_value,vector2int_value,vector3_value,vector3int_value,vector4_value,enum_value,private_int_value
10,123.45,""abcdefg"",False,(10,10.5),(0,-1),(1.1,2.2,3.3),(1,1,1),(1.1,2.2,3.3,4),Two,12
";
        Assert.That(expect == csv);
    }

    [Test]
    public void ParseFileTest()
    {
        var path = Application.dataPath.Replace("Assets", "");
        path = Path.Combine(path, "Packages/SimpleCSVParser/Test/TestData/Test.csv");
        var obj = CsvParser.ParseFile<TestData>(path, ",");
        Debug.Log(obj.Count);
    }

    [Test]
    public void ParseTest()
    {
        var csv = @"int_value,float_value,string_value,bool_value,vector2_value,vector2int_value,vector3_value,vector3int_value,vector4_value,enum_value,private_int_value
10,123.45,""abcdefg"",False,(10,10.5),(0,-1),(1.1,2.2,3.3),(1,1,1),(1.1,2.2,3.3,4),Two,12
";
        var obj = CsvParser.Parse<TestData>(csv, ",");
        Debug.Log(obj.Count);
        Assert.That(obj.Count == 1);
        var data = obj.First();
        Assert.That(data.intValue == 10);
        Assert.That(data.floatValue == 123.45f);
        Assert.That(data.stringValue == "abcdefg");
        Assert.That(data.boolValue == false);
        Assert.That(data.vector2Value == new Vector2(10f, 10.5f));
        Assert.That(data.vector2IntValue == Vector2Int.down);
        Assert.That(data.vector3Value == new Vector3(1.1f, 2.2f, 3.3f));
        Assert.That(data.vector3IntValue == Vector3Int.one);
        Assert.That(data.vector4Value == new Vector4(1.1f, 2.2f, 3.3f, 4));
        Assert.That(data.enumValue == TestEnum.Two);
    }
}
