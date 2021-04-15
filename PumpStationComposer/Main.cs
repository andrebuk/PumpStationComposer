using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;


namespace PumpStationComposer
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document document = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidocument = commandData.Application.ActiveUIDocument;
            PumpConfigurator pc = new PumpConfigurator(document, uidocument);
            
            pc.ShowDialog();
            return Result.Succeeded;

        }
    }
}
