using System.Collections.Generic;
using System.Linq;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Data;

namespace HumanErrorProject.Engine.Utilities
{
    public class ClangAbstractSyntaxTreeClassExtractor : IAbstractSyntaxTreeClassExtractor
    {
        protected ILineSplitter Splitter;

        public ClangAbstractSyntaxTreeClassExtractor(ILineSplitter splitter)
        {
            Splitter = splitter;
        }

        public AbstractSyntaxTreeNode Extract(AbstractSyntaxTreeNode root, string name)
        {
            var visitor = new ClassExtractorDeclVisitor(new ClassExtractorObj(), Splitter);
            root.PreOrder(visitor);

            var classNode = GatherDeclInformation(name, visitor.ExtractorObj, out var hashCode);

            var node = GatherMethodDeclaration(classNode, visitor.ExtractorObj, hashCode);

            return node;
        }

        public AbstractSyntaxTreeNode GatherMethodDeclaration(AbstractSyntaxTreeNode classNode, ClassExtractorObj obj, string hashCode)
        {
            var root = new AbstractSyntaxTreeNode("Root");
            root.Append(classNode);

            foreach (var node in obj.CxxMethodDecls.Where(wrapper => IsDefinedClassMethod(wrapper, hashCode)))
            {
                root.Append(node);
            }
            return root;
        }

        public bool IsDefinedClassMethod(AbstractSyntaxTreeNode node, string hashCode)
        {
            var values = Splitter.Split(node.Value);

            if (values.Length <= 4 || values[2] != "parent" || values[3] != hashCode)
                return false;

            if (node.Value.EndsWith("default"))
                return true;

            var visitor = new AbstractSyntaxTreeWordSearchVisitor(Splitter, "CompoundStmt");
            node.BreadthFirst(visitor);
            return visitor.Found;

        }

        public AbstractSyntaxTreeNode GatherDeclInformation(string name, ClassExtractorObj obj, out string hashCode)
        {
            hashCode = null;
            foreach (var node in obj.ClassTemplateDecls)
            {
                if (!Splitter.Split(node.Value).Any(v => v.Equals(name)) || !ContainsDefinitionData(node)) continue;

                hashCode = GetClassHashCodeOrDefault(node);

                if (hashCode == null) continue;

                return node;
            }
            throw new EngineReportExceptionData($"Unable to find c++ class named \'{name}\'")
            {
                Type = "Compilation"
            };
        }

        public string GetClassHashCodeOrDefault(AbstractSyntaxTreeNode node)
        {
            var visitor = new AbstractSyntaxTreeWordSearchVisitor(Splitter, "CXXRecordDecl");
            node.BreadthFirst(visitor);
            node.BreadthFirst(visitor);

            if (!visitor.Found) return null;

            var values = Splitter.Split(visitor.Node.Value);

            return values.Length > 1 ? values[1] : null;
        }

        public bool ContainsDefinitionData(AbstractSyntaxTreeNode node)
        {
            var visitor = new AbstractSyntaxTreeWordSearchVisitor(Splitter, "DefinitionData");
            node.BreadthFirst(visitor);
            return visitor.Found;
        }

        public class ClassExtractorObj
        {
            public IList<AbstractSyntaxTreeNode> ClassTemplateDecls = new List<AbstractSyntaxTreeNode>();
            public IList<AbstractSyntaxTreeNode> CxxMethodDecls = new List<AbstractSyntaxTreeNode>();
        }

        public class ClassExtractorDeclVisitor : IAbstractSyntaxTreeVisitor
        {
            public ClassExtractorObj ExtractorObj { get; }
            protected ILineSplitter Spiltter;

            public ClassExtractorDeclVisitor(ClassExtractorObj extractorObj, ILineSplitter spiltter)
            {
                ExtractorObj = extractorObj;
                Spiltter = spiltter;
            }

            public void Visit(AbstractSyntaxTreeNode node)
            {
                var values = Spiltter.Split(node.Value);
                if (values.Length < 1) return;

                switch (values[0])
                {
                    case "ClassTemplateDecl":
                    case "CXXRecordDecl":
                        ExtractorObj.ClassTemplateDecls.Add(node);
                        break;
                    case "CXXMethodDecl":
                    case "CXXConstructorDecl":
                    case "CXXDestructorDecl":
                    case "CXXConversionDecl":
                        ExtractorObj.CxxMethodDecls.Add(node);
                        break;
                }
            }
        }

    }
}
