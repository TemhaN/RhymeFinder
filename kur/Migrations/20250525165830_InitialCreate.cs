using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace kur.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Phonemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phoneme = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phonemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RhymeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RhymeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RhymeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StressPosition = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rhymes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    RhymeId = table.Column<int>(type: "int", nullable: false),
                    RhymeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rhymes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rhymes_RhymeTypes_RhymeTypeId",
                        column: x => x.RhymeTypeId,
                        principalTable: "RhymeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rhymes_Words_RhymeId",
                        column: x => x.RhymeId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rhymes_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordPhonemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    PhonemeId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordPhonemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordPhonemes_Phonemes_PhonemeId",
                        column: x => x.PhonemeId,
                        principalTable: "Phonemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordPhonemes_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Phonemes",
                columns: new[] { "Id", "Phoneme" },
                values: new object[,]
                {
                    { 1, "А" },
                    { 2, "Б" },
                    { 3, "В" },
                    { 4, "Г" },
                    { 5, "Д" },
                    { 6, "Е" },
                    { 7, "Ё" },
                    { 8, "Ж" },
                    { 9, "З" },
                    { 10, "И" },
                    { 11, "Й" },
                    { 12, "К" },
                    { 13, "Л" },
                    { 14, "М" },
                    { 15, "Н" },
                    { 16, "О" },
                    { 17, "П" },
                    { 18, "Р" },
                    { 19, "С" },
                    { 20, "Т" },
                    { 21, "У" },
                    { 22, "Ф" },
                    { 23, "Х" },
                    { 24, "Ц" },
                    { 25, "Ч" },
                    { 26, "Ш" },
                    { 27, "Щ" },
                    { 28, "Ъ" },
                    { 29, "Ы" },
                    { 30, "Ь" },
                    { 31, "Э" },
                    { 32, "Ю" },
                    { 33, "Я" }
                });

            migrationBuilder.InsertData(
                table: "RhymeTypes",
                columns: new[] { "Id", "RhymeName" },
                values: new object[,]
                {
                    { 1, "Точная" },
                    { 2, "Неточная" },
                    { 3, "Ассонанс" },
                    { 4, "Диссонанс" },
                    { 5, "Мужская" },
                    { 6, "Женская" },
                    { 7, "Дактилическая" },
                    { 8, "Гипердактилическая" },
                    { 9, "Богатая" },
                    { 10, "Бедная" },
                    { 11, "Составная" },
                    { 12, "Кольцевая" },
                    { 13, "Перекрестная" },
                    { 14, "Парная" },
                    { 15, "Охватная" },
                    { 16, "Свободная" },
                    { 17, "Внутренняя" },
                    { 18, "Начальная" },
                    { 19, "Конечная" },
                    { 20, "Полная" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[] { 1, "admin", "admin", 1 });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "StressPosition", "Word" },
                values: new object[,]
                {
                    { 1, 2, "любовь" },
                    { 2, 1, "кровь" },
                    { 3, 1, "снова" },
                    { 4, 2, "слова" },
                    { 5, 2, "мечта" },
                    { 6, 1, "света" },
                    { 7, 2, "звезда" },
                    { 8, 2, "вода" },
                    { 9, 2, "огонь" },
                    { 10, 1, "конь" },
                    { 11, 1, "день" },
                    { 12, 1, "тень" },
                    { 13, 1, "путь" },
                    { 14, 1, "грудь" },
                    { 15, 1, "друг" },
                    { 16, 1, "круг" },
                    { 17, 2, "рука" },
                    { 18, 2, "река" },
                    { 19, 2, "душа" },
                    { 20, 3, "тишина" },
                    { 21, 2, "весна" },
                    { 22, 2, "зима" },
                    { 23, 2, "луна" },
                    { 24, 2, "волна" },
                    { 25, 2, "судьба" },
                    { 26, 2, "тоска" },
                    { 27, 1, "песня" },
                    { 28, 1, "травка" },
                    { 29, 1, "сердце" },
                    { 30, 1, "солнце" },
                    { 31, 1, "небо" },
                    { 32, 1, "ветер" },
                    { 33, 1, "пламя" },
                    { 34, 1, "знамя" },
                    { 35, 1, "поле" },
                    { 36, 1, "море" },
                    { 37, 1, "горе" },
                    { 38, 1, "радость" },
                    { 39, 1, "счастье" },
                    { 40, 1, "жизнь" },
                    { 41, 1, "смерть" },
                    { 42, 1, "свет" },
                    { 43, 1, "цвет" },
                    { 44, 1, "дом" },
                    { 45, 1, "сом" },
                    { 46, 1, "лес" },
                    { 47, 1, "вес" },
                    { 48, 1, "сон" },
                    { 49, 1, "тон" },
                    { 50, 1, "мир" },
                    { 51, 1, "птица" },
                    { 52, 1, "улица" },
                    { 53, 1, "дверь" },
                    { 54, 1, "тень" },
                    { 55, 1, "воля" },
                    { 56, 1, "доля" },
                    { 57, 2, "земля" },
                    { 58, 2, "семья" },
                    { 59, 1, "книга" },
                    { 60, 1, "лига" },
                    { 61, 1, "вера" },
                    { 62, 1, "мера" },
                    { 63, 1, "слава" },
                    { 64, 1, "пава" },
                    { 65, 2, "тропа" },
                    { 66, 2, "европа" },
                    { 67, 1, "дерево" },
                    { 68, 1, "пламя" },
                    { 69, 1, "камни" },
                    { 70, 1, "дни" },
                    { 71, 1, "звери" },
                    { 72, 1, "двери" },
                    { 73, 2, "песок" },
                    { 74, 2, "носок" },
                    { 75, 1, "берег" },
                    { 76, 2, "ковчег" },
                    { 77, 1, "луг" },
                    { 78, 1, "друг" },
                    { 79, 1, "снег" },
                    { 80, 1, "век" },
                    { 81, 2, "цветок" },
                    { 82, 2, "поток" },
                    { 83, 1, "голос" },
                    { 84, 1, "колос" },
                    { 85, 1, "взгляд" },
                    { 86, 1, "сад" },
                    { 87, 1, "меч" },
                    { 88, 1, "печь" },
                    { 89, 1, "песнь" },
                    { 90, 1, "честность" },
                    { 91, 2, "дорога" },
                    { 92, 2, "нога" },
                    { 93, 2, "береза" },
                    { 94, 2, "слеза" },
                    { 95, 1, "сила" },
                    { 96, 2, "мила" },
                    { 97, 2, "свобода" },
                    { 98, 2, "природа" },
                    { 99, 2, "победа" },
                    { 100, 2, "беда" },
                    { 101, 3, "человек" },
                    { 102, 1, "век" },
                    { 103, 2, "любить" },
                    { 104, 2, "простить" },
                    { 105, 2, "лететь" },
                    { 106, 2, "смотреть" },
                    { 107, 2, "идти" },
                    { 108, 2, "нести" },
                    { 109, 1, "петь" },
                    { 110, 2, "светить" },
                    { 111, 1, "ждать" },
                    { 112, 2, "искать" },
                    { 113, 2, "мечтать" },
                    { 114, 2, "страдать" },
                    { 115, 2, "гореть" },
                    { 116, 2, "творец" },
                    { 117, 2, "бежать" },
                    { 118, 2, "лежать" },
                    { 119, 2, "кричать" },
                    { 120, 2, "молчать" },
                    { 121, 2, "дышать" },
                    { 122, 2, "писать" },
                    { 123, 2, "читать" },
                    { 124, 1, "звать" },
                    { 125, 1, "дать" },
                    { 126, 1, "взять" },
                    { 127, 1, "знать" },
                    { 128, 1, "спать" },
                    { 129, 2, "стоять" },
                    { 130, 2, "гулять" },
                    { 131, 2, "смеять" },
                    { 132, 1, "плакать" },
                    { 133, 2, "бросать" },
                    { 134, 1, "росать" },
                    { 135, 1, "видеть" },
                    { 136, 2, "сидеть" },
                    { 137, 2, "держать" },
                    { 138, 2, "рожать" },
                    { 139, 1, "думать" },
                    { 140, 2, "просить" },
                    { 141, 2, "хитрить" },
                    { 142, 2, "творить" },
                    { 143, 2, "бродить" },
                    { 144, 2, "водить" },
                    { 145, 2, "сверкать" },
                    { 146, 1, "меркать" },
                    { 147, 2, "шагать" },
                    { 148, 1, "бегать" },
                    { 149, 1, "плавать" },
                    { 150, 1, "править" },
                    { 151, 1, "славить" },
                    { 152, 2, "ловить" },
                    { 153, 2, "свободный" },
                    { 154, 2, "родной" },
                    { 155, 2, "высокий" },
                    { 156, 2, "глубокий" },
                    { 157, 1, "ясный" },
                    { 158, 1, "красный" },
                    { 159, 1, "чистый" },
                    { 160, 2, "простой" },
                    { 161, 1, "светлый" },
                    { 162, 1, "летний" },
                    { 163, 1, "зимний" },
                    { 164, 2, "весенний" },
                    { 165, 2, "осенний" },
                    { 166, 2, "далекий" },
                    { 167, 1, "близкий" },
                    { 168, 2, "горячий" },
                    { 169, 1, "холодный" },
                    { 170, 1, "мудрый" },
                    { 171, 1, "добрый" },
                    { 172, 1, "смелый" },
                    { 173, 1, "верный" },
                    { 174, 1, "честный" },
                    { 175, 1, "грустный" },
                    { 176, 2, "веселый" },
                    { 177, 2, "пустой" },
                    { 178, 1, "крепкий" },
                    { 179, 1, "цепкий" },
                    { 180, 1, "быстрый" },
                    { 181, 1, "острый" },
                    { 182, 1, "мягкий" },
                    { 183, 1, "гладкий" },
                    { 184, 1, "шаткий" },
                    { 185, 1, "рваный" },
                    { 186, 2, "прямой" },
                    { 187, 2, "кривой" },
                    { 188, 2, "живой" },
                    { 189, 1, "свежий" },
                    { 190, 1, "нежный" },
                    { 191, 1, "белый" },
                    { 192, 1, "синий" },
                    { 193, 3, "зеленый" },
                    { 194, 1, "серый" },
                    { 195, 1, "черный" },
                    { 196, 1, "яркий" },
                    { 197, 1, "темный" },
                    { 198, 1, "бледный" },
                    { 199, 2, "громкий" },
                    { 200, 1, "тонкий" }
                });

            migrationBuilder.InsertData(
                table: "Rhymes",
                columns: new[] { "Id", "RhymeId", "RhymeTypeId", "WordId" },
                values: new object[,]
                {
                    { 1, 2, 1, 1 },
                    { 2, 4, 1, 3 },
                    { 3, 6, 2, 5 },
                    { 4, 8, 1, 7 },
                    { 5, 10, 1, 9 },
                    { 6, 12, 1, 11 },
                    { 7, 14, 1, 13 },
                    { 8, 16, 1, 15 },
                    { 9, 18, 1, 17 },
                    { 10, 20, 2, 19 },
                    { 11, 22, 3, 21 },
                    { 12, 24, 1, 23 },
                    { 13, 26, 2, 25 },
                    { 14, 28, 2, 27 },
                    { 15, 30, 3, 29 },
                    { 16, 32, 2, 31 },
                    { 17, 34, 1, 33 },
                    { 18, 36, 2, 35 },
                    { 19, 36, 1, 37 },
                    { 20, 39, 2, 38 },
                    { 21, 41, 3, 40 },
                    { 22, 43, 1, 42 },
                    { 23, 45, 1, 44 },
                    { 24, 47, 1, 46 },
                    { 25, 49, 1, 48 },
                    { 26, 51, 2, 50 },
                    { 27, 53, 1, 52 },
                    { 28, 55, 1, 54 },
                    { 29, 57, 1, 56 },
                    { 30, 59, 1, 58 },
                    { 31, 61, 1, 60 },
                    { 32, 63, 1, 62 },
                    { 33, 65, 1, 64 },
                    { 34, 33, 2, 66 },
                    { 35, 68, 1, 67 },
                    { 36, 70, 1, 69 },
                    { 37, 72, 1, 71 },
                    { 38, 74, 1, 73 },
                    { 39, 15, 1, 75 },
                    { 40, 77, 1, 76 },
                    { 41, 79, 1, 78 },
                    { 42, 81, 1, 80 },
                    { 43, 83, 1, 82 },
                    { 44, 85, 2, 84 },
                    { 45, 87, 1, 86 },
                    { 46, 89, 1, 88 },
                    { 47, 91, 1, 90 },
                    { 48, 93, 1, 92 },
                    { 49, 95, 2, 94 },
                    { 50, 77, 2, 96 },
                    { 51, 98, 1, 97 },
                    { 52, 100, 2, 99 },
                    { 53, 102, 1, 101 },
                    { 54, 104, 2, 103 },
                    { 55, 106, 1, 105 },
                    { 56, 108, 1, 107 },
                    { 57, 110, 1, 109 },
                    { 58, 112, 1, 111 },
                    { 59, 114, 1, 113 },
                    { 60, 116, 2, 115 },
                    { 61, 118, 1, 117 },
                    { 62, 120, 1, 119 },
                    { 63, 122, 1, 121 },
                    { 64, 124, 2, 123 },
                    { 65, 126, 1, 125 },
                    { 66, 128, 1, 127 },
                    { 67, 130, 2, 129 },
                    { 68, 132, 2, 131 },
                    { 69, 134, 2, 133 },
                    { 70, 136, 2, 135 },
                    { 71, 138, 2, 137 },
                    { 72, 140, 2, 139 },
                    { 73, 142, 2, 141 },
                    { 74, 144, 1, 143 },
                    { 75, 146, 2, 145 },
                    { 76, 148, 2, 147 },
                    { 77, 150, 2, 149 },
                    { 78, 152, 2, 151 },
                    { 79, 154, 1, 153 },
                    { 80, 156, 1, 155 },
                    { 81, 158, 2, 157 },
                    { 82, 160, 2, 159 },
                    { 83, 162, 2, 161 }
                });

            migrationBuilder.InsertData(
                table: "WordPhonemes",
                columns: new[] { "Id", "PhonemeId", "Position", "WordId" },
                values: new object[,]
                {
                    { 1, 14, 1, 1 },
                    { 2, 10, 2, 1 },
                    { 3, 18, 3, 1 },
                    { 4, 19, 1, 2 },
                    { 5, 7, 2, 2 },
                    { 6, 31, 3, 2 },
                    { 7, 20, 4, 2 },
                    { 8, 13, 1, 3 },
                    { 9, 21, 2, 3 },
                    { 10, 2, 3, 3 },
                    { 11, 15, 4, 3 },
                    { 12, 3, 5, 3 },
                    { 13, 2, 6, 3 },
                    { 14, 9, 1, 4 },
                    { 15, 16, 2, 4 },
                    { 16, 3, 3, 4 },
                    { 17, 30, 4, 4 },
                    { 18, 2, 1, 5 },
                    { 19, 18, 2, 5 },
                    { 20, 3, 3, 5 },
                    { 21, 14, 4, 5 },
                    { 22, 10, 5, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rhymes_RhymeId",
                table: "Rhymes",
                column: "RhymeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rhymes_RhymeTypeId",
                table: "Rhymes",
                column: "RhymeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rhymes_WordId",
                table: "Rhymes",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordPhonemes_PhonemeId",
                table: "WordPhonemes",
                column: "PhonemeId");

            migrationBuilder.CreateIndex(
                name: "IX_WordPhonemes_WordId",
                table: "WordPhonemes",
                column: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rhymes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WordPhonemes");

            migrationBuilder.DropTable(
                name: "RhymeTypes");

            migrationBuilder.DropTable(
                name: "Phonemes");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
