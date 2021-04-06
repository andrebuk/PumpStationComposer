using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
//test comment33333333
namespace PumpStationComposer
{
    public class SMFamily
    {
        public Document document;
        public Element element;
        public string familyTypeName;
        public XYZ insertPoint;
        //public string paramname;
        //public string paramValue;
        public SMFamily(Document doc)
        {
            document = doc;
        }
        public FamilySymbol IsExist(string familyTypeName, Document document)
        {
            FilteredElementCollector collector = new FilteredElementCollector(document);
            ICollection<Element> collection = collector.WhereElementIsNotElementType().ToElements();
            List<FamilySymbol> allFamilySymbols = new FilteredElementCollector(document)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>().ToList();
            FamilySymbol result = null;
            foreach (var currentFamilySymbol in allFamilySymbols)
            {
                if (currentFamilySymbol.Name == familyTypeName)
                {
                    result = currentFamilySymbol;
                }
            }

            return result;
        }

        public void insert()
        {
            SMFamily test = new SMFamily(document);
            FamilySymbol testFS = test.IsExist(familyTypeName, document);
            if (testFS != null)
            {
                using (Transaction t = new Transaction(document, "Создание объектов насосной станции"))
                {
                    t.Start();
                    FamilyInstance instance = document.Create.NewFamilyInstance(insertPoint, testFS, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    element = instance as Element;
                    t.Commit();
                    //return element;
                    //el.LookupParameter("ADSK_Наименование").Set("Шкаф МСС");

                }
            }
            //else
            //{
            //    return null;
            //}
        }
        public void setParameterValue(string parameterName, string parameterValue)
        {
            string transactionName = "Назначение параметра " + parameterName;
            using (Transaction t = new Transaction(document, transactionName))
            {
                t.Start();
                element.LookupParameter(parameterName).Set(parameterValue);
                t.Commit();
            }

        }
    }
}
