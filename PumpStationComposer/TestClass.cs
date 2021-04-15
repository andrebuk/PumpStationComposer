using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace PumpStationComposer
{
    class TestClass
    {
        public UIDocument uidocument;

        public TestClass(UIDocument UIdoc)
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
                    t.Start();
                    var selectedElement = uidocument.Selection.GetElementIds().First();
                   activeView.IsolateElementTemporary(selectedElement);
                    t.Commit();
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
