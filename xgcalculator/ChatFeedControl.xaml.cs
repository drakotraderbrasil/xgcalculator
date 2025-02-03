using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XGCalculator
{
    public partial class ChatFeedControl : UserControl
    {
        public ChatFeedControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adiciona uma mensagem simples ao chat.
        /// </summary>
        public void AddMessage(string senderName, string message)
        {
            var border = new Border
            {
                Background = senderName == "Eu" ? new SolidColorBrush(Color.FromRgb(173, 216, 230))
                                               : new SolidColorBrush(Color.FromRgb(220, 220, 220)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                MaxWidth = 400,
                HorizontalAlignment = senderName == "Eu" ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };

            var textBlock = new TextBlock
            {
                Text = $"{senderName}: {message}",
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.Black
            };

            border.Child = textBlock;
            MessagesPanel.Children.Add(border);
            ScrollToBottom();
        }

        /// <summary>
        /// Adiciona uma mensagem de evento com o nome do evento e o ícone.
        /// </summary>
        public void AddEventMessage(string eventName, string iconUri)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                MaxWidth = 400,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var sp = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };

            if (!string.IsNullOrEmpty(iconUri))
            {
                var image = new Image
                {
                    Width = 24,
                    Height = 24,
                    Margin = new Thickness(0, 0, 5, 0),
                    Stretch = Stretch.Uniform
                };

                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(iconUri, UriKind.RelativeOrAbsolute);
                    bitmap.DecodePixelWidth = 24;
                    bitmap.DecodePixelHeight = 24;
                    bitmap.EndInit();
                    image.Source = bitmap;
                }
                catch
                {
                    // Se não conseguir carregar o ícone, ignora
                }

                sp.Children.Add(image);
            }

            var textBlock = new TextBlock
            {
                Text = eventName,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.Black,
                TextWrapping = TextWrapping.Wrap
            };

            sp.Children.Add(textBlock);
            border.Child = sp;
            MessagesPanel.Children.Add(border);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            if (MessagesPanel.Parent is ScrollViewer sv)
            {
                sv.ScrollToEnd();
            }
        }
    }
}
