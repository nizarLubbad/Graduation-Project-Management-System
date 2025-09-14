using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Migrations
{
    /// <inheritdoc />
    public partial class addedReplyTablewithconstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Links",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateTable(
                name: "Replys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeedbackId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<long>(type: "bigint", nullable: true),
                    SupervisorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Replys_Feedbacks_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Replys_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Replys_Supervisors_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Supervisors",
                        principalColumn: "SupervisorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Replys_FeedbackId",
                table: "Replys",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Replys_StudentId",
                table: "Replys",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Replys_SupervisorId",
                table: "Replys",
                column: "SupervisorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Replys");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Links",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");
        }
    }
}
