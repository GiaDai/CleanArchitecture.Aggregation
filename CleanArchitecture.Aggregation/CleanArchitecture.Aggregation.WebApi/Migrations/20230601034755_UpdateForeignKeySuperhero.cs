using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Aggregation.WebApi.Migrations
{
    public partial class UpdateForeignKeySuperhero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Superheros_SuperheroId1",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Superpowers_Superheros_SuperheroId1",
                table: "Superpowers");

            migrationBuilder.DropIndex(
                name: "IX_Superpowers_SuperheroId1",
                table: "Superpowers");

            migrationBuilder.DropIndex(
                name: "IX_Movies_SuperheroId1",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "SuperheroId1",
                table: "Superpowers");

            migrationBuilder.DropColumn(
                name: "SuperheroId1",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "SuperheroId",
                table: "Superpowers",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "SuperheroId",
                table: "Movies",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Superpowers_SuperheroId",
                table: "Superpowers",
                column: "SuperheroId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_SuperheroId",
                table: "Movies",
                column: "SuperheroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Superheros_SuperheroId",
                table: "Movies",
                column: "SuperheroId",
                principalTable: "Superheros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Superpowers_Superheros_SuperheroId",
                table: "Superpowers",
                column: "SuperheroId",
                principalTable: "Superheros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Superheros_SuperheroId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Superpowers_Superheros_SuperheroId",
                table: "Superpowers");

            migrationBuilder.DropIndex(
                name: "IX_Superpowers_SuperheroId",
                table: "Superpowers");

            migrationBuilder.DropIndex(
                name: "IX_Movies_SuperheroId",
                table: "Movies");

            migrationBuilder.AlterColumn<Guid>(
                name: "SuperheroId",
                table: "Superpowers",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SuperheroId1",
                table: "Superpowers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SuperheroId",
                table: "Movies",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SuperheroId1",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Superpowers_SuperheroId1",
                table: "Superpowers",
                column: "SuperheroId1");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_SuperheroId1",
                table: "Movies",
                column: "SuperheroId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Superheros_SuperheroId1",
                table: "Movies",
                column: "SuperheroId1",
                principalTable: "Superheros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Superpowers_Superheros_SuperheroId1",
                table: "Superpowers",
                column: "SuperheroId1",
                principalTable: "Superheros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
