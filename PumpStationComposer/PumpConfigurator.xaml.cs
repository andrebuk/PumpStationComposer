using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;


namespace PumpStationComposer
{
    /// <summary>
    /// Логика взаимодействия для PumpConfigurator.xaml
    /// </summary>
    public partial class PumpConfigurator : Window
    {
        public Document document;
        public Dictionary<string, bool> PumpStationOptions = new Dictionary<string, bool>(20);
        public Dictionary<string, int> PumpDimensions = new Dictionary<string, int>(3);
        public Dictionary<string, int> PumpNozzle = new Dictionary<string, int>(2);

        public Dictionary<string, string> PumpEngine = new Dictionary<string, string>(1);
        public XYZ testPoint = new XYZ();
        public PumpConfigurator(Document doc)
        {
            document = doc;
            InitializeComponent();
        }

        private void Build_Click(object sender, RoutedEventArgs e)
        {
            testPoint = testPoint + new XYZ(100, 0, 0);

            PumpStation PST = new PumpStation();
            PST.startPoint = testPoint;
            PST.document = document;
            PST.PumpStationOptions = PumpStationOptions;
            PST.Create();
        }

        private void MSS_Checked(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("Draw");
            PumpStationOptions.Remove("Шкаф МСС");
            PumpStationOptions.Add("Шкаф МСС", true);

        }

        private void MSS_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Do not Draw");
            PumpStationOptions.Remove("Шкаф МСС");
            PumpStationOptions.Add("Шкаф МСС", false);

        }
    }
}
