using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace PumpStationComposer
{
    public class FamilyInstanceInsert 
    {
        public Boolean checkIfFamilyExists(string FamilyName)
        { 



            return true;
        }
        public static Element FindElementByName(
                                                Document doc,
                                                Type targetType,
                                                string targetName)
                {
                    return new FilteredElementCollector(doc)
                      .OfClass(targetType)
                      .FirstOrDefault<Element>(
                        e => e.Name.Equals(targetName));
                }
    }
}
