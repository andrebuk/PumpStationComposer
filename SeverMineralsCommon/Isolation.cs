using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace CommonCommands
{
    [Transaction(TransactionMode.Manual)]
    class Isolation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidocument = commandData.Application.ActiveUIDocument;
            IS isolationObject = new IS(uidocument);
            isolationObject.isolate();
            return Result.Succeeded;
        }
    }
    class IS
    {
        public UIDocument uidocument;

        public IS(UIDocument UIdoc)
        {
            uidocument = UIdoc;
        }
        public void isolate()
        {
            View activeView = uidocument.Document.ActiveView;
            var b = uidocument.ActiveView.IsTemporaryHideIsolateActive();
            if (b == false)
            {
                using (Transaction t = new Transaction(uidocument.Document, "Isolate element"))
                {

                    var selectedElements = uidocument.Selection.GetElementIds();
                    if (selectedElements.Count > 0)
                    {
                        t.Start();
                        //activeView.IsolateElementsTemporary
                        activeView.IsolateElementsTemporary(selectedElements);
                        t.Commit();
                    }
                }
            }
            else
            {
                using (Transaction t = new Transaction(uidocument.Document, "Isolate element"))
                {
                    t.Start();
                    TemporaryViewMode tempView = TemporaryViewMode.TemporaryHideIsolate;
                    activeView.DisableTemporaryViewMode(tempView);
                    t.Commit();
                }
            }
        }
    }
}
