using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AosAdjutant.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "unit",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()"
            );

            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "unit",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "modified_at",
                table: "unit",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "modified_by_user_id",
                table: "unit",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "faction",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()"
            );

            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "faction",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "modified_at",
                table: "faction",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "modified_by_user_id",
                table: "faction",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "battle_formation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()"
            );

            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "battle_formation",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "modified_at",
                table: "battle_formation",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "modified_by_user_id",
                table: "battle_formation",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "attack_profile",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()"
            );

            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "attack_profile",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "modified_at",
                table: "attack_profile",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "modified_by_user_id",
                table: "attack_profile",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "ability",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()"
            );

            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "ability",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "modified_at",
                table: "ability",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "modified_by_user_id",
                table: "ability",
                type: "integer",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_unit_created_by_user_id",
                table: "unit",
                column: "created_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_unit_modified_by_user_id",
                table: "unit",
                column: "modified_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_faction_created_by_user_id",
                table: "faction",
                column: "created_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_faction_modified_by_user_id",
                table: "faction",
                column: "modified_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_battle_formation_created_by_user_id",
                table: "battle_formation",
                column: "created_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_battle_formation_modified_by_user_id",
                table: "battle_formation",
                column: "modified_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_attack_profile_created_by_user_id",
                table: "attack_profile",
                column: "created_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_attack_profile_modified_by_user_id",
                table: "attack_profile",
                column: "modified_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ability_created_by_user_id",
                table: "ability",
                column: "created_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ability_modified_by_user_id",
                table: "ability",
                column: "modified_by_user_id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ability_user_created_by_user_id",
                table: "ability",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ability_user_modified_by_user_id",
                table: "ability",
                column: "modified_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_attack_profile_user_created_by_user_id",
                table: "attack_profile",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_attack_profile_user_modified_by_user_id",
                table: "attack_profile",
                column: "modified_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_battle_formation_user_created_by_user_id",
                table: "battle_formation",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_battle_formation_user_modified_by_user_id",
                table: "battle_formation",
                column: "modified_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_faction_user_created_by_user_id",
                table: "faction",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_faction_user_modified_by_user_id",
                table: "faction",
                column: "modified_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_unit_user_created_by_user_id",
                table: "unit",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_unit_user_modified_by_user_id",
                table: "unit",
                column: "modified_by_user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict
            );

            // created_at should not have a default, after using default for existing data, drop default
            foreach (
                var table in new[]
                {
                    "unit",
                    "faction",
                    "battle_formation",
                    "attack_profile",
                    "ability",
                }
            )
            {
                migrationBuilder.Sql(
                    $"ALTER TABLE \"{table}\" ALTER COLUMN created_at DROP DEFAULT;"
                );
            }

            // grand_alliance still has remaining default that shouldn't exists anymore
            migrationBuilder.Sql("ALTER TABLE faction ALTER COLUMN grand_alliance DROP DEFAULT;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ability_user_created_by_user_id",
                table: "ability"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ability_user_modified_by_user_id",
                table: "ability"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_attack_profile_user_created_by_user_id",
                table: "attack_profile"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_attack_profile_user_modified_by_user_id",
                table: "attack_profile"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_battle_formation_user_created_by_user_id",
                table: "battle_formation"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_battle_formation_user_modified_by_user_id",
                table: "battle_formation"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_faction_user_created_by_user_id",
                table: "faction"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_faction_user_modified_by_user_id",
                table: "faction"
            );

            migrationBuilder.DropForeignKey(name: "FK_unit_user_created_by_user_id", table: "unit");

            migrationBuilder.DropForeignKey(
                name: "FK_unit_user_modified_by_user_id",
                table: "unit"
            );

            migrationBuilder.DropIndex(name: "IX_unit_created_by_user_id", table: "unit");

            migrationBuilder.DropIndex(name: "IX_unit_modified_by_user_id", table: "unit");

            migrationBuilder.DropIndex(name: "IX_faction_created_by_user_id", table: "faction");

            migrationBuilder.DropIndex(name: "IX_faction_modified_by_user_id", table: "faction");

            migrationBuilder.DropIndex(
                name: "IX_battle_formation_created_by_user_id",
                table: "battle_formation"
            );

            migrationBuilder.DropIndex(
                name: "IX_battle_formation_modified_by_user_id",
                table: "battle_formation"
            );

            migrationBuilder.DropIndex(
                name: "IX_attack_profile_created_by_user_id",
                table: "attack_profile"
            );

            migrationBuilder.DropIndex(
                name: "IX_attack_profile_modified_by_user_id",
                table: "attack_profile"
            );

            migrationBuilder.DropIndex(name: "IX_ability_created_by_user_id", table: "ability");

            migrationBuilder.DropIndex(name: "IX_ability_modified_by_user_id", table: "ability");

            migrationBuilder.DropColumn(name: "created_at", table: "unit");

            migrationBuilder.DropColumn(name: "created_by_user_id", table: "unit");

            migrationBuilder.DropColumn(name: "modified_at", table: "unit");

            migrationBuilder.DropColumn(name: "modified_by_user_id", table: "unit");

            migrationBuilder.DropColumn(name: "created_at", table: "faction");

            migrationBuilder.DropColumn(name: "created_by_user_id", table: "faction");

            migrationBuilder.DropColumn(name: "modified_at", table: "faction");

            migrationBuilder.DropColumn(name: "modified_by_user_id", table: "faction");

            migrationBuilder.DropColumn(name: "created_at", table: "battle_formation");

            migrationBuilder.DropColumn(name: "created_by_user_id", table: "battle_formation");

            migrationBuilder.DropColumn(name: "modified_at", table: "battle_formation");

            migrationBuilder.DropColumn(name: "modified_by_user_id", table: "battle_formation");

            migrationBuilder.DropColumn(name: "created_at", table: "attack_profile");

            migrationBuilder.DropColumn(name: "created_by_user_id", table: "attack_profile");

            migrationBuilder.DropColumn(name: "modified_at", table: "attack_profile");

            migrationBuilder.DropColumn(name: "modified_by_user_id", table: "attack_profile");

            migrationBuilder.DropColumn(name: "created_at", table: "ability");

            migrationBuilder.DropColumn(name: "created_by_user_id", table: "ability");

            migrationBuilder.DropColumn(name: "modified_at", table: "ability");

            migrationBuilder.DropColumn(name: "modified_by_user_id", table: "ability");

            migrationBuilder.Sql("ALTER TABLE faction ALTER COLUMN grand_alliance SET DEFAULT '';");
        }
    }
}
