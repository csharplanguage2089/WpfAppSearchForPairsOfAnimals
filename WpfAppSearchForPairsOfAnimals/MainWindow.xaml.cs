using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfAppSearchForPairsOfAnimals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            TimeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                TimeTextBlock.Text = TimeTextBlock.Text + " - Play again?";

                // Показывать текст выхода из игры после ее окончания
                TextBlock textBlockAlt = sender as TextBlock;
                CloseThisWindow.Visibility = Visibility.Visible;
            }
        }

        private void SetUpGame()
        {
            // Создает список из 8 пар emoji
            List<string> animalEmoji = new List<string>()
            {
                "🐶", "🐶",
                "🐱", "🐱",
                "🐰", "🐰",
                "🐼", "🐼",
                "🐷", "🐷",
                "🦞", "🦞",
                "🐠", "🐠",
                "🐬", "🐬",
            };

            // Создает новый генератор случайных чисел
            Random random = new Random();

            // Находит каждый элемент TextBlock в сетке и повторяет следующие команды для каждого элемента
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if ((textBlock.Name != "TimeTextBlock") && (textBlock.Name != "CloseThisWindow"))
                {
                    textBlock.Visibility = Visibility.Visible;

                    // Выбирает случайное число от 0 до количества emoji в списке и назначает ему имя index
                    int index = random.Next(animalEmoji.Count);

                    // Использует случайное число с именем index для получения случайного emoji из списка
                    string nextEmoji = animalEmoji[index];

                    // Обновляет TextBlock случайным emoji из списка
                    textBlock.Text = nextEmoji;

                    // Удаляет случайный emoji из списка
                    animalEmoji.RemoveAt(index);
                }
            }

            // Запуск таймера и сброс содержимого полей
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;

            // Возвращение кнопки окончания игры к ее первоначальному состоянию Hidden
            CloseThisWindow.Visibility = Visibility.Hidden;
        }

        /*
         * Обработка щелчков
         * Если щелчок сделан на 1-м животном в паре, сохранить информацию о том, на каком элементе TextBlock щелкнул пользователь, и убрать животное с экрана.
         * Если это 2-е животное в паре, либо убрать его с экрана (если животные составляют пару), либо вернуть на экран первое животное (если животные разные).
         */

        TextBlock lastTextBlockClicked;

        // Этот признак определяет, щелкнул ли игрок на первом животном в паре, и теперь пытается найти для него пару
        bool findingMatch = false;

        // Обработчик событий - метод, который вызывается вашим приложением в ответ на такие события, как щелчок кнопкой мыши, нажатие клавиши, изменение размеров окна и т. д.
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            if (findingMatch == false)
            {
                // Игрок только что щелкнул на первом животном в паре, поэтому это животное становится невидимым, а соответствующий элемент TextBlock сохраняется на случай, если его придется делать видимым снова
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                // Игрок нашел пару! Второе животное в паре становится невидимым (а при дальнейших щелчках на нем ничего не происходит), признак findingMatch сбрасывается на false, чтобы следующее животное, на котором щелкнет игрок, снова считалось первым в паре
                matchesFound++; // Увеличение значения matchesFound с каждой успешно найденной парой
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                // Игрок щелкнул на животном, которое не совпадает с первым, поэтому первое выбранное животное снова становится видимым, а признак findingMatch сбрасывается на false
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Сбрасывает игру, если были найдены все 8 пар
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }

        private void CloseThisWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                Close();
            }
        }
    }
}
