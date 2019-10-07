using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube3Editor
{
    public partial class XmlViewForm : Form
    {
        public XmlViewForm(System.Xml.Linq.XDocument xDoc)
        {
            InitializeComponent();

            xmlEditor.Text = xDoc.ToString();
            xmlEditor.AllowXmlFormatting = true;
        }
    }
}
