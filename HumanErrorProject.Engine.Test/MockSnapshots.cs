using System;
using System.Collections.Generic;
using System.IO;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test
{
    public static class MockSnapshots
    {
        public static string ClangCommand = "clang++";
        public static string ClangArguments = "-c -fno-delayed-template-parsing -fno-color-diagnostics -Xclang -ast-dump";
        public static string ClangOutputFile = "abstractsyntaxtree.txt";

        public static string GetShortRoot() => "C:\\Temp";

        public static byte[] GetCalculatorSnapshots()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "Mocks\\MockCalculator\\Snapshots.zip");
            Assert.IsTrue(File.Exists(path), 
                $"{path} doesn't exists");
            var bytes = File.ReadAllBytes(path);
            return bytes;
        }

        public static byte[] GetFirstCalculatorSnapshotFile()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "Mocks\\MockCalculator\\Snapshot12-20-2018_12.00.47.15.zip");
            Assert.IsTrue(File.Exists(path),
                $"{path} doesn't exists");
            var bytes = File.ReadAllBytes(path);
            return bytes;
        }
        public static byte[] GetSecondCalculatorSnapshotFile()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "Mocks\\MockCalculator\\Snapshot12-20-2018_12.19.18.34.zip");
            Assert.IsTrue(File.Exists(path),
                $"{path} doesn't exists");
            var bytes = File.ReadAllBytes(path);
            return bytes;
        }

        public static byte[] GetCalculatorSolutionFiles()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "Mocks\\MockCalculator\\Solution.zip");
            Assert.IsTrue(File.Exists(path),
                $"{path} doesn't exists");
            var bytes = File.ReadAllBytes(path);
            return bytes;
        }

        public static int GetNumberOfNewSnapshotsAfterSecond() => 2;
        public static string GetCalculatorFile() => "Calculator.hpp";
        public static string GetFirstCalculatorSnapshotName() => "Snapshot12-20-2018_12.00.47.15";
        public static string GetSecondCalculatorSnapshotName() => "Snapshot12-20-2018_12.19.18.34";
        public static string GetThirdCalculatorSnapshotName() => "Snapshot12-20-2018_12.19.57.46";
        public static string GetLastCalculatorSnpahostName() => "Snapshot12-20-2018_15.12.20.72";

        public static DateTime GetFirstCalculatorSnapshotTime() => new DateTime(2018,12,20,12,0,47);
        public static DateTime GetSecondCalculatorSnapshotTime() => new DateTime(2018,12,20,12,19,18);
        public static StreamReader GetClassWithDefaultParameterAbstractSyntaxTreeReader() => File.OpenText("Mocks\\MockAbstractSyntaxTreeFiles\\ClassDefaultParameter.txt");
        public static int GetClassWithDefaultParameterHeight() => 5;

        public static string GetCalculatorClassName() => "Calculator";

        public static AbstractSyntaxTreeNode GetCalculatorFullAbstractSyntaxTreeNode()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "Mocks\\MockCalculator\\AbstractSyntaxTrees\\calculator_full_tree.txt");
            Assert.IsTrue(File.Exists(path),
                $"{path} doesn't exists");
            using (var reader = new StreamReader(path))
            {
                return new ClangAbstractSyntaxTreeExtractor().Extract(reader);
            }
        }

        public static AbstractSyntaxTreeNode GetCalculatorClassAbstractSyntaxTreeNode()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "Mocks\\MockCalculator\\AbstractSyntaxTrees\\calculator_class_tree.txt");
            Assert.IsTrue(File.Exists(path),
                $"{path} doesn't exists");
            using (var reader = new StreamReader(path))
            {
                return new ClangAbstractSyntaxTreeExtractor().Extract(reader);
            }
        }

        public static string GetCalculatorAbstractSyntaxTreeClassValue() =>
            @"CXXRecordDecl 0x2545bb56778 <.\Calculator.hpp:4:1, line:11:1> line:4:7 class Calculator definition";

        public static string GetCalculatorAbstractSyntaxTreeAddValue() =>
            @"CXXMethodDecl 0x2545bb57298 parent 0x2545bb56778 prev 0x2545bb56af8 <line:13:1, line:16:1> line:13:24 add 'int (int, int)' inline";
        
        public static string GetCalculatorAbstractSyntaxTreeSubtValue() =>
            @"CXXMethodDecl 0x2545d460178 parent 0x2545bb56778 prev 0x2545bb56cd0 <line:18:1, line:21:1> line:18:24 subt 'int (int, int)' inline";
        
        public static string GetCalculatorAbstractSyntaxTreeMultValue() =>
            @"CXXMethodDecl 0x2545d460478 parent 0x2545bb56778 prev 0x2545bb56ea8 <line:23:1, line:26:1> line:23:24 mult 'int (int, int)' inline";
        
        public static string GetCalculatorAbstractSyntaxTreeDivValue() =>
            @"CXXMethodDecl 0x2545d460778 parent 0x2545bb56778 prev 0x2545bb57080 <line:28:1, line:31:1> line:28:24 div 'int (int, int)' inline";

        public static MethodDeclaration GetCalculatorAddMethodDeclaration()
        {
            return new MethodDeclaration()
            {
                Id = 1,
                PreprocessorDirective = "ADDITION",
                AstMethodRegexExpression = "add",
                AstMethodParameterRegexExpression = @"'int \(int, int\)'",
                AstType = "CXXMethodDecl",
            };
        }
        
        public static MethodDeclaration GetCalculatorSubtMethodDeclaration()
        {
            return new MethodDeclaration()
            {
                Id = 2,
                PreprocessorDirective = "SUBTRACTION",
                AstMethodRegexExpression = "subt",
                AstMethodParameterRegexExpression = @"'int \(int, int\)'",
                AstType = "CXXMethodDecl",
            };
        }
        
        public static MethodDeclaration GetCalculatorMultMethodDeclaration()
        {
            return new MethodDeclaration()
            {
                Id = 3,
                PreprocessorDirective = "MULTIPLICATION",
                AstMethodRegexExpression = "mult",
                AstMethodParameterRegexExpression = @"'int \(int, int\)'",
                AstType = "CXXMethodDecl",
            };
        }
        
        public static MethodDeclaration GetCalculatorDivMethodDeclaration()
        {
            return new MethodDeclaration()
            {
                Id = 4,
                PreprocessorDirective = "DIVISION",
                AstMethodRegexExpression = "div",
                AstMethodParameterRegexExpression = @"'int \(int, int\)'",
                AstType = "CXXMethodDecl",
            };
        }

        public static ICollection<MethodDeclaration> GetCalculatorMethodDeclaration()
        {
            return new List<MethodDeclaration>()
            {
                GetCalculatorAddMethodDeclaration(),
                GetCalculatorSubtMethodDeclaration(),
                GetCalculatorMultMethodDeclaration(),
                GetCalculatorDivMethodDeclaration(),
            };
        }

        public static string PowershellScript()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "Mocks\\GenerateTestResults.ps1");
            return path;
        }

        public static string PowershellPassedValue() => "Passed";

        public static byte[] GetCalculatorTestProjectFiles()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "Mocks\\MockCalculator\\CalculatorTestProject.zip");
            Assert.IsTrue(File.Exists(path),
                $"{path} doesn't exists");
            var bytes = File.ReadAllBytes(path);
            return bytes;
        }

        public static string GetCalculatorTestProjectFile()
            => "Calculator.Test\\Calculator.Test.vcxproj";

        public static string GetCalculatorTestProjectDll()
            => "Calculator.Test\\Release\\Calculator.Test.dll";

        public static string GetCalculatorTestProjectFolder()
            => "Calculator.Test";

        public static ICollection<UnitTest> GetCalculatorUnitTests()
        {
            return new List<UnitTest>()
            {
                new UnitTest()
                {
                    Id = 1,
                    Name = "Addition",
                },
                new UnitTest()
                {
                    Id = 2,
                    Name = "Subtraction",
                },
                new UnitTest()
                {
                    Id = 3,
                    Name = "Multiplication",
                },
                new UnitTest()
                {
                    Id = 4,
                    Name = "Division",
                },
            };
        }

    }
}
