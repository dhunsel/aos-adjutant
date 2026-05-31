using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AosAdjutant.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    public_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    email = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    identity_provider = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    subject = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                }
            );

            migrationBuilder.CreateTable(
                name: "weapon_effect",
                columns: table => new
                {
                    weapon_effect_id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    key = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    name = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapon_effect", x => x.weapon_effect_id);
                }
            );

            migrationBuilder.CreateTable(
                name: "ability",
                columns: table => new
                {
                    ability_id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    name = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    reaction = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: true
                    ),
                    declaration = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: true
                    ),
                    effect = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    phase = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    restriction = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: true
                    ),
                    turn = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: true
                    ),
                    is_generic = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    modified_by_user_id = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ability", x => x.ability_id);
                    table.ForeignKey(
                        name: "FK_ability_user_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_ability_user_modified_by_user_id",
                        column: x => x.modified_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "faction",
                columns: table => new
                {
                    faction_id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    name = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    grand_alliance = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    modified_by_user_id = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faction", x => x.faction_id);
                    table.ForeignKey(
                        name: "FK_faction_user_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_faction_user_modified_by_user_id",
                        column: x => x.modified_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "battle_formation",
                columns: table => new
                {
                    battle_formation_id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    name = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    faction_id = table.Column<int>(type: "integer", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    modified_by_user_id = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battle_formation", x => x.battle_formation_id);
                    table.ForeignKey(
                        name: "FK_battle_formation_faction_faction_id",
                        column: x => x.faction_id,
                        principalTable: "faction",
                        principalColumn: "faction_id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_battle_formation_user_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_battle_formation_user_modified_by_user_id",
                        column: x => x.modified_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "faction_ability",
                columns: table => new
                {
                    ability_id = table.Column<int>(type: "integer", nullable: false),
                    faction_id = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faction_ability", x => new { x.ability_id, x.faction_id });
                    table.ForeignKey(
                        name: "FK_faction_ability_ability_ability_id",
                        column: x => x.ability_id,
                        principalTable: "ability",
                        principalColumn: "ability_id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_faction_ability_faction_faction_id",
                        column: x => x.faction_id,
                        principalTable: "faction",
                        principalColumn: "faction_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "unit",
                columns: table => new
                {
                    unit_id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    name = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    health = table.Column<int>(type: "integer", nullable: false),
                    move = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    save = table.Column<int>(type: "integer", nullable: false),
                    control = table.Column<int>(type: "integer", nullable: false),
                    ward_save = table.Column<int>(type: "integer", nullable: true),
                    faction_id = table.Column<int>(type: "integer", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    modified_by_user_id = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit", x => x.unit_id);
                    table.ForeignKey(
                        name: "FK_unit_faction_faction_id",
                        column: x => x.faction_id,
                        principalTable: "faction",
                        principalColumn: "faction_id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_unit_user_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_unit_user_modified_by_user_id",
                        column: x => x.modified_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "battle_formation_ability",
                columns: table => new
                {
                    ability_id = table.Column<int>(type: "integer", nullable: false),
                    battle_formation_id = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_battle_formation_ability",
                        x => new { x.ability_id, x.battle_formation_id }
                    );
                    table.ForeignKey(
                        name: "FK_battle_formation_ability_ability_ability_id",
                        column: x => x.ability_id,
                        principalTable: "ability",
                        principalColumn: "ability_id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_battle_formation_ability_battle_formation_battle_formation_~",
                        column: x => x.battle_formation_id,
                        principalTable: "battle_formation",
                        principalColumn: "battle_formation_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "attack_profile",
                columns: table => new
                {
                    attack_profile_id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    name = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    is_ranged = table.Column<bool>(type: "boolean", nullable: false),
                    range = table.Column<int>(type: "integer", nullable: true),
                    attacks = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    to_hit = table.Column<int>(type: "integer", nullable: false),
                    to_wound = table.Column<int>(type: "integer", nullable: false),
                    rend = table.Column<int>(type: "integer", nullable: true),
                    damage = table.Column<string>(
                        type: "character varying(250)",
                        maxLength: 250,
                        nullable: false
                    ),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    modified_by_user_id = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attack_profile", x => x.attack_profile_id);
                    table.ForeignKey(
                        name: "FK_attack_profile_unit_unit_id",
                        column: x => x.unit_id,
                        principalTable: "unit",
                        principalColumn: "unit_id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_attack_profile_user_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_attack_profile_user_modified_by_user_id",
                        column: x => x.modified_by_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "unit_ability",
                columns: table => new
                {
                    ability_id = table.Column<int>(type: "integer", nullable: false),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit_ability", x => new { x.ability_id, x.unit_id });
                    table.ForeignKey(
                        name: "FK_unit_ability_ability_ability_id",
                        column: x => x.ability_id,
                        principalTable: "ability",
                        principalColumn: "ability_id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_unit_ability_unit_unit_id",
                        column: x => x.unit_id,
                        principalTable: "unit",
                        principalColumn: "unit_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "attack_profile_weapon_effect",
                columns: table => new
                {
                    attack_profile_id = table.Column<int>(type: "integer", nullable: false),
                    weapon_effect_id = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_attack_profile_weapon_effect",
                        x => new { x.attack_profile_id, x.weapon_effect_id }
                    );
                    table.ForeignKey(
                        name: "FK_attack_profile_weapon_effect_attack_profile_attack_profile_~",
                        column: x => x.attack_profile_id,
                        principalTable: "attack_profile",
                        principalColumn: "attack_profile_id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_attack_profile_weapon_effect_weapon_effect_weapon_effect_id",
                        column: x => x.weapon_effect_id,
                        principalTable: "weapon_effect",
                        principalColumn: "weapon_effect_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.InsertData(
                table: "weapon_effect",
                columns: new[] { "weapon_effect_id", "key", "name" },
                values: new object[,]
                {
                    { 1, "crit_2_hits", "Crit (2 Hits)" },
                    { 2, "crit_auto_wound", "Crit (Auto-wound)" },
                    { 3, "crit_mortal", "Crit (Mortal)" },
                    { 4, "charge_1_damage", "Charge (+1 Damage)" },
                    { 5, "companion", "Companion" },
                    { 6, "shoot_in_combat", "Shoot in Combat" },
                    { 7, "anti_monster", "Anti-Monster (+1 Rend)" },
                    { 8, "anti_hero", "Anti-Hero (+1 Rend)" },
                    { 9, "anti_infantry", "Anti-Infantry (+1 Rend)" },
                    { 10, "anti_cavalry", "Anti-Cavalry (+1 Rend)" },
                    { 11, "anti_wizard", "Anti-Wizard (+1 Rend)" },
                    { 12, "anti_manifestation", "Anti-Manifestation (+1 Rend)" },
                    { 13, "anti_charge", "Anti-Charge (+1 Rend)" },
                }
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
                name: "IX_attack_profile_name_unit_id",
                table: "attack_profile",
                columns: new[] { "name", "unit_id" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_attack_profile_unit_id",
                table: "attack_profile",
                column: "unit_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_attack_profile_weapon_effect_weapon_effect_id",
                table: "attack_profile_weapon_effect",
                column: "weapon_effect_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_battle_formation_created_by_user_id",
                table: "battle_formation",
                column: "created_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_battle_formation_faction_id_name",
                table: "battle_formation",
                columns: new[] { "faction_id", "name" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_battle_formation_modified_by_user_id",
                table: "battle_formation",
                column: "modified_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_battle_formation_ability_ability_id",
                table: "battle_formation_ability",
                column: "ability_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_battle_formation_ability_battle_formation_id",
                table: "battle_formation_ability",
                column: "battle_formation_id"
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
                name: "IX_faction_name",
                table: "faction",
                column: "name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_faction_ability_ability_id",
                table: "faction_ability",
                column: "ability_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_faction_ability_faction_id",
                table: "faction_ability",
                column: "faction_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_unit_created_by_user_id",
                table: "unit",
                column: "created_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_unit_faction_id_name",
                table: "unit",
                columns: new[] { "faction_id", "name" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_unit_modified_by_user_id",
                table: "unit",
                column: "modified_by_user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_unit_ability_ability_id",
                table: "unit_ability",
                column: "ability_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_unit_ability_unit_id",
                table: "unit_ability",
                column: "unit_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_user_identity_provider_subject",
                table: "user",
                columns: new[] { "identity_provider", "subject" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_user_public_id",
                table: "user",
                column: "public_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_weapon_effect_key",
                table: "weapon_effect",
                column: "key",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_weapon_effect_name",
                table: "weapon_effect",
                column: "name",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "attack_profile_weapon_effect");

            migrationBuilder.DropTable(name: "battle_formation_ability");

            migrationBuilder.DropTable(name: "faction_ability");

            migrationBuilder.DropTable(name: "unit_ability");

            migrationBuilder.DropTable(name: "attack_profile");

            migrationBuilder.DropTable(name: "weapon_effect");

            migrationBuilder.DropTable(name: "battle_formation");

            migrationBuilder.DropTable(name: "ability");

            migrationBuilder.DropTable(name: "unit");

            migrationBuilder.DropTable(name: "faction");

            migrationBuilder.DropTable(name: "user");
        }
    }
}
