using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonTools.Web.Infrastructure.Db.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    AbilityId = table.Column<int>(type: "integer", nullable: false, comment: "特性ID PokeAPIでのid"),
                    AbilityName = table.Column<string>(type: "text", nullable: false, comment: "特性名 日本語名"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table => table.PrimaryKey("PK_Abilities", x => x.AbilityId),
                comment: "特性");

            migrationBuilder.CreateTable(
                name: "IndividualCategories",
                columns: table => new
                {
                    IndividualCategoryId = table.Column<int>(type: "integer", nullable: false, comment: "個体カテゴリID 連番"),
                    IndividualCategoryName = table.Column<string>(type: "text", nullable: false, comment: "個体カテゴリ名"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table => table.PrimaryKey("PK_IndividualCategories", x => x.IndividualCategoryId),
                comment: "個体カテゴリ");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false, comment: "道具ID PokeAPIでのid"),
                    ItemName = table.Column<string>(type: "text", nullable: false, comment: "道具名 日本語名"),
                    FlingPower = table.Column<int>(type: "integer", nullable: true, comment: "投げつけるの威力 投げつけるが失敗する道具はNULL"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table => table.PrimaryKey("PK_Items", x => x.ItemId),
                comment: "道具");

            migrationBuilder.CreateTable(
                name: "MoveDamageClasses",
                columns: table => new
                {
                    MoveDamageClassId = table.Column<int>(type: "integer", nullable: false, comment: "技分類ID PokeAPIでのid"),
                    MoveDamageClassName = table.Column<string>(type: "text", nullable: false, comment: "技分類名 日本語名"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table => table.PrimaryKey("PK_MoveDamageClasses", x => x.MoveDamageClassId),
                comment: "技分類");

            migrationBuilder.CreateTable(
                name: "StatAlignments",
                columns: table => new
                {
                    StatAlignmentId = table.Column<int>(type: "integer", nullable: false, comment: "能力補正ID PokeAPIでのid"),
                    StatAlignmentName = table.Column<string>(type: "text", nullable: false, comment: "能力補正名 日本語名"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table => table.PrimaryKey("PK_StatAlignments", x => x.StatAlignmentId),
                comment: "能力補正");

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "integer", nullable: false, comment: "タイプID PokeAPIでのid"),
                    TypeName = table.Column<string>(type: "text", nullable: false, comment: "タイプ名 日本語名"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table => table.PrimaryKey("PK_Types", x => x.TypeId),
                comment: "ポケモンのタイプ");

            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    MoveId = table.Column<int>(type: "integer", nullable: false, comment: "技ID PokeAPIでのid"),
                    MoveName = table.Column<string>(type: "text", nullable: false, comment: "技名 日本語名"),
                    TypeId = table.Column<int>(type: "integer", nullable: false, comment: "タイプID"),
                    MoveDamageClassId = table.Column<int>(type: "integer", nullable: false, comment: "技分類ID"),
                    Power = table.Column<int>(type: "integer", nullable: true, comment: "威力"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.MoveId);
                    table.ForeignKey(
                        name: "FK_Moves_MoveDamageClasses_MoveDamageClassId",
                        column: x => x.MoveDamageClassId,
                        principalTable: "MoveDamageClasses",
                        principalColumn: "MoveDamageClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Moves_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "技");

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "integer", nullable: false, comment: "種族ID PokeAPIでのpokemonエンドポイントのid"),
                    SpeciesName = table.Column<string>(type: "text", nullable: false, comment: "種族名 日本語名"),
                    Type1Id = table.Column<int>(type: "integer", nullable: false, comment: "タイプ1ID"),
                    Type2Id = table.Column<int>(type: "integer", nullable: true, comment: "タイプ2ID"),
                    Ability1Id = table.Column<int>(type: "integer", nullable: false, comment: "特性1ID"),
                    Ability2Id = table.Column<int>(type: "integer", nullable: true, comment: "特性2ID"),
                    HiddenAbilityId = table.Column<int>(type: "integer", nullable: true, comment: "隠れ特性ID"),
                    BaseStatHp = table.Column<int>(type: "integer", nullable: false, comment: "種族値::HP"),
                    BaseStatAttack = table.Column<int>(type: "integer", nullable: false, comment: "種族値::こうげき"),
                    BaseStatDefense = table.Column<int>(type: "integer", nullable: false, comment: "種族値::ぼうぎょ"),
                    BaseStatSpecialAttack = table.Column<int>(type: "integer", nullable: false, comment: "種族値::とくこう"),
                    BaseStatSpecialDefense = table.Column<int>(type: "integer", nullable: false, comment: "種族値::とくぼう"),
                    BaseStatSpeed = table.Column<int>(type: "integer", nullable: false, comment: "種族値::すばやさ"),
                    Weight = table.Column<int>(type: "integer", nullable: false, comment: "体重 PokeAPIでの値そのまま"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.SpeciesId);
                    table.ForeignKey(
                        name: "FK_Species_Abilities_Ability1Id",
                        column: x => x.Ability1Id,
                        principalTable: "Abilities",
                        principalColumn: "AbilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Species_Abilities_Ability2Id",
                        column: x => x.Ability2Id,
                        principalTable: "Abilities",
                        principalColumn: "AbilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Species_Abilities_HiddenAbilityId",
                        column: x => x.HiddenAbilityId,
                        principalTable: "Abilities",
                        principalColumn: "AbilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Species_Types_Type1Id",
                        column: x => x.Type1Id,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Species_Types_Type2Id",
                        column: x => x.Type2Id,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "ポケモンの種族");

            migrationBuilder.CreateTable(
                name: "Individuals",
                columns: table => new
                {
                    IndividualId = table.Column<string>(type: "text", nullable: false, comment: "個体ID prefix_uuidv7"),
                    IndividualName = table.Column<string>(type: "text", nullable: true, comment: "個体名 NULLだったら表示時に種族名を表示"),
                    SpeciesId = table.Column<int>(type: "integer", nullable: false, comment: "種族ID"),
                    StatAlignmentId = table.Column<int>(type: "integer", nullable: false, comment: "能力補正ID"),
                    AbilityId = table.Column<int>(type: "integer", nullable: false, comment: "特性ID"),
                    StatPointHp = table.Column<int>(type: "integer", nullable: false, comment: "能力ポイント::HP"),
                    StatPointAttack = table.Column<int>(type: "integer", nullable: false, comment: "能力ポイント::こうげき"),
                    StatPointDefense = table.Column<int>(type: "integer", nullable: false, comment: "能力ポイント::ぼうぎょ"),
                    StatPointSpecialAttack = table.Column<int>(type: "integer", nullable: false, comment: "能力ポイント::とくこう"),
                    StatPointSpecialDefense = table.Column<int>(type: "integer", nullable: false, comment: "能力ポイント::とくぼう"),
                    StatPointSpeed = table.Column<int>(type: "integer", nullable: false, comment: "能力ポイント::すばやさ"),
                    Move1Id = table.Column<int>(type: "integer", nullable: false, comment: "技1ID"),
                    Move2Id = table.Column<int>(type: "integer", nullable: true, comment: "技2ID"),
                    Move3Id = table.Column<int>(type: "integer", nullable: true, comment: "技3ID"),
                    Move4Id = table.Column<int>(type: "integer", nullable: true, comment: "技4ID"),
                    HeldItemId = table.Column<int>(type: "integer", nullable: true, comment: "持ち物 道具ID"),
                    TeraTypeId = table.Column<int>(type: "integer", nullable: false, comment: "テラスタイプID"),
                    Memo = table.Column<string>(type: "text", nullable: true, comment: "備考"),
                    CategoryId = table.Column<int>(type: "integer", nullable: false, comment: "カテゴリID"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individuals", x => x.IndividualId);
                    table.ForeignKey(
                        name: "FK_Individuals_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalTable: "Abilities",
                        principalColumn: "AbilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_IndividualCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "IndividualCategories",
                        principalColumn: "IndividualCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_Items_HeldItemId",
                        column: x => x.HeldItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_Moves_Move1Id",
                        column: x => x.Move1Id,
                        principalTable: "Moves",
                        principalColumn: "MoveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_Moves_Move2Id",
                        column: x => x.Move2Id,
                        principalTable: "Moves",
                        principalColumn: "MoveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_Moves_Move3Id",
                        column: x => x.Move3Id,
                        principalTable: "Moves",
                        principalColumn: "MoveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_Moves_Move4Id",
                        column: x => x.Move4Id,
                        principalTable: "Moves",
                        principalColumn: "MoveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_StatAlignments_StatAlignmentId",
                        column: x => x.StatAlignmentId,
                        principalTable: "StatAlignments",
                        principalColumn: "StatAlignmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individuals_Types_TeraTypeId",
                        column: x => x.TeraTypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "個体（育成済ポケモン）");

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    PartyId = table.Column<string>(type: "text", nullable: false, comment: "パーティID prefix_uuidv7"),
                    PartyName = table.Column<string>(type: "text", nullable: false, comment: "パーティ名"),
                    Individual1Id = table.Column<string>(type: "text", nullable: true, comment: "個体1ID"),
                    Individual2Id = table.Column<string>(type: "text", nullable: true, comment: "個体2ID"),
                    Individual3Id = table.Column<string>(type: "text", nullable: true, comment: "個体3ID"),
                    Individual4Id = table.Column<string>(type: "text", nullable: true, comment: "個体4ID"),
                    Individual5Id = table.Column<string>(type: "text", nullable: true, comment: "個体5ID"),
                    Individual6Id = table.Column<string>(type: "text", nullable: true, comment: "個体6ID"),
                    Memo = table.Column<string>(type: "text", nullable: true, comment: "備考"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "作成日時"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新日時")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.PartyId);
                    table.ForeignKey(
                        name: "FK_Parties_Individuals_Individual1Id",
                        column: x => x.Individual1Id,
                        principalTable: "Individuals",
                        principalColumn: "IndividualId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parties_Individuals_Individual2Id",
                        column: x => x.Individual2Id,
                        principalTable: "Individuals",
                        principalColumn: "IndividualId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parties_Individuals_Individual3Id",
                        column: x => x.Individual3Id,
                        principalTable: "Individuals",
                        principalColumn: "IndividualId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parties_Individuals_Individual4Id",
                        column: x => x.Individual4Id,
                        principalTable: "Individuals",
                        principalColumn: "IndividualId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parties_Individuals_Individual5Id",
                        column: x => x.Individual5Id,
                        principalTable: "Individuals",
                        principalColumn: "IndividualId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parties_Individuals_Individual6Id",
                        column: x => x.Individual6Id,
                        principalTable: "Individuals",
                        principalColumn: "IndividualId",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "パーティ");

            migrationBuilder.InsertData(
                table: "IndividualCategories",
                columns: ["IndividualCategoryId", "IndividualCategoryName"],
                values: new object[,]
                {
                    { 1, "手持ち個体" },
                    { 2, "ダメージ計算プリセット個体" }
                });

            migrationBuilder.InsertData(
                table: "MoveDamageClasses",
                columns: ["MoveDamageClassId", "MoveDamageClassName"],
                values: new object[,]
                {
                    { 1, "へんか" },
                    { 2, "ぶつり" },
                    { 3, "とくしゅ" }
                });

            migrationBuilder.InsertData(
                table: "StatAlignments",
                columns: ["StatAlignmentId", "StatAlignmentName"],
                values: new object[,]
                {
                    { 1, "がんばりや" },
                    { 2, "ずぶとい" },
                    { 3, "ひかえめ" },
                    { 4, "おだやか" },
                    { 5, "おくびょう" },
                    { 6, "さみしがり" },
                    { 7, "すなお" },
                    { 8, "おっとり" },
                    { 9, "おとなしい" },
                    { 10, "せっかち" },
                    { 11, "いじっぱり" },
                    { 12, "わんぱく" },
                    { 13, "てれや" },
                    { 14, "しんちょう" },
                    { 15, "うっかりや" },
                    { 16, "ようき" },
                    { 17, "やんちゃ" },
                    { 18, "のうてんき" },
                    { 19, "きまぐれ" },
                    { 20, "むじゃき" },
                    { 21, "ゆうかん" },
                    { 22, "のんき" },
                    { 23, "れいせい" },
                    { 24, "なまいき" },
                    { 25, "まじめ" }
                });

            migrationBuilder.InsertData(
                table: "Types",
                columns: ["TypeId", "TypeName"],
                values: new object[,]
                {
                    { 1, "ノーマル" },
                    { 2, "かくとう" },
                    { 3, "ひこう" },
                    { 4, "どく" },
                    { 5, "じめん" },
                    { 6, "いわ" },
                    { 7, "むし" },
                    { 8, "ゴースト" },
                    { 9, "はがね" },
                    { 10, "ほのお" },
                    { 11, "みず" },
                    { 12, "くさ" },
                    { 13, "でんき" },
                    { 14, "エスパー" },
                    { 15, "こおり" },
                    { 16, "ドラゴン" },
                    { 17, "あく" },
                    { 18, "フェアリー" },
                    { 19, "ステラ" },
                    { 10001, "???" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_AbilityId",
                table: "Individuals",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_CategoryId",
                table: "Individuals",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_CreatedAt",
                table: "Individuals",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_HeldItemId",
                table: "Individuals",
                column: "HeldItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_Move1Id",
                table: "Individuals",
                column: "Move1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_Move2Id",
                table: "Individuals",
                column: "Move2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_Move3Id",
                table: "Individuals",
                column: "Move3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_Move4Id",
                table: "Individuals",
                column: "Move4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_SpeciesId",
                table: "Individuals",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_StatAlignmentId",
                table: "Individuals",
                column: "StatAlignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_TeraTypeId",
                table: "Individuals",
                column: "TeraTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_UpdatedAt",
                table: "Individuals",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_MoveDamageClassId",
                table: "Moves",
                column: "MoveDamageClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_TypeId",
                table: "Moves",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_CreatedAt",
                table: "Parties",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Individual1Id",
                table: "Parties",
                column: "Individual1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Individual2Id",
                table: "Parties",
                column: "Individual2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Individual3Id",
                table: "Parties",
                column: "Individual3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Individual4Id",
                table: "Parties",
                column: "Individual4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Individual5Id",
                table: "Parties",
                column: "Individual5Id");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Individual6Id",
                table: "Parties",
                column: "Individual6Id");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_UpdatedAt",
                table: "Parties",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Ability1Id",
                table: "Species",
                column: "Ability1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Ability2Id",
                table: "Species",
                column: "Ability2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Species_HiddenAbilityId",
                table: "Species",
                column: "HiddenAbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Type1Id",
                table: "Species",
                column: "Type1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Type2Id",
                table: "Species",
                column: "Type2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "Individuals");

            migrationBuilder.DropTable(
                name: "IndividualCategories");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "StatAlignments");

            migrationBuilder.DropTable(
                name: "MoveDamageClasses");

            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
