using MahApps.Metro.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace XGCalculator
{
    public class ZoneOption
    {
        public string Description { get; set; }
        public double XGValue { get; set; }
        public string Icon { get; set; }
    }

    public partial class MainWindow : MetroWindow
    {
        private readonly List<Move> moves = new List<Move>();
        public SeriesCollection SeriesCollection { get; set; }
        private Point _touchStart;
        private DispatcherTimer _chartOverlayTimer;

        private readonly double NormalWidth = 1000;
        private readonly double NormalHeight = 700;
        private readonly double MiniWidth = 800;
        private readonly double MiniHeight = 450;
        private bool isMiniMode = false;

        // Regiões para detecção de cliques (para os balões de evento)
        private readonly Rect A_Escanteio1Rect = new Rect(0.09, 0.05, 0.05, 0.07);
        private readonly Rect A_Escanteio2Rect = new Rect(0.09, 0.92, 0.05, 0.07);
        private readonly Rect A_PequenaAreaRect = new Rect(0.11, 0.39, 0.04, 0.25);
        private readonly Rect A_GrandeAreaRect = new Rect(0.11, 0.25, 0.12, 0.54);
        private readonly Rect A_ContraAtaqueRect = new Rect(0.43, 0.07, 0.08, 0.90);
        private readonly Rect A_BolaParadaMaisDistanteRect = new Rect(0.31, 0.08, 0.11, 0.89);
        private readonly Rect A_BolaParadaLateralSuperiorRect = new Rect(0.24, 0.07, 0.06, 0.18);
        private readonly Rect A_BolaParadaLateralInferiorRect = new Rect(0.23, 0.79, 0.07, 0.17);
        private readonly Rect A_RiscoProximoGrandeAreaRect = new Rect(0.23, 0.26, 0.06, 0.54);
        private readonly Rect A_InfiltracaoSuperiorRect = new Rect(0.11, 0.13, 0.13, 0.12);
        private readonly Rect A_InfiltracaoInferiorRect = new Rect(0.11, 0.80, 0.12, 0.10);
        private readonly Rect A_Zona14Rect = new Rect(0.23, 0.42, 0.06, 0.22);

        private readonly Rect B_Escanteio1Rect = new Rect(0.87, 0.05, 0.04, 0.07);
        private readonly Rect B_Escanteio2Rect = new Rect(0.87, 0.92, 0.05, 0.07);
        private readonly Rect B_PequenaAreaRect = new Rect(0.86, 0.40, 0.04, 0.25);
        private readonly Rect B_GrandeAreaRect = new Rect(0.78, 0.25, 0.13, 0.54);
        private readonly Rect B_ContraAtaqueRect = new Rect(0.51, 0.07, 0.08, 0.90);
        private readonly Rect B_BolaParadaMaisDistanteRect = new Rect(0.59, 0.07, 0.13, 0.90);
        private readonly Rect B_BolaParadaLateralSuperiorRect = new Rect(0.72, 0.07, 0.06, 0.17);
        private readonly Rect B_BolaParadaLateralInferiorRect = new Rect(0.72, 0.80, 0.06, 0.18);
        private readonly Rect B_RiscoProximoGrandeAreaRect = new Rect(0.72, 0.25, 0.06, 0.54);
        private readonly Rect B_InfiltracaoSuperiorRect = new Rect(0.78, 0.12, 0.12, 0.12);
        private readonly Rect B_InfiltracaoInferiorRect = new Rect(0.78, 0.80, 0.13, 0.11);
        private readonly Rect B_Zona14Rect = new Rect(0.73, 0.42, 0.05, 0.21);

        public MainWindow()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();
            RefreshTeamList();

            _chartOverlayTimer = new DispatcherTimer();
            _chartOverlayTimer.Interval = TimeSpan.FromSeconds(5);
            _chartOverlayTimer.Tick += (s, e) => { _chartOverlayTimer.Stop(); };
        }

        // ***** EVENTOS DO MODO NORMAL *****
        private void FieldCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _touchStart = e.GetPosition(FieldCanvas);
        }

        private void FieldCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(FieldCanvas);
            ProcessFieldClick(clickPoint, FieldCanvas.ActualWidth, FieldCanvas.ActualHeight, miniMode: false);
        }

        // ***** EVENTOS DO MODO MINI *****
        private void FieldCanvasMini_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _touchStart = e.GetPosition(FieldCanvasMini);
        }

        private void FieldCanvasMini_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(FieldCanvasMini);
            ProcessFieldClick(clickPoint, FieldCanvasMini.ActualWidth, FieldCanvasMini.ActualHeight, miniMode: true);
        }

        // Fecha os overlays ao clicar fora (modo mini)
        private void MiniModeGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is FrameworkElement fe &&
                  (fe.Name.StartsWith("btn") || fe.Name.StartsWith("Overlay") || fe.Name == "BackButtonMini")))
            {
                CloseAllOverlays();
            }
        }

        // Handler para o retângulo transparente (se utilizado) para fechar overlays
        private void OverlayCloseArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseAllOverlays();
            e.Handled = true;
        }

        private void CloseAllOverlays()
        {
            if (OverlayTimesMini.Visibility == Visibility.Visible)
                SlideOverlay(OverlayTimesMini, ttTimesMini, false);
            if (OverlayChatMini.Visibility == Visibility.Visible)
                SlideOverlay(OverlayChatMini, ttChatMini, false);
            if (OverlayChartMini.Visibility == Visibility.Visible)
                SlideOverlay(OverlayChartMini, ttChartMini, false);
        }

        // Processa cliques no campo (para ambos os modos)
        private void ProcessFieldClick(Point clickPoint, double canvasWidth, double canvasHeight, bool miniMode = false)
        {
            double normX = clickPoint.X / canvasWidth;
            double normY = clickPoint.Y / canvasHeight;
            Point normalizedPoint = new Point(normX, normY);

            if (A_Zona14Rect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Zona 14 - Golden Square", GetOptionsForRegion("Zona14"), "Time A", miniMode);
            else if (A_Escanteio1Rect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Escanteio Superior", GetOptionsForRegion("Escanteio"), "Time A", miniMode);
            else if (A_Escanteio2Rect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Escanteio Inferior", GetOptionsForRegion("Escanteio"), "Time A", miniMode);
            else if (A_PequenaAreaRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Pequena Área", GetOptionsForRegion("Pequena Área"), "Time A", miniMode);
            else if (A_GrandeAreaRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Grande Área", GetOptionsForRegion("Grande Área"), "Time A", miniMode);
            else if (A_ContraAtaqueRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Contra Ataque", GetOptionsForRegion("ContraAtaque"), "Time A", miniMode);
            else if (A_BolaParadaMaisDistanteRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Bola Parada Mais Distante", GetOptionsForRegion("BolaParadaMaisDistante"), "Time A", miniMode);
            else if (A_BolaParadaLateralSuperiorRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Bola Parada Lateral Superior Perigosa", GetOptionsForRegion("BolaParadaLateralSuperior"), "Time A", miniMode);
            else if (A_BolaParadaLateralInferiorRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Bola Parada Lateral Inferior Perigosa", GetOptionsForRegion("BolaParadaLateralInferior"), "Time A", miniMode);
            else if (A_RiscoProximoGrandeAreaRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Risco Próximo a Grande Área", GetOptionsForRegion("RiscoProximoGrandeArea"), "Time A", miniMode);
            else if (A_InfiltracaoSuperiorRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Infiltração Próximo a Linha de fundo (Superior)", GetOptionsForRegion("Infiltracao"), "Time A", miniMode);
            else if (A_InfiltracaoInferiorRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Infiltração Próximo a Linha de fundo (Inferior)", GetOptionsForRegion("Infiltracao"), "Time A", miniMode);
            else if (B_Zona14Rect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Zona 14 - Golden Square", GetOptionsForRegion("Zona14"), "Time B", miniMode);
            else if (B_Escanteio1Rect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Escanteio Superior", GetOptionsForRegion("Escanteio"), "Time B", miniMode);
            else if (B_Escanteio2Rect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Escanteio Inferior", GetOptionsForRegion("Escanteio"), "Time B", miniMode);
            else if (B_PequenaAreaRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Pequena Área", GetOptionsForRegion("Pequena Área"), "Time B", miniMode);
            else if (B_GrandeAreaRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Grande Área", GetOptionsForRegion("Grande Área"), "Time B", miniMode);
            else if (B_ContraAtaqueRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Contra Ataque", GetOptionsForRegion("ContraAtaque"), "Time B", miniMode);
            else if (B_BolaParadaMaisDistanteRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Bola Parada Mais Distante", GetOptionsForRegion("BolaParadaMaisDistante"), "Time B", miniMode);
            else if (B_BolaParadaLateralSuperiorRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Bola Parada Lateral Superior Perigosa", GetOptionsForRegion("BolaParadaLateralSuperior"), "Time B", miniMode);
            else if (B_BolaParadaLateralInferiorRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Bola Parada Lateral Inferior Perigosa", GetOptionsForRegion("BolaParadaLateralInferior"), "Time B", miniMode);
            else if (B_RiscoProximoGrandeAreaRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Risco Próximo a Grande Área", GetOptionsForRegion("RiscoProximoGrandeArea"), "Time B", miniMode);
            else if (B_InfiltracaoSuperiorRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Infiltração Próximo a Linha de fundo (Superior)", GetOptionsForRegion("Infiltracao"), "Time B", miniMode);
            else if (B_InfiltracaoInferiorRect.Contains(normalizedPoint))
                ShowBalloon(clickPoint, "Infiltração Próximo a Linha de fundo (Inferior)", GetOptionsForRegion("Infiltracao"), "Time B", miniMode);
        }

        // Exibe o balão de evento e, após a seleção, mostra o feedback do evento
        private void ShowBalloon(Point point, string regionName, List<ZoneOption> options, string team, bool miniMode = false)
        {
            BalloonControl balloon = new BalloonControl();
            balloon.SetRegion(regionName);
            balloon.SetOptions(options);
            balloon.OptionSelected += (s, opt) =>
            {
                RegisterEvent(team, opt);
                // Exibe o feedback do evento na parte inferior central do campo
                if (miniMode)
                    ShowFeedbackEventMini(opt.Description);
                else
                    ShowFeedbackEvent(opt.Description);

                if (miniMode)
                    EventPopupMini.IsOpen = false;
                else
                    EventPopup.IsOpen = false;
            };

            if (miniMode)
            {
                EventPopupMini.Child = balloon;
                EventPopupMini.HorizontalOffset = point.X;
                EventPopupMini.VerticalOffset = point.Y;
                EventPopupMini.IsOpen = true;
            }
            else
            {
                EventPopup.Child = balloon;
                EventPopup.HorizontalOffset = point.X;
                EventPopup.VerticalOffset = point.Y;
                EventPopup.IsOpen = true;
            }
        }

        // Método que retorna a lista de opções para uma região específica
        public List<ZoneOption> GetOptionsForRegion(string regionType)
        {
            var options = new List<ZoneOption>();
            if (regionType == "Escanteio")
            {
                options.Add(new ZoneOption
                {
                    Description = "Escanteio",
                    XGValue = 0.07,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_escanteio.png"
                });
            }
            else if (regionType == "Pequena Área")
            {
                options.Add(new ZoneOption
                {
                    Description = "Chute a gol de dentro da Pequena Área",
                    XGValue = 0.32,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_pequena_area_gol.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Chute para fora de dentro da Pequena Área",
                    XGValue = 0.11,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_pequena_area_fora.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Chute na trave de dentro da Pequena Área",
                    XGValue = 0.16,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_pequena_area_trave.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Chute de dentro da Pequena Área e escanteio",
                    XGValue = 0.05,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_pequena_area_escanteio.png"
                });
            }
            else if (regionType == "Grande Área")
            {
                options.Add(new ZoneOption
                {
                    Description = "Chute a gol de dentro da Grande Área",
                    XGValue = 0.22,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_grande_area_gol.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Chute para fora de dentro da Grande Área",
                    XGValue = 0.06,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_grande_area_fora.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Chute na trave de dentro da Grande Área",
                    XGValue = 0.12,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_grande_area_trave.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Chute de dentro da Grande Área gerou escanteio",
                    XGValue = 0.04,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_grande_area_escanteio.png"
                });
            }
            else if (regionType == "ContraAtaque")
            {
                options.Add(new ZoneOption
                {
                    Description = "Contra Ataque com finalização certeira",
                    XGValue = 0.90,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_contra_ataque_certeiro.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Contra Ataque finalizado sem chance",
                    XGValue = 0.05,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_contra_ataque_sem_chance.png"
                });
            }
            else if (regionType == "BolaParadaMaisDistante")
            {
                options.Add(new ZoneOption
                {
                    Description = "Bola parada para Grande área, distante menor risco.",
                    XGValue = 0.09,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_bola_parada_mais_distante.png"
                });
            }
            else if (regionType == "BolaParadaLateralSuperior")
            {
                options.Add(new ZoneOption
                {
                    Description = "Bola parada lateral perigosa.",
                    XGValue = 0.07,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_bola_parada_lateral_superior.png"
                });
            }
            else if (regionType == "BolaParadaLateralInferior")
            {
                options.Add(new ZoneOption
                {
                    Description = "Bola parada lateral perigosa.",
                    XGValue = 0.08,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_bola_parada_lateral_inferior.png"
                });
            }
            else if (regionType == "RiscoProximoGrandeArea")
            {
                options.Add(new ZoneOption
                {
                    Description = "Bola parada perigosa para Grande área",
                    XGValue = 0.09,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_risco_proximo_grande_area.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Chute a gol de Fora da Grande Área",
                    XGValue = 0.06,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_risco_proximo_fora.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Chute ao lado do Gol de Fora da Grande Área",
                    XGValue = 0.04,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_risco_proximo_lateral.png"
                });
            }
            else if (regionType == "Infiltracao")
            {
                options.Add(new ZoneOption
                {
                    Description = "Infiltração na área pela Lateral",
                    XGValue = 0.16,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_infiltracao_lateral.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Bola Parada perigosa Lateral",
                    XGValue = 0.11,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_infiltracao_bola_parada.png"
                });
            }
            else if (regionType == "Zona14")
            {
                options.Add(new ZoneOption
                {
                    Description = "Bola Parada perigosíssima.",
                    XGValue = 0.25,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_zona14_bola_parada.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Chute a gol.",
                    XGValue = 0.35,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_zona14_chute.png"
                });
                options.Add(new ZoneOption
                {
                    Description = "Infiltração na grande área.",
                    XGValue = 0.18,
                    Icon = "pack://application:,,,/xgcalculator;component/Images/icone_zona14_infiltracao.png"
                });
            }
            else
            {
                options.Add(new ZoneOption
                {
                    Description = "Opção padrão",
                    XGValue = 0.0,
                    Icon = string.Empty
                });
            }
            return options;
        }

        // Exibe o feedback do evento na parte inferior central do campo (modo normal)
        private void ShowFeedbackEvent(string text)
        {
            if (FeedbackCanvas.Children.Count >= 3)
                FeedbackCanvas.Children.RemoveAt(0);

            TextBlock tb = new TextBlock
            {
                Text = text,
                FontSize = 24,
                Foreground = Brushes.White,
                Opacity = 1,
                FontWeight = FontWeights.Bold
            };

            tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double tbWidth = tb.DesiredSize.Width;
            double left = (FeedbackCanvas.ActualWidth - tbWidth) / 2;
            double bottom = FeedbackCanvas.Children.Count * 30;

            Canvas.SetLeft(tb, left);
            Canvas.SetBottom(tb, bottom);

            FeedbackCanvas.Children.Add(tb);

            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(3)));
            fadeOut.Completed += (s, e) => { FeedbackCanvas.Children.Remove(tb); };
            tb.BeginAnimation(OpacityProperty, fadeOut);
        }

        // Exibe o feedback do evento na parte inferior central do campo (modo mini)
        private void ShowFeedbackEventMini(string text)
        {
            if (FeedbackCanvasMini.Children.Count >= 3)
                FeedbackCanvasMini.Children.RemoveAt(0);

            TextBlock tb = new TextBlock
            {
                Text = text,
                FontSize = 24,
                Foreground = Brushes.White,
                Opacity = 1,
                FontWeight = FontWeights.Bold
            };

            tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double tbWidth = tb.DesiredSize.Width;
            double left = (FeedbackCanvasMini.ActualWidth - tbWidth) / 2;
            double bottom = FeedbackCanvasMini.Children.Count * 30;

            Canvas.SetLeft(tb, left);
            Canvas.SetBottom(tb, bottom);

            FeedbackCanvasMini.Children.Add(tb);

            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(3)));
            fadeOut.Completed += (s, e) => { FeedbackCanvasMini.Children.Remove(tb); };
            tb.BeginAnimation(OpacityProperty, fadeOut);
        }

        // Atualiza a lista de times e atualiza o overlay de times no modo mini
        private void RefreshTeamList()
        {
            var teamTotals = moves.GroupBy(m => m.Team)
                                  .Select(g => new TeamTotal { Team = g.Key, TotalXG = g.Sum(m => m.XG) })
                                  .ToList();
            TeamListView.ItemsSource = teamTotals;
            OverlayTeamListMini.ItemsSource = teamTotals;
        }

        // Atualiza o gráfico nos controles normais e no modo mini
        private void UpdateChart()
        {
            SeriesCollection.Clear();
            var teams = moves.Select(m => m.Team).Distinct();
            foreach (var team in teams)
            {
                var teamMoves = moves.Where(m => m.Team == team).ToList();
                var cumulative = new List<double>();
                double sum = 0;
                foreach (var move in teamMoves)
                {
                    sum += move.XG;
                    cumulative.Add(sum);
                }
                SeriesCollection.Add(new LineSeries
                {
                    Title = team,
                    Values = new ChartValues<double>(cumulative),
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 8
                });
            }
            XGChart.Series = SeriesCollection;
            MiniOverlayChart.Series = SeriesCollection;
        }

        // Registra o evento e exibe feedback
        private void RegisterEvent(string team, ZoneOption option)
        {
            moves.Add(new Move { Team = team, Jogada = option.Description, XG = option.XGValue });
            RefreshTeamList();
            UpdateChart();
            if (isMiniMode)
                ShowFeedbackEventMini(option.Description);
            else
                ShowFeedbackEvent(option.Description);

            if (isMiniMode)
                OverlayChatFeedMini.AddEventMessage(option.Description, option.Icon);
            else
                ChatFeed.AddEventMessage(option.Description, option.Icon);
        }

        private void UndoMoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (moves.Count == 0)
            {
                MessageBox.Show("Nenhuma jogada para desfazer!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            moves.RemoveAt(moves.Count - 1);
            RefreshTeamList();
            UpdateChart();
        }

        private void ResetDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Deseja resetar os dados?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                moves.Clear();
                RefreshTeamList();
                UpdateChart();
            }
        }

        private void SaveCSVButton_Click(object sender, RoutedEventArgs e)
        {
            if (moves.Count == 0)
            {
                MessageBox.Show("Não há dados para salvar!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "xg_data.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.WriteLine("Time,Tipo de Jogada,xG");
                    foreach (var move in moves)
                    {
                        sw.WriteLine($"{move.Team},{move.Jogada},{move.XG}");
                    }
                }
                MessageBox.Show("Dados salvos com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // ***** MÉTODOS PARA O EFEITO DESLIZANTE DOS OVERLAYS NO MODO MINI *****
        private void SlideOverlay(Grid overlay, TranslateTransform tt, bool slideIn)
        {
            DoubleAnimation anim = new DoubleAnimation();
            anim.Duration = TimeSpan.FromMilliseconds(150);
            if (slideIn)
            {
                anim.From = 200;
                anim.To = 0;
                overlay.Visibility = Visibility.Visible;
            }
            else
            {
                anim.From = 0;
                anim.To = 200;
                anim.Completed += (s, e) => { overlay.Visibility = Visibility.Collapsed; };
            }
            tt.BeginAnimation(TranslateTransform.YProperty, anim);
        }

        // Handlers para os botões de ícone no modo mini com efeito deslizante
        private void BtnTimesMini_Click(object sender, RoutedEventArgs e)
        {
            if (OverlayTimesMini.Visibility == Visibility.Visible)
            {
                SlideOverlay(OverlayTimesMini, ttTimesMini, false);
            }
            else
            {
                RefreshTeamList();
                if (OverlayChatMini.Visibility == Visibility.Visible)
                    SlideOverlay(OverlayChatMini, ttChatMini, false);
                if (OverlayChartMini.Visibility == Visibility.Visible)
                    SlideOverlay(OverlayChartMini, ttChartMini, false);
                SlideOverlay(OverlayTimesMini, ttTimesMini, true);
            }
            e.Handled = true;
        }

        private void BtnChatMini_Click(object sender, RoutedEventArgs e)
        {
            if (OverlayChatMini.Visibility == Visibility.Visible)
            {
                SlideOverlay(OverlayChatMini, ttChatMini, false);
            }
            else
            {
                if (OverlayTimesMini.Visibility == Visibility.Visible)
                    SlideOverlay(OverlayTimesMini, ttTimesMini, false);
                if (OverlayChartMini.Visibility == Visibility.Visible)
                    SlideOverlay(OverlayChartMini, ttChartMini, false);
                SlideOverlay(OverlayChatMini, ttChatMini, true);
            }
            e.Handled = true;
        }

        private void BtnChartMini_Click(object sender, RoutedEventArgs e)
        {
            if (OverlayChartMini.Visibility == Visibility.Visible)
            {
                SlideOverlay(OverlayChartMini, ttChartMini, false);
            }
            else
            {
                UpdateChart();
                if (OverlayTimesMini.Visibility == Visibility.Visible)
                    SlideOverlay(OverlayTimesMini, ttTimesMini, false);
                if (OverlayChatMini.Visibility == Visibility.Visible)
                    SlideOverlay(OverlayChatMini, ttChatMini, false);
                SlideOverlay(OverlayChartMini, ttChartMini, true);
            }
            e.Handled = true;
        }

        // Alterna entre os modos Normal e Mini
        private void MiniModeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isMiniMode)
            {
                isMiniMode = true;
                this.Width = MiniWidth;
                this.Height = MiniHeight;
                NormalModeGrid.Visibility = Visibility.Collapsed;
                MiniModeGrid.Visibility = Visibility.Visible;
                ToggleModeButton.Visibility = Visibility.Collapsed;
                RefreshTeamList();
                UpdateChart();
            }
            else
            {
                isMiniMode = false;
                this.Width = NormalWidth;
                this.Height = NormalHeight;
                NormalModeGrid.Visibility = Visibility.Visible;
                MiniModeGrid.Visibility = Visibility.Collapsed;
                ToggleModeButton.Visibility = Visibility.Visible;
                ToggleModeButton.Content = "Modo Mini";
            }
        }

        // Handler para o botão "Voltar" no modo mini
        private void BackButtonMini_Click(object sender, RoutedEventArgs e)
        {
            isMiniMode = false;
            this.Width = NormalWidth;
            this.Height = NormalHeight;
            NormalModeGrid.Visibility = Visibility.Visible;
            MiniModeGrid.Visibility = Visibility.Collapsed;
            ToggleModeButton.Visibility = Visibility.Visible;
            ToggleModeButton.Content = "Modo Mini";
        }
    }

    public class Move
    {
        public string Team { get; set; } = string.Empty;
        public string Jogada { get; set; } = string.Empty;
        public double XG { get; set; }
    }

    public class TeamTotal
    {
        public string Team { get; set; } = string.Empty;
        public double TotalXG { get; set; }
    }
}
