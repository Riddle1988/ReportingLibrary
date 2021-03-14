using System;
using System.Collections.Generic;
using System.Reflection;

namespace Reporting
{
    public class ReportingBase
    {
        public Dictionary<string, string> AttributesForXml { get; protected set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ElementsForXml { get; protected set; } = new Dictionary<string, string>();
        public String ElementTextForXml { get; set; }
    }

    public class TestCase : ReportingBase
    {
        public String Name { get; set; } = "";
        public String Description { get; set; } = "";
        public List<TestStep> TestElements { get; set; } = new List<TestStep>();
    }

    public class TestDefinition : ReportingBase
    {
        public String DefinitionTitle { get; set; } = "";
    }

    public class TestStep : ReportingBase
    {
        public String Name { get; set; } = "";
        public String Timestamp { get; set; } = "";
    }

    public class TestLog : TestStep
    {
        public String LogLink { get; set; } = "";
        public String Type { get; set; } = "";

        public void AddFileLink(string filePath)
        {
            AttributesForXml["FileLink"] = filePath;
        }
        public void AddData(string data)
        {
            AttributesForXml["Data"] = data;
        }
    }

    public class TestResults : ReportingBase
    {
        public String Formatversion { get; private set; } = AssemblyName.GetAssemblyName("Reporting.dll").Version.ToString();
        public List<TestSuite> TestSuites { get; set; } = new List<TestSuite>();
    }

    public class TestSuite : ReportingBase
    {
        public String SuiteName { get; set; } = "Test Suite";
        public List<TestDefinition> TestDefinitions { get; set; } = new List<TestDefinition>();
        public List<TestCase> TestCases { get; set; } = new List<TestCase>();
    }

    public class Fact : TestStep
    {
        public FactOutcome Verdict { get; set; } = FactOutcome.Notset;
        public String ExpectedResult { get; set; } = "";
        public String ActualResult { get; set; } = "";
        public String LogLink { get; set; } = "";
        public String Comment { get; set; } = "";
        public enum FactOutcome
        {
            Pass,
            Fail,
            Inconclusive,
            Error,
            Notset,
        }
    }

    public class TestLogFile : TestLog
    {
        public string Filename { get; set; }
    }
}
