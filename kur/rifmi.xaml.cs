using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace kur
{
    public partial class rifmi : Page
    {
        private readonly Entities _entities;
        private Words _selectedWord;
        private Words _selectedRhyme;

        // Конструктор класса rifmi
        public rifmi()
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
            Loaded += Rifmi_Loaded;
        }
        // Обработчик события загрузки страницы
        private async void Rifmi_Loaded(object sender, RoutedEventArgs e)
        {
            cmbRhymeType.ItemsSource = await _entities.RhymeTypes.ToListAsync();
            await LoadRhymesAsync();
        }

        // Метод для загрузки рифм из базы данных
        private async Task LoadRhymesAsync()
        {
            var rhymes = await _entities.Rhymes
                .Select(r => new RhymeDisplay
                {
                    RhymeId = r.Id,
                    Word = r.Word.Word,
                    Rhyme = r.Rhyme.Word,
                    RhymeType = r.RhymeType.RhymeName
                })
                .ToListAsync();
            dgRhymes.ItemsSource = rhymes;
        }
        // Обработчик события изменения текста в поле поиска слов
        private void TxtWordSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем текст из поля поиска и преобразуем его в нижний регистр
            string searchText = txtWordSearch.Text.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                lbWordResults.ItemsSource = null;
                lbWordResults.Visibility = Visibility.Collapsed;
                return;
            }
            // Выполняем поиск слов, содержащих введенный текст
            var results = _entities.Words
                .Where(w => w.Word.ToLower().Contains(searchText))
                .ToList();
            lbWordResults.ItemsSource = results;
            lbWordResults.Visibility = Visibility.Visible;
        }

        // Обработчик события изменения выбранного слова в списке результатов
        private async void LbWordResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранное слово из списка результатов
            _selectedWord = (Words)lbWordResults.SelectedItem;
            lbWordResults.Visibility = Visibility.Collapsed;
            if (_selectedWord != null)
            {
                txtWordSearch.Text = _selectedWord.Word;
                UpdateRhymeSuggestions();
                await LoadRhymesAsync();
            }
        }

        // Обработчик события изменения текста в поле поиска рифм
        private void TxtRhymeSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtRhymeSearch.Text.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                lbRhymeResults.ItemsSource = null;
                lbRhymeResults.Visibility = Visibility.Collapsed;
                return;
            }
            // Выполняем поиск рифм, содержащих введенный текст
            var results = _entities.Words
                .Where(w => w.Word.ToLower().Contains(searchText))
                .ToList();
            lbRhymeResults.ItemsSource = results;
            lbRhymeResults.Visibility = Visibility.Visible;
        }
        // Обработчик события изменения выбранной рифмы в списке результатов
        private async void LbRhymeResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRhyme = (Words)lbRhymeResults.SelectedItem;
            lbRhymeResults.Visibility = Visibility.Collapsed;
            if (_selectedRhyme != null)
            {
                txtRhymeSearch.Text = _selectedRhyme.Word;
                await LoadRhymesAsync();
            }
        }
        // Обработчик события нажатия кнопки "Добавить рифму"
        private async void BtnAddRhyme_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedWord == null || _selectedRhyme == null)
            {
                MessageBox.Show("Выберите слово и рифму.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Проверяем, что выбран тип рифмы

            var selectedRhymeType = (RhymeTypes)cmbRhymeType.SelectedItem;
            if (selectedRhymeType == null)
            {
                MessageBox.Show("Выберите тип рифмы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Создаем новую рифму и добавляем ее в базу данных
                var newRhyme = new Rhymes
                {
                    WordId = _selectedWord.Id,
                    RhymeId = _selectedRhyme.Id,
                    RhymeTypeId = selectedRhymeType.Id
                };
                _entities.Rhymes.Add(newRhyme);
                await _entities.SaveChangesAsync();
                await LoadRhymesAsync();
                MessageBox.Show("Рифма успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении рифмы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Обработчик события изменения выбранной рифмы в таблице

        private void DgRhymes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, что выбран элемент в таблице рифм
            var selectedRhyme = (RhymeDisplay)dgRhymes.SelectedItem;
            txtSelectedRhyme.Text = selectedRhyme != null
                ? $"Слово: {selectedRhyme.Word}, Рифма: {selectedRhyme.Rhyme}"
                : string.Empty;
        }
        // Обработчик события нажатия кнопки "Удалить рифму"
        private async void BtnDeleteRhyme_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что выбран элемент в таблице рифм
            var selectedRhyme = (RhymeDisplay)dgRhymes.SelectedItem;
            if (selectedRhyme == null)
            {
                MessageBox.Show("Выберите рифму для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Подтверждаем удаление рифмы

            var result = MessageBox.Show($"Вы уверены, что хотите удалить рифму: {selectedRhyme.Word} - {selectedRhyme.Rhyme}?",
                                        "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                var rhymeToDelete = await _entities.Rhymes.FirstOrDefaultAsync(r => r.Id == selectedRhyme.RhymeId);
                // Проверяем, существует ли рифма для удаления
                if (rhymeToDelete != null)
                {
                    _entities.Rhymes.Remove(rhymeToDelete);
                    await _entities.SaveChangesAsync();
                    await LoadRhymesAsync();
                    txtSelectedRhyme.Text = string.Empty;
                    MessageBox.Show("Рифма успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Рифма не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении рифмы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Обработчик события нажатия кнопки "Экспорт рифм"

        private void BtnExportRhyme_Click(object sender, RoutedEventArgs e)
        {
            var rhymesToExport = dgRhymes.ItemsSource as List<RhymeDisplay>;
            if (rhymesToExport == null || rhymesToExport.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            // Открываем диалоговое окно для сохранения файла
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Документ Word (*.docx)|*.docx|Текстовый файл (*.txt)|*.txt",
                Title = "Сохранить рифмы"
            };
            // Проверяем, выбрал ли пользователь файл для сохранения

            if (saveFileDialog.ShowDialog() != true)
                return;

            try
            {
                // Получаем путь к файлу и проверяем его расширение
                string filePath = saveFileDialog.FileName;
                if (filePath.EndsWith(".docx"))
                {
                    using (var doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                    {
                        // Создаем основной документ и добавляем в него таблицу
                        var mainPart = doc.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        var body = mainPart.Document.AppendChild(new Body());
                        var table = body.AppendChild(new Table());

                        var tableProperties = new TableProperties(
                            new TableBorders(
                                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                            )
                        );
                        table.AppendChild(tableProperties);
                        // Создаем заголовок таблицы и добавляем его в таблицу
                        var headerRow = table.AppendChild(new TableRow());
                        headerRow.Append(
                            CreateCell("ID", true),
                            CreateCell("Слово", true),
                            CreateCell("Рифма", true),
                            CreateCell("Тип рифмы", true)
                        );
                        // Добавляем данные рифм в таблицу
                        foreach (var rhyme in rhymesToExport)
                        {
                            var row = table.AppendChild(new TableRow());
                            row.Append(
                                CreateCell(rhyme.RhymeId.ToString(), false),
                                CreateCell(rhyme.Word, false),
                                CreateCell(rhyme.Rhyme, false),
                                CreateCell(rhyme.RhymeType, false)
                            );
                        }
                    }
                }
                // Если выбран текстовый файл, записываем данные в текстовый файл
                else if (filePath.EndsWith(".txt"))
                {
                    using (var writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine("ID\tСлово\tРифма\tТип рифмы");
                        foreach (var rhyme in rhymesToExport)
                        {
                            writer.WriteLine($"{rhyme.RhymeId}\t{rhyme.Word}\t{rhyme.Rhyme}\t{rhyme.RhymeType}");
                        }
                    }
                }
                MessageBox.Show("Данные успешно экспортированы.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Метод для создания ячейки таблицы с текстом
        private TableCell CreateCell(string text, bool isBold)
        {
            var cell = new TableCell();
            var paragraph = cell.AppendChild(new Paragraph());
            var run = paragraph.AppendChild(new Run());
            if (isBold)
            {
                run.RunProperties = new RunProperties(new Bold());
            }
            run.AppendChild(new Text(text));
            return cell;
        }

        // Метод для поиска рифм по окончанию слова
        private List<Words> FindRhymesByEnding(string word, int endingLength = 3)
        {
            if (string.IsNullOrEmpty(word) || word.Length < endingLength)
                return new List<Words>();

            string wordEnding = word.Substring(word.Length - endingLength).ToLower();
            return _entities.Words
                .Where(w => w.Word != word && w.Word.ToLower().EndsWith(wordEnding))
                .ToList();
        }

        // Метод для поиска рифм по позиции ударения
        private List<Words> FindRhymesByStress(int? stressPosition)
        {
            if (!stressPosition.HasValue)
                return new List<Words>();

            return _entities.Words
                .Where(w => w.StressPosition == stressPosition)
                .ToList();
        }

        // Обновление предложений рифм
        private void UpdateRhymeSuggestions()
        {
            if (_selectedWord == null)
            {
                lbRhymeResults.ItemsSource = null;
                lbRhymeResults.Visibility = Visibility.Collapsed;
                return;
            }

            // Поиск по окончанию
            var rhymesByEnding = FindRhymesByEnding(_selectedWord.Word, 3);

            // Поиск по ударению
            var rhymesByStress = FindRhymesByStress(_selectedWord.StressPosition);

            // Объединяем результаты, исключая дубликаты
            var combinedRhymes = rhymesByEnding
                .Union(rhymesByStress, new WordsEqualityComparer())
                .Where(w => w.Id != _selectedWord.Id)
                .ToList();

            lbRhymeResults.ItemsSource = combinedRhymes;
            lbRhymeResults.Visibility = Visibility.Visible;
        }

        // Класс для сравнения слов при объединении результатов
        private class WordsEqualityComparer : IEqualityComparer<Words>
        {
            // Сравнение слов по Id
            public bool Equals(Words x, Words y)
            {
                return x?.Id == y?.Id;
            }
            // Получение хэш-кода слова по Id

            public int GetHashCode(Words obj)
            {
                return obj.Id.GetHashCode();
            }
        }
        // Класс для отображения рифм в DataGrid
        public class RhymeDisplay
        {
            public int RhymeId { get; set; }
            public string Word { get; set; }
            public string Rhyme { get; set; }
            public string RhymeType { get; set; }
        }
    }
}