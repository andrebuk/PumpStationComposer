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

        public PumpConfigurator(Document doc)
        {
            document = doc;
            InitializeComponent();
        }

        private void Build_Click(object sender, RoutedEventArgs e)
        {
            String ts = this.TestString.Text;
            SMFamily test = new SMFamily(document);
            FamilySymbol testFS = test.IsExist(ts,document);
            if (testFS!=null)
            {
                using(Transaction t = new Transaction(document, "Создание объектов насосной станции"))
                {
                    t.Start();
                    FamilyInstance instance = document.Create.NewFamilyInstance(new XYZ(0, 0, 0), testFS, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    
                    t.Commit();

                }
            }
            
            

            
            



        }
    }
}
