namespace xgcalculator
{
    public class ZoneOption
    {
        public string Name { get; set; } // Nome da jogada (ex.: "Chute a gol", "Escanteio")
        public double XGValue { get; set; } // Valor de xG associado à jogada
        public string Icon { get; set; } // Caminho do ícone para exibição visual

        public ZoneOption(string name, double xgValue, string icon)
        {
            Name = name;
            XGValue = xgValue;
            Icon = icon;
        }
    }
}
