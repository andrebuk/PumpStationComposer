using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace PumpStationComposer
{
    public class SMFamily
    {
        public Document document;
        public string familyTypeName;
        public SMFamily(Document doc)
        {
            document = doc;
        }
        public FamilySymbol IsExist (string familyTypeName, Document document)
        {
            FilteredElementCollector collector = new FilteredElementCollector(document);
            ICollection<Element> collection = collector.WhereElementIsNotElementType().ToElements();
            List<FamilySymbol> allFamilySymbols = new FilteredElementCollector(document)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>().ToList();
            FamilySymbol result=null;
            foreach (var currentFamilySymbol  in allFamilySymbols)
            {
                if (currentFamilySymbol.Name == familyTypeName)
                {
                    result = currentFamilySymbol;
                }
            }
            
             return result;
        }
    }
}
