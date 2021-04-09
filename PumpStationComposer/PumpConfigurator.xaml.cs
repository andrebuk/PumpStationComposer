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
        public Dictionary<string, double> PumpDimensions=new Dictionary<string, double>(3);
            
        //    = new Dictionary<string, double>
        //{
        //    {"Длина", 1500.0 },
        //    {"Ширина", 1500.0 },
        //    {"Высота", 1500.0 }
        //};
        public Dictionary<string, int> PumpNozzle = new Dictionary<string, int>(2);
        public Dictionary<string, string> PumpEngine = new Dictionary<string, string>(1);
        public XYZ testPoint = new XYZ();
        public bool onlyLink;
        public PumpConfigurator(Document doc)
        {
            document = doc;
            InitializeComponent();
        }

        

        private void DrawBound_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            List<double> testList = new List<double> { 2.0, 3.0, 6.0 };
            double maxValue = testList.Max();
            MessageBox.Show(maxValue.ToString());
        }
        private void Build_Click(object sender, RoutedEventArgs e)
        {
            //testPoint = testPoint + new XYZ(1500 / 304.8, 0, 0);
            PumpDimensions.Clear();
            PumpDimensions.Add("Длина", Convert.ToDouble(this.PumpLength.Text));
            PumpDimensions.Add("Ширина", Convert.ToDouble(this.PumpWidth.Text));
            PumpDimensions.Add("Высота", Convert.ToDouble(this.PumpHeight.Text));

            PumpStation PST = new PumpStation();
            PST.startPoint = testPoint;
            PST.document = document;
            PST.PumpStationOptions = PumpStationOptions;
            PST.PumpDimensions = PumpDimensions;
            PST.PumpNozzle = PumpNozzle;
            PST.onlyLink = onlyLink;
            PST.Create();
            
        }
        //Шкаф МСС
        private void MSS_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Draw");
            PumpStationOptions.Remove("МСС с ПЧ");
            PumpStationOptions.Add("МСС с ПЧ", true);
        }
        private void MSS_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Do not Draw");
            PumpStationOptions.Remove("МСС с ПЧ");
            PumpStationOptions.Add("МСС с ПЧ", false);
        }
        //Шкаф ШУН (с ПЛК)
        private void SHUN_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Draw");
            PumpStationOptions.Remove("ШУН (с ПЛК)");
            PumpStationOptions.Add("ШУН (с ПЛК)", true);
        }

        private void SHUN_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Do not Draw");
            PumpStationOptions.Remove("ШУН (с ПЛК)");
            PumpStationOptions.Add("ШУН (с ПЛК)", false);
        }
        //Шкаф ВРУ
        private void VRU_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Draw");
            PumpStationOptions.Remove("ВРУ");
            PumpStationOptions.Add("ВРУ", true);
        }

        private void VRU_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Do not Draw");
            PumpStationOptions.Remove("ВРУ");
            PumpStationOptions.Add("ВРУ", false);
        }
        //Шкаф ШСН
        private void SHSN_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Draw");
            PumpStationOptions.Remove("ШСН");
            PumpStationOptions.Add("ШСН", true);
        }

        private void SHSN_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Do not Draw");
            PumpStationOptions.Remove("ШСН");
            PumpStationOptions.Add("ШСН", false);
        }
        //Шкаф ТСН
        private void TSN_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Draw");
            PumpStationOptions.Remove("ТСН");
            PumpStationOptions.Add("ТСН", true);
        }

        private void TSN_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Do not Draw");
            PumpStationOptions.Remove("ТСН");
            PumpStationOptions.Add("ТСН", false);
        }
        //Шкаф ОПС
        private void OPS_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Draw");
            PumpStationOptions.Remove("ОПС");
            PumpStationOptions.Add("ОПС", true);
        }

        private void OPS_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Do not Draw");
            PumpStationOptions.Remove("ОПС");
            PumpStationOptions.Add("ОПС", false);
        }

        //private void PumpLength_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    double length = Convert.ToDouble(this.PumpLength.Text);
        //    PumpDimensions.Remove("Длина");
        //    PumpDimensions.Add("Длина", length);
        //}

        //private void PumpWidth_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    double width = Convert.ToDouble(this.PumpWidth.Text);
        //    PumpDimensions.Remove("Ширина");
        //    PumpDimensions.Add("Ширина", width);
        //}

        //private void PumpHeight_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    double height = Convert.ToDouble(this.PumpHeight.Text);
        //    PumpDimensions.Remove("Высота");
        //    PumpDimensions.Add("Высота", height);
        //}

        private void Link_Checked(object sender, RoutedEventArgs e)
        {
            onlyLink = true;
        }
        private void Link_Unchecked(object sender, RoutedEventArgs e)
        {
            onlyLink = false;
        }
    }
}
