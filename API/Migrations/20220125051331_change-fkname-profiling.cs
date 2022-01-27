using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class changefknameprofiling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Profiling_TB_M_Education_EducationId",
                table: "TB_M_Profiling");

            migrationBuilder.DropIndex(
                name: "IX_TB_M_Profiling_EducationId",
                table: "TB_M_Profiling");

            migrationBuilder.DropColumn(
                name: "EducationId",
                table: "TB_M_Profiling");

            migrationBuilder.AddColumn<int>(
                name: "Education_Id",
                table: "TB_M_Profiling",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Profiling_Education_Id",
                table: "TB_M_Profiling",
                column: "Education_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Profiling_TB_M_Education_Education_Id",
                table: "TB_M_Profiling",
                column: "Education_Id",
                principalTable: "TB_M_Education",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Profiling_TB_M_Education_Education_Id",
                table: "TB_M_Profiling");

            migrationBuilder.DropIndex(
                name: "IX_TB_M_Profiling_Education_Id",
                table: "TB_M_Profiling");

            migrationBuilder.DropColumn(
                name: "Education_Id",
                table: "TB_M_Profiling");

            migrationBuilder.AddColumn<int>(
                name: "EducationId",
                table: "TB_M_Profiling",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Profiling_EducationId",
                table: "TB_M_Profiling",
                column: "EducationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Profiling_TB_M_Education_EducationId",
                table: "TB_M_Profiling",
                column: "EducationId",
                principalTable: "TB_M_Education",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
