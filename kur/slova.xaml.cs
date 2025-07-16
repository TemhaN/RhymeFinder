using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace kur
{
    public partial class slova : System.Windows.Controls.Page
    {
        private readonly Entities _entities;

        // Конструктор класса slova
        public slova()
        {
            InitializeComponent();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var options = new DbContextOptionsBuilder<Entities>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .Options;
            _entities = new Entities(options, configuration);
            Loaded += Slova_Loaded;
        }
        // Обработчик события загрузки страницы
        private async void Slova_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadWordsAsync();
        }
        // Метод для загрузки слов из базы данных
        private async Task LoadWordsAsync()
        {
            try
            {
                wordsgrid.ItemsSource = await _entities.Words.ToListAsync();
                txtStatus.Text = "Готов";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке слов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Ошибка при загрузке слов.";
            }
        }
        // Обработчик события добавления нового слова
        private async void BtnAddWord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, что поле ввода слова не пустое
                string newWord = txtNewWord.Text.Trim();
                if (string.IsNullOrEmpty(newWord))
                {
                    MessageBox.Show("Введите слово.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Проверяем, что слово состоит только из букв русского алфавита
                int? stressPosition = null;
                if (!string.IsNullOrEmpty(txtNewStressPosition.Text) && int.TryParse(txtNewStressPosition.Text, out int parsedStress))
                {
                    if (parsedStress >= 0 && parsedStress < newWord.Length)
                    {
                        stressPosition = parsedStress;
                    }
                    else
                    {
                        MessageBox.Show("Позиция ударения должна быть в пределах длины слова (начиная с 0).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                string newWordLower = newWord.ToLower();
                if (_entities.Words.Any(w => w.Word == newWordLower && w.StressPosition == stressPosition))
                {
                    MessageBox.Show("Слово с такой позицией ударения уже существует в базе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var word = new Words { Word = newWordLower, StressPosition = stressPosition };
                _entities.Words.Add(word);
                await _entities.SaveChangesAsync();
                // Получаем идентификатор нового слова для добавления фонем
                int wordId = word.Id;
                var phonemes = GetPhonemesForWord(newWordLower);
                for (int i = 0; i < phonemes.Count; i++)
                {
                    var wordPhoneme = new WordPhonemes { WordId = wordId, PhonemeId = phonemes[i].Id, Position = i };
                    _entities.WordPhonemes.Add(wordPhoneme);
                }
                // Сохраняем изменения в базе данных
                await _entities.SaveChangesAsync();
                await LoadWordsAsync();
                txtNewWord.Text = string.Empty;
                txtNewStressPosition.Text = string.Empty;
                txtStatus.Text = "Слово успешно добавлено.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении слова: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Ошибка при добавлении слова.";
            }
        }

        // Обработчик события редактирования ударения слова
        private async void BtnEditWord_Click(object sender, RoutedEventArgs e)
        {
            var selectedWord = (Words)((Button)sender).DataContext;
            if (selectedWord == null)
            {
                MessageBox.Show("Выберите слово для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Открываем диалоговое окно для ввода новой позиции ударения
            var input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Введите новую позицию ударения для слова '{selectedWord.Word}' (начиная с 0, оставьте пустым для удаления ударения):",
                "Редактирование ударения",
                selectedWord.StressPosition?.ToString() ?? ""
            );
            // Проверяем, что пользователь ввел корректное значение
            try
            {
                int? newStressPosition = null;
                if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int parsedStress))
                {
                    if (parsedStress >= 0 && parsedStress < selectedWord.Word.Length)
                    {
                        newStressPosition = parsedStress;
                    }
                    else
                    {
                        MessageBox.Show("Позиция ударения должна быть в пределах длины слова (начиная с 0).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                // Проверяем, что слово с такой позицией ударения уже не существует
                if (_entities.Words.Any(w => w.Word == selectedWord.Word && w.StressPosition == newStressPosition && w.Id != selectedWord.Id))
                {
                    MessageBox.Show("Слово с такой позицией ударения уже существует в базе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                selectedWord.StressPosition = newStressPosition;
                await _entities.SaveChangesAsync();
                await LoadWordsAsync();
                txtStatus.Text = "Ударение успешно обновлено.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании ударения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Ошибка при редактировании ударения.";
            }
        }
        // Обработчик события удаления слова
        private async void BtnDeleteWord_Click(object sender, RoutedEventArgs e)
        {
            var del = MessageBox.Show("Вы действительно хотите удалить это слово?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (del == MessageBoxResult.No)
                return;
            // Получаем выбранное слово из контекста кнопки
            var selectedWord = (Words)((Button)sender).DataContext;
            if (selectedWord == null)
            {
                MessageBox.Show("Выберите слово для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Проверяем, что слово не является пустым
            try
            {
                var wordPhonemes = await _entities.WordPhonemes.Where(wp => wp.WordId == selectedWord.Id).ToListAsync();
                _entities.WordPhonemes.RemoveRange(wordPhonemes);
                _entities.Words.Remove(selectedWord);
                await _entities.SaveChangesAsync();
                await LoadWordsAsync();
                txtStatus.Text = "Слово успешно удалено.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении слова: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Ошибка при удалении слова.";
            }
        }
        // Обработчик события импорта слов из файла
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Открываем диалоговое окно для выбора файла словаря
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Выберите файл словаря"
            };
            // Проверяем, что пользователь выбрал файл
            if (openFileDialog.ShowDialog() != true)
                return;
            // Проверяем, что файл не пустой
            string filePath = openFileDialog.FileName;
            try
            {
                // Читаем строки из файла и обрабатываем каждую строку
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length == 0)
                {
                    MessageBox.Show("Выбранный файл словаря пуст.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // Проверяем, что файл содержит корректные данные
                int insertedCount = 0;
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (string.IsNullOrEmpty(trimmedLine)) continue;
                    // Проверяем, что строка не является комментарием
                    var parts = trimmedLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0) continue;

                    string word = parts[0].ToLower();
                    int? stressPosition = null;
                    if (parts.Length > 1 && int.TryParse(parts[1], out int parsedStress))
                    {
                        // В примере "слон 3" ударение на третью букву, но позиция начинается с 0
                        if (parsedStress > 0 && parsedStress <= word.Length)
                        {
                            stressPosition = parsedStress - 1;
                        }
                        else
                        {
                            MessageBox.Show($"Некорректная позиция ударения для слова '{word}' ({parsedStress}). Пропускаем.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }
                    }

                    // Проверяем дубликат по слову и позиции ударения
                    if (_entities.Words.Any(w => w.Word == word && w.StressPosition == stressPosition))
                    {
                        MessageBox.Show($"Слово '{word}' с позицией ударения '{stressPosition}' уже существует. Пропускаем.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        continue;
                    }
                    var newWord = new Words
                    {
                        Word = word,
                        StressPosition = stressPosition
                    };
                    _entities.Words.Add(newWord);
                    await _entities.SaveChangesAsync();
                    // Получаем идентификатор нового слова для добавления фонем
                    int wordId = newWord.Id;
                    var phonemes = GetPhonemesForWord(word);
                    for (int i = 0; i < phonemes.Count; i++)
                    {
                        var wordPhoneme = new WordPhonemes { WordId = wordId, PhonemeId = phonemes[i].Id, Position = i };
                        _entities.WordPhonemes.Add(wordPhoneme);
                    }
                    insertedCount++;
                }
                // Сохраняем изменения в базе данных после добавления всех слов
                await _entities.SaveChangesAsync();
                await LoadWordsAsync();
                MessageBox.Show($"Успешно импортировано {insertedCount} слов.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                txtStatus.Text = $"Успешно импортировано {insertedCount} слов.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Ошибка при импорте данных.";
            }
        }
        // Метод для получения фонем для слова
        private List<Phonemes> GetPhonemesForWord(string word)
        {
            // Проверяем, что слово не пустое
            var phonemes = new List<Phonemes>();
            string wordUpper = word.ToUpper();
            foreach (char letter in wordUpper)
            {
                string letterString = letter.ToString();
                var phoneme = _entities.Phonemes.FirstOrDefault(p => p.Phoneme == letterString);
                if (phoneme != null)
                {
                    phonemes.Add(phoneme);
                }
                else
                {
                    txtStatus.Text = $"Фонема для буквы '{letterString}' не найдена.";
                }
            }
            return phonemes;
        }
    }
}