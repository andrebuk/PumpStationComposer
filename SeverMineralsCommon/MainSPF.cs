using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;


namespace CommonCommands
{
    [Transaction(TransactionMode.Manual)]
    class MainSPF : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document document = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidocument = commandData.Application.ActiveUIDocument;


            DefinitionFile defFile = uidocument.Application.Application.OpenSharedParameterFile();
            if (defFile != null)
            {
                SPF spf = new SPF(document, uidocument);
                spf.setGroups();
                spf.setFamilyParamaterGroups();
                spf.ShowDialog();
            }
            else
            {
                TaskDialog.Show("Ошибка", "Не указан или не найден файл общих параметров");
            }

            return Result.Succeeded;
        }
    }
}
