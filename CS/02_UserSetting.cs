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
    [Transaction(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class UserSetting : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            Dictionary<int, string> connTypeDict = new Dictionary<int,string>();
            PublicBlocks.ConnTypeCollector(doc, out connTypeDict);

            Dictionary<string, string> typeDict = new Dictionary<string, string>();

            using(FormUserSetting thisForm = new FormUserSetting(connTypeDict))
            {
                thisForm.ShowDialog();
                if(thisForm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                typeDict = thisForm.renewTypeDict;
            }

            return Result.Succeeded;
        }
    }
}

