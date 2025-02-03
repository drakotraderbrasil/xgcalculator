using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XGCalculator
{
    public partial class BalloonControl : UserControl
    {
        // Evento para notificar a opção selecionada
        public event EventHandler<ZoneOption> OptionSelected;

        public BalloonControl()
        {
            InitializeComponent();
        }

        // Define o título do balão
        public void SetRegion(string regionName)
        {
            RegionTextBlock.Text = regionName;
        }

        // Cria dinamicamente as opções (eventos) com ícone PNG e texto
        public void SetOptions(List<ZoneOption> options)
        {
            OptionsPanel.Children.Clear();

            foreach (var option in options)
            {
                // Container horizontal para o ícone e o texto, centralizado
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 3, 0, 3),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Contêiner para o ícone, com tamanho fixo para preservar a proporção (20x20)
                var iconContainer = new Grid
                {
                    Width = 20,
                    Height = 20,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Usaremos o Image para exibir o PNG
                var icon = new Image
                {
                    Stretch = Stretch.Uniform,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (!string.IsNullOrEmpty(option.Icon))
                {
                    try
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(option.Icon, UriKind.RelativeOrAbsolute);
                        bitmap.DecodePixelWidth = 20;
                        bitmap.DecodePixelHeight = 20;
                        bitmap.EndInit();
                        RenderOptions.SetBitmapScalingMode(bitmap, BitmapScalingMode.HighQuality);
                        icon.Source = bitmap;
                    }
                    catch
                    {
                        icon.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    icon.Visibility = Visibility.Collapsed;
                }

                iconContainer.Children.Add(icon);

                // Texto do evento, centralizado
                var txt = new TextBlock
                {
                    Text = option.Description,
                    VerticalAlignment = VerticalAlignment.Center,
                    Style = (Style)FindResource("EventTextStyle"),
                    Margin = new Thickness(4, 0, 0, 0)
                };

                sp.Children.Add(iconContainer);
                sp.Children.Add(txt);

                // Botão que engloba o evento com efeito hover
                var btn = new Button
                {
                    Content = sp,
                    Tag = option,
                    Margin = new Thickness(0, 3, 0, 0),
                    Style = (Style)FindResource("EventButtonStyle")
                };

                btn.Click += (s, e) =>
                {
                    if (btn.Tag is ZoneOption opt)
                    {
                        OptionSelected?.Invoke(this, opt);
                    }
                };

                OptionsPanel.Children.Add(btn);
            }
        }
    }
}
