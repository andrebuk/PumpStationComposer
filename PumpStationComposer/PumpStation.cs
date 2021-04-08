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
        public Dictionary<string, double> PumpDimensions;
        public Dictionary<string, int> PumpNozzle;
        public XYZ startPoint;

        public void Create()
        {
            XYZ deltaXYZ = startPoint;
            List<string> boxTypes = new List<string> { "МСС с ПЧ", "ШУН (с ПЛК)", "ВРУ","ШСН","ТСН","ОПС" };
            foreach (string currentBoxType in boxTypes)
            {
                PumpStationOptions.TryGetValue(currentBoxType, out bool value);
                if (value)
                {
                    SMFamily mss = new SMFamily(document);
                    mss.insertPoint = deltaXYZ;
                    mss.familyTypeName = "Шкаф";
                    mss.insert();
                    string boxesTypeToParameter = "Шкаф " + currentBoxType;
                    mss.setStringParameterValue("ADSK_Наименование", boxesTypeToParameter);

                    deltaXYZ = deltaXYZ + new XYZ(0, 600 / 304.8, 0);

                }

            }
            //вернем точку на край последнего шкафа
            PumpDimensions.TryGetValue("Длина", out double pumpLength);
            PumpDimensions.TryGetValue("Ширина", out double pumpWidth);
            PumpDimensions.TryGetValue("Высота", out double pumpHeight);
            deltaXYZ = deltaXYZ  + new XYZ(0, pumpLength / 304.8/2, 0);
            //разместим двигатель
            SMFamily pump = new SMFamily(document);
            pump.insertPoint = deltaXYZ;
            pump.familyTypeName = "Насос с двигателем";
            pump.insert();
            string pumpDescription = "Насос ";
            pump.setStringParameterValue("ADSK_Наименование",pumpDescription);
            pump.setDoubleParameterValue("Насос длина", pumpLength/304.8);
            pump.setDoubleParameterValue("Насос ширина", pumpWidth / 304.8);
            pump.setDoubleParameterValue("Насос высота", pumpHeight/304.8);


        }

    }
}
