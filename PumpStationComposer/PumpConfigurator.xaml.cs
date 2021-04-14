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
            PST.Create(true);
            
        }
        private void Compose_Click(object sender, RoutedEventArgs e)
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
            PST.Create(false);
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

        private void OVVO_Checked(object sender, RoutedEventArgs e)
        {            
            PumpStationOptions.Remove("Отсек высоковольтного оборудования");
            PumpStationOptions.Add("Отсек высоковольтного оборудования", true);
        }
        private void OVVO_Unchecked(object sender, RoutedEventArgs e)
        {            
            PumpStationOptions.Remove("Отсек высоковольтного оборудования");
            PumpStationOptions.Add("Отсек высоковольтного оборудования", false);
        }
        //--
        private void Vent_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Система вентиляции");
            PumpStationOptions.Add("Система вентиляции", true);
        }
        private void Vent_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Система вентиляции");
            PumpStationOptions.Add("Система вентиляции", false);
        }
        private void Heat_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Система отопления");
            PumpStationOptions.Add("Система отопления", true);
        }
        private void Heat_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Система отопления");
            PumpStationOptions.Add("Система отопления", false);
        }
        private void Cond_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Система кондиционирования");
            PumpStationOptions.Add("Система кондиционирования", true);
        }
        private void Cond_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Система кондиционирования");
            PumpStationOptions.Add("Система кондиционирования", false);
        }
        private void VEP_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("ЗРА на всасе с электроприводом");
            PumpStationOptions.Add("ЗРА на всасе с электроприводом", true);
        }
        private void VEP_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("ЗРА на всасе с электроприводом");
            PumpStationOptions.Add("ЗРА на всасе с электроприводом", false);
        }
        private void NEPP_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("ЗРА на нагнетании с электроприводом и позиционером");
            PumpStationOptions.Add("ЗРА на нагнетании с электроприводом и позиционером", true);
        }
        private void NEPP_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("ЗРА на нагнетании с электроприводом и позиционером");
            PumpStationOptions.Add("ЗРА на нагнетании с электроприводом и позиционером", false);
        }

        private void NPSN_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("ЗРА на патрубке слива напорной магистрали с электроприводом");
            PumpStationOptions.Add("ЗРА на патрубке слива напорной магистрали с электроприводом", true);
        }
        private void NPSN_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("ЗРА на патрубке слива напорной магистрали с электроприводом");
            PumpStationOptions.Add("ЗРА на патрубке слива напорной магистрали с электроприводом", false);
        }
        private void OKSD_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Обратный клапан с демфером");
            PumpStationOptions.Add("Обратный клапан с демфером", true);
        }
        private void OKSD_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Обратный клапан с демфером");
            PumpStationOptions.Add("Обратный клапан с демфером", false);
        }
        private void DDNV_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Датчик давления на всасе");
            PumpStationOptions.Add("Датчик давления на всасе", true);
        }
        private void DDNV_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Датчик давления на всасе");
            PumpStationOptions.Add("Датчик давления на всасе", false);
        }
        private void DDNN_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Датчик давления на нагнетании");
            PumpStationOptions.Add("Датчик давления на нагнетании", true);
        }
        private void DDNN_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Датчик давления на нагнетании");
            PumpStationOptions.Add("Датчик давления на нагнетании", false);
        }
        private void RNP_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Расходомер на нагнетании");
            PumpStationOptions.Add("Расходомер на нагнетании", true);
        }
        private void RNP_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Расходомер на нагнетании");
            PumpStationOptions.Add("Расходомер на нагнетании", false);
        }
        private void VSZ_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Вакуумная система заполнения всаса насоса");
            PumpStationOptions.Add("Вакуумная система заполнения всаса насоса", true);
        }
        private void VSZ_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Вакуумная система заполнения всаса насоса");
            PumpStationOptions.Add("Вакуумная система заполнения всаса насоса", false);
        }

        private void Pipe_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Трубопровод на всасе 5м");
            PumpStationOptions.Add("Трубопровод на всасе 5м", true);
        }
        private void Pipe_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Трубопровод на всасе 5м");
            PumpStationOptions.Add("Трубопровод на всасе 5м", false);
        }
        private void Flow_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Плавающий водозаборный оголовок");
            PumpStationOptions.Add("Плавающий водозаборный оголовок", true);
        }
        private void Flow_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Плавающий водозаборный оголовок");
            PumpStationOptions.Add("Плавающий водозаборный оголовок", false);
        }
        private void SDP_Checked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Стрела для подъема всасывающей трубы");
            PumpStationOptions.Add("Стрела для подъема всасывающей трубы", true);
        }
        private void SDP_Unchecked(object sender, RoutedEventArgs e)
        {
            PumpStationOptions.Remove("Стрела для подъема всасывающей трубы");
            PumpStationOptions.Add("Стрела для подъема всасывающей трубы", false);
        }







    }
}
