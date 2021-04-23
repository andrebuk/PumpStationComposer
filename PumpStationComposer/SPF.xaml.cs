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
        public SPF(Document doc, UIDocument uidoc)
        {
            document = doc;
            uiDocument = uidoc;

            InitializeComponent();
        }
        public void setGroups()
        {


            //FamilyManager familyManager = document.FamilyManager;

            var fileName = uiDocument.Application.Application.SharedParametersFilename;
            DefinitionFile defFile = uiDocument.Application.Application.OpenSharedParameterFile();

            DefinitionGroups allDefGroups = defFile.Groups;
            foreach (var currentDefGroup in allDefGroups)
            {
                this.Groups.Items.Add(currentDefGroup.Name);
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TaskDialog.Show("Revit message", "Done!");
        }
    }
}
