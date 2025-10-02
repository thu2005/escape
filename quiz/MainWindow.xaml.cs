using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace quiz
{
    public sealed partial class MainWindow : Window
    {
        // List question and answer
        private readonly List<(string Question, string[] Answer)> questions = new()
        {
            ("Đuổi hình bắt chữ", new[] { "Y", "Ế", "U", "Ớ", "T" }),
            ("Sự kiện này diễn ra năm mấy?", new[] { "2", "0", "1", "8", "" }),
            ("Tìm giá trị?", new[] { "5", "", "", "", "" })
        };

        private int currentQuestionIndex = 0;

        public MainWindow()
        {
            this.InitializeComponent();

            // Set full screen
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);

            // LoadQuestion();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            startGrid.Visibility = Visibility.Collapsed;
            gameGrid.Visibility = Visibility.Visible;
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            var q = questions[currentQuestionIndex];
            txtQuestion.Text = q.Question;
            txt1.Text = txt2.Text = txt3.Text = txt4.Text = txt5.Text = "";
            txtError.Visibility = Visibility.Collapsed;

            switch (currentQuestionIndex)
            {
                case 0:
                    questionImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/chilli.png"));
                    break;
                case 1:
                    questionImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/football.png"));
                    break;
                case 2:
                    questionImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/balloon.png"));
                    break;
            }

            // Show/Hide text boxes based on answer length
            txt1.Visibility = txt2.Visibility = txt3.Visibility = txt4.Visibility = txt5.Visibility = Visibility.Visible;
            if (currentQuestionIndex == 1) // Question 2: 4 characters
            {
                txt5.Visibility = Visibility.Collapsed;
            }
            else if (currentQuestionIndex == 2) // Question 3: 1 character
            {
                txt2.Visibility = txt3.Visibility = txt4.Visibility = txt5.Visibility = Visibility.Collapsed;
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var correct = questions[currentQuestionIndex].Answer;
            var inputs = new[] { txt1.Text, txt2.Text, txt3.Text, txt4.Text, txt5.Text };

            bool isCorrect = inputs.SequenceEqual(correct, StringComparer.OrdinalIgnoreCase);

            if (isCorrect)
            {
                currentQuestionIndex++;
                if (currentQuestionIndex < questions.Count)
                {
                    LoadQuestion();
                }
                else
                {
                    txtQuestion.Text = "Correct!";
                    txt1.Visibility = txt2.Visibility = txt3.Visibility = txt4.Visibility = txt5.Visibility = Visibility.Collapsed;
                    btnSubmit.Visibility = Visibility.Collapsed;
                    txtError.Visibility = Visibility.Collapsed;
                    questionImage.Visibility = Visibility.Collapsed;
                    emailGrid.Visibility = Visibility.Visible;
                }
            }
            else
            {
                txtError.Text = "Uncorrect!";
                txtError.Visibility = Visibility.Visible;
            }
        }

        private async void CopyEmail_Click(object sender, RoutedEventArgs e)
        {
            var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
            dataPackage.SetText("thunguyendoanxuan0405@gmail.com");
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);

            // Show feedback
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = "Email copied to clipboard!",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
