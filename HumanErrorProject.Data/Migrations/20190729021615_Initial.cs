using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbstractSyntaxTreeMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Rotations = table.Column<int>(nullable: false),
                    Insertations = table.Column<int>(nullable: false),
                    Deletions = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbstractSyntaxTreeMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSolutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Files = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSolutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BagOfWordsMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Difference = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagOfWordsMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Term = table.Column<string>(nullable: false),
                    Course = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreAssignmentReport",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    Report = table.Column<string>(nullable: true),
                    PreAssignmentCompileFailureReport_Report = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreAssignmentReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotReport",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    Report = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    Response = table.Column<string>(nullable: true),
                    Selection = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyAnswer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Required = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Prompt = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Range = table.Column<int>(nullable: true),
                    Example = table.Column<string>(nullable: true),
                    Explaination = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestProjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TestFolder = table.Column<string>(nullable: false),
                    TestProjectFile = table.Column<string>(nullable: false),
                    TestDllFile = table.Column<string>(nullable: false),
                    Files = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeAnalysisMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BagOfWordsMertricId = table.Column<int>(nullable: false),
                    BagOfWordsMetricId = table.Column<int>(nullable: true),
                    AbstractSyntaxTreeMetricId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeAnalysisMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeAnalysisMetrics_AbstractSyntaxTreeMetrics_AbstractSyntaxTreeMetricId",
                        column: x => x.AbstractSyntaxTreeMetricId,
                        principalTable: "AbstractSyntaxTreeMetrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeAnalysisMetrics_BagOfWordsMetrics_BagOfWordsMetricId",
                        column: x => x.BagOfWordsMetricId,
                        principalTable: "BagOfWordsMetrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MethodDeclarations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PreprocessorDirective = table.Column<string>(maxLength: 256, nullable: false),
                    AstType = table.Column<string>(maxLength: 256, nullable: false),
                    AstMethodRegexExpression = table.Column<string>(nullable: false),
                    AstMethodParameterRegexExpression = table.Column<string>(nullable: false),
                    AssignmentSolutionId = table.Column<int>(nullable: true),
                    PreAssignmentMissingMethodsFailureReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodDeclarations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodDeclarations_AssignmentSolutions_AssignmentSolutionId",
                        column: x => x.AssignmentSolutionId,
                        principalTable: "AssignmentSolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MethodDeclarations_PreAssignmentReport_PreAssignmentMissingMethodsFailureReportId",
                        column: x => x.PreAssignmentMissingMethodsFailureReportId,
                        principalTable: "PreAssignmentReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotSubmission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Files = table.Column<byte[]>(nullable: false),
                    StudentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotSubmission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SnapshotSubmission_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourseClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<string>(nullable: false),
                    CourseClassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourseClasses_CourseClasses_CourseClassId",
                        column: x => x.CourseClassId,
                        principalTable: "CourseClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourseClasses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PostedTime = table.Column<DateTime>(nullable: false),
                    StudentId = table.Column<string>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Filename = table.Column<string>(maxLength: 256, nullable: false),
                    CourseClassId = table.Column<int>(nullable: false),
                    AssignmentSolutionId = table.Column<int>(nullable: false),
                    TestProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_AssignmentSolutions_AssignmentSolutionId",
                        column: x => x.AssignmentSolutionId,
                        principalTable: "AssignmentSolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_CourseClasses_CourseClassId",
                        column: x => x.CourseClassId,
                        principalTable: "CourseClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_TestProjects_TestProjectId",
                        column: x => x.TestProjectId,
                        principalTable: "TestProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Filename = table.Column<string>(maxLength: 256, nullable: false),
                    CourseClassId = table.Column<int>(nullable: false),
                    AssignmentSolutionId = table.Column<int>(nullable: false),
                    TestProjectId = table.Column<int>(nullable: false),
                    PreAssignmentReportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreAssignments_AssignmentSolutions_AssignmentSolutionId",
                        column: x => x.AssignmentSolutionId,
                        principalTable: "AssignmentSolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreAssignments_CourseClasses_CourseClassId",
                        column: x => x.CourseClassId,
                        principalTable: "CourseClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreAssignments_PreAssignmentReport_PreAssignmentReportId",
                        column: x => x.PreAssignmentReportId,
                        principalTable: "PreAssignmentReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreAssignments_TestProjects_TestProjectId",
                        column: x => x.TestProjectId,
                        principalTable: "TestProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitTests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    PreAssignmentFailTestsFailureReportId = table.Column<int>(nullable: true),
                    TestProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitTests_PreAssignmentReport_PreAssignmentFailTestsFailureReportId",
                        column: x => x.PreAssignmentFailTestsFailureReportId,
                        principalTable: "PreAssignmentReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitTests_TestProjects_TestProjectId",
                        column: x => x.TestProjectId,
                        principalTable: "TestProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotMethods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Declared = table.Column<bool>(nullable: false),
                    MethodDeclarationId = table.Column<int>(nullable: false),
                    CodeAnalysisMetricId = table.Column<int>(nullable: true),
                    SnapshotSuccessReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SnapshotMethods_CodeAnalysisMetrics_CodeAnalysisMetricId",
                        column: x => x.CodeAnalysisMetricId,
                        principalTable: "CodeAnalysisMetrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotMethods_MethodDeclarations_MethodDeclarationId",
                        column: x => x.MethodDeclarationId,
                        principalTable: "MethodDeclarations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotMethods_SnapshotReport_SnapshotSuccessReportId",
                        column: x => x.SnapshotSuccessReportId,
                        principalTable: "SnapshotReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SurveyQuestionId = table.Column<int>(nullable: false),
                    SurveyAnswerId = table.Column<int>(nullable: false),
                    SurveyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyResponses_SurveyAnswer_SurveyAnswerId",
                        column: x => x.SurveyAnswerId,
                        principalTable: "SurveyAnswer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyResponses_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveyResponses_SurveyQuestion_SurveyQuestionId",
                        column: x => x.SurveyQuestionId,
                        principalTable: "SurveyQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkovModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Publish = table.Column<DateTime>(nullable: false),
                    Finished = table.Column<bool>(nullable: false),
                    AssignmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkovModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkovModels_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SnapshotSubmissionId = table.Column<int>(nullable: false),
                    AssignmentId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: false),
                    SurveyId = table.Column<string>(nullable: true),
                    SnapshotReportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Snapshots_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Snapshots_SnapshotReport_SnapshotReportId",
                        column: x => x.SnapshotReportId,
                        principalTable: "SnapshotReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Snapshots_SnapshotSubmission_SnapshotSubmissionId",
                        column: x => x.SnapshotSubmissionId,
                        principalTable: "SnapshotSubmission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Snapshots_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Snapshots_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnitTestResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UnitTestId = table.Column<int>(nullable: false),
                    Passed = table.Column<bool>(nullable: false),
                    SnapshotSuccessReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitTestResults_SnapshotReport_SnapshotSuccessReportId",
                        column: x => x.SnapshotSuccessReportId,
                        principalTable: "SnapshotReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitTestResults_UnitTests_UnitTestId",
                        column: x => x.UnitTestId,
                        principalTable: "UnitTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkovModelStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<int>(nullable: false),
                    MarkovModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkovModelStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkovModelStates_MarkovModels_MarkovModelId",
                        column: x => x.MarkovModelId,
                        principalTable: "MarkovModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarkovModelSnapshot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SnapshotId = table.Column<int>(nullable: false),
                    MarkovModelStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkovModelSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkovModelSnapshot_MarkovModelStates_MarkovModelStateId",
                        column: x => x.MarkovModelStateId,
                        principalTable: "MarkovModelStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarkovModelTransitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    To = table.Column<int>(nullable: false),
                    Probability = table.Column<double>(nullable: false),
                    MarkovModelStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkovModelTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkovModelTransitions_MarkovModelStates_MarkovModelStateId",
                        column: x => x.MarkovModelStateId,
                        principalTable: "MarkovModelStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignmentSolutionId",
                table: "Assignments",
                column: "AssignmentSolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseClassId",
                table: "Assignments",
                column: "CourseClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_TestProjectId",
                table: "Assignments",
                column: "TestProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeAnalysisMetrics_AbstractSyntaxTreeMetricId",
                table: "CodeAnalysisMetrics",
                column: "AbstractSyntaxTreeMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeAnalysisMetrics_BagOfWordsMetricId",
                table: "CodeAnalysisMetrics",
                column: "BagOfWordsMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkovModels_AssignmentId",
                table: "MarkovModels",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkovModelSnapshot_MarkovModelStateId",
                table: "MarkovModelSnapshot",
                column: "MarkovModelStateId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkovModelStates_MarkovModelId",
                table: "MarkovModelStates",
                column: "MarkovModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkovModelTransitions_MarkovModelStateId",
                table: "MarkovModelTransitions",
                column: "MarkovModelStateId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodDeclarations_AssignmentSolutionId",
                table: "MethodDeclarations",
                column: "AssignmentSolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodDeclarations_PreAssignmentMissingMethodsFailureReportId",
                table: "MethodDeclarations",
                column: "PreAssignmentMissingMethodsFailureReportId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAssignments_AssignmentSolutionId",
                table: "PreAssignments",
                column: "AssignmentSolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAssignments_CourseClassId",
                table: "PreAssignments",
                column: "CourseClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAssignments_PreAssignmentReportId",
                table: "PreAssignments",
                column: "PreAssignmentReportId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAssignments_TestProjectId",
                table: "PreAssignments",
                column: "TestProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotMethods_CodeAnalysisMetricId",
                table: "SnapshotMethods",
                column: "CodeAnalysisMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotMethods_MethodDeclarationId",
                table: "SnapshotMethods",
                column: "MethodDeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotMethods_SnapshotSuccessReportId",
                table: "SnapshotMethods",
                column: "SnapshotSuccessReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_AssignmentId",
                table: "Snapshots",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_SnapshotReportId",
                table: "Snapshots",
                column: "SnapshotReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_SnapshotSubmissionId",
                table: "Snapshots",
                column: "SnapshotSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_StudentId",
                table: "Snapshots",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_SurveyId",
                table: "Snapshots",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotSubmission_StudentId",
                table: "SnapshotSubmission",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseClasses_CourseClassId",
                table: "StudentCourseClasses",
                column: "CourseClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseClasses_StudentId",
                table: "StudentCourseClasses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SurveyAnswerId",
                table: "SurveyResponses",
                column: "SurveyAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SurveyId",
                table: "SurveyResponses",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SurveyQuestionId",
                table: "SurveyResponses",
                column: "SurveyQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_StudentId",
                table: "Surveys",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitTestResults_SnapshotSuccessReportId",
                table: "UnitTestResults",
                column: "SnapshotSuccessReportId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitTestResults_UnitTestId",
                table: "UnitTestResults",
                column: "UnitTestId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitTests_PreAssignmentFailTestsFailureReportId",
                table: "UnitTests",
                column: "PreAssignmentFailTestsFailureReportId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitTests_TestProjectId",
                table: "UnitTests",
                column: "TestProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "MarkovModelSnapshot");

            migrationBuilder.DropTable(
                name: "MarkovModelTransitions");

            migrationBuilder.DropTable(
                name: "PreAssignments");

            migrationBuilder.DropTable(
                name: "SnapshotMethods");

            migrationBuilder.DropTable(
                name: "Snapshots");

            migrationBuilder.DropTable(
                name: "StudentCourseClasses");

            migrationBuilder.DropTable(
                name: "SurveyResponses");

            migrationBuilder.DropTable(
                name: "UnitTestResults");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MarkovModelStates");

            migrationBuilder.DropTable(
                name: "CodeAnalysisMetrics");

            migrationBuilder.DropTable(
                name: "MethodDeclarations");

            migrationBuilder.DropTable(
                name: "SnapshotSubmission");

            migrationBuilder.DropTable(
                name: "SurveyAnswer");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "SurveyQuestion");

            migrationBuilder.DropTable(
                name: "SnapshotReport");

            migrationBuilder.DropTable(
                name: "UnitTests");

            migrationBuilder.DropTable(
                name: "MarkovModels");

            migrationBuilder.DropTable(
                name: "AbstractSyntaxTreeMetrics");

            migrationBuilder.DropTable(
                name: "BagOfWordsMetrics");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "PreAssignmentReport");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "AssignmentSolutions");

            migrationBuilder.DropTable(
                name: "CourseClasses");

            migrationBuilder.DropTable(
                name: "TestProjects");
        }
    }
}
