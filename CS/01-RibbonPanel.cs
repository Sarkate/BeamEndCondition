using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Windows.Input;

namespace EndRelease
{
    public class MainPanel : IExternalApplication
    {
        //Create Tool Tab
        public Result OnStartup(UIControlledApplication application)
        {
            //Create a custom tab
            string tabName = "Connection Tools";
            application.CreateRibbonTab(tabName);

            //Creat Push Buttons
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData butnPinPlacerData = new PushButtonData("Pin Connection", "Pin \n Connection", thisAssemblyPath, "EndRelease.PinPlacer");
            PushButtonData butnMomPlacerData = new PushButtonData("Moment Connection", "Moment \n Connection", thisAssemblyPath, "EndRelease.MomentPlacer");
            PushButtonData butnCanPlacerData = new PushButtonData("Cantilever Connection", "Cantilever \n Connection", thisAssemblyPath, "EndRelease.CantileverPlacer");
            PushButtonData butnSettingData = new PushButtonData("Setting", "Setting", thisAssemblyPath, "EndRelease.UserSetting");

            //Creat Ribbon Panel(Connection)
            RibbonPanel connectionPanel = application.CreateRibbonPanel(tabName, "Connection Type");
            PushButton butnPinPlacer = connectionPanel.AddItem(butnPinPlacerData) as PushButton;
            PushButton butnMomPlacer = connectionPanel.AddItem(butnMomPlacerData) as PushButton;
            PushButton butnCanPlacer = connectionPanel.AddItem(butnCanPlacerData) as PushButton;

            //Creat Ribbon Panel(Setting)
            RibbonPanel settingPanel = application.CreateRibbonPanel(tabName, "Setting");
            PushButton buttSetting = settingPanel.AddItem(butnSettingData) as PushButton;

            //Get Image
            BitmapImage imagePin = new BitmapImage();
            imagePin.BeginInit();
            imagePin.UriSource = new Uri(@"C:\GlennTools\Images\PIN.png");
            imagePin.EndInit();
            butnPinPlacer.LargeImage = imagePin;

            BitmapImage imageMom = new BitmapImage();
            imageMom.BeginInit();
            imageMom.UriSource = new Uri(@"C:\GlennTools\Images\MOMENT FRAME.png");
            imageMom.EndInit();
            butnMomPlacer.LargeImage = imageMom;

            BitmapImage imageCan = new BitmapImage();
            imageCan.BeginInit();
            imageCan.UriSource = new Uri(@"C:\GlennTools\Images\CANTILEVER FRAME.png");
            imageCan.EndInit();
            butnCanPlacer.LargeImage = imageCan;

            BitmapImage imageSetting = new BitmapImage();
            imageSetting.BeginInit();
            imageSetting.UriSource = new Uri(@"C:\GlennTools\Images\setting.png");
            imageSetting.EndInit();
            buttSetting.LargeImage = imageSetting;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            return Result.Succeeded;
        }
    }
}
