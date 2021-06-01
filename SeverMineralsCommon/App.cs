using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using Autodesk.Windows;

namespace SeverMineralsCommon
{
    class App : IExternalApplication
    {
        // define a method that will create our tab and button

        static void AddTab(UIControlledApplication application, string tabName)
        {
            bool needToCreateTab = true;
            foreach (RibbonTab item in Autodesk.Windows.ComponentManager.Ribbon.Tabs)
            {
                if (item.Name == tabName)
                {
                    needToCreateTab = false;
                    break;
                }

            }
            if (needToCreateTab)
            { application.CreateRibbonTab(tabName); }

        }
        static Autodesk.Revit.UI.RibbonPanel createRibbonPanel(UIControlledApplication application, string tabName, string ribbonPanelName)
        {
            AddTab(application, tabName);
            return application.CreateRibbonPanel(tabName, ribbonPanelName);
        }
        static void AddButtonToRibbonPanelPSC(UIControlledApplication application)
        {
            String tabName = "Sever Minerals";
            String ribbonPanelName = "Инструменты";
            AddTab(application, tabName);
            Autodesk.Revit.UI.RibbonPanel ribbonPanel = createRibbonPanel(application, tabName, ribbonPanelName);
            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            // create push button for CurveTotalLength
            PushButtonData b1Data = new PushButtonData(
                "cmdPSC",
               // "Total" + System.Environment.NewLine + "  Length  ",
               "Станция9",
                thisAssemblyPath,
                "SeverMineralsCommon.Main");
            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Конструктор2 насосной станции";
            BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/SeverMineralsCommon;component/Resources/PumpStationComposer.png"));
            pb1.LargeImage = pb1Image;
        }
        static void AddButtonToRibbonPanelIsolate(UIControlledApplication application)
        {
            String tabName = "Sever Minerals";
            String ribbonPanelName = "Прочее";
            AddTab(application, tabName);
            Autodesk.Revit.UI.RibbonPanel ribbonPanel = createRibbonPanel(application, tabName, ribbonPanelName);
            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            // create push button for CurveTotalLength
            PushButtonData b1Data = new PushButtonData(
                "cmdIzolate",
               // "Total" + System.Environment.NewLine + "  Length  ",
               "Изоляция объектов",
                thisAssemblyPath,
                "SeverMineralsCommon.Isolation");
            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Изолировать выбранный объект";
            BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/SeverMineralsCommon;component/Resources/IsolateButton.png"));
            pb1.LargeImage = pb1Image;
        }
        static void AddButtonToRibbonPanelSCopy(UIControlledApplication application)
        {
            String tabName = "Sever Minerals";
            String ribbonPanelName = "Спецификации";
            AddTab(application, tabName);
            Autodesk.Revit.UI.RibbonPanel ribbonPanel = createRibbonPanel(application, tabName, ribbonPanelName);
            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            // create push button for CurveTotalLength
            PushButtonData b1Data = new PushButtonData(
                "cmdSCopy",
               // "Total" + System.Environment.NewLine + "  Length  ",
               "Копирование спецификации",
                thisAssemblyPath,
                "SeverMineralsCommon.MainSCopy");
            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Скопировать спецификацию";
            BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/SeverMineralsCommon;component/Resources/SPF_icon.png"));
            pb1.LargeImage = pb1Image;
        }
        //Команда по копированию спецификаций
        static void AddButtonToRibbonPanelSPF(UIControlledApplication application)
        {
            String tabName = "Sever Minerals";
            String ribbonPanelName = "Параметры";
            AddTab(application, tabName);
            Autodesk.Revit.UI.RibbonPanel ribbonPanel = createRibbonPanel(application, tabName, ribbonPanelName);
            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            // create push button for CurveTotalLength
            PushButtonData b1Data = new PushButtonData(
                "cmdSPF",
               // "Total" + System.Environment.NewLine + "  Length  ",
               "Общие",
                thisAssemblyPath,
                "CommonCommands.MainSPF");
            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Изолировать выбранный объект";
            BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/SeverMineralsCommon;component/Resources/SPF_icon.png"));
            pb1.LargeImage = pb1Image;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // do nothing
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // call our method that will load up our toolbar

            //Autodesk.Revit.UI.TaskDialog.Show("Сообщение", Environment.UserName);
            if (Environment.UserName == "bukhvan")
            {
                AddButtonToRibbonPanelPSC(application);

            }
            AddButtonToRibbonPanelIsolate(application);
            AddButtonToRibbonPanelSPF(application);
            AddButtonToRibbonPanelSCopy(application);

            return Result.Succeeded;
        }
    }
}