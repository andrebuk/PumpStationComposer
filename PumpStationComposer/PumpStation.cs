using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PumpStationComposer
{
    class PumpStation
    {
        public Document document;
        public Dictionary<string, bool> PumpStationOptions;
        public Dictionary<string, int> PumpDimensions;
        public Dictionary<string, int> PumpNozzle;
        public XYZ startPoint;

        public void Create()
        {
            PumpStationOptions.TryGetValue("Шкаф МСС", out bool value);
            if (value)
            {
                SMFamily mss = new SMFamily(document);
                mss.insertPoint = startPoint;
                mss.familyTypeName = "Шкаф";
                mss.insert();
                mss.setParameterValue("ADSK_Наименование", "Шкаф МСС");
            }
            //String ts = "Шкаф";
            //SMFamily test = new SMFamily(document);
            //FamilySymbol testFS = test.IsExist(ts, document);
            //if (testFS != null)
            //{
            //    using (Transaction t = new Transaction(document, "Создание объектов насосной станции"))
            //    {

            //        PumpStationOptions.TryGetValue("Шкаф МСС", out bool value);
            //        if (value)
            //        {
            //            t.Start();
            //            FamilyInstance instance = document.Create.NewFamilyInstance(new XYZ(0, 0, 0), testFS, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
            //            Element el = instance as Element;

            //            el.LookupParameter("ADSK_Наименование").Set("Шкаф МСС");
            //            t.Commit();
            //        }


            //    }
            //}
        }
        
    }
}
