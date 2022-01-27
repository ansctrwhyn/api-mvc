using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class changefknameeducation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Education_TB_M_University_UniversityId",
                table: "TB_M_Education");

            migrationBuilder.DropIndex(
                name: "IX_TB_M_Education_UniversityId",
                table: "TB_M_Education");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "TB_M_Education");

            migrationBuilder.AddColumn<int>(
                name: "University_Id",
                table: "TB_M_Education",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Education_University_Id",
                table: "TB_M_Education",
                column: "University_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Education_TB_M_University_University_Id",
                table: "TB_M_Education",
                column: "University_Id",
                principalTable: "TB_M_University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Education_TB_M_University_University_Id",
                table: "TB_M_Education");

            migrationBuilder.DropIndex(
                name: "IX_TB_M_Education_University_Id",
                table: "TB_M_Education");

            migrationBuilder.DropColumn(
                name: "University_Id",
                table: "TB_M_Education");

            migrationBuilder.AddColumn<int>(
                name: "UniversityId",
                table: "TB_M_Education",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Education_UniversityId",
                table: "TB_M_Education",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Education_TB_M_University_UniversityId",
                table: "TB_M_Education",
                column: "UniversityId",
                principalTable: "TB_M_University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
