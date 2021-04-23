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


namespace CommonCommands
{
    /// <summary>
    /// Логика взаимодействия для SPF.xaml
    /// </summary>
    public partial class SPF : Window
    {
        public Document document;
        public UIDocument uiDocument;
        public DefinitionFile defFile;
        public DefinitionGroups allDefGroups;
        public Definitions definitions;
        public SPF(Document doc, UIDocument uidoc)
        {
            document = doc;
            uiDocument = uidoc;
            defFile = uiDocument.Application.Application.OpenSharedParameterFile();
            allDefGroups = defFile.Groups;
            
            InitializeComponent();
        }
        public void setGroups()
        {
            //FamilyManager familyManager = document.FamilyManager;
            // var fileName = uiDocument.Application.Application.SharedParametersFilename;

            foreach (var currentDefGroup in allDefGroups)
            {
                this.Groups.Items.Add(currentDefGroup.Name);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FamilyManager familyManager = document.FamilyManager;
            System.Collections.IList selectedParameters = this.Parameters.SelectedItems;
            //DefinitionGroup myGroup = allDefGroups.get_Item(this.Groups.SelectedItem.ToString());
            //definitions = myGroup.Definitions;
            foreach (object currentParameter in selectedParameters)
            {
                ExternalDefinition eDef = definitions.get_Item(currentParameter.ToString()) as ExternalDefinition;
                using (Transaction t = new Transaction(document, "Назначение общего параметра"))
                {
                    t.Start();
                    try
                    {
                        familyManager.AddParameter(eDef, BuiltInParameterGroup.PG_GENERAL, this.paraInst.IsChecked==true);
                    }
                    catch (Exception)
                    {
                    }
                    t.Commit();

                }

            }

            TaskDialog.Show("Revit message", "df");
        }

        private void Groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Parameters.Items.Clear();
            DefinitionGroup myGroup = allDefGroups.get_Item(this.Groups.SelectedItem.ToString());
            definitions = myGroup.Definitions;
            foreach (Definition item in definitions)
            {
                this.Parameters.Items.Add(item.Name);
            }


        }

        private void paraType_Checked(object sender, RoutedEventArgs e)
        {
            this.paraInst.IsChecked = false;
        }

        private void paraInst_Checked(object sender, RoutedEventArgs e)
        {
            this.paraType.IsChecked = false;
        }
    }
}
