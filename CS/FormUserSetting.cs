using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace EndRelease
{
    public partial class FormUserSetting : Form
    {
        public List<string> revitConnTypeList = new List<string>();

        public Dictionary<string, string> renewTypeDict = new Dictionary<string, string>();

        public FormUserSetting(Dictionary<int, string> connTypeDict)
        {
            InitializeComponent();

            Dictionary<string, string> xmlDict = new Dictionary<string, string>();
           
            try
            {
                XmlProcessor.xmlReader(PublicBlocks.settingFileName, out xmlDict);
                if (xmlDict.Count() != 0)
                {
                    tbMoment.Text = xmlDict.Where(a => a.Key.Contains("MOMENT")).Select(a => a.Value).FirstOrDefault();
                    tbCantilever.Text = xmlDict.Where(a => a.Key.Contains("CANTILEVER")).Select(a => a.Value).FirstOrDefault();
                }
            }
            catch
            {
                 Dictionary<string, string> backUpTypeDict = new Dictionary<string, string>();
                 backUpTypeDict.Add("MOMENT", "MOMENT CONNECTION");
                 backUpTypeDict.Add("CANTILEVER", "CANTILEVER CONNECTION");
                XmlProcessor.xmlWriter(PublicBlocks.settingFileName, backUpTypeDict);
            }

            //this will past to connection type selection list.
            foreach(var v in connTypeDict)
            {
                revitConnTypeList.Add(v.Value);
            }
        }

        private void popForSelection(List<string> revitConnTypeList, out string selectedType)
        {
            using (FormSymbolSelection formSelection = new FormSymbolSelection(revitConnTypeList))
            {
                formSelection.ShowDialog();
                if (formSelection.DialogResult == DialogResult.Cancel)
                {
                    Close();
                }
                selectedType = formSelection.selectedType;
            }
        }

        private void butnSelMom_Click(object sender, EventArgs e)
        {
            string selTypeMom = "";
            popForSelection(revitConnTypeList, out selTypeMom);
            tbMoment.Text = selTypeMom;
        }

        private void butnSelCan_Click(object sender, EventArgs e)
        {
            string selType = "";
            popForSelection(revitConnTypeList, out selType);
            tbCantilever.Text = selType;
        }

        private void butnOk_Click(object sender, EventArgs e)
        {
            renewTypeDict.Add("MOMENT", tbMoment.Text.ToUpper());
            renewTypeDict.Add("CANTILEVER", tbCantilever.Text.ToUpper());
            XmlProcessor.xmlWriter(PublicBlocks.settingFileName, renewTypeDict);
            this.DialogResult = DialogResult.OK;
        }

        private void butnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
