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

    public class PublicBlocks
    {
        public static void ConnTypeCollector(Document doc, out Dictionary<int, string> revitConnTypeDict)
        {
            revitConnTypeDict = new Dictionary<int, string>();
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(StructuralConnectionType));
            if(collector.Count() !=0)
            {
                 foreach(Element e in collector)
                {
                     string applyTo = (e as StructuralConnectionType).ApplyTo.ToString();
                     if(applyTo ==("BeamsAndBraces"))
                     {
                         revitConnTypeDict.Add(e.Id.IntegerValue,e.Name.ToUpper());
                     }
                }
            }
        }

        public static void ConnectionTypeSelector(Dictionary<String,string> xmlConnTypeDict, Dictionary<int, string> revitConnTypeDict, string targetType, out int resultTypeId)
        {
            resultTypeId = 0;
            bool typeFound = false;
            string resultType = "";

            foreach (var v in xmlConnTypeDict)
            {
                if (targetType.Equals(v.Key))
                {
                    resultType = v.Value;
                }
            }

            foreach(var v in revitConnTypeDict)
            {
                if (resultType.Equals(v.Value))
                {
                    typeFound = true;
                    resultTypeId = v.Key;
                }
            }

            if (typeFound == false)
            {
                TaskDialog.Show("Info", "Connection Type " + targetType + " cannot be found is Revit Setting");
            }

        }

        public static void BeamGenerator(Document doc, ElementId eId, out Beam beam)
        {
            beam = new Beam();
            Element e = doc.GetElement(eId);

            FamilyInstance fi = e as FamilyInstance;
            LocationCurve locCurve = fi.Location as LocationCurve;

            beam.id = eId;
            beam.name = e.Name;
            beam.end0 = (locCurve.Curve as Line).GetEndPoint(0);
            beam.end1 = (locCurve.Curve as Line).GetEndPoint(1);
            beam.startOffset = e.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION).AsDouble();
            beam.endOffset = e.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION).AsDouble();
            beam.refLevel = doc.GetElement(e.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM).AsElementId()) as Level;
            beam.zOffset = e.get_Parameter(BuiltInParameter.Z_OFFSET_VALUE).AsDouble();
            beam.startConn = e.get_Parameter(BuiltInParameter.STRUCT_CONNECTION_BEAM_START).AsElementId();
            beam.endConn = e.get_Parameter(BuiltInParameter.STRUCT_CONNECTION_BEAM_END).AsElementId();
        }

        public static void BeamFinder(Document doc, XYZ point, double viewHeight, out List<Beam> beamList)
        {
            beamList = new List<Beam>();
            //Get Arc Center Point
            Double px = point.X;
            Double py = point.Y;
            Double pz = point.Z-viewHeight;
            XYZ arcCenter = new XYZ(px, py, pz);
            double solidHeight = viewHeight;

            List<ElementId> beamIdList = new List<ElementId>();

            SolidFinder solidBuilder = new SolidFinder();
            solidBuilder.solidBeamFinder(doc, arcCenter, solidHeight, out beamIdList);

            if (beamIdList == null)
            {
                TaskDialog.Show("Error Message: ", "Please place the cursors over a beam");
            }
            else
            {
                foreach (ElementId eId in beamIdList)
                {
                    Beam beam = new Beam();
                    BeamGenerator(doc, eId, out beam);
                    beamList.Add(beam);
                }
            }
        }

        public static void ConnectionModifier(Document doc, Element e, int connSymbolId, int targetEnd)
        {
                Parameter MFstart = e.get_Parameter(BuiltInParameter.STRUCT_CONNECTION_BEAM_START);
                Parameter MFend = e.get_Parameter(BuiltInParameter.STRUCT_CONNECTION_BEAM_END);

                if (targetEnd == 0)//Set Start
                {
                     MFstart.Set(new ElementId(connSymbolId));
                }
                 else//Set End
                {
                    MFend.Set(new ElementId(connSymbolId));
                }
                doc.Regenerate();
        }

        public static void TargetEndFinder(XYZ pickPoint, XYZ end0, XYZ end1, out int targetEnd)
        {
            targetEnd = 0;
            XYZ pXy = new XYZ(pickPoint.X, pickPoint.Y, 0);
            XYZ end0Xy = new XYZ(end0.X, end0.Y, 0);
            XYZ end1Xy = new XYZ(end1.X, end1.Y, 0);
            Line L0 = Line.CreateBound(pXy,end0Xy);
            Line L1 = Line.CreateBound(pXy,end1Xy);
            double Length0 = L0.ApproximateLength;
            double Length1 = L1.ApproximateLength;

            if (Length0 > Length1)
                targetEnd = 1;
        }

        public static void getCurrentView(Document doc, out ViewItem viewItem)
        {
            viewItem = new ViewItem();
            double topPoint;
            double cutPoint;
            double bottPoint;
            double depthPoint;
            double topToCut;
            double cutToBott;
            double bottToDepth;
            double totalRange;

            double originElevOff;
            double originElev = 0;
            originPosition(doc, out originElev);
            originElevOff = 0 - originElev;

            ViewPlan vP = doc.ActiveView as ViewPlan;
            PlanViewRange viewRange = vP.GetViewRange();
            Level hostLevel = doc.GetElement(vP.LevelId) as Level;

            #region view range info
            //Get top clip data
            Level topClipLevel = doc.GetElement(viewRange.GetLevelId(PlanViewPlane.TopClipPlane) as ElementId) as Level;
            double topClipElev = topClipLevel.Elevation + originElevOff;
            double topClipOffset = viewRange.GetOffset(PlanViewPlane.TopClipPlane);
            topPoint = topClipElev + topClipOffset;

            //Get cut plane
            Level cutLevel = doc.GetElement(viewRange.GetLevelId(PlanViewPlane.CutPlane) as ElementId) as Level;
            double cutElevation = cutLevel.Elevation + originElevOff;
            double cutOffset = viewRange.GetOffset(PlanViewPlane.CutPlane);
            cutPoint = cutElevation + cutOffset;

            //Get bottom clip data
            Level bottClipLevel = doc.GetElement(viewRange.GetLevelId(PlanViewPlane.BottomClipPlane) as ElementId) as Level;
            double bottClipElev = bottClipLevel.Elevation + originElevOff;
            double bottClipOffset = viewRange.GetOffset(PlanViewPlane.BottomClipPlane);
            bottPoint = bottClipElev + bottClipOffset;

            //Get depth clip data
            Level depthClipLevel = doc.GetElement(viewRange.GetLevelId(PlanViewPlane.ViewDepthPlane) as ElementId) as Level;
            double depthClipElev = bottClipLevel.Elevation + originElevOff;
            double depthClipOffset = viewRange.GetOffset(PlanViewPlane.ViewDepthPlane);
            depthPoint = depthClipElev + depthClipOffset;

            topToCut = topPoint - cutPoint;
            cutToBott = cutPoint - bottPoint;
            bottToDepth = bottPoint - depthPoint;
            totalRange = topPoint - depthPoint;
            #endregion

            #region add data to ViewItem.
            viewItem.id = vP.Id;
            viewItem.name = vP.Name;
            viewItem.hostLevel = hostLevel;
            viewItem.topClipLevel = topClipLevel;
            viewItem.topClipOffset = topClipOffset;
            viewItem.cutLevel = cutLevel;
            viewItem.cutOffset = cutOffset;
            viewItem.bottClipLevel = bottClipLevel;
            viewItem.bottClipOffset = bottClipOffset;
            viewItem.depthCLipLevel = depthClipLevel;
            viewItem.depthClipOffset = depthClipOffset;
            viewItem.topToCut = topToCut;
            viewItem.cutToBott = cutToBott;
            viewItem.bottToDepth = bottToDepth;
            viewItem.totalRange = totalRange;
            #endregion
        }

        private static void originPosition(Document doc, out double originElev)
        {
            ProjectLocation projectLocation = doc.ActiveProjectLocation;
            XYZ origin = new XYZ(0, 0, 0);
            ProjectPosition position = projectLocation.get_ProjectPosition(origin);
            originElev = position.Elevation;
        }

        public static void ConnectionTypePlacer(Document doc, UIApplication uiApp, string targetType)
        {
            //COLLECT ALL CONNECTION TYPE
            Dictionary<int, string> revitConnTypeDict = new Dictionary<int, string>();
            PublicBlocks.ConnTypeCollector(doc, out revitConnTypeDict);

            Dictionary<string, string> xmlConnTypeDict = new Dictionary<string, string>();
            XmlProcessor.xmlReader(PublicBlocks.settingFileName, out xmlConnTypeDict);

            int connTypeId = -1;
            if(targetType != "PIN")
            {
                PublicBlocks.ConnectionTypeSelector(xmlConnTypeDict, revitConnTypeDict, targetType, out connTypeId);
            }

            //Get View info
            ViewItem currentView = new ViewItem();
            PublicBlocks.getCurrentView(doc, out currentView);
            int KeepRun = 0;
            while (KeepRun == 0)
            {
                Selection sel = uiApp.ActiveUIDocument.Selection;
                XYZ point = null;
                #region//Pick point
                try
                {
                    point = sel.PickPoint();
                }
                catch (Exception)
                {
                    return;
                }
                #endregion

                List<Beam> beamList = new List<Beam>();
                PublicBlocks.BeamFinder(doc, point, currentView.cutToBott, out beamList);

                using (Transaction t = new Transaction(doc, "Set end connection type"))
                {
                    t.Start();
                    foreach (Beam bm in beamList)
                    {
                        Element e = doc.GetElement(bm.id);
                        int targetEnd = 0;
                        PublicBlocks.TargetEndFinder(point, bm.end0, bm.end1, out targetEnd);
                        PublicBlocks.ConnectionModifier(doc, e, connTypeId, targetEnd);
                    }
                    t.Commit();
                }
            }
        }

        public static string settingFileName
        {
            get
            {
                string name = @"C:\GlennTools\EndRelease\Setting\setting.xml";
                return name;
            }
        }

        public static void KeyEvent(Keys k, out string type)
        {
            type = "MOMENT";
            int index = 0;

            if (k == Keys.Space)
            {
                index = 1;
                type = "CANTILEVER";
            }
            if (index == 1 && k == Keys.Space)
            {
                index = 0;
                type = "MOMENT";
            }
        }
    }
}

