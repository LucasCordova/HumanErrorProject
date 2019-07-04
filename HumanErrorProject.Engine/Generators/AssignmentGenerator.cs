using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine.Generators
{
    public class AssignmentGenerator : IAssignmentGenerator
    {
        protected IRepository<PreAssignment, int> PreAssignmentRepository { get; }
        protected IAbstractSyntaxTreeGenerator AbstractSyntaxTreeGenerator { get; }
        protected IAbstractSyntaxTreeClassExtractor AbstractSyntaxTreeClassExtractor { get; }
        protected IAbstractSyntaxTreeMethodExtractor AbstractSyntaxTreeMethodExtractor { get; }
        protected IUnitTestGenerator UnitTestGenerator { get; }

        public AssignmentGenerator(IRepository<PreAssignment, int> preAssignmentRepository, IAbstractSyntaxTreeGenerator abstractSyntaxTreeGenerator, IAbstractSyntaxTreeClassExtractor abstractSyntaxTreeClassExtractor, IAbstractSyntaxTreeMethodExtractor abstractSyntaxTreeMethodExtractor, IUnitTestGenerator unitTestGenerator)
        {
            PreAssignmentRepository = preAssignmentRepository;
            AbstractSyntaxTreeGenerator = abstractSyntaxTreeGenerator;
            AbstractSyntaxTreeClassExtractor = abstractSyntaxTreeClassExtractor;
            AbstractSyntaxTreeMethodExtractor = abstractSyntaxTreeMethodExtractor;
            UnitTestGenerator = unitTestGenerator;
        }


        public async Task Generate(PreAssignment assignment, DirectoryHandler handler)
        {
            try
            {
                await GenerateImplementation(assignment, handler);
            }
            catch (EngineAssignmentExceptionData exceptionReport)
            {
                assignment.PreAssignmentReport = exceptionReport.Report;
                await PreAssignmentRepository.Update(assignment);
            }
        }

        public async Task GenerateImplementation(PreAssignment assignment, DirectoryHandler handler)
        {
            var solutionRoot = EngineFileUtilities.ExtractZip(handler.Directory, "Solution", 
                assignment.Solution.Files);
            var solution = AbstractSyntaxTreeGenerator.CreateFromFile(handler, Path.Combine(solutionRoot, assignment.Filename));
            var solutionClass = GetClassAbstractSyntaxTreeNode(assignment, solution);

            ValidateMethodDeclarations(assignment, solutionClass);

            var unitTests = await UnitTestGenerator.GenerateResults(assignment, handler, solutionRoot);

            assignment.TestProject.UnitTests = unitTests;
            assignment.PreAssignmentReport = new PreAssignmentSucessReport();
            await PreAssignmentRepository.Update(assignment);
        }

        public AbstractSyntaxTreeNode GetClassAbstractSyntaxTreeNode(PreAssignment assignment,
            AbstractSyntaxTreeNode root)
        {
            try
            {
                return AbstractSyntaxTreeClassExtractor.Extract(root, assignment.Solution.Name);
            }
            catch (EngineReportExceptionData)
            {
                throw new EngineAssignmentExceptionData()
                {
                    Report = new PreAssignmentNoClassFailureReport()
                };
            }
        }

        public void ValidateMethodDeclarations(PreAssignment assignment, AbstractSyntaxTreeNode root)
        {
            var undeclaredMethods = new List<MethodDeclaration>();

            foreach (var method in assignment.Solution.MethodDeclarations)
            {
                var methodNode = AbstractSyntaxTreeMethodExtractor.ExtractOrDefault(root, method);
                if (methodNode == null) undeclaredMethods.Add(method);
            }

            if (undeclaredMethods.Any())
            {
                throw new EngineAssignmentExceptionData()
                {
                    Report = new PreAssignmentMissingMethodsFailureReport()
                    {
                        MissingMethodDeclarations = undeclaredMethods
                    }
                };
            }
        }


    }
}
