using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cran.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "CranContainer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranContainer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CranCourse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    NumQuestionsToAsk = table.Column<int>(nullable: false),
                    IdLanguage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranCourse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CranLogEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranLogEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CranTag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ShortDescDe = table.Column<string>(nullable: true),
                    ShortDescEn = table.Column<string>(nullable: true),
                    IdTagType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CranText",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    ContentDe = table.Column<string>(nullable: true),
                    ContentEn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranText", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CranUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    IsAnonymous = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
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
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
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
                name: "CranRelCourseTag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdCourse = table.Column<int>(nullable: false),
                    IdTag = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranRelCourseTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranRelCourseTag_CranCourse_IdCourse",
                        column: x => x.IdCourse,
                        principalTable: "CranCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranRelCourseTag_CranTag_IdTag",
                        column: x => x.IdTag,
                        principalTable: "CranTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranBinary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    ContentType = table.Column<string>(nullable: true),
                    ContentDisposition = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Length = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranBinary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranBinary_CranUser_IdUser",
                        column: x => x.IdUser,
                        principalTable: "CranUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranCourseInstance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    IdCourse = table.Column<int>(nullable: false),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    EndedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranCourseInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranCourseInstance_CranCourse_IdCourse",
                        column: x => x.IdCourse,
                        principalTable: "CranCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranCourseInstance_CranUser_IdUser",
                        column: x => x.IdUser,
                        principalTable: "CranUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranNotificationSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    Endpoint = table.Column<string>(nullable: true),
                    ExpirationTime = table.Column<DateTime>(nullable: true),
                    P256DiffHell = table.Column<string>(nullable: true),
                    Auth = table.Column<string>(nullable: true),
                    AsString = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranNotificationSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranNotificationSubscription_CranUser_IdUser",
                        column: x => x.IdUser,
                        principalTable: "CranUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Explanation = table.Column<string>(nullable: true),
                    IdUser = table.Column<int>(nullable: false),
                    IdContainer = table.Column<int>(nullable: false),
                    IdQuestionCopySource = table.Column<int>(nullable: true),
                    ApprovalDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IdLanguage = table.Column<int>(nullable: false),
                    IdQuestionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranQuestion_CranContainer_IdContainer",
                        column: x => x.IdContainer,
                        principalTable: "CranContainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranQuestion_CranQuestion_IdQuestionCopySource",
                        column: x => x.IdQuestionCopySource,
                        principalTable: "CranQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CranQuestion_CranUser_IdUser",
                        column: x => x.IdUser,
                        principalTable: "CranUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranUserCourseFavorite",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    IdCourse = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranUserCourseFavorite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranUserCourseFavorite_CranCourse_IdCourse",
                        column: x => x.IdCourse,
                        principalTable: "CranCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranUserCourseFavorite_CranUser_IdUser",
                        column: x => x.IdUser,
                        principalTable: "CranUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdBinary = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: true),
                    Height = table.Column<int>(nullable: true),
                    Full = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranImage_CranBinary_IdBinary",
                        column: x => x.IdBinary,
                        principalTable: "CranBinary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranComment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    IdQuestion = table.Column<int>(nullable: false),
                    CommentText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranComment_CranQuestion_IdQuestion",
                        column: x => x.IdQuestion,
                        principalTable: "CranQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranComment_CranUser_IdUser",
                        column: x => x.IdUser,
                        principalTable: "CranUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranCourseInstanceQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdCourseInstance = table.Column<int>(nullable: false),
                    IdQuestion = table.Column<int>(nullable: false),
                    Correct = table.Column<bool>(nullable: false),
                    AnswerShown = table.Column<bool>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    AnsweredAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranCourseInstanceQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranCourseInstanceQuestion_CranCourseInstance_IdCourseInstance",
                        column: x => x.IdCourseInstance,
                        principalTable: "CranCourseInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranCourseInstanceQuestion_CranQuestion_IdQuestion",
                        column: x => x.IdQuestion,
                        principalTable: "CranQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranQuestionOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdQuestion = table.Column<int>(nullable: false),
                    IsTrue = table.Column<bool>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranQuestionOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranQuestionOption_CranQuestion_IdQuestion",
                        column: x => x.IdQuestion,
                        principalTable: "CranQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranRating",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    IdQuestion = table.Column<int>(nullable: false),
                    QuestionRating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranRating_CranQuestion_IdQuestion",
                        column: x => x.IdQuestion,
                        principalTable: "CranQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranRating_CranUser_IdUser",
                        column: x => x.IdUser,
                        principalTable: "CranUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranRelQuestionTag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdQuestion = table.Column<int>(nullable: false),
                    IdTag = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranRelQuestionTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranRelQuestionTag_CranQuestion_IdQuestion",
                        column: x => x.IdQuestion,
                        principalTable: "CranQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranRelQuestionTag_CranTag_IdTag",
                        column: x => x.IdTag,
                        principalTable: "CranTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranRelQuestionImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdQuestion = table.Column<int>(nullable: false),
                    IdImage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranRelQuestionImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranRelQuestionImage_CranImage_IdImage",
                        column: x => x.IdImage,
                        principalTable: "CranImage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranRelQuestionImage_CranQuestion_IdQuestion",
                        column: x => x.IdQuestion,
                        principalTable: "CranQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CranCourseInstanceQuestionOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertUser = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IdCourseInstanceQuestion = table.Column<int>(nullable: false),
                    IdQuestionOption = table.Column<int>(nullable: false),
                    Correct = table.Column<bool>(nullable: false),
                    Checked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CranCourseInstanceQuestionOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CranCourseInstanceQuestionOption_CranCourseInstanceQuestion_IdCourseInstanceQuestion",
                        column: x => x.IdCourseInstanceQuestion,
                        principalTable: "CranCourseInstanceQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CranCourseInstanceQuestionOption_CranQuestionOption_IdQuestionOption",
                        column: x => x.IdQuestionOption,
                        principalTable: "CranQuestionOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_CranBinary_IdUser",
                table: "CranBinary",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_CranComment_IdQuestion",
                table: "CranComment",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_CranComment_IdUser",
                table: "CranComment",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_CranCourseInstance_IdCourse",
                table: "CranCourseInstance",
                column: "IdCourse");

            migrationBuilder.CreateIndex(
                name: "IX_CranCourseInstance_IdUser",
                table: "CranCourseInstance",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_CranCourseInstanceQuestion_IdCourseInstance",
                table: "CranCourseInstanceQuestion",
                column: "IdCourseInstance");

            migrationBuilder.CreateIndex(
                name: "IX_CranCourseInstanceQuestion_IdQuestion",
                table: "CranCourseInstanceQuestion",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_CranCourseInstanceQuestionOption_IdCourseInstanceQuestion",
                table: "CranCourseInstanceQuestionOption",
                column: "IdCourseInstanceQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_CranCourseInstanceQuestionOption_IdQuestionOption",
                table: "CranCourseInstanceQuestionOption",
                column: "IdQuestionOption");

            migrationBuilder.CreateIndex(
                name: "IX_CranImage_IdBinary",
                table: "CranImage",
                column: "IdBinary");

            migrationBuilder.CreateIndex(
                name: "IX_CranNotificationSubscription_IdUser",
                table: "CranNotificationSubscription",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_CranQuestion_IdContainer",
                table: "CranQuestion",
                column: "IdContainer");

            migrationBuilder.CreateIndex(
                name: "IX_CranQuestion_IdQuestionCopySource",
                table: "CranQuestion",
                column: "IdQuestionCopySource");

            migrationBuilder.CreateIndex(
                name: "IX_CranQuestion_IdUser",
                table: "CranQuestion",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_CranQuestionOption_IdQuestion",
                table: "CranQuestionOption",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_CranRating_IdQuestion",
                table: "CranRating",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_CranRating_IdUser",
                table: "CranRating",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_CranRelCourseTag_IdCourse",
                table: "CranRelCourseTag",
                column: "IdCourse");

            migrationBuilder.CreateIndex(
                name: "IX_CranRelCourseTag_IdTag",
                table: "CranRelCourseTag",
                column: "IdTag");

            migrationBuilder.CreateIndex(
                name: "IX_CranRelQuestionImage_IdImage",
                table: "CranRelQuestionImage",
                column: "IdImage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CranRelQuestionImage_IdQuestion",
                table: "CranRelQuestionImage",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_CranRelQuestionTag_IdQuestion",
                table: "CranRelQuestionTag",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_CranRelQuestionTag_IdTag",
                table: "CranRelQuestionTag",
                column: "IdTag");

            migrationBuilder.CreateIndex(
                name: "IX_CranUserCourseFavorite_IdCourse",
                table: "CranUserCourseFavorite",
                column: "IdCourse");

            migrationBuilder.CreateIndex(
                name: "IX_CranUserCourseFavorite_IdUser",
                table: "CranUserCourseFavorite",
                column: "IdUser");
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
                name: "CranComment");

            migrationBuilder.DropTable(
                name: "CranCourseInstanceQuestionOption");

            migrationBuilder.DropTable(
                name: "CranLogEntry");

            migrationBuilder.DropTable(
                name: "CranNotificationSubscription");

            migrationBuilder.DropTable(
                name: "CranRating");

            migrationBuilder.DropTable(
                name: "CranRelCourseTag");

            migrationBuilder.DropTable(
                name: "CranRelQuestionImage");

            migrationBuilder.DropTable(
                name: "CranRelQuestionTag");

            migrationBuilder.DropTable(
                name: "CranText");

            migrationBuilder.DropTable(
                name: "CranUserCourseFavorite");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CranCourseInstanceQuestion");

            migrationBuilder.DropTable(
                name: "CranQuestionOption");

            migrationBuilder.DropTable(
                name: "CranImage");

            migrationBuilder.DropTable(
                name: "CranTag");

            migrationBuilder.DropTable(
                name: "CranCourseInstance");

            migrationBuilder.DropTable(
                name: "CranQuestion");

            migrationBuilder.DropTable(
                name: "CranBinary");

            migrationBuilder.DropTable(
                name: "CranCourse");

            migrationBuilder.DropTable(
                name: "CranContainer");

            migrationBuilder.DropTable(
                name: "CranUser");
        }
    }
}
