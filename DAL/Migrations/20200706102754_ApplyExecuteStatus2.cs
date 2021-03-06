using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ApplyExecuteStatus2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyExcuteStatus_ExecuteStatusDetailId",
                table: "Applies");

            migrationBuilder.DropIndex(
                name: "IX_Applies_ExecuteStatusDetailId",
                table: "Applies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Applies_ExecuteStatusDetailId",
                table: "Applies",
                column: "ExecuteStatusDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyExcuteStatus_ExecuteStatusDetailId",
                table: "Applies",
                column: "ExecuteStatusDetailId",
                principalTable: "ApplyExcuteStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
