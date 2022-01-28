using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class changeforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_AccountRole_TB_M_Account_Account_Id",
                table: "TB_M_AccountRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_M_AccountRole",
                table: "TB_M_AccountRole");

            migrationBuilder.DropIndex(
                name: "IX_TB_M_AccountRole_Account_Id",
                table: "TB_M_AccountRole");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TB_M_AccountRole");

            migrationBuilder.DropColumn(
                name: "Account_Id",
                table: "TB_M_AccountRole");

            migrationBuilder.AddColumn<string>(
                name: "Account_NIK",
                table: "TB_M_AccountRole",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_M_AccountRole",
                table: "TB_M_AccountRole",
                columns: new[] { "Account_NIK", "Role_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_AccountRole_TB_M_Account_Account_NIK",
                table: "TB_M_AccountRole",
                column: "Account_NIK",
                principalTable: "TB_M_Account",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_AccountRole_TB_M_Account_Account_NIK",
                table: "TB_M_AccountRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_M_AccountRole",
                table: "TB_M_AccountRole");

            migrationBuilder.DropColumn(
                name: "Account_NIK",
                table: "TB_M_AccountRole");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TB_M_AccountRole",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Account_Id",
                table: "TB_M_AccountRole",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_M_AccountRole",
                table: "TB_M_AccountRole",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_AccountRole_Account_Id",
                table: "TB_M_AccountRole",
                column: "Account_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_AccountRole_TB_M_Account_Account_Id",
                table: "TB_M_AccountRole",
                column: "Account_Id",
                principalTable: "TB_M_Account",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
