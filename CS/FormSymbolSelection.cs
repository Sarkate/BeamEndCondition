using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EndRelease
{
    public partial class FormSymbolSelection : Form
    {
        public FormSymbolSelection(List<string> revitConnTypeList)
        {
            InitializeComponent();
            foreach(var s in revitConnTypeList)
            {
                lbExConnType.Items.Add(s);
            }
        }

        public string selectedType = "";

        private void butnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void butnOk_Click(object sender, EventArgs e)
        {
            selectedType = lbExConnType.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
        }
    }
}
