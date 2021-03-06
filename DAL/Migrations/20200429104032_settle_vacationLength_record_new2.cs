using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class settle_vacationLength_record_new2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppuserSettleModefyRecord");

            migrationBuilder.CreateTable(
                name: "AppUsersSettleModefyRecord",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Length = table.Column<double>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsNewYearInitData = table.Column<bool>(nullable: false),
                    SettleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsersSettleModefyRecord", x => x.Code);
                    table.ForeignKey(
                        name: "FK_AppUsersSettleModefyRecord_AUserSocialInfoSettles_SettleId",
                        column: x => x.SettleId,
                        principalTable: "AUserSocialInfoSettles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsersSettleModefyRecord_SettleId",
                table: "AppUsersSettleModefyRecord",
                column: "SettleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsersSettleModefyRecord");

            migrationBuilder.CreateTable(
                name: "AppuserSettleModefyRecord",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsNewYearInitData = table.Column<bool>(nullable: false),
                    Length = table.Column<double>(nullable: false),
                    SettleId = table.Column<Guid>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppuserSettleModefyRecord", x => x.Code);
                    table.ForeignKey(
                        name: "FK_AppuserSettleModefyRecord_AUserSocialInfoSettles_SettleId",
                        column: x => x.SettleId,
                        principalTable: "AUserSocialInfoSettles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppuserSettleModefyRecord_SettleId",
                table: "AppuserSettleModefyRecord",
                column: "SettleId");
        }
    }
}
