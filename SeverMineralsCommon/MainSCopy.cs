using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;


namespace SeverMineralsCommon

{
    [Transaction(TransactionMode.Manual)]
    class MainSCopy : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document document = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidocument = commandData.Application.ActiveUIDocument;



            SCopy sCopy = new SCopy(document, uidocument);
            sCopy.SpecSelection();
            //spf.setFamilyParamaterGroups();
            sCopy.ShowDialog();


            return Result.Succeeded;
        }
    }
}
