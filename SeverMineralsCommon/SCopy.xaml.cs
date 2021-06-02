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
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;


namespace SeverMineralsCommon
{
    /// <summary>
    /// Логика взаимодействия для SPF.xaml
    /// </summary>
    public partial class SCopy : Window
    {
        public Document document;
        public UIDocument uiDocument;
        //public DefinitionFile defFile;
        //public DefinitionGroups AllParamaterGroups;
        //public Definitions definitions;
        //public Dictionary<string, BuiltInParameterGroup> familyParameterGroupDict;



        public SCopy(Document doc, UIDocument uidoc)
        {
            document = doc;
            uiDocument = uidoc;


            InitializeComponent();

        }
        //метод заполнения всех спецификаций
        public void SpecSelection()
        {


            //пробуем найти выбранную спецификацию
            View selectedSpec = document.ActiveView;

            string selectedSpecType = selectedSpec.GetType().Name;

            if (selectedSpecType == "ViewSchedule")
            {
                this.ViewShedules.Items.Add(selectedSpec.Name);
            }
            else
            {

                FilteredElementCollector fec = new FilteredElementCollector(document);
                fec.OfClass(typeof(ViewSchedule));
                foreach (Element item in fec)
                {
                    this.ViewShedules.Items.Add(item.Name);
                }
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string SpecToCopyName = this.ViewShedules.SelectedItem.ToString();
            SMViewSchedule viewShedule = new SMViewSchedule(document);
            Dictionary<string, View> allViewSchedulles = viewShedule.GetAllViewScheduleNameView();
            allViewSchedulles.TryGetValue(SpecToCopyName, out View specToCopyView);
            using (Transaction t = new Transaction(document, "Копирование спецификации"))
            {
                t.Start();
                ViewSchedule currentNewSpec = document.GetElement(specToCopyView.Duplicate(ViewDuplicateOption.Duplicate)) as ViewSchedule;
                currentNewSpec.Name = specToCopyView.Name + "1";
                ScheduleFilter sFilter = currentNewSpec.Definition.GetFilter(0);
                var sValue = currentNewSpec.Definition.GetFilters();
                foreach (ScheduleFilter item in sValue)
                {
                    if (item.FilterType == ScheduleFilterType.Equal)
                    {
                        MessageBox.Show(item.GetStringValue());
                    }

                }
                var testValue = sFilter.FilterType;
                var testValue01 = sFilter.FieldId;
                var testValue02 = sFilter.GetStringValue();


                //MessageBox.Show(testValue02);
                //ICollection<ElementId> allFilters = currentNewSpec.GetFilters();
                //foreach (var item in allFilters)
                //{
                //    MessageBox.Show(item.ToString());
                //}



                t.Commit();
            }

            //Ищем ведомость по имени

        }
        
    }
}
