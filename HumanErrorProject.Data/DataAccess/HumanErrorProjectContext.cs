using HumanErrorProject.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Data.DataAccess
{
    public class HumanErrorProjectContext : IdentityDbContext
    {
        public HumanErrorProjectContext(DbContextOptions<HumanErrorProjectContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SnapshotReport>()
                .ToTable("SnapshotReport")
                .HasDiscriminator(x => x.Type)
                .HasValue<SnapshotSuccessReport>(SnapshotReport.SnapshotReportTypes.Success)
                .HasValue<SnapshotFailureReport>(SnapshotReport.SnapshotReportTypes.Failure);

            builder.Entity<SurveyQuestion>()
                .ToTable("SurveyQuestion")
                .HasDiscriminator(x => x.Type)
                .HasValue<SurveyQuestionRate>(SurveyQuestion.SurveyQuestionTypes.Rate)
                .HasValue<SurveyQuestionQualitative>(SurveyQuestion.SurveyQuestionTypes.Qualitative);

            builder.Entity<SurveyAnswer>()
                .ToTable("SurveyAnswer")
                .HasDiscriminator(x => x.Type)
                .HasValue<SurveyAnswerQualitative>(SurveyAnswer.SurveyAnswerTypes.Qualitative)
                .HasValue<SurveyAnswerRate>(SurveyAnswer.SurveyAnswerTypes.Rate);

            builder.Entity<PreAssignmentReport>()
                .ToTable("PreAssignmentReport")
                .HasDiscriminator(x => x.Type)
                .HasValue<PreAssignmentPendingReport>(PreAssignmentReport.PreAssignmentReportTypes.Pending)
                .HasValue<PreAssignmentSucessReport>(PreAssignmentReport.PreAssignmentReportTypes.Success)
                .HasValue<PreAssignmentNoFileFailureReport>(PreAssignmentReport.PreAssignmentReportTypes.NoFileFailure)
                .HasValue<PreAssignmentCompileFailureReport>(
                    PreAssignmentReport.PreAssignmentReportTypes.CompileFailure)
                .HasValue<PreAssignmentBuildFailureReport>(PreAssignmentReport.PreAssignmentReportTypes.BuildFailure)
                .HasValue<PreAssignmentNoClassFailureReport>(
                    PreAssignmentReport.PreAssignmentReportTypes.NoClassFailure)
                .HasValue<PreAssignmentMissingMethodsFailureReport>(PreAssignmentReport.PreAssignmentReportTypes
                    .MissingMethodsFailure)
                .HasValue<PreAssignmentFailTestsFailureReport>(PreAssignmentReport.PreAssignmentReportTypes
                    .FailTestsFailure)
                .HasValue<PreAssignmentBadTestFolderReport>(PreAssignmentReport.PreAssignmentReportTypes.BadTestFolder);

            builder.Entity<Survey>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(builder);
        }

        public DbSet<PreAssignment> PreAssignments { get; set; }
        public DbSet<PreAssignmentReport> PreAssignmentReports { get; set; }
        public DbSet<PreAssignmentPendingReport> PreAssignmentPendingReports { get; set; }
        public DbSet<PreAssignmentSucessReport> PreAssignmentSucessReports { get; set; }
        public DbSet<PreAssignmentNoFileFailureReport> PreAssignmentNoFileFailureReports { get; set; }
        public DbSet<PreAssignmentCompileFailureReport> PreAssignmentCompileFailureReports { get; set; }
        public DbSet<PreAssignmentBuildFailureReport> PreAssignmentBuildFailureReports { get; set; }
        public DbSet<PreAssignmentNoClassFailureReport> PreAssignmentNoClassFailureReports { get; set; }
        public DbSet<PreAssignmentMissingMethodsFailureReport> PreAssignmentMissingMethodsFailureReports { get; set; }
        public DbSet<PreAssignmentBadTestFolderReport> PreAssignmentBadTestFolderReports { get; set; }
        public DbSet<PreAssignmentFailTestsFailureReport> PreAssignmentFailTestsFailureReports { get; set; }
        public DbSet<AbstractSyntaxTreeMetric> AbstractSyntaxTreeMetrics { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSolution> AssignmentSolutions { get; set; }
        public DbSet<BagOfWordsMetric> BagOfWordsMetrics { get; set; }
        public DbSet<CodeAnalysisMetric> CodeAnalysisMetrics { get; set; }
        public DbSet<CourseClass> CourseClasses { get; set; }
        public DbSet<MarkovModel> MarkovModels { get; set; }
        public DbSet<MarkovModelState> MarkovModelStates { get; set; }
        public DbSet<MarkovModelTransition> MarkovModelTransitions { get; set; }
        public DbSet<MethodDeclaration> MethodDeclarations { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
        public DbSet<SnapshotFailureReport> SnapshotFailureReports { get; set; }
        public DbSet<SnapshotMethod> SnapshotMethods { get; set; }
        public DbSet<SnapshotReport> SnapshotReports { get; set; }
        public DbSet<SnapshotSuccessReport> SnapshotSuccessReports { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourseClass> StudentCourseClasses { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyAnswerQualitative> SurveyAnswerQualitatives { get; set; }
        public DbSet<SurveyAnswerRate> SurveyAnswerRates { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyQuestionQualitative> SurveyQuestionQualitatives { get; set; }
        public DbSet<SurveyQuestionRate> SurveyQuestionRates { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public DbSet<TestProject> TestProjects { get; set; }
        public DbSet<UnitTest> UnitTests { get; set; }
        public DbSet<UnitTestResult> UnitTestResults { get; set; }
    }
}
