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
        public Dictionary<string, double> PumpDimensions = new Dictionary<string, double>
        {
            {"Длина", 1500.0 },
            {"Ширина", 1500.0 },
            {"Высота", 1500.0 }
        };
        public Dictionary<string, int> PumpNozzle = new Dictionary<string, int>(2);
        public Dictionary<string, string> PumpEngine = new Dictionary<string, string>(1);
        public XYZ testPoint = new XYZ();
        public PumpConfigurator(Document doc)
        {
            document = doc;
            InitializeComponent();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            FilteredElementCollector allElements = new FilteredElementCollector(document);
            ICollection<ElementId> allElementIDs = allElements.OfCategory(BuiltInCategory.OST_MechanicalEquipment).ToElementIds();
            using (Transaction t = new Transaction(document, "Удаление всех объектов"))
            {
                t.Start();

                document.Delete(allElementIDs);


                t.Commit();



            }
        }

        private void DrawBound_Click(object sender, RoutedEventArgs e)
        {
            double deltaForWalls = 150.0 / 304.8;
            //Удалим все линии
            FilteredElementCollector LinesCollector = new FilteredElementCollector(document);
            List<ElementId> Lines = (List<ElementId>)LinesCollector.OfCategory(BuiltInCategory.OST_Walls).ToElementIds();
            using (Transaction t = new Transaction(document, "Удаляем прежние структурные линии"))
            {
                t.Start();

                foreach (ElementId currentLine in Lines)
                {
                    try
                    {
                        document.Delete(currentLine);
                    }
                    catch (Exception)
                    {                       
                    }
                    

                }
                t.Commit();
            }

            List<double> maxXs = new List<double>();
            List<double> maxYs = new List<double>();
            List<double> minXs = new List<double>();
            List<double> minYs = new List<double>();

            FilteredElementCollector allElements = new FilteredElementCollector(document);
            IList<Element> allElementID = allElements.OfCategory(BuiltInCategory.OST_MechanicalEquipment).WhereElementIsNotElementType().ToElements();
            foreach (Element item in allElementID)
            {
                BoundingBoxXYZ currentBB = item.get_BoundingBox(document.ActiveView);
                maxXs.Add(currentBB.Max.X);
                maxYs.Add(currentBB.Max.Y);
                minXs.Add(currentBB.Min.X);
                minYs.Add(currentBB.Min.Y);
            }
            double maxX = maxXs.Max() + deltaForWalls;
            double maxY = maxYs.Max() + deltaForWalls;
            double minX = minXs.Min() - deltaForWalls;
            double minY = minYs.Min() - deltaForWalls;

            XYZ point1 = new XYZ(minX, minY, 0);
            XYZ point2 = new XYZ(minX, maxY, 0);
            XYZ point3 = new XYZ(maxX, maxY, 0);
            XYZ point4 = new XYZ(maxX, minY, 0);
            IList<Curve> curveLoop = new List<Curve>();
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(point1, point2));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(point2, point3));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(point3, point4));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(point4, point1));

            //using (Transaction t = new Transaction(document, "Создем линии для стен"))
            //{
            //    t.Start();

            //    //Wall wall = Wall.Create(document, curveLoop, false);

            //    document.Create.NewDetailCurve(document.ActiveView, Autodesk.Revit.DB.Line.CreateBound(point1, point2));
            //    document.Create.NewDetailCurve(document.ActiveView, Autodesk.Revit.DB.Line.CreateBound(point2, point3));
            //    document.Create.NewDetailCurve(document.ActiveView, Autodesk.Revit.DB.Line.CreateBound(point3, point4));
            //    document.Create.NewDetailCurve(document.ActiveView, Autodesk.Revit.DB.Line.CreateBound(point4, point1));
            //    t.Commit();

            //}

            using (Transaction t = new Transaction(document, "Пытаемся создать стены"))
            {
                t.Start();
                Level levelElement = document.ActiveView.GenLevel;
                ElementId leveId = levelElement.Id;
                foreach (Curve item in curveLoop)
                {
                    //ElementId currentId = item.Id;
                    Wall.Create(document, item, leveId, true);
                }

                t.Commit();

            }





        }
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            List<double> testList = new List<double> { 2.0, 3.0, 6.0 };
            double maxValue = testList.Max();
            MessageBox.Show(maxValue.ToString());
        }
        private void Build_Click(object sender, RoutedEventArgs e)
        {
            testPoint = testPoint + new XYZ(600 / 304.8, 0, 0);
            PumpStation PST = new PumpStation();
            PST.startPoint = testPoint;
            PST.document = document;
            PST.PumpStationOptions = PumpStationOptions;
            PST.PumpDimensions = PumpDimensions;
            PST.PumpNozzle = PumpNozzle;
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

        private void PumpLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            double length = Convert.ToDouble(this.PumpLength.Text);
            PumpDimensions.Remove("Длина");
            PumpDimensions.Add("Длина", length);
        }

        private void PumpWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            double width = Convert.ToDouble(this.PumpWidth.Text);
            PumpDimensions.Remove("Ширина");
            PumpDimensions.Add("Ширина", width);
        }

        private void PumpHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            double height = Convert.ToDouble(this.PumpHeight.Text);
            PumpDimensions.Remove("Высота");
            PumpDimensions.Add("Высота", height);
        }


    }
}
