using System;
using System.Collections.Generic;
using System.Reflection;
using Reporting;

namespace UsageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            TestResults results = new TestResults();
            TestSuite bubbleFish = new TestSuite() { SuiteName = "Bubble fish V1.0.0.0" };
            TestSuite crocodile = new TestSuite() { SuiteName = "Crocodile" };
            TestSuite guiRobot = new TestSuite() { SuiteName = "GuiRobot GSImaging" };

            results.TestSuites.AddRange(new List<TestSuite>() { bubbleFish, crocodile, guiRobot });

            TestDefinition times = new TestDefinition() { DefinitionTitle = "Times" };

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddMinutes(5);
            times.ElementsForXml.Add("StartTime", startTime.ToString());
            times.ElementsForXml.Add("EndTime", endTime.ToString());
            times.ElementsForXml.Add("RunTime", endTime.Subtract(startTime).TotalSeconds.ToString());

            bubbleFish.TestDefinitions.Add(times);

            TestDefinition dependencies = new TestDefinition() { DefinitionTitle = "Dependencies" };
            dependencies.ElementsForXml.Add("ReportingLibrary", Assembly.LoadFrom("Reporting.dll").GetName().Version.ToString());
            dependencies.ElementsForXml.Add("TestSuite", "New SuperDuper suite 2.0.0");

            bubbleFish.TestDefinitions.Add(dependencies);
            guiRobot.TestDefinitions.Add(dependencies);

            TestDefinition info = new TestDefinition() { DefinitionTitle = "Info" };
            info.ElementsForXml.Add("ComputerName", Environment.MachineName);
            info.ElementsForXml.Add("OS", Environment.OSVersion.Platform.ToString() + " " + Environment.OSVersion.Version.ToString());
            info.ElementsForXml.Add("TestType", "Functional test");
            info.ElementsForXml.Add("TestStage", "Software test");
            info.ElementsForXml.Add("TesterName", "Maurice Moss IT Crowd");
            info.ElementsForXml.Add("Configuration", "D:/TopSecret/MegaPriority/BestProgrammerEver/NewSolution/ForTheFirstTime/Foo/Boo/Buzz/ThisIsIt/42.json");

            bubbleFish.TestDefinitions.Add(info);
            guiRobot.TestDefinitions.Add(info);


            TestDefinition infoCrocodile = new TestDefinition() { DefinitionTitle = "Statistic" };
            infoCrocodile.ElementsForXml.Add("NameOfThisPC", "This is laptop");
            infoCrocodile.ElementsForXml.Add("OperatingSystem", "Hacked Windowz" + Environment.OSVersion.Version.ToString());
            infoCrocodile.ElementsForXml.Add("TesterNameTesterNameTesterNameTesterNameTesterNameTesterName", "Maurice Moss IT Crowd");
            infoCrocodile.ElementsForXml.Add("A", "D:/TopSecret/MegaPriority/BestProgrammerEver/NewSolution/ForTheFirstTime/Foo/Boo/Buzz/ThisIsIt/42.json");

            crocodile.TestDefinitions.Add(infoCrocodile);

            TestCase connectionTest = new TestCase() { Name = "Connection Test", Description = "Test Connection test to bubble fish API" };

            TestStep readConfigurationFile = new TestStep() { Timestamp = DateTime.Now.ToString(), Name = "Ip address to connect is 127.0.0.1" };
            TestStep startDummyGrpcServer = new TestStep() { Timestamp = DateTime.Now.Millisecond.ToString(), Name = "Started test grpc service" };
            TestStep startNetworkMonitoring = new TestStep() { Timestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.ffff"), Name = "TCP monitoring tool started" };
            TestStep tryToConnect = new TestStep() { Timestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.ffffff"), Name = "Trying to connectTrying to connectTrying to connectTrying to connectTrying to connectTrying to connectTrying to connectTrying to connectTrying to connectTrying to connectTrying to connectTrying to connectTrying to connectTrying to connect" };

            Fact connectionStatus = new Fact() { Name = "connection status", Timestamp = "", ActualResult = "Disconnected", ExpectedResult = "Connected", Verdict = Fact.FactOutcome.Fail };

            TestStep serviceStatus = new TestStep() { Name = "Test Service is down waiting 10 seconds for reboot" };

            Fact newConnectionStatus = new Fact() { Name = "Connection status", Timestamp = "555", ActualResult = Fact.FactOutcome.Error.ToString(), ExpectedResult = "Connected", Verdict = Fact.FactOutcome.Error };

            TestStep errorDetected = new TestStep() { Name = "Test suite has encounterd and error attempting to compensate", Timestamp = "Between now and never" };

            Fact anotherConnectionStatus = new Fact() { Name = "Connection Status", Timestamp = "", ExpectedResult = "Connected", ActualResult = "Connecting", Verdict = Fact.FactOutcome.Inconclusive };

            TestStep unknown = new TestStep() { Name = "Due to unknown issue we are in connecting status attempting to override" };

            Fact tryNewConnection = new Fact() { Name = "Connecting Status", Timestamp = "Blast from the past", ActualResult = "Connected", ExpectedResult = "Connected", Verdict = Fact.FactOutcome.Pass };

            Fact justToProve = new Fact() { Name = "This is just a fact", ActualResult = "42 is the answer to the meaning of life, but what was the real question about the meaning of life, maybe we are all inte the matrix", ExpectedResult = "47 is the number that is repeating it self in almost all of the star trek episodes, so could it be that 47 is the answer and not 42?", Verdict = Fact.FactOutcome.Notset, Timestamp = "SuperDuperStarTrooper", Comment = "This is usless" };

            Fact withLink = new Fact() { Name = "Fact with link", Verdict = Fact.FactOutcome.Pass, LogLink ="555" };
            Fact withFileLink = new Fact() { Name = "Fact with file link", Verdict = Fact.FactOutcome.Error, LogLink = "556" };

            for (int i = 0; i < 100; i++)
            {
                TestLog justAttribut = new TestLog() { Name = $@"Reconnect attempts {i}"};
                justAttribut.ElementTextForXml = i.ToString();
                connectionTest.TestElements.Add(justAttribut);
            }

            TestLog firstLog = new TestLog() { Name = "Link will point here", Timestamp = "TimeHasStoped", LogLink = "555", Type = "Integer" };
            firstLog.ElementTextForXml = "Data with link to testcase";
            TestLogFile firstFile = new TestLogFile() { Name = "Open this file by clicking on a link", LogLink = "556", Filename = "someFile.txt" };

            connectionTest.TestElements.AddRange(new List<TestStep>() {
                readConfigurationFile, startDummyGrpcServer, startNetworkMonitoring,
                tryToConnect, connectionStatus, serviceStatus, newConnectionStatus,
                errorDetected, anotherConnectionStatus, unknown,
                tryNewConnection, justToProve, withLink, withFileLink, firstLog, firstFile });

            bubbleFish.TestCases.Add(connectionTest);

            TestCase metodTest = new TestCase() { Name = "Method test", Description = "Test SumNumbers - gRPC method" };
            Fact firstFact = new Fact() { Name = "Connection status", Verdict = Fact.FactOutcome.Pass };
            metodTest.TestElements.Add(firstFact);

            bubbleFish.TestCases.Add(metodTest);

            for (int i = 0; i < 15; i++)
            {
                TestCase methd = new TestCase() { Name = $@"RPC Method test {i}", Description = "Test gRPC methods API" };
                for (int j = 0; j < 2; j++)
                {
                    TestStep prepareForMethodCall = new TestStep() { Timestamp = DateTime.Now.ToString(), Name = "Check is API available via reflection" };
                    methd.TestElements.Add(prepareForMethodCall);
                }
                for (int j = 0; j < 3; j++)
                {
                    Fact methodCall = new Fact() { Name = $@"RPC Method call {j}", Timestamp = "", ActualResult = "Method returend correctly", ExpectedResult = "Method returend correctly", Verdict = RandomVerdictValue(), LogLink = $@"{j}" };
                    methd.TestElements.Add(methodCall);
                }
                for (int j = 0; j < 10; j++)
                {
                    TestLog justAttribut = new TestLog() { Name = $@"Call attempts {j}", LogLink = $@"{j}" };
                    justAttribut.ElementTextForXml = j.ToString();
                    methd.TestElements.Add(justAttribut);
                }
                bubbleFish.TestCases.Add(methd);
            }

            var xml = results.ToXml();
            xml.Transform("reporting.xslt");
            xml.Save("test.xml");
        }

        public static Fact.FactOutcome RandomVerdictValue()
        {
            Array values = Enum.GetValues(typeof(Fact.FactOutcome));
            Random random = new Random();
            return (Fact.FactOutcome)values.GetValue(random.Next(values.Length));
        }
    }
}
