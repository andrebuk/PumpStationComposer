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
    public partial class SPF : Window
    {
        public Document document;
        public UIDocument uiDocument;
        public DefinitionFile defFile;
        public DefinitionGroups AllParamaterGroups;
        public Definitions definitions;
        public Dictionary<string, BuiltInParameterGroup> familyParameterGroupDict;



        public SPF(Document doc, UIDocument uidoc)
        {
            document = doc;
            uiDocument = uidoc;
            defFile = uiDocument.Application.Application.OpenSharedParameterFile();

            //TaskDialog.Show("Ошибка", "Не указан или не найден файл общих параметров");

            AllParamaterGroups = defFile.Groups;

            familyParameterGroupDict = this.FamilyParameterGroupDict();

            InitializeComponent();

        }
        //метод нахождения всех параметров в семействе

        public Dictionary<string, FamilyParameter> GetAllParameters()

        {
            FamilyManager familyManager = document.FamilyManager;
            var allParameters = familyManager.GetParameters();
            Dictionary<string, FamilyParameter> result = new Dictionary<string, FamilyParameter>();
            foreach (FamilyParameter fp in allParameters)
            {
                if (fp.IsShared)
                {
                    Guid guid = fp.GUID;
                    string fpName = fp.Definition.Name;
                    result.Add(fpName, fp);
                }

            }
            return result;

        }
        public void setGroups()
        {
            //FamilyManager familyManager = document.FamilyManager;
            // var fileName = uiDocument.Application.Application.SharedParametersFilename;

            foreach (var currentDefGroup in AllParamaterGroups)
            {
                this.Groups.Items.Add(currentDefGroup.Name);
            }
        }

        public void setFamilyParamaterGroups()
        {
            //this.ParameterGroupDict();

            Dictionary<string, BuiltInParameterGroup>.KeyCollection AllFamilyParamaterGroups = familyParameterGroupDict.Keys;

            foreach (string item in AllFamilyParamaterGroups)
            {
                this.ParameterGroup.Items.Add(item);
            }
            ParameterGroup.SelectedIndex = 0;
        }

        private void Buttontype_Click(object sender, RoutedEventArgs e)
        {
            SetParameters(false);
        }
        private void Buttoninst_Click(object sender, RoutedEventArgs e)
        {
            SetParameters(true);
        }

        private void SetParameters(bool isInstance)
        {
            FamilyManager familyManager = document.FamilyManager;
            System.Collections.IList selectedParameters = this.Parameters.SelectedItems;
            Dictionary<string, FamilyParameter> allParam = this.GetAllParameters();
            List<string>  allParamnames = new List<string> (allParam.Keys);

            //DefinitionGroup myGroup = allDefGroups.get_Item(this.Groups.SelectedItem.ToString());
            //definitions = myGroup.Definitions;
            foreach (object currentParameter in selectedParameters)
            {
                ExternalDefinition eDef = definitions.get_Item(currentParameter.ToString()) as ExternalDefinition;
                using (Transaction t = new Transaction(document, "Назначение общего параметра"))
                {
                    t.Start();

                    if (allParamnames.Contains(currentParameter.ToString()))
                        //если параметр уже есть  - то меняем его тип
                    {
                        allParam.TryGetValue(currentParameter.ToString(), out FamilyParameter existingParam);
                        //Пока считаем что ошибка возникает только из за того что такой общий уже существует
                        if (isInstance)
                        {
                            
                            familyManager.MakeInstance(existingParam);

                        }
                        else
                        {
                            familyManager.MakeType(existingParam);
                        }
                    }
                   //Если параметр не существует, то создаем его
                    else

                    {
                        try
                        {
                            familyParameterGroupDict.TryGetValue((string)this.ParameterGroup.SelectedItem, out BuiltInParameterGroup currentFamilyParameterGroup);


                            familyManager.AddParameter(eDef, currentFamilyParameterGroup, isInstance);

                        }
                        catch (Exception e)
                        {
                            //найдем сам параметр
                            //FamilyParameter existingParam;


                        }
                    }


                   
                    t.Commit();

                }

            }
            //this.Parameters.Items.Clear();
            this.Parameters.UnselectAll();


        }

        private void Groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Parameters.Items.Clear();
            DefinitionGroup myGroup = AllParamaterGroups.get_Item(this.Groups.SelectedItem.ToString());
            definitions = myGroup.Definitions;
            foreach (Definition item in definitions)
            {
                this.Parameters.Items.Add(item.Name);
            }


        }

        private Dictionary<string, BuiltInParameterGroup> FamilyParameterGroupDict()
        {
            //Dictionary<string, string> parameterGroupDict = new Dictionary<string, string>;
            Dictionary<string, BuiltInParameterGroup> result = new Dictionary<string, BuiltInParameterGroup>();
            result.Add("Общие", BuiltInParameterGroup.PG_GENERAL);
            result.Add("Данные", BuiltInParameterGroup.PG_DATA);
            result.Add("Текст", BuiltInParameterGroup.PG_TEXT);
            result.Add("Графика", BuiltInParameterGroup.PG_GRAPHICS);
            result.Add("Свойства модели", BuiltInParameterGroup.PG_ADSK_MODEL_PROPERTIES);
            result.Add("PG_LENGTH", BuiltInParameterGroup.PG_LENGTH);




            return result;

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    GetAllParameters();
        //}

        public void ShowAllParameters()

        {
            FamilyManager fm = document.FamilyManager;
            IList<FamilyParameter> allParam = fm.GetParameters();
            foreach (FamilyParameter currentParameter in allParam)
            {
                currentParameter.ToString();
            }


        }

    }
}
