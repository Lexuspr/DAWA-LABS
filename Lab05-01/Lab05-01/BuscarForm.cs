using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lab05_01
{
    public partial class BuscarForm : Form
    {
        SqlConnection conn;
        public BuscarForm(SqlConnection conn)

        {
            this.conn = conn;
            InitializeComponent();
           
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            conn.Open();
            
            
            conn.Close();
        }
    }
}
