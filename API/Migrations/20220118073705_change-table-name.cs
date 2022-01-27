using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class changetablename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "TB_M_Employee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_M_Employee",
                table: "TB_M_Employee",
                column: "NIK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_M_Employee",
                table: "TB_M_Employee");

            migrationBuilder.RenameTable(
                name: "TB_M_Employee",
                newName: "Employees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "NIK");
        }
    }
}
