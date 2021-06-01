using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;



namespace SeverMineralsCommon
{
    class SMViewSchedule

    {
        public Document document;
        public SMViewSchedule(Document doc)
        {
            document = doc;
        }

        public Dictionary<string, View> GetAllViewScheduleNameView()

        {
            Dictionary<string, View> allViewSchedule = new Dictionary<string, View>();
            FilteredElementCollector fes = new FilteredElementCollector(document);
            fes.OfClass(typeof(ViewSchedule));
            foreach (Element viewSchedule  in fes)
            {
                allViewSchedule.Add(viewSchedule.Name, viewSchedule as View);
            }

            return allViewSchedule;
        }
        public Dictionary<ElementId, string> GetAllViewScheduleIdName()

        {
            Dictionary<ElementId, string> allViewSchedule = new Dictionary<ElementId, string>();
            FilteredElementCollector fes = new FilteredElementCollector(document);
            fes.OfClass(typeof(ViewSchedule));
            foreach (Element viewSchedule  in fes)
            {
                allViewSchedule.Add(viewSchedule.Id, viewSchedule.Name );
                

            }

            return allViewSchedule;
        }
    }
}
