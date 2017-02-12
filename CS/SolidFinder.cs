using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Analysis;
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
    public class SolidFinder
    {
        //Build solid
        public void incrementIntercetor(Document doc, XYZ startOfInterest, Parameter zOffsetVar, out ElementId columnId, out ElementId beamId)
        {
            columnId = null;
            beamId = null;
            
            double radius = 0;
            double limit = 0.75;
            double zOffset = zOffsetVar.AsDouble();
            //lower arc center
            double centerZ = (startOfInterest.Z + zOffset) - 0.25;
            XYZ arcCenter = new XYZ(startOfInterest.X, startOfInterest.Y, centerZ);

             //Build a solid cylinder
            for (radius = .125; radius < limit; radius = radius + 0.1)
            {
                // Create a vertical half-circle loop in the frame location.
                List<CurveLoop> curveloops = new List<CurveLoop>();
                CurveLoop circle = new CurveLoop();
                circle.Append(Arc.Create(arcCenter, radius, 0, Math.PI, XYZ.BasisX, XYZ.BasisY));
                circle.Append(Arc.Create(arcCenter, radius, Math.PI, 2 * Math.PI, XYZ.BasisX, XYZ.BasisY));
                curveloops.Add(circle);
                
                Solid cylinder = GeometryCreationUtilities.CreateExtrusionGeometry(curveloops, XYZ.BasisZ, (0.25));
                //PaintSolid(commandData, cylinder, 5);
                //Find column
                IEnumerable<Element> columns = new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilyInstance))
                    .OfCategory(BuiltInCategory.OST_StructuralColumns)
                    .WherePasses(new ElementIntersectsSolidFilter(cylinder));

                if (columns.Count() > 0)
                {
                    
                    foreach(Element e in columns)
                    {
                        FamilyInstance fi = e as FamilyInstance;
                        FamilySymbol fs = fi.Symbol;
                        columnId =e.Id;
                    }
                    break;
                }

                //Find beam
                IEnumerable<Element> beams = new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilyInstance))
                    .OfCategory(BuiltInCategory.OST_StructuralFraming)
                    .WherePasses(new ElementIntersectsSolidFilter(cylinder));

                if (beams.Count() > 0)
                {
                    
                    foreach (Element e in beams)
                    {
                        FamilyInstance fi = e as FamilyInstance;
                        FamilySymbol fs = fi.Symbol;
                        beamId = e.Id;
                    }
                    break;
                }

            }
            //End of loop
        }

        public void solidBeamFinder(Document doc, XYZ startOfInterest, double solidHeight, out List<ElementId> beamIds)
        {
            beamIds= new List<ElementId>();
            double radius = 0.1;

             XYZ arcCenter = new XYZ(startOfInterest.X, startOfInterest.Y, startOfInterest.Z);

            //Build a solid cylinder
                // Create a vertical half-circle loop in the frame location.
                List<CurveLoop> curveloops = new List<CurveLoop>();
                CurveLoop circle = new CurveLoop();
                circle.Append(Arc.Create(arcCenter, radius, 0, Math.PI, XYZ.BasisX, XYZ.BasisY));
                circle.Append(Arc.Create(arcCenter, radius, Math.PI, 2 * Math.PI, XYZ.BasisX, XYZ.BasisY));
                curveloops.Add(circle);

                Solid cylinder = GeometryCreationUtilities.CreateExtrusionGeometry(curveloops, XYZ.BasisZ, (solidHeight));
                //PaintSolid(commandData, cylinder, 5);
               //Find beam
                IEnumerable<Element> beams = new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilyInstance))
                    .OfCategory(BuiltInCategory.OST_StructuralFraming)
                    .WherePasses(new ElementIntersectsSolidFilter(cylinder));

                if (beams.Count() > 0)
                {
                   
                    foreach (Element e in beams)
                    {

                        beamIds.Add(e.Id);
                    }
                }

        }

        public static void PaintSolid(Document doc,Solid solid, double value)
        {
            Autodesk.Revit.DB.View view = doc.ActiveView;

            if (view.AnalysisDisplayStyleId == ElementId.InvalidElementId)
            {
                CreateAVFDisplayStyle(doc, view);
            }

            SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(view);
            if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(view, 1);
            sfm.Clear();

            IList<int> results = sfm.GetRegisteredResults();

            AnalysisResultSchema resultSchema1 = new AnalysisResultSchema("PaintedSolid2", "Description");
            int schemaId = sfm.RegisterResult(resultSchema1);

            foreach (Face face in solid.Faces)
            {
                int idx = sfm.AddSpatialFieldPrimitive(face, Transform.Identity);

                IList<UV> uvPts = new List<UV>();
                List<double> doubleList = new List<double>();
                IList<ValueAtPoint> valList = new List<ValueAtPoint>();
                BoundingBoxUV bb = face.GetBoundingBox();
                uvPts.Add(bb.Min);
                doubleList.Add(value);
                valList.Add(new ValueAtPoint(doubleList));
                FieldDomainPointsByUV pnts = new FieldDomainPointsByUV(uvPts);
                FieldValues vals = new FieldValues(valList);

                sfm.UpdateSpatialFieldPrimitive(idx, pnts, vals, schemaId);
            }

         //   UIDocument uidoc = new UIDocument(doc);
            //uidoc.RefreshActiveView();
        }

        private static void CreateAVFDisplayStyle(Document doc, Autodesk.Revit.DB.View view)
        {
            Transaction t = new Transaction(doc, "Create AVF style");
            t.Start();
            AnalysisDisplayColoredSurfaceSettings coloredSurfaceSettings = new AnalysisDisplayColoredSurfaceSettings();
            coloredSurfaceSettings.ShowGridLines = true;
            AnalysisDisplayColorSettings colorSettings = new AnalysisDisplayColorSettings();
            AnalysisDisplayLegendSettings legendSettings = new AnalysisDisplayLegendSettings();
            legendSettings.ShowLegend = false;
            string name = "Paint Solid";
            AnalysisDisplayStyle analysisDisplayStyle = new FilteredElementCollector(doc).OfClass(typeof(AnalysisDisplayStyle)).Cast<AnalysisDisplayStyle>().FirstOrDefault(q => q.Name == name);
            if (analysisDisplayStyle == null)
                analysisDisplayStyle = AnalysisDisplayStyle.CreateAnalysisDisplayStyle(doc, name, coloredSurfaceSettings, colorSettings, legendSettings);

            view.AnalysisDisplayStyleId = analysisDisplayStyle.Id;
            t.Commit();
        }
    }
}


 

