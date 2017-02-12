using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.ExternalService;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndRelease
{
    [Transaction(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class SetMaterial : IExternalCommand
    {
        #region Get Material
        //collect materials from current Revit project return their element ID  
        private void GetMaterial(Document doc, out int hssMat, out int boxMat, out int wfMat, out int a36, out int gbMat)
        {
            //initialaze material id valuiable
            hssMat = 0;
            boxMat = 0;
            wfMat = 0;
            a36 = 0;
            gbMat = 0;
            //Collecte all project material
            FilteredElementCollector matCollector = new FilteredElementCollector(doc).OfClass(typeof(Material));
            //Extreact material that is going to use
            foreach (Element e in matCollector)
            {
                string matName = e.Name.ToUpper();
                if (matName.ToUpper().Equals("STEEL ASTM A992"))
                {
                    wfMat = e.Id.IntegerValue;
                }

                if (matName.ToUpper().Contains("STEEL ASTM A500, GRADE C, RECTANGULAR AND SQUARE"))
                {
                    hssMat = e.Id.IntegerValue;
                }

                if (matName.ToUpper().Equals("STEEL ASTM A572"))
                {
                    boxMat = e.Id.IntegerValue;
                }

                if (matName.ToUpper().Equals("STEEL ASTM A36"))
                {
                    a36 = e.Id.IntegerValue;
                }

                if (matName.ToUpper().Equals("CONCRETE - CAST-IN-PLACE CONCRETE"))
                {
                    gbMat = e.Id.IntegerValue;
                }
            }
        }
        #endregion

        #region Set Material
        private void setMat(Element e, int materialId)
        {
            Parameter matPara = e.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM);
            matPara.Set(new ElementId(materialId));
        }
        #endregion

        #region Set Ksi
        private void SetKsi(Element e, FamilySymbol fs)
        {
            double ksiDoub = 0;
            //lookup ksi according to section name
            if (!fs.Name.Contains("BOX") && fs.Name.Contains("3/4") || fs.Name.Contains("7/8"))
            {
                ksiDoub = 52.7;
            }

            if (fs.Name.Contains("BOX"))
            {
                ksiDoub = 55;
            }

            if (!fs.Name.Contains("BOX") && !fs.Name.Contains("3/4") && !fs.Name.Contains("7/8"))
            {
                ksiDoub = 56;
            }

            //Get Fy parameter and make sure the it is exist
            IList<Parameter> fyParameters = e.GetParameters("Fy");
            try
            {
                if (fyParameters != null)
                {
                    Parameter ksi = e.LookupParameter("Fy");
                    {
                        ksi.Set(ksiDoub);
                    }
                }
            }
            catch
            {
                return;
            }

        }
        #endregion

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication app = commandData.Application;
            Document doc = app.ActiveUIDocument.Document;

            #region Get material ids
            int hssMat = 0;
            int boxMat = 0;
            int wfMat = 0;
            int a36 = 0;
            int gbMat = 0;
            GetMaterial(doc, out hssMat, out boxMat, out wfMat, out a36, out gbMat);
            #endregion

            //Collect columns and beams
            FilteredElementCollector colCollector = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_StructuralColumns);
            FilteredElementCollector bmCollector = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_StructuralFraming);

            using (Transaction t = new Transaction(doc, "Set Parameter"))
            {
                t.Start();

                #region Set column parameter
                foreach (Element e in colCollector)
                {
                    FamilyInstance fi = e as FamilyInstance;
                    FamilySymbol fs = fi.Symbol;
                    Family fam = fs.Family;
                    string familyName = fam.Name.ToUpper().ToUpper();
                    if (familyName.Contains("CONX MOMENT"))
                    {
                        SetKsi(e, fs);
                    }

                    if (familyName.Contains("HSS"))
                    {
                        setMat(e, hssMat);

                    }
                    else if (familyName.Contains("BOX"))
                    {
                        setMat(e, boxMat);
                    }

                    else
                    {
                        setMat(e, a36);
                    }
                }
            #endregion

                #region Set beam parameter
                foreach (Element e in bmCollector)
                {
                    FamilyInstance fi = e as FamilyInstance;
                    FamilySymbol fs = fi.Symbol;
                    Family fam = fs.Family;
                    string familyName = fam.Name.ToUpper().ToUpper();
                    if (familyName.Contains("W-WIDE FLANGE"))
                    {
                        setMat(e, wfMat);
                    }

                    else if (familyName.Contains("HSS"))
                    {
                        setMat(e, hssMat);

                    }

                    else if (familyName.Contains("CONCRETE GB") || familyName.Contains("GRADE BEAM"))
                    {
                        setMat(e, gbMat);
                    }

                    else
                    {
                        setMat(e, a36);
                    }
                }
                #endregion

                t.Commit();
            }
            return Result.Succeeded;
        }
    }
}
