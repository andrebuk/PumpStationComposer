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
        public XYZ pumpStationPoint1;//нижняя левая точка
        public XYZ pumpStationPoint3;//верхняя правая точка
        public double PumpStationHeight;
        public double PumpStationLength;
        public double PumpStationWidth;
        private double PumpStationHeightMin = 2500.0;
        public string BoxName = "Шкаф";
        public string PumpName = "Насос с двигателем";
        public string PlatformName = "Опорная конструкция";


        public bool onlyLink;
        public double deltaFromEquipmentToWall = 100 / 304.8;//расстояние от краев оборудования до оси стены

        public void CalcDimensions()

        {
            //Собираем все оборудование и определяем габариты
            List<double> maxXs = new List<double>();
            List<double> maxYs = new List<double>();
            List<double> minXs = new List<double>();
            List<double> minYs = new List<double>();
            List<double> minZs = new List<double>();
            List<double> maxZs = new List<double>();
            FilteredElementCollector allElements = new FilteredElementCollector(document);
            IList<Element> allElementID = allElements.OfCategory(BuiltInCategory.OST_MechanicalEquipment).WhereElementIsNotElementType().ToElements();
            foreach (Element item in allElementID)
            {
                BoundingBoxXYZ currentBB = item.get_BoundingBox(document.ActiveView);
                maxXs.Add(currentBB.Max.X);
                maxYs.Add(currentBB.Max.Y);
                maxZs.Add(currentBB.Max.Z);
                minXs.Add(currentBB.Min.X);
                minYs.Add(currentBB.Min.Y);
                minZs.Add(currentBB.Min.Z);
            }
            double maxX = (maxXs.Max() + deltaFromEquipmentToWall);
            double maxY = (maxYs.Max() + deltaFromEquipmentToWall);
            double minX = (minXs.Min() - deltaFromEquipmentToWall);
            double minY = (minYs.Min() - deltaFromEquipmentToWall);
            double maxZ = maxZs.Max();
            double minZ = minZs.Max();
            pumpStationPoint1 = new XYZ(minX, minY, 0);
            pumpStationPoint3 = new XYZ(maxX, maxY, 0);
            PumpStationLength = maxY - minY;
            PumpStationWidth = maxX - minX;
            PumpStationHeight = maxZ - minZ;
            if (PumpStationHeight < PumpStationHeightMin)
            {
                PumpStationHeight = PumpStationHeightMin;
            }




        }

        public void DrawPlatform()
        {
            this.CalcDimensions();
            //добавим платформу
            //Опорная конструкция
            SMFamily platform = new SMFamily(document);
            platform.insertPoint = pumpStationPoint1 + new XYZ(0.5 * (pumpStationPoint3.X - pumpStationPoint1.X), 0.5 * (pumpStationPoint3.Y - pumpStationPoint1.Y), 0);
            platform.familyTypeName = PlatformName;
            platform.insert();

            platform.setDoubleParameterValue("Длина", pumpStationPoint3.Y - pumpStationPoint1.Y);
            platform.setDoubleParameterValue("Шиирина", pumpStationPoint3.X - pumpStationPoint1.X);


            //-----

            /*
            double deltaForWalls = 150.0 / 304.8;
            //Удалим все линии
            FilteredElementCollector LinesCollector = new FilteredElementCollector(document);
            List<ElementId> Lines = (List<ElementId>)LinesCollector.OfCategory(BuiltInCategory.OST_Walls).ToElementIds();
            using (Transaction t = new Transaction(document, "Удаляем прежние структурные линии"))
            {
                t.Start();

                foreach (ElementId currentLine in Lines)
                {
                    try
                    {
                        document.Delete(currentLine);
                    }
                    catch (Exception)
                    {
                    }


                }
                t.Commit();
            }

            List<double> maxXs = new List<double>();
            List<double> maxYs = new List<double>();
            List<double> minXs = new List<double>();
            List<double> minYs = new List<double>();

            FilteredElementCollector allElements = new FilteredElementCollector(document);
            IList<Element> allElementID = allElements.OfCategory(BuiltInCategory.OST_MechanicalEquipment).WhereElementIsNotElementType().ToElements();
            foreach (Element item in allElementID)
            {
                BoundingBoxXYZ currentBB = item.get_BoundingBox(document.ActiveView);
                maxXs.Add(currentBB.Max.X);
                maxYs.Add(currentBB.Max.Y);
                minXs.Add(currentBB.Min.X);
                minYs.Add(currentBB.Min.Y);
            }
            double maxX = maxXs.Max() + deltaForWalls;
            double maxY = maxYs.Max() + deltaForWalls;
            double minX = minXs.Min() - deltaForWalls;
            double minY = minYs.Min() - deltaForWalls;

            XYZ point1 = new XYZ(minX, minY, 0);
            XYZ point2 = new XYZ(minX, maxY, 0);
            XYZ point3 = new XYZ(maxX, maxY, 0);
            XYZ point4 = new XYZ(maxX, minY, 0);
            IList<Curve> curveLoop = new List<Curve>();
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(point1, point2));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(point2, point3));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(point3, point4));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(point4, point1));

            using (Transaction t = new Transaction(document, "Пытаемся создать стены"))
            {
                t.Start();
                Level levelElement = document.ActiveView.GenLevel;
                ElementId leveId = levelElement.Id;
                foreach (Curve item in curveLoop)
                {
                    //ElementId currentId = item.Id;
                    Wall.Create(document, item, leveId, true);
                }

                t.Commit();

            }
            */

        }

        public void DrawWalls()
        {
            //this.CalcDimensions();

            //Удалим все линии
            //FilteredElementCollector LinesCollector = new FilteredElementCollector(document);
            //List<ElementId> Lines = (List<ElementId>)LinesCollector.OfCategory(BuiltInCategory.OST_Walls).ToElementIds();
            //using (Transaction t = new Transaction(document, "Удаляем прежние структурные линии"))
            //{
            //    t.Start();

            //    foreach (ElementId currentLine in Lines)
            //    {
            //        try
            //        {
            //            document.Delete(currentLine);
            //        }
            //        catch (Exception)
            //        {
            //        }


            //    }
            //    t.Commit();
            //}

            //List<double> maxXs = new List<double>();
            //List<double> maxYs = new List<double>();
            //List<double> minXs = new List<double>();
            //List<double> minYs = new List<double>();

            //FilteredElementCollector allElements = new FilteredElementCollector(document);
            //IList<Element> allElementID = allElements.OfCategory(BuiltInCategory.OST_MechanicalEquipment).WhereElementIsNotElementType().ToElements();
            //foreach (Element item in allElementID)
            //{
            //    BoundingBoxXYZ currentBB = item.get_BoundingBox(document.ActiveView);
            //    maxXs.Add(currentBB.Max.X);
            //    maxYs.Add(currentBB.Max.Y);
            //    minXs.Add(currentBB.Min.X);
            //    minYs.Add(currentBB.Min.Y);
            //}
            //double maxX = maxXs.Max() + deltaForWalls;
            //double maxY = maxYs.Max() + deltaForWalls;
            //double minX = minXs.Min() - deltaForWalls;
            //double minY = minYs.Min() - deltaForWalls;

            //XYZ point1 = new XYZ(minX, minY, 0);
            //XYZ point2 = new XYZ(minX, maxY, 0);
            //XYZ point3 = new XYZ(maxX, maxY, 0);
            //XYZ point4 = new XYZ(maxX, minY, 0);
            IList<Curve> curveLoop = new List<Curve>();
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(pumpStationPoint1, pumpStationPoint1 + new XYZ(PumpStationWidth, 0, 0)));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(pumpStationPoint1, pumpStationPoint1 + new XYZ(0, PumpStationLength, 0)));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(pumpStationPoint3, pumpStationPoint3 + new XYZ(-1 * PumpStationWidth, 0, 0)));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(pumpStationPoint3, pumpStationPoint3 + new XYZ(0, -1 * PumpStationLength, 0)));

            using (Transaction t = new Transaction(document, "Пытаемся создать стены"))
            {
                t.Start();
                Level levelElement = document.ActiveView.GenLevel;
                ElementId leveId = levelElement.Id;
                foreach (Curve item in curveLoop)
                {
                    //ElementId currentId = item.Id;
                    Wall.Create(document, item, leveId, true);

                }

                t.Commit();

            }


        }
        public void Create()
        {
            XYZ deltaXYZ = startPoint;
            if (onlyLink == true)
            {
                this.ClearAll();
                this.DrawPlatform();
                this.DrawWalls();
            }
            else
            {
                this.ClearAll();
                List<string> boxTypes = new List<string> { "МСС с ПЧ", "ШУН (с ПЛК)", "ВРУ", "ШСН", "ТСН", "ОПС" };
                foreach (string currentBoxType in boxTypes)
                {
                    PumpStationOptions.TryGetValue(currentBoxType, out bool value);
                    if (value)
                    {
                        SMFamily mss = new SMFamily(document);
                        mss.insertPoint = deltaXYZ;
                        mss.familyTypeName = BoxName;
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
                deltaXYZ = deltaXYZ + new XYZ(0, pumpLength / 304.8 / 2, 0);
                //разместим двигатель
                SMFamily pump = new SMFamily(document);
                pump.insertPoint = deltaXYZ;
                pump.familyTypeName = PumpName;
                pump.insert();
                string pumpDescription = "Насос ";
                pump.setStringParameterValue("ADSK_Наименование", pumpDescription);
                pump.setDoubleParameterValue("Насос длина", pumpLength / 304.8);
                pump.setDoubleParameterValue("Насос ширина", pumpWidth / 304.8);
                pump.setDoubleParameterValue("Насос высота", pumpHeight / 304.8);



            }


        }

        private void ClearAll()
        {
            // FilteredElementCollector allEquipments = new FilteredElementCollector(document);
            // ICollection<ElementId> allEquipmentsIds = allEquipments.OfCategory(BuiltInCategory.OST_MechanicalEquipment).WhereElementIsNotElementType().ToElementIds();
            //var Platform = new FilteredElementCollector(document).OfClass(typeof(FamilyInstance)).Where(x => x.Name == "Опорная конструкция") as ElementId;
            try
            {
                Element Platform = new FilteredElementCollector(document)
                    .OfCategory(BuiltInCategory.OST_MechanicalEquipment).
                    WhereElementIsNotElementType()
                    .Where(x => x.Name == "Опорная конструкция").First();
                using (Transaction t = new Transaction(document, "Удаление всего оборудования"))
                {
                    t.Start();
                    try
                    {
                        document.Delete(Platform.Id);
                    }
                    catch (Exception)
                    {

                    }
                    t.Commit();
                }
            }
            catch (Exception)
            { }



            FilteredElementCollector allWalls = new FilteredElementCollector(document);
            ICollection<ElementId> allWallsIds = allWalls.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElementIds();
            using (Transaction t = new Transaction(document, "Удаление всех стен"))
            {
                t.Start();
                try
                {
                    document.Delete(allWallsIds);
                }
                catch (Exception)
                {

                }

                t.Commit();
            }


        }
    }
}
