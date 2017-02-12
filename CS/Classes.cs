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
    public class Beam
    {
        public ElementId id { get; set; }
        public string name { get; set; }
        public XYZ end0 { get; set; }
        public XYZ end1 { get; set; }
        public Level refLevel { get; set; }
        public double startOffset { get; set; }
        public double endOffset { get; set; }
        public double zOffset { get; set; }
        public ElementId startConn{ get; set; }
        public ElementId endConn{ get; set; }
    }

    public class BeamEndPoint
        {
            public Beam hostBeam { get; set; }
            public int pointNumber { get; set; }
            public XYZ pointLocation { get; set; }
            public bool isFixPoint { get; set; }
            public bool isShearPoint { get; set; }
        }

    public class ViewItem
    {
        public ElementId id { get; set; }
        public string name { get; set; }
        public Level hostLevel { get; set; }
        public Level topClipLevel { get; set; }
        public double topClipOffset { get; set; }
        public Level cutLevel { get; set; }
        public double cutOffset { get; set; }
        public Level bottClipLevel { get; set; }
        public double bottClipOffset { get; set; }
        public Level depthCLipLevel { get; set; }
        public double depthClipOffset { get; set; }
        public double topToCut { get; set; }
        public double cutToBott { get; set; }
        public double bottToDepth { get; set; }
        public double totalRange { get; set; }
    }

}

