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
        //Далее точки прямоугольника, ограничиващего размещенное оборудование
        public XYZ pumpStationPartsPoint1;//нижняя левая точка
        public XYZ pumpStationPartsPoint3;//верхняя правая точка
        //Точки ограничивающие платформу
        public XYZ pumpStationPoint1;
        public XYZ pumpStationPoint2;
        public XYZ pumpStationPoint3;
        public XYZ pumpStationPoint4;
        //точки для построния перегородки
        public XYZ pumpStationPoint5;
        public XYZ pumpStationPoint6;

        public XYZ pumpCentralPoint;//точка вставки насоса
        public double PumpStationPartsHeight;
        public double PumpStationPartsLength;
        public double PumpStationPartsWidth;
        private double PumpStationPartsHeightMin = 2500.0;
        public string BoxName = "Шкаф";
        public string SystemName = "Опции модели";
        public string PumpName = "Насос с двигателем";
        public string PlatformName = "Опорная конструкция";
        //Параметры элемента для создания стен и перегородок
        public string WallElementName = "Перегородка";
        public string DoorElementName = "ДПГ 21-6";
        public string RoofName = "Крыша";
        public double WallThickness = 100.0;
        public double WallElementWidth = 60.0;
        public double WallElementLength = 211.0;

        public double PumpStationMaxWidth = 2450.0;
        public double stepForLength = 100.0;
        public double inch = 304.8;

        Wall wallForDoors;
        Wall wallPartition;



        public bool onlyLink;
        public double deltaFromEquipmentToWall = 0;//расстояние от краев оборудования до оси стены

        public void CalcPartsDimensions()

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
            double maxX = (maxXs.Max() + deltaFromEquipmentToWall / inch);
            double maxY = (maxYs.Max() + deltaFromEquipmentToWall / inch);
            double minX = (minXs.Min() - deltaFromEquipmentToWall / inch);
            double minY = (minYs.Min() - deltaFromEquipmentToWall / inch);
            double maxZ = maxZs.Max();
            double minZ = minZs.Max();

            //Считаем габариты только оборудования, без каких-либо прибавок размеров
            //pumpStationPartsPoint1 - нижняя левая точка
            //pumpStationPartsPoint3 - верхняя правая точка
            pumpStationPartsPoint1 = new XYZ(minX, minY, 0);
            pumpStationPartsPoint3 = new XYZ(maxX, maxY, 0);
            PumpStationPartsLength = maxY - minY;
            PumpStationPartsWidth = maxX - minX;
            PumpStationPartsHeight = maxZ - minZ;
            if (PumpStationPartsHeight < PumpStationPartsHeightMin)
            {
                PumpStationPartsHeight = PumpStationPartsHeightMin;
            }
        }

        public void CalcDimensions()
        {

            //проверка, не выходим ли мы за ширину контейнера
            double widthToLeft = pumpCentralPoint.X - pumpStationPartsPoint1.X + WallThickness / inch;
            double widthToRight = -pumpCentralPoint.X + pumpStationPartsPoint3.X + WallThickness / inch;
            if ((widthToLeft > PumpStationMaxWidth / 2 / inch) | (widthToRight > PumpStationMaxWidth / 2 / inch))
            {
                MessageBox.Show("Превышена максимально допустимая ширина контейнера " + PumpStationMaxWidth.ToString() + " мм");
            }

            //находим границы платформы. считаем что стены внутри платформы

            double xMin = pumpCentralPoint.X - PumpStationMaxWidth / 2 / inch;
            double xMax = pumpCentralPoint.X + PumpStationMaxWidth / 2 / inch;
            double yMax = pumpStationPartsPoint3.Y + WallThickness / inch;
            double yMin = pumpStationPartsPoint1.Y - WallThickness / inch;
            pumpStationPoint1 = new XYZ(xMin, yMin, 0);
            pumpStationPoint2 = new XYZ(xMin, yMax, 0);

            pumpStationPoint3 = new XYZ(xMax, yMax, 0);
            pumpStationPoint4 = new XYZ(xMax, yMin, 0);

            PumpDimensions.TryGetValue("Длина", out double pumpLength);
            double yMinOfPump = pumpCentralPoint.Y - pumpLength / 2 / inch - WallThickness / 2 / inch;
            pumpStationPoint5 = new XYZ(xMin, yMinOfPump, 0);
            pumpStationPoint6 = new XYZ(xMax, yMinOfPump, 0);







        }

        //метод возвращающий точку вставки насоса, так как вокруг его продольной оси будет строится вся конструкция
        //.Where(x => x.Name == "Опорная конструкция")

        public void getPumpCentralPoint()
        {
            FilteredElementCollector myCollector = new FilteredElementCollector(document);
            Element pump = myCollector.OfCategory(BuiltInCategory.OST_MechanicalEquipment).WhereElementIsNotElementType().Where(x => x.Name == "Насос с двигателем").First();

            FamilyInstance pumpFI = pump as FamilyInstance;

            pumpCentralPoint = ((LocationPoint)pumpFI?.Location)?.Point;
        }

        public void DrawPlatform()
        {
            //CalcPartsDimensions();
            //CalcDimensions();
            //добавим платформу
            //Опорная конструкция
            SMFamily platform = new SMFamily(document);
            platform.insertPoint = pumpStationPartsPoint1 + new XYZ(0.5 * (pumpStationPartsPoint3.X - pumpStationPartsPoint1.X), 0.5 * (pumpStationPartsPoint3.Y - pumpStationPartsPoint1.Y), 0);//внимание тесто
            platform.familyTypeName = PlatformName;
            platform.insert();
            platform.setDoubleParameterValue("Длина", pumpStationPartsPoint3.Y - pumpStationPartsPoint1.Y);
            platform.setDoubleParameterValue("Шиирина", pumpStationPartsPoint3.X - pumpStationPartsPoint1.X);

        }
        public void DrawPlatformNew()
        {
            CalcPartsDimensions();
            CalcDimensions();
            //добавим платформу
            //Опорная конструкция
            SMFamily platform = new SMFamily(document);
            platform.insertPoint = pumpStationPoint1 + new XYZ(0.5 * (pumpStationPoint3.X - pumpStationPoint1.X), 0.5 * (pumpStationPoint3.Y - pumpStationPoint1.Y), 0);//внимание тесто
            platform.familyTypeName = PlatformName;
            platform.insert();
            platform.setDoubleParameterValue("Длина", pumpStationPoint3.Y - pumpStationPoint1.Y);
            platform.setDoubleParameterValue("Шиирина", pumpStationPoint3.X - pumpStationPoint1.X);

        }
        public void DrawRoof()
        {
            //this.CalcPartsDimensions();
            //добавим платформу
            //Опорная конструкция
            SMFamily roof = new SMFamily(document);
            roof.insertPoint = pumpStationPoint1 + new XYZ(0.5 * (pumpStationPoint3.X - pumpStationPoint1.X), 0.5 * (pumpStationPoint3.Y - pumpStationPoint1.Y), PumpStationPartsHeight / inch);
            roof.familyTypeName = RoofName;
            roof.insert();
            roof.setDoubleParameterValue("Длина", pumpStationPoint3.Y - pumpStationPoint1.Y);
            roof.setDoubleParameterValue("Шиирина", pumpStationPoint3.X - pumpStationPoint1.X);

        }

        public void DrawWalls()
        {
            //Найдем Id нужной нам стены (пока считаем что она одна ее и ищем)
            FilteredElementCollector allWalls = new FilteredElementCollector(document);
            ICollection<ElementId> allWallsIds = allWalls.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType().ToElementIds();
            ElementId myWallId = allWallsIds.First();


            IList<Curve> curveLoop = new List<Curve>();
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(pumpStationPoint1 + new XYZ(WallThickness / inch / 2, WallThickness / inch / 2, 0),
                pumpStationPoint2 + new XYZ(WallThickness / inch / 2, -WallThickness / inch / 2, 0)));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(pumpStationPoint2 + new XYZ(WallThickness / inch / 2, -WallThickness / inch / 2, 0),
                pumpStationPoint3 + new XYZ(-WallThickness / inch / 2, -WallThickness / inch / 2, 0)));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(pumpStationPoint3 + new XYZ(-WallThickness / inch / 2, -WallThickness / inch / 2, 0),
                pumpStationPoint4 + new XYZ(-WallThickness / inch / 2, WallThickness / inch / 2, 0)));
            curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(pumpStationPoint4 + new XYZ(-WallThickness / inch / 2, WallThickness / inch / 2, 0),
                pumpStationPoint1 + new XYZ(WallThickness / inch / 2, WallThickness / inch / 2, 0)));

            //нужно ли строить перегородку
            PumpStationOptions.TryGetValue("Отсек высоковольтного оборудования", out bool OVVO_build);
            if (OVVO_build)
            {
                curveLoop.Add(Autodesk.Revit.DB.Line.CreateBound(pumpStationPoint5, pumpStationPoint6));
            }

            Level levelElement = document.ActiveView.GenLevel;
            ElementId leveId = levelElement.Id;
            int wallIndex = 0;

            using (Transaction t = new Transaction(document, "Пытаемся создать стены"))
            {
                t.Start();

                foreach (Curve item in curveLoop)
                {
                    wallIndex += 1;

                    Wall currentWall = Wall.Create(document, item, myWallId, leveId, 2500 / inch, 0.0, true, true);
                    if (wallIndex == 3)
                    {
                        wallForDoors = currentWall;
                    }
                    if (wallIndex == 5)
                    {
                        wallPartition = currentWall;
                    }

                }

                t.Commit();

            }


        }


        public void DrawWallFromElements(XYZ p1, XYZ p2)

        {
            CalcPoints pointsForLeftWall = new CalcPoints();
            pointsForLeftWall.startPoint = p1;
            pointsForLeftWall.endPoint = p2;
            pointsForLeftWall.segmentLength = WallElementLength / 304.8;
            List<XYZ> pointsForWallCreation = pointsForLeftWall.calcPOintsList();

            foreach (XYZ item in pointsForWallCreation)
            {
                SMFamily currentWallElement = new SMFamily(document);
                currentWallElement.familyTypeName = WallElementName;
                currentWallElement.insertPoint = item;
                currentWallElement.angle = pointsForLeftWall.CalcAngle();
                currentWallElement.insert();
                currentWallElement.setDoubleParameterValue("Высота", PumpStationPartsHeight / 304.8);

            }


        }
        public void DrawWallsFromElements()
        {
            DrawWallFromElements(pumpStationPartsPoint1, pumpStationPartsPoint1 + new XYZ(0, PumpStationPartsLength, 0));
            DrawWallFromElements(pumpStationPartsPoint1 + new XYZ(0, PumpStationPartsLength, 0), pumpStationPartsPoint3);
            DrawWallFromElements(pumpStationPartsPoint3, pumpStationPartsPoint3 - new XYZ(0, PumpStationPartsLength, 0));
            DrawWallFromElements(pumpStationPartsPoint1 + new XYZ(PumpStationPartsWidth, 0, 0), pumpStationPartsPoint1);
        }

        public void DrawDoors()

        {
            Level levelElement = document.ActiveView.GenLevel;
            PumpStationOptions.TryGetValue("Отсек высоковольтного оборудования", out bool OVVO_build);
            if (OVVO_build)
            {

            }
            else
            {
                SMFamily door = new SMFamily(document);
                door.insertPoint = pumpStationPoint3+ new XYZ(0,1000,0);
                door.familyTypeName = DoorElementName;
                door.baseElement = wallForDoors;
                door.baseLevel = levelElement;
                door.insert();

            }
        }
        public void Create(bool type)
        {
            XYZ deltaXYZ = startPoint;
            //строим контейнер вокруг оборудования
            if (type == false)
            {
                this.ClearAll();
                this.getPumpCentralPoint();
                this.DrawPlatformNew();
                //this.DrawPlatform();
                this.DrawWalls();
                this.DrawRoof();
                this.DrawDoors();
            }
            else
            //Размещаем оборудование
            {
                this.ClearAll();
                //Размещение шкафов
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
                //Размещение систем
                XYZ SystemInsertPoint = new XYZ();
                List<string> systemTypes = new List<string> { "Система вентиляции", "Система отопления", "Система кондиционирования",
                "ЗРА на всасе с электроприводом", "ЗРА на нагнетании с электроприводом и позиционером","ЗРА на патрубке слива напорной магистрали с электроприводом",
                "Обратный клапан с демфером","Датчик давления на всасе","Датчик давления на нагнетании","Расходомер на нагнетании","Трубопровод на всасе 5м",
                "Плавающий водозаборный оголовок","Стрела для подъема всасывающей трубы"};
                foreach (string currentSystemType in systemTypes)
                {
                    PumpStationOptions.TryGetValue(currentSystemType, out bool value);
                    if (value)
                    {
                        SMFamily mss = new SMFamily(document);
                        mss.insertPoint = SystemInsertPoint;
                        mss.familyTypeName = SystemName;
                        mss.insert();
                        mss.setStringParameterValue("ADSK_Наименование", currentSystemType);
                        SystemInsertPoint = SystemInsertPoint + new XYZ(1 / 304.8, 0, 0);

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



            FilteredElementCollector allGMs = new FilteredElementCollector(document);
            ICollection<ElementId> allGMsIds = allGMs.OfCategory(BuiltInCategory.OST_GenericModel).WhereElementIsNotElementType().ToElementIds();
            using (Transaction t = new Transaction(document, "Удаление всех стен"))
            {
                t.Start();
                try
                {
                    document.Delete(allGMsIds);
                }
                catch (Exception)
                {

                }

                t.Commit();
            }

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
