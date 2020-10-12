using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsGatheringService.DAL.Migrations
{
    public partial class NewsUrlTableWasAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_NewsUrl",
                table: "NewsUrls");

            migrationBuilder.DropIndex(
                name: "IX_NewsUrls_NewsId",
                table: "NewsUrls");

            migrationBuilder.AlterColumn<Guid>(
                name: "NewsId",
                table: "NewsUrls",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_NewsUrls_NewsId",
                table: "NewsUrls",
                column: "NewsId",
                unique: true,
                filter: "[NewsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_News_NewsUrls",
                table: "NewsUrls",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_NewsUrls",
                table: "NewsUrls");

            migrationBuilder.DropIndex(
                name: "IX_NewsUrls_NewsId",
                table: "NewsUrls");

            migrationBuilder.AlterColumn<Guid>(
                name: "NewsId",
                table: "NewsUrls",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsUrls_NewsId",
                table: "NewsUrls",
                column: "NewsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_News_NewsUrl",
                table: "NewsUrls",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
