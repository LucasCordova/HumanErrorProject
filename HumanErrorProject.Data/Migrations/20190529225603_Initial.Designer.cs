﻿// <auto-generated />
using System;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HumanErrorProject.Data.Migrations
{
    [DbContext(typeof(HumanErrorProjectContext))]
    [Migration("20190529225603_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HumanErrorProject.Data.Models.AbstractSyntaxTreeMetric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Deletions");

                    b.Property<int>("Insertations");

                    b.Property<int>("Rotations");

                    b.HasKey("Id");

                    b.ToTable("AbstractSyntaxTreeMetrics");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.Assignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssignmentSolutionId");

                    b.Property<int>("CourseClassId");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int>("TestProjectId");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentSolutionId");

                    b.HasIndex("CourseClassId");

                    b.HasIndex("TestProjectId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.AssignmentSolution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Files")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("AssignmentSolutions");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.BagOfWordsMetric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Difference");

                    b.HasKey("Id");

                    b.ToTable("BagOfWordsMetrics");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.CodeAnalysisMetric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AbstractSyntaxTreeMetricId");

                    b.Property<int>("BagOfWordsMertricId");

                    b.Property<int?>("BagOfWordsMetricId");

                    b.HasKey("Id");

                    b.HasIndex("AbstractSyntaxTreeMetricId");

                    b.HasIndex("BagOfWordsMetricId");

                    b.ToTable("CodeAnalysisMetrics");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.CourseClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Course")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Term")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CourseClasses");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.MarkovModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssignmentId");

                    b.Property<bool>("Finished");

                    b.Property<DateTime>("Publish");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentId");

                    b.ToTable("MarkovModels");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.MarkovModelState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MarkovModelId");

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.HasIndex("MarkovModelId");

                    b.ToTable("MarkovModelStates");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.MarkovModelTransition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MarkovModelStateId");

                    b.Property<double>("Probability");

                    b.Property<int>("To");

                    b.HasKey("Id");

                    b.HasIndex("MarkovModelStateId");

                    b.ToTable("MarkovModelTransitions");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.MethodDeclaration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AssignmentSolutionId");

                    b.Property<string>("AstMethodParameterRegexExpression")
                        .IsRequired();

                    b.Property<string>("AstMethodRegexExpression")
                        .IsRequired();

                    b.Property<string>("AstType")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("PreprocessorDirective")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("AssignmentSolutionId");

                    b.ToTable("MethodDeclarations");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.Snapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssignmentId");

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<byte[]>("Files")
                        .IsRequired();

                    b.Property<int?>("MarkovModelStateId");

                    b.Property<int>("SnapshotReportId");

                    b.Property<int>("StudentId");

                    b.Property<int?>("SurveyId");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentId");

                    b.HasIndex("MarkovModelStateId");

                    b.HasIndex("SnapshotReportId");

                    b.HasIndex("StudentId");

                    b.HasIndex("SurveyId");

                    b.ToTable("Snapshots");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SnapshotMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CodeAnalysisMetricId");

                    b.Property<bool>("Declared");

                    b.Property<int>("MethodDeclarationId");

                    b.Property<int?>("SnapshotSuccessReportId");

                    b.HasKey("Id");

                    b.HasIndex("CodeAnalysisMetricId");

                    b.HasIndex("MethodDeclarationId");

                    b.HasIndex("SnapshotSuccessReportId");

                    b.ToTable("SnapshotMethods");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SnapshotReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("SnapshotReport");

                    b.HasDiscriminator<int>("Type");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("IdentityUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.StudentCourseClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseClassId");

                    b.Property<int>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("CourseClassId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentCourseClasses");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.Survey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsCompleted");

                    b.Property<string>("Key")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("PostedTime");

                    b.Property<int>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SurveyAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("SurveyAnswer");

                    b.HasDiscriminator<int>("Type");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SurveyQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Required");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("SurveyQuestion");

                    b.HasDiscriminator<int>("Type");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SurveyResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SurveyAnswerId");

                    b.Property<int?>("SurveyId");

                    b.Property<int>("SurveyQuestionId");

                    b.HasKey("Id");

                    b.HasIndex("SurveyAnswerId");

                    b.HasIndex("SurveyId");

                    b.HasIndex("SurveyQuestionId");

                    b.ToTable("SurveyResponses");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.TestProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Files")
                        .IsRequired();

                    b.Property<string>("TestDllFile")
                        .IsRequired();

                    b.Property<string>("TestFolder")
                        .IsRequired();

                    b.Property<string>("TestProjectFolder")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TestProjects");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.UnitTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("TestProjectId");

                    b.HasKey("Id");

                    b.HasIndex("TestProjectId");

                    b.ToTable("UnitTests");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.UnitTestResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Passed");

                    b.Property<int?>("SnapshotSuccessReportId");

                    b.Property<int>("UnitTestId");

                    b.HasKey("Id");

                    b.HasIndex("SnapshotSuccessReportId");

                    b.HasIndex("UnitTestId");

                    b.ToTable("UnitTestResults");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SnapshotFailureReport", b =>
                {
                    b.HasBaseType("HumanErrorProject.Data.Models.SnapshotReport");

                    b.Property<string>("Report")
                        .IsRequired();

                    b.ToTable("SnapshotFailureReport");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SnapshotSuccessReport", b =>
                {
                    b.HasBaseType("HumanErrorProject.Data.Models.SnapshotReport");


                    b.ToTable("SnapshotSuccessReport");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SurveyAnswerQualitative", b =>
                {
                    b.HasBaseType("HumanErrorProject.Data.Models.SurveyAnswer");

                    b.Property<string>("Response");

                    b.ToTable("SurveyAnswerQualitative");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SurveyAnswerRate", b =>
                {
                    b.HasBaseType("HumanErrorProject.Data.Models.SurveyAnswer");

                    b.Property<int>("Selection");

                    b.ToTable("SurveyAnswerRate");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SurveyQuestionQualitative", b =>
                {
                    b.HasBaseType("HumanErrorProject.Data.Models.SurveyQuestion");

                    b.Property<string>("Prompt")
                        .IsRequired();

                    b.ToTable("SurveyQuestionQualitative");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SurveyQuestionRate", b =>
                {
                    b.HasBaseType("HumanErrorProject.Data.Models.SurveyQuestion");

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("Example")
                        .IsRequired();

                    b.Property<string>("Explaination")
                        .IsRequired();

                    b.Property<int>("Range");

                    b.ToTable("SurveyQuestionRate");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.Assignment", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.AssignmentSolution", "Solution")
                        .WithMany()
                        .HasForeignKey("AssignmentSolutionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.CourseClass", "CourseClass")
                        .WithMany("Assignments")
                        .HasForeignKey("CourseClassId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.TestProject", "TestProject")
                        .WithMany()
                        .HasForeignKey("TestProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.CodeAnalysisMetric", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.AbstractSyntaxTreeMetric", "AbstractSyntaxTreeMetric")
                        .WithMany()
                        .HasForeignKey("AbstractSyntaxTreeMetricId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.BagOfWordsMetric", "BagOfWordsMetric")
                        .WithMany()
                        .HasForeignKey("BagOfWordsMetricId");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.MarkovModel", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.Assignment", "Assignment")
                        .WithMany()
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.MarkovModelState", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.MarkovModel")
                        .WithMany("States")
                        .HasForeignKey("MarkovModelId");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.MarkovModelTransition", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.MarkovModelState")
                        .WithMany("Transitions")
                        .HasForeignKey("MarkovModelStateId");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.MethodDeclaration", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.AssignmentSolution")
                        .WithMany("MethodDeclarations")
                        .HasForeignKey("AssignmentSolutionId");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.Snapshot", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.Assignment", "Assignment")
                        .WithMany("Snapshots")
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.MarkovModelState")
                        .WithMany("Snapshots")
                        .HasForeignKey("MarkovModelStateId");

                    b.HasOne("HumanErrorProject.Data.Models.SnapshotReport", "Report")
                        .WithMany()
                        .HasForeignKey("SnapshotReportId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.Student", "Student")
                        .WithMany("Snapshots")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.Survey", "Survey")
                        .WithMany("Snapshots")
                        .HasForeignKey("SurveyId");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SnapshotMethod", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.CodeAnalysisMetric", "CodeAnalysisMetric")
                        .WithMany()
                        .HasForeignKey("CodeAnalysisMetricId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.MethodDeclaration", "MethodDeclaration")
                        .WithMany()
                        .HasForeignKey("MethodDeclarationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.SnapshotSuccessReport")
                        .WithMany("SnapshotMethods")
                        .HasForeignKey("SnapshotSuccessReportId");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.StudentCourseClass", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.CourseClass", "Class")
                        .WithMany("StudentCourseClasses")
                        .HasForeignKey("CourseClassId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.Student", "Student")
                        .WithMany("StudentCourseClasses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.Survey", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.Student", "Student")
                        .WithMany("Surveys")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.SurveyResponse", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.SurveyAnswer", "Answer")
                        .WithMany()
                        .HasForeignKey("SurveyAnswerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanErrorProject.Data.Models.Survey")
                        .WithMany("SurveyResponses")
                        .HasForeignKey("SurveyId");

                    b.HasOne("HumanErrorProject.Data.Models.SurveyQuestion", "Question")
                        .WithMany()
                        .HasForeignKey("SurveyQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.UnitTest", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.TestProject")
                        .WithMany("UnitTests")
                        .HasForeignKey("TestProjectId");
                });

            modelBuilder.Entity("HumanErrorProject.Data.Models.UnitTestResult", b =>
                {
                    b.HasOne("HumanErrorProject.Data.Models.SnapshotSuccessReport")
                        .WithMany("UnitTestResults")
                        .HasForeignKey("SnapshotSuccessReportId");

                    b.HasOne("HumanErrorProject.Data.Models.UnitTest", "UnitTest")
                        .WithMany()
                        .HasForeignKey("UnitTestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
