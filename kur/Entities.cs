using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;


namespace kur
{
    public class Entities : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Words> Words { get; set; }
        public DbSet<Rhymes> Rhymes { get; set; }
        public DbSet<RhymeTypes> RhymeTypes { get; set; }
        public DbSet<Phonemes> Phonemes { get; set; }
        public DbSet<WordPhonemes> WordPhonemes { get; set; }
        public Entities(DbContextOptions<Entities> options, IConfiguration configuration)
                    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Words>().ToTable("Words");
            modelBuilder.Entity<Rhymes>().ToTable("Rhymes");
            modelBuilder.Entity<RhymeTypes>().ToTable("RhymeTypes");
            modelBuilder.Entity<Phonemes>().ToTable("Phonemes");
            modelBuilder.Entity<WordPhonemes>().ToTable("WordPhonemes");

            modelBuilder.Entity<Rhymes>()
                .HasOne(r => r.Word)
                .WithMany()
                .HasForeignKey(r => r.WordId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rhymes>()
                .HasOne(r => r.Rhyme)
                .WithMany()
                .HasForeignKey(r => r.RhymeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rhymes>()
                .HasOne(r => r.RhymeType)
                .WithMany()
                .HasForeignKey(r => r.RhymeTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WordPhonemes>()
                .HasOne(wp => wp.Word)
                .WithMany()
                .HasForeignKey(wp => wp.WordId);

            modelBuilder.Entity<WordPhonemes>()
                .HasOne(wp => wp.Phoneme)
                .WithMany()
                .HasForeignKey(wp => wp.PhonemeId);

            base.OnModelCreating(modelBuilder);

            // --- RhymeTypes ---
            modelBuilder.Entity<RhymeTypes>().HasData(
                new RhymeTypes { Id = 1, RhymeName = "Точная" },
                new RhymeTypes { Id = 2, RhymeName = "Неточная" },
                new RhymeTypes { Id = 3, RhymeName = "Ассонанс" },
                new RhymeTypes { Id = 4, RhymeName = "Диссонанс" },
                new RhymeTypes { Id = 5, RhymeName = "Мужская" },
                new RhymeTypes { Id = 6, RhymeName = "Женская" },
                new RhymeTypes { Id = 7, RhymeName = "Дактилическая" },
                new RhymeTypes { Id = 8, RhymeName = "Гипердактилическая" },
                new RhymeTypes { Id = 9, RhymeName = "Богатая" },
                new RhymeTypes { Id = 10, RhymeName = "Бедная" },
                new RhymeTypes { Id = 11, RhymeName = "Составная" },
                new RhymeTypes { Id = 12, RhymeName = "Кольцевая" },
                new RhymeTypes { Id = 13, RhymeName = "Перекрестная" },
                new RhymeTypes { Id = 14, RhymeName = "Парная" },
                new RhymeTypes { Id = 15, RhymeName = "Охватная" },
                new RhymeTypes { Id = 16, RhymeName = "Свободная" },
                new RhymeTypes { Id = 17, RhymeName = "Внутренняя" },
                new RhymeTypes { Id = 18, RhymeName = "Начальная" },
                new RhymeTypes { Id = 19, RhymeName = "Конечная" },
                new RhymeTypes { Id = 20, RhymeName = "Полная" }
            );

            // --- Phonemes ---
            modelBuilder.Entity<Phonemes>().HasData(
                new Phonemes { Id = 1, Phoneme = "А" },
                new Phonemes { Id = 2, Phoneme = "Б" },
                new Phonemes { Id = 3, Phoneme = "В" },
                new Phonemes { Id = 4, Phoneme = "Г" },
                new Phonemes { Id = 5, Phoneme = "Д" },
                new Phonemes { Id = 6, Phoneme = "Е" },
                new Phonemes { Id = 7, Phoneme = "Ё" },
                new Phonemes { Id = 8, Phoneme = "Ж" },
                new Phonemes { Id = 9, Phoneme = "З" },
                new Phonemes { Id = 10, Phoneme = "И" },
                new Phonemes { Id = 11, Phoneme = "Й" },
                new Phonemes { Id = 12, Phoneme = "К" },
                new Phonemes { Id = 13, Phoneme = "Л" },
                new Phonemes { Id = 14, Phoneme = "М" },
                new Phonemes { Id = 15, Phoneme = "Н" },
                new Phonemes { Id = 16, Phoneme = "О" },
                new Phonemes { Id = 17, Phoneme = "П" },
                new Phonemes { Id = 18, Phoneme = "Р" },
                new Phonemes { Id = 19, Phoneme = "С" },
                new Phonemes { Id = 20, Phoneme = "Т" },
                new Phonemes { Id = 21, Phoneme = "У" },
                new Phonemes { Id = 22, Phoneme = "Ф" },
                new Phonemes { Id = 23, Phoneme = "Х" },
                new Phonemes { Id = 24, Phoneme = "Ц" },
                new Phonemes { Id = 25, Phoneme = "Ч" },
                new Phonemes { Id = 26, Phoneme = "Ш" },
                new Phonemes { Id = 27, Phoneme = "Щ" },
                new Phonemes { Id = 28, Phoneme = "Ъ" },
                new Phonemes { Id = 29, Phoneme = "Ы" },
                new Phonemes { Id = 30, Phoneme = "Ь" },
                new Phonemes { Id = 31, Phoneme = "Э" },
                new Phonemes { Id = 32, Phoneme = "Ю" },
                new Phonemes { Id = 33, Phoneme = "Я" }
            );

            // --- Users ---
            modelBuilder.Entity<Users>().HasData(
                new Users { Id = 1, Login = "admin", Password = "admin", Role = 1 }
            );

            // --- Words ---
			modelBuilder.Entity<Words>().HasData(
                new Words { Id = 1, Word = "любовь", StressPosition = 2 },
                new Words { Id = 2, Word = "кровь", StressPosition = 1 },
                new Words { Id = 3, Word = "снова", StressPosition = 1 },
                new Words { Id = 4, Word = "слова", StressPosition = 2 },
                new Words { Id = 5, Word = "мечта", StressPosition = 2 },
                new Words { Id = 6, Word = "света", StressPosition = 1 },
                new Words { Id = 7, Word = "звезда", StressPosition = 2 },
                new Words { Id = 8, Word = "вода", StressPosition = 2 },
                new Words { Id = 9, Word = "огонь", StressPosition = 2 },
                new Words { Id = 10, Word = "конь", StressPosition = 1 },
                new Words { Id = 11, Word = "день", StressPosition = 1 },
                new Words { Id = 12, Word = "тень", StressPosition = 1 },
                new Words { Id = 13, Word = "путь", StressPosition = 1 },
                new Words { Id = 14, Word = "грудь", StressPosition = 1 },
                new Words { Id = 15, Word = "друг", StressPosition = 1 },
                new Words { Id = 16, Word = "круг", StressPosition = 1 },
                new Words { Id = 17, Word = "рука", StressPosition = 2 },
                new Words { Id = 18, Word = "река", StressPosition = 2 },
                new Words { Id = 19, Word = "душа", StressPosition = 2 },
                new Words { Id = 20, Word = "тишина", StressPosition = 3 },
                new Words { Id = 21, Word = "весна", StressPosition = 2 },
                new Words { Id = 22, Word = "зима", StressPosition = 2 },
                new Words { Id = 23, Word = "луна", StressPosition = 2 },
                new Words { Id = 24, Word = "волна", StressPosition = 2 },
                new Words { Id = 25, Word = "судьба", StressPosition = 2 },
                new Words { Id = 26, Word = "тоска", StressPosition = 2 },
                new Words { Id = 27, Word = "песня", StressPosition = 1 },
                new Words { Id = 28, Word = "травка", StressPosition = 1 },
                new Words { Id = 29, Word = "сердце", StressPosition = 1 },
                new Words { Id = 30, Word = "солнце", StressPosition = 1 },
                new Words { Id = 31, Word = "небо", StressPosition = 1 },
                new Words { Id = 32, Word = "ветер", StressPosition = 1 },
                new Words { Id = 33, Word = "пламя", StressPosition = 1 },
                new Words { Id = 34, Word = "знамя", StressPosition = 1 },
                new Words { Id = 35, Word = "поле", StressPosition = 1 },
                new Words { Id = 36, Word = "море", StressPosition = 1 },
                new Words { Id = 37, Word = "горе", StressPosition = 1 },
                new Words { Id = 38, Word = "радость", StressPosition = 1 },
                new Words { Id = 39, Word = "счастье", StressPosition = 1 },
                new Words { Id = 40, Word = "жизнь", StressPosition = 1 },
                new Words { Id = 41, Word = "смерть", StressPosition = 1 },
                new Words { Id = 42, Word = "свет", StressPosition = 1 },
                new Words { Id = 43, Word = "цвет", StressPosition = 1 },
                new Words { Id = 44, Word = "дом", StressPosition = 1 },
                new Words { Id = 45, Word = "сом", StressPosition = 1 },
                new Words { Id = 46, Word = "лес", StressPosition = 1 },
                new Words { Id = 47, Word = "вес", StressPosition = 1 },
                new Words { Id = 48, Word = "сон", StressPosition = 1 },
                new Words { Id = 49, Word = "тон", StressPosition = 1 },
                new Words { Id = 50, Word = "мир", StressPosition = 1 },
                new Words { Id = 51, Word = "птица", StressPosition = 1 },
                new Words { Id = 52, Word = "улица", StressPosition = 1 },
                new Words { Id = 53, Word = "дверь", StressPosition = 1 },
                new Words { Id = 54, Word = "тень", StressPosition = 1 },
                new Words { Id = 55, Word = "воля", StressPosition = 1 },
                new Words { Id = 56, Word = "доля", StressPosition = 1 },
                new Words { Id = 57, Word = "земля", StressPosition = 2 },
                new Words { Id = 58, Word = "семья", StressPosition = 2 },
                new Words { Id = 59, Word = "книга", StressPosition = 1 },
                new Words { Id = 60, Word = "лига", StressPosition = 1 },
                new Words { Id = 61, Word = "вера", StressPosition = 1 },
                new Words { Id = 62, Word = "мера", StressPosition = 1 },
                new Words { Id = 63, Word = "слава", StressPosition = 1 },
                new Words { Id = 64, Word = "пава", StressPosition = 1 },
                new Words { Id = 65, Word = "тропа", StressPosition = 2 },
                new Words { Id = 66, Word = "европа", StressPosition = 2 },
                new Words { Id = 67, Word = "дерево", StressPosition = 1 },
                new Words { Id = 68, Word = "пламя", StressPosition = 1 },
                new Words { Id = 69, Word = "камни", StressPosition = 1 },
                new Words { Id = 70, Word = "дни", StressPosition = 1 },
                new Words { Id = 71, Word = "звери", StressPosition = 1 },
                new Words { Id = 72, Word = "двери", StressPosition = 1 },
                new Words { Id = 73, Word = "песок", StressPosition = 2 },
                new Words { Id = 74, Word = "носок", StressPosition = 2 },
                new Words { Id = 75, Word = "берег", StressPosition = 1 },
                new Words { Id = 76, Word = "ковчег", StressPosition = 2 },
                new Words { Id = 77, Word = "луг", StressPosition = 1 },
                new Words { Id = 78, Word = "друг", StressPosition = 1 },
                new Words { Id = 79, Word = "снег", StressPosition = 1 },
                new Words { Id = 80, Word = "век", StressPosition = 1 },
                new Words { Id = 81, Word = "цветок", StressPosition = 2 },
                new Words { Id = 82, Word = "поток", StressPosition = 2 },
                new Words { Id = 83, Word = "голос", StressPosition = 1 },
                new Words { Id = 84, Word = "колос", StressPosition = 1 },
                new Words { Id = 85, Word = "взгляд", StressPosition = 1 },
                new Words { Id = 86, Word = "сад", StressPosition = 1 },
                new Words { Id = 87, Word = "меч", StressPosition = 1 },
                new Words { Id = 88, Word = "печь", StressPosition = 1 },
                new Words { Id = 89, Word = "песнь", StressPosition = 1 },
                new Words { Id = 90, Word = "честность", StressPosition = 1 },
                new Words { Id = 91, Word = "дорога", StressPosition = 2 },
                new Words { Id = 92, Word = "нога", StressPosition = 2 },
                new Words { Id = 93, Word = "береза", StressPosition = 2 },
                new Words { Id = 94, Word = "слеза", StressPosition = 2 },
                new Words { Id = 95, Word = "сила", StressPosition = 1 },
                new Words { Id = 96, Word = "мила", StressPosition = 2 },
                new Words { Id = 97, Word = "свобода", StressPosition = 2 },
                new Words { Id = 98, Word = "природа", StressPosition = 2 },
                new Words { Id = 99, Word = "победа", StressPosition = 2 },
                new Words { Id = 100, Word = "беда", StressPosition = 2 },
                new Words { Id = 101, Word = "человек", StressPosition = 3 },
                new Words { Id = 102, Word = "век", StressPosition = 1 },
                new Words { Id = 103, Word = "любить", StressPosition = 2 },
                new Words { Id = 104, Word = "простить", StressPosition = 2 },
                new Words { Id = 105, Word = "лететь", StressPosition = 2 },
                new Words { Id = 106, Word = "смотреть", StressPosition = 2 },
                new Words { Id = 107, Word = "идти", StressPosition = 2 },
                new Words { Id = 108, Word = "нести", StressPosition = 2 },
                new Words { Id = 109, Word = "петь", StressPosition = 1 },
                new Words { Id = 110, Word = "светить", StressPosition = 2 },
                new Words { Id = 111, Word = "ждать", StressPosition = 1 },
                new Words { Id = 112, Word = "искать", StressPosition = 2 },
                new Words { Id = 113, Word = "мечтать", StressPosition = 2 },
                new Words { Id = 114, Word = "страдать", StressPosition = 2 },
                new Words { Id = 115, Word = "гореть", StressPosition = 2 },
                new Words { Id = 116, Word = "творец", StressPosition = 2 },
                new Words { Id = 117, Word = "бежать", StressPosition = 2 },
                new Words { Id = 118, Word = "лежать", StressPosition = 2 },
                new Words { Id = 119, Word = "кричать", StressPosition = 2 },
                new Words { Id = 120, Word = "молчать", StressPosition = 2 },
                new Words { Id = 121, Word = "дышать", StressPosition = 2 },
                new Words { Id = 122, Word = "писать", StressPosition = 2 },
                new Words { Id = 123, Word = "читать", StressPosition = 2 },
                new Words { Id = 124, Word = "звать", StressPosition = 1 },
                new Words { Id = 125, Word = "дать", StressPosition = 1 },
                new Words { Id = 126, Word = "взять", StressPosition = 1 },
                new Words { Id = 127, Word = "знать", StressPosition = 1 },
                new Words { Id = 128, Word = "спать", StressPosition = 1 },
                new Words { Id = 129, Word = "стоять", StressPosition = 2 },
                new Words { Id = 130, Word = "гулять", StressPosition = 2 },
                new Words { Id = 131, Word = "смеять", StressPosition = 2 },
                new Words { Id = 132, Word = "плакать", StressPosition = 1 },
                new Words { Id = 133, Word = "бросать", StressPosition = 2 },
                new Words { Id = 134, Word = "росать", StressPosition = 1 },
                new Words { Id = 135, Word = "видеть", StressPosition = 1 },
                new Words { Id = 136, Word = "сидеть", StressPosition = 2 },
                new Words { Id = 137, Word = "держать", StressPosition = 2 },
                new Words { Id = 138, Word = "рожать", StressPosition = 2 },
                new Words { Id = 139, Word = "думать", StressPosition = 1 },
                new Words { Id = 140, Word = "просить", StressPosition = 2 },
                new Words { Id = 141, Word = "хитрить", StressPosition = 2 },
                new Words { Id = 142, Word = "творить", StressPosition = 2 },
                new Words { Id = 143, Word = "бродить", StressPosition = 2 },
                new Words { Id = 144, Word = "водить", StressPosition = 2 },
                new Words { Id = 145, Word = "сверкать", StressPosition = 2 },
                new Words { Id = 146, Word = "меркать", StressPosition = 1 },
                new Words { Id = 147, Word = "шагать", StressPosition = 2 },
                new Words { Id = 148, Word = "бегать", StressPosition = 1 },
                new Words { Id = 149, Word = "плавать", StressPosition = 1 },
                new Words { Id = 150, Word = "править", StressPosition = 1 },
                new Words { Id = 151, Word = "славить", StressPosition = 1 },
                new Words { Id = 152, Word = "ловить", StressPosition = 2 },
                new Words { Id = 153, Word = "свободный", StressPosition = 2 },
                new Words { Id = 154, Word = "родной", StressPosition = 2 },
                new Words { Id = 155, Word = "высокий", StressPosition = 2 },
                new Words { Id = 156, Word = "глубокий", StressPosition = 2 },
                new Words { Id = 157, Word = "ясный", StressPosition = 1 },
                new Words { Id = 158, Word = "красный", StressPosition = 1 },
                new Words { Id = 159, Word = "чистый", StressPosition = 1 },
                new Words { Id = 160, Word = "простой", StressPosition = 2 },
                new Words { Id = 161, Word = "светлый", StressPosition = 1 },
                new Words { Id = 162, Word = "летний", StressPosition = 1 },
                new Words { Id = 163, Word = "зимний", StressPosition = 1 },
                new Words { Id = 164, Word = "весенний", StressPosition = 2 },
                new Words { Id = 165, Word = "осенний", StressPosition = 2 },
                new Words { Id = 166, Word = "далекий", StressPosition = 2 },
                new Words { Id = 167, Word = "близкий", StressPosition = 1 },
                new Words { Id = 168, Word = "горячий", StressPosition = 2 },
                new Words { Id = 169, Word = "холодный", StressPosition = 1 },
                new Words { Id = 170, Word = "мудрый", StressPosition = 1 },
                new Words { Id = 171, Word = "добрый", StressPosition = 1 },
                new Words { Id = 172, Word = "смелый", StressPosition = 1 },
                new Words { Id = 173, Word = "верный", StressPosition = 1 },
                new Words { Id = 174, Word = "честный", StressPosition = 1 },
                new Words { Id = 175, Word = "грустный", StressPosition = 1 },
                new Words { Id = 176, Word = "веселый", StressPosition = 2 },
                new Words { Id = 177, Word = "пустой", StressPosition = 2 },
                new Words { Id = 178, Word = "крепкий", StressPosition = 1 },
                new Words { Id = 179, Word = "цепкий", StressPosition = 1 },
                new Words { Id = 180, Word = "быстрый", StressPosition = 1 },
                new Words { Id = 181, Word = "острый", StressPosition = 1 },
                new Words { Id = 182, Word = "мягкий", StressPosition = 1 },
                new Words { Id = 183, Word = "гладкий", StressPosition = 1 },
                new Words { Id = 184, Word = "шаткий", StressPosition = 1 },
                new Words { Id = 185, Word = "рваный", StressPosition = 1 },
                new Words { Id = 186, Word = "прямой", StressPosition = 2 },
                new Words { Id = 187, Word = "кривой", StressPosition = 2 },
                new Words { Id = 188, Word = "живой", StressPosition = 2 },
                new Words { Id = 189, Word = "свежий", StressPosition = 1 },
                new Words { Id = 190, Word = "нежный", StressPosition = 1 },
                new Words { Id = 191, Word = "белый", StressPosition = 1 },
                new Words { Id = 192, Word = "синий", StressPosition = 1 },
                new Words { Id = 193, Word = "зеленый", StressPosition = 3 },
                new Words { Id = 194, Word = "серый", StressPosition = 1 },
                new Words { Id = 195, Word = "черный", StressPosition = 1 },
                new Words { Id = 196, Word = "яркий", StressPosition = 1 },
                new Words { Id = 197, Word = "темный", StressPosition = 1 },
                new Words { Id = 198, Word = "бледный", StressPosition = 1 },
                new Words { Id = 199, Word = "громкий", StressPosition = 2 },
                new Words { Id = 200, Word = "тонкий", StressPosition = 1 }
            );

            // --- WordPhonemes ---
            modelBuilder.Entity<WordPhonemes>().HasData(
                // мир
                new WordPhonemes { Id = 1, WordId = 1, PhonemeId = 14, Position = 1 },
                new WordPhonemes { Id = 2, WordId = 1, PhonemeId = 10, Position = 2 },
                new WordPhonemes { Id = 3, WordId = 1, PhonemeId = 18, Position = 3 },

                // свет
                new WordPhonemes { Id = 4, WordId = 2, PhonemeId = 19, Position = 1 },
                new WordPhonemes { Id = 5, WordId = 2, PhonemeId = 7, Position = 2 },
                new WordPhonemes { Id = 6, WordId = 2, PhonemeId = 31, Position = 3 },
                new WordPhonemes { Id = 7, WordId = 2, PhonemeId = 20, Position = 4 },

                // любовь
                new WordPhonemes { Id = 8, WordId = 3, PhonemeId = 13, Position = 1 },
                new WordPhonemes { Id = 9, WordId = 3, PhonemeId = 21, Position = 2 },
                new WordPhonemes { Id = 10, WordId = 3, PhonemeId = 2, Position = 3 },
                new WordPhonemes { Id = 11, WordId = 3, PhonemeId = 15, Position = 4 },
                new WordPhonemes { Id = 12, WordId = 3, PhonemeId = 3, Position = 5 },
                new WordPhonemes { Id = 13, WordId = 3, PhonemeId = 2, Position = 6 },

                // жизнь
                new WordPhonemes { Id = 14, WordId = 4, PhonemeId = 9, Position = 1 },
                new WordPhonemes { Id = 15, WordId = 4, PhonemeId = 16, Position = 2 },
                new WordPhonemes { Id = 16, WordId = 4, PhonemeId = 3, Position = 3 },
                new WordPhonemes { Id = 17, WordId = 4, PhonemeId = 30, Position = 4 },

                // время
                new WordPhonemes { Id = 18, WordId = 5, PhonemeId = 2, Position = 1 },
                new WordPhonemes { Id = 19, WordId = 5, PhonemeId = 18, Position = 2 },
                new WordPhonemes { Id = 20, WordId = 5, PhonemeId = 3, Position = 3 },
                new WordPhonemes { Id = 21, WordId = 5, PhonemeId = 14, Position = 4 },
                new WordPhonemes { Id = 22, WordId = 5, PhonemeId = 10, Position = 5 }
            );

            // --- Rhymes ---
            modelBuilder.Entity<Rhymes>().HasData(
                new Rhymes { Id = 1, WordId = 1, RhymeId = 2, RhymeTypeId = 1 },
                new Rhymes { Id = 2, WordId = 3, RhymeId = 4, RhymeTypeId = 1 },
                new Rhymes { Id = 3, WordId = 5, RhymeId = 6, RhymeTypeId = 2 },
                new Rhymes { Id = 4, WordId = 7, RhymeId = 8, RhymeTypeId = 1 },
                new Rhymes { Id = 5, WordId = 9, RhymeId = 10, RhymeTypeId = 1 },
                new Rhymes { Id = 6, WordId = 11, RhymeId = 12, RhymeTypeId = 1 },
                new Rhymes { Id = 7, WordId = 13, RhymeId = 14, RhymeTypeId = 1 },
                new Rhymes { Id = 8, WordId = 15, RhymeId = 16, RhymeTypeId = 1 },
                new Rhymes { Id = 9, WordId = 17, RhymeId = 18, RhymeTypeId = 1 },
                new Rhymes { Id = 10, WordId = 19, RhymeId = 20, RhymeTypeId = 2 },
                new Rhymes { Id = 11, WordId = 21, RhymeId = 22, RhymeTypeId = 3 },
                new Rhymes { Id = 12, WordId = 23, RhymeId = 24, RhymeTypeId = 1 },
                new Rhymes { Id = 13, WordId = 25, RhymeId = 26, RhymeTypeId = 2 },
                new Rhymes { Id = 14, WordId = 27, RhymeId = 28, RhymeTypeId = 2 },
                new Rhymes { Id = 15, WordId = 29, RhymeId = 30, RhymeTypeId = 3 },
                new Rhymes { Id = 16, WordId = 31, RhymeId = 32, RhymeTypeId = 2 },
                new Rhymes { Id = 17, WordId = 33, RhymeId = 34, RhymeTypeId = 1 },
                new Rhymes { Id = 18, WordId = 35, RhymeId = 36, RhymeTypeId = 2 },
                new Rhymes { Id = 19, WordId = 37, RhymeId = 36, RhymeTypeId = 1 },
                new Rhymes { Id = 20, WordId = 38, RhymeId = 39, RhymeTypeId = 2 },
                new Rhymes { Id = 21, WordId = 40, RhymeId = 41, RhymeTypeId = 3 },
                new Rhymes { Id = 22, WordId = 42, RhymeId = 43, RhymeTypeId = 1 },
                new Rhymes { Id = 23, WordId = 44, RhymeId = 45, RhymeTypeId = 1 },
                new Rhymes { Id = 24, WordId = 46, RhymeId = 47, RhymeTypeId = 1 },
                new Rhymes { Id = 25, WordId = 48, RhymeId = 49, RhymeTypeId = 1 },
                new Rhymes { Id = 26, WordId = 50, RhymeId = 51, RhymeTypeId = 2 },
                new Rhymes { Id = 27, WordId = 52, RhymeId = 53, RhymeTypeId = 1 },
                new Rhymes { Id = 28, WordId = 54, RhymeId = 55, RhymeTypeId = 1 },
                new Rhymes { Id = 29, WordId = 56, RhymeId = 57, RhymeTypeId = 1 },
                new Rhymes { Id = 30, WordId = 58, RhymeId = 59, RhymeTypeId = 1 },
                new Rhymes { Id = 31, WordId = 60, RhymeId = 61, RhymeTypeId = 1 },
                new Rhymes { Id = 32, WordId = 62, RhymeId = 63, RhymeTypeId = 1 },
                new Rhymes { Id = 33, WordId = 64, RhymeId = 65, RhymeTypeId = 1 },
                new Rhymes { Id = 34, WordId = 66, RhymeId = 33, RhymeTypeId = 2 },
                new Rhymes { Id = 35, WordId = 67, RhymeId = 68, RhymeTypeId = 1 },
                new Rhymes { Id = 36, WordId = 69, RhymeId = 70, RhymeTypeId = 1 },
                new Rhymes { Id = 37, WordId = 71, RhymeId = 72, RhymeTypeId = 1 },
                new Rhymes { Id = 38, WordId = 73, RhymeId = 74, RhymeTypeId = 1 },
                new Rhymes { Id = 39, WordId = 75, RhymeId = 15, RhymeTypeId = 1 },
                new Rhymes { Id = 40, WordId = 76, RhymeId = 77, RhymeTypeId = 1 },
                new Rhymes { Id = 41, WordId = 78, RhymeId = 79, RhymeTypeId = 1 },
                new Rhymes { Id = 42, WordId = 80, RhymeId = 81, RhymeTypeId = 1 },
                new Rhymes { Id = 43, WordId = 82, RhymeId = 83, RhymeTypeId = 1 },
                new Rhymes { Id = 44, WordId = 84, RhymeId = 85, RhymeTypeId = 2 },
                new Rhymes { Id = 45, WordId = 86, RhymeId = 87, RhymeTypeId = 1 },
                new Rhymes { Id = 46, WordId = 88, RhymeId = 89, RhymeTypeId = 1 },
                new Rhymes { Id = 47, WordId = 90, RhymeId = 91, RhymeTypeId = 1 },
                new Rhymes { Id = 48, WordId = 92, RhymeId = 93, RhymeTypeId = 1 },
                new Rhymes { Id = 49, WordId = 94, RhymeId = 95, RhymeTypeId = 2 },
                new Rhymes { Id = 50, WordId = 96, RhymeId = 77, RhymeTypeId = 2 },
                new Rhymes { Id = 51, WordId = 97, RhymeId = 98, RhymeTypeId = 1 },
                new Rhymes { Id = 52, WordId = 99, RhymeId = 100, RhymeTypeId = 2 },
                new Rhymes { Id = 53, WordId = 101, RhymeId = 102, RhymeTypeId = 1 },
                new Rhymes { Id = 54, WordId = 103, RhymeId = 104, RhymeTypeId = 2 },
                new Rhymes { Id = 55, WordId = 105, RhymeId = 106, RhymeTypeId = 1 },
                new Rhymes { Id = 56, WordId = 107, RhymeId = 108, RhymeTypeId = 1 },
                new Rhymes { Id = 57, WordId = 109, RhymeId = 110, RhymeTypeId = 1 },
                new Rhymes { Id = 58, WordId = 111, RhymeId = 112, RhymeTypeId = 1 },
                new Rhymes { Id = 59, WordId = 113, RhymeId = 114, RhymeTypeId = 1 },
                new Rhymes { Id = 60, WordId = 115, RhymeId = 116, RhymeTypeId = 2 },
                new Rhymes { Id = 61, WordId = 117, RhymeId = 118, RhymeTypeId = 1 },
                new Rhymes { Id = 62, WordId = 119, RhymeId = 120, RhymeTypeId = 1 },
                new Rhymes { Id = 63, WordId = 121, RhymeId = 122, RhymeTypeId = 1 },
                new Rhymes { Id = 64, WordId = 123, RhymeId = 124, RhymeTypeId = 2 },
                new Rhymes { Id = 65, WordId = 125, RhymeId = 126, RhymeTypeId = 1 },
                new Rhymes { Id = 66, WordId = 127, RhymeId = 128, RhymeTypeId = 1 },
                new Rhymes { Id = 67, WordId = 129, RhymeId = 130, RhymeTypeId = 2 },
                new Rhymes { Id = 68, WordId = 131, RhymeId = 132, RhymeTypeId = 2 },
                new Rhymes { Id = 69, WordId = 133, RhymeId = 134, RhymeTypeId = 2 },
                new Rhymes { Id = 70, WordId = 135, RhymeId = 136, RhymeTypeId = 2 },
                new Rhymes { Id = 71, WordId = 137, RhymeId = 138, RhymeTypeId = 2 },
                new Rhymes { Id = 72, WordId = 139, RhymeId = 140, RhymeTypeId = 2 },
                new Rhymes { Id = 73, WordId = 141, RhymeId = 142, RhymeTypeId = 2 },
                new Rhymes { Id = 74, WordId = 143, RhymeId = 144, RhymeTypeId = 1 },
                new Rhymes { Id = 75, WordId = 145, RhymeId = 146, RhymeTypeId = 2 },
                new Rhymes { Id = 76, WordId = 147, RhymeId = 148, RhymeTypeId = 2 },
                new Rhymes { Id = 77, WordId = 149, RhymeId = 150, RhymeTypeId = 2 },
                new Rhymes { Id = 78, WordId = 151, RhymeId = 152, RhymeTypeId = 2 },
                new Rhymes { Id = 79, WordId = 153, RhymeId = 154, RhymeTypeId = 1 },
                new Rhymes { Id = 80, WordId = 155, RhymeId = 156, RhymeTypeId = 1 },
                new Rhymes { Id = 81, WordId = 157, RhymeId = 158, RhymeTypeId = 2 },
                new Rhymes { Id = 82, WordId = 159, RhymeId = 160, RhymeTypeId = 2 },
                new Rhymes { Id = 83, WordId = 161, RhymeId = 162, RhymeTypeId = 2 }
            );
        }
    }

    public class Users
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
    }

    public class Words
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int? StressPosition { get; set; }
    }

    public class Rhymes
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public int RhymeId { get; set; }
        public int RhymeTypeId { get; set; }
        public Words Word { get; set; }
        public Words Rhyme { get; set; }
        public RhymeTypes RhymeType { get; set; }
    }

    public class RhymeTypes
    {
        public int Id { get; set; }
        public string RhymeName { get; set; }
    }

    public class Phonemes
    {
        public int Id { get; set; }
        public string Phoneme { get; set; }
    }

    public class WordPhonemes
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public int PhonemeId { get; set; }
        public int Position { get; set; }
        public Words Word { get; set; }
        public Phonemes Phoneme { get; set; }
    }
}
