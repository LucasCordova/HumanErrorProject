using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;

namespace HumanErrorProject.Engine.Utilities
{
    public class ClangAbstractSyntaxTreeMethodExtractor : IAbstractSyntaxTreeMethodExtractor
    {
        protected ILineSplitter Splitter;
        public ClangAbstractSyntaxTreeMethodExtractor(ILineSplitter splitter)
        {
            Splitter = splitter;
        }

        public AbstractSyntaxTreeNode ExtractOrDefault(AbstractSyntaxTreeNode root, MethodDeclaration methodDeclaration)
        {
            var extractorObj = new MethodExtractorObj();
            var methodExtractorVisitor = new MethodExtractorVisitor(extractorObj, Splitter, methodDeclaration.AstType);
            root.PreOrder(methodExtractorVisitor);

            var methodNode = GetMethodOrDefault(extractorObj, methodDeclaration, out var hashCode);

            if (methodNode == null) return null;

            var callExpressionExtractorVisitor = new CallExpressionExtractorVisitor(extractorObj, Splitter);
            methodNode.PreOrder(callExpressionExtractorVisitor);

            var node = GatherCallExpressions(extractorObj, methodNode, root, hashCode);

            return node;
        }

        public AbstractSyntaxTreeNode GatherCallExpressions(MethodExtractorObj obj, AbstractSyntaxTreeNode methodNode, AbstractSyntaxTreeNode root, string methodHashCode)
        {
            var hasCodes = new List<string>();

            foreach (var callExpressionNode in obj.CallExpressions)
            {
                var visitor = new AbstractSyntaxTreeWordSearchVisitor(Splitter, "MemberExpr");
                callExpressionNode.BreadthFirst(visitor);
                if (!visitor.Found) continue;

                var hashCode = Splitter.Split(visitor.Node.Value).Last();

                if (hashCode.Equals(methodHashCode) || hasCodes.Contains(hashCode)) continue;

                hasCodes.Add(hashCode);

                var callNode = GetCallNodeOrDefault(root, hashCode);

                if (callNode == null) continue;

                methodNode.Append(callNode);
            }
            return methodNode;
        }

        public AbstractSyntaxTreeNode GetCallNodeOrDefault(AbstractSyntaxTreeNode root, string hashCode)
        {
            var visitor = new HashCodeExtractorSearchVisitor(Splitter, hashCode);
            root.BreadthFirst(visitor);
            return visitor.Node;
        }

        public AbstractSyntaxTreeNode GetMethodOrDefault(MethodExtractorObj obj, MethodDeclaration methodDeclaration, out string hashCode)
        {
            var root = new AbstractSyntaxTreeNode("Root");
            foreach (var methodNode in obj.Methods)
            {
                if (!NodeIsMethod(methodNode, methodDeclaration)) continue;

                hashCode = GetMethodHashCodeOrDefault(methodNode, methodDeclaration.AstType);

                if (hashCode == null) continue;

                if (methodNode.Value.EndsWith("default"))
                {
                    root.Append(methodNode);
                    return root;
                }

                var visitor = new AbstractSyntaxTreeWordSearchVisitor(Splitter, "CompoundStmt");
                methodNode.BreadthFirst(visitor);

                if (!visitor.Found) continue;

                root.Append(methodNode);
                return root;
            }
            hashCode = null;
            return null;
        }

        public string GetMethodHashCodeOrDefault(AbstractSyntaxTreeNode node, string type)
        {
            var visitor = new AbstractSyntaxTreeWordSearchVisitor(Splitter, type);
            node.BreadthFirst(visitor);

            if (!visitor.Found) return null;

            var values = Splitter.Split(visitor.Node.Value);
            return values.Length > 1 ? values[1] : null;
        }

        public bool NodeIsMethod(AbstractSyntaxTreeNode methodNode, MethodDeclaration methodDeclaration)
        {
            var methodName = false;
            var parameter = false;
            foreach (var value in Splitter.Split(methodNode.Value))
            {
                if (Regex.IsMatch(value, $"^({methodDeclaration.AstMethodRegexExpression})$"))
                    methodName = true;
                if (Regex.IsMatch(value, $"^({methodDeclaration.AstMethodParameterRegexExpression})$"))
                    parameter = true;
            }
            return methodName && parameter;
        }

        public class MethodExtractorObj
        {
            public IList<AbstractSyntaxTreeNode> Methods = new List<AbstractSyntaxTreeNode>();
            public IList<AbstractSyntaxTreeNode> CallExpressions = new List<AbstractSyntaxTreeNode>();
        }

        public class MethodExtractorVisitor : IAbstractSyntaxTreeVisitor
        {
            public MethodExtractorObj ExtractorObj { get; }
            protected ILineSplitter Splitter;
            protected string Type;

            public MethodExtractorVisitor(MethodExtractorObj extractorObj, ILineSplitter splitter, string type)
            {
                ExtractorObj = extractorObj;
                Splitter = splitter;
                Type = type;
            }

            public void Visit(AbstractSyntaxTreeNode node)
            {
                var values = Splitter.Split(node.Value);

                if (values.Length < 1 || !values[0].Equals(Type)) return;

                ExtractorObj.Methods.Add(node);
            }
        }

        
        public class CallExpressionExtractorVisitor : IAbstractSyntaxTreeVisitor
        {
            public MethodExtractorObj ExtractorObj { get; }
            protected ILineSplitter Splitter;
            public CallExpressionExtractorVisitor(MethodExtractorObj extractorObj, ILineSplitter splitter)
            {
                ExtractorObj = extractorObj;
                Splitter = splitter;
            }

            public void Visit(AbstractSyntaxTreeNode node)
            {
                var values = Splitter.Split(node.Value);
                if (values.Length < 1 || !values[0].Equals("CallExpr")) return;

                ExtractorObj.CallExpressions.Add(node);
            }
        }

        public class HashCodeExtractorSearchVisitor : IAbstractSyntaxTreeSearchVisitor
        {
            public AbstractSyntaxTreeNode Node { get; private set; }
            public bool Found { get; private set; }
            protected ILineSplitter Splitter;
            protected string HashCode;

            public HashCodeExtractorSearchVisitor(ILineSplitter splitter, string hashCode)
            {
                Splitter = splitter;
                HashCode = hashCode;
            }

            public void Visit(AbstractSyntaxTreeNode node)
            {
                var values = Splitter.Split(node.Value);

                if (values.Length < 2 || !values[1].Equals(HashCode)) return;

                Node = node;
                Found = true;
            }
        }
    }
}
