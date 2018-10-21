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

namespace lab06_01
{
    public partial class ManPerson : Form
    {
        SqlConnection conn;
        DataSet ds = new DataSet();
        DataTable tablePerson = new DataTable();
        Boolean btn_estado;

        public ManPerson()
        {
            
            InitializeComponent();
        }

        private void ManPerson_Load(object sender, EventArgs e)
        {
            String str = "Server=localhost\\SQLSERVER2017;DataBase=School;Integrated Security=true;";
            conn = new SqlConnection(str);
            btn_estado = true;
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM Person";
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;

            adapter.Fill(ds, "Person");

            tablePerson = ds.Tables["Person"];

            dgvListado.DataSource = tablePerson;
            dgvListado.Update();
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("InsertPerson", conn);

            cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50, "LastName");
            cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50, "FirstName");
            cmd.Parameters.Add("@HireDate", SqlDbType.Date).SourceColumn = "HireDate";
            cmd.Parameters.Add("@EnrollmentDate", SqlDbType.Date).SourceColumn =  "EnrollmentDate";

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = cmd;
            adapter.InsertCommand.CommandType = CommandType.StoredProcedure;

            DataRow fila = tablePerson.NewRow();
            if (dtpHireDate.Checked && !dtpEnrollmentDate.Checked)
            {
                fila["LastName"] = txtLastName.Text;
                fila["FirstName"] = txtFirstName.Text;
                fila["HireDate"] = dtpHireDate.Text;
                fila["EnrollmentDate"] = DBNull.Value;
            } else if (!dtpHireDate.Checked && dtpEnrollmentDate.Checked)
            {
                fila["LastName"] = txtLastName.Text;
                fila["FirstName"] = txtFirstName.Text;
                fila["HireDate"] = DBNull.Value;
                fila["EnrollmentDate"] = dtpEnrollmentDate.Checked;
            } else if (!dtpHireDate.Checked && !dtpEnrollmentDate.Checked)
            {
                fila["LastName"] = txtLastName.Text;
                fila["FirstName"] = txtFirstName.Text;
                fila["HireDate"] = DBNull.Value;
                fila["EnrollmentDate"] = DBNull.Value;
            }
            else
            {
                fila["LastName"] = txtLastName.Text;
                fila["FirstName"] = txtFirstName.Text;
                fila["HireDate"] = dtpHireDate.Text;
                fila["EnrollmentDate"] = dtpEnrollmentDate.Text;
            }
            

            tablePerson.Rows.Add(fila);

            adapter.Update(tablePerson);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("UpdatePerson", conn);

            cmd.Parameters.Add("@PersonID", SqlDbType.VarChar).SourceColumn = "PersonID";
            cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50, "LastName");
            cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50, "FirstName");
            cmd.Parameters.Add("@HireDate", SqlDbType.Date).SourceColumn = "HireDate";
            cmd.Parameters.Add("@EnrollmentDate", SqlDbType.Date).SourceColumn = "EnrollmentDate";

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = cmd;
            adapter.UpdateCommand.CommandType = CommandType.StoredProcedure;

            DataRow[] fila = tablePerson.Select("PersonID = '" + txtPersonID.Text + "'");
            fila[0]["LastName"] = txtLastName.Text;
            fila[0]["FirstName"] = txtFirstName.Text;
            fila[0]["HireDate"] = dtpHireDate.Text;
            fila[0]["EnrollmentDate"] = dtpEnrollmentDate.Text;

            adapter.Update(tablePerson);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("DeletePerson", conn);
            cmd.Parameters.Add("@PersonID", SqlDbType.VarChar).SourceColumn = "PersonID";

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.DeleteCommand = cmd;
            adapter.DeleteCommand.CommandType = CommandType.StoredProcedure;

            DataRow[] fila = tablePerson.Select("PersonID = '" + txtPersonID.Text + "'");

            tablePerson.Rows.Remove(fila[0]);

            adapter.Update(tablePerson);

        }

        private void btnOrdApe_Click(object sender, EventArgs e)
        {
            DataView dv = new DataView(tablePerson);
            dv.Sort = "LastName ASC";
            dgvListado.DataSource = dv;
        }

        private void btnBuscarCod_Click(object sender, EventArgs e)
        {
            DataView dv = new DataView(tablePerson);
            dv.RowFilter = "PersonID = '" + txtPersonID.Text + "'";
            dgvListado.DataSource = dv;
        }

        private void dgvListado_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvListado.SelectedRows.Count > 0)
            {
                txtPersonID.Text = dgvListado.SelectedRows[0].Cells[0].Value.ToString();
                txtFirstName.Text = dgvListado.SelectedRows[0].Cells[2].Value.ToString();
                txtLastName.Text = dgvListado.SelectedRows[0].Cells[1].Value.ToString();
                String hireDate = dgvListado.SelectedRows[0].Cells[3].Value.ToString();
                if (String.IsNullOrEmpty(hireDate))
                    dtpHireDate.Checked = false;
                else
                    dtpHireDate.Text = hireDate;
                String enrollmenDate= dgvListado.SelectedRows[0].Cells[4].Value.ToString();
                if (String.IsNullOrEmpty(enrollmenDate))
                    dtpEnrollmentDate.Checked = false;
                else
                    dtpEnrollmentDate.Text = enrollmenDate;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            btn_estado = !btn_estado;
            if (btn_estado)
            {
                btnBuscar.Text = "Buscar";
                lblContrato.Text = "Contrato";
                lblInscripcion.Text = "Inscripcion";
                lblBuscar.Visible = false;
                //txtBuscar.Visible = false;
                cbxCriterio.Visible = false;
                txtPersonID.Enabled = true;
                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                dtpEnrollmentDate.Enabled = true;
                dtpHireDate.Enabled = true;

                switch (cbxCriterio.SelectedIndex)
                {
                    case 0:
                        String sql = "SELECT * FROM Person WHERE PersonID LIKE '%" + txtPersonID.Text + "%'";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;

                        adapter.Fill(ds, "Person");

                        tablePerson = ds.Tables["Person"];

                        dgvListado.DataSource = tablePerson;
                        dgvListado.Update();

                        break;

                    case 1:
                        sql = "SELECT * FROM Person WHERE FirstName LIKE '%" + txtFirstName.Text + "%'";
                        cmd = new SqlCommand(sql, conn);

                        adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;

                        adapter.Fill(ds, "Person");

                        tablePerson = ds.Tables["Person"];

                        dgvListado.DataSource = tablePerson;
                        dgvListado.Update();
                        break;

                    case 2:
                        sql = "SELECT * FROM Person WHERE LastName LIKE '%" + txtLastName.Text + "%'";
                        cmd = new SqlCommand(sql, conn);

                        adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;

                        adapter.Fill(ds, "Person");

                        tablePerson = ds.Tables["Person"];

                        dgvListado.DataSource = tablePerson;
                        dgvListado.Update();
                        break;

                    case 3:
                        sql = "SELECT * FROM Person WHERE HireDate BETWEEN '" + dtpHireDate.Text + "' AND '" + dtpEnrollmentDate.Text + "'";
                        cmd = new SqlCommand(sql, conn);

                        adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;

                        adapter.Fill(ds, "Person");

                        tablePerson = ds.Tables["Person"];

                        dgvListado.DataSource = tablePerson;
                        dgvListado.Update();
                        break;

                    case 4:
                        sql = "SELECT * FROM Person WHERE EnrollmentDate BETWEEN '" + dtpHireDate.Text + "' AND '" + dtpEnrollmentDate.Text + "'";
                        cmd = new SqlCommand(sql, conn);

                        adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;

                        adapter.Fill(ds, "Person");

                        tablePerson = ds.Tables["Person"];

                        dgvListado.DataSource = tablePerson;
                        dgvListado.Update();
                        break;

                }
            }
            else
            {
                btnBuscar.Text = "GO";
                lblContrato.Text = "Contrato";
                lblInscripcion.Text = "Inscripcion";
                //txtPersonID.Enabled = false;
                //txtFirstName.Enabled = false;
                //txtLastName.Enabled = false;
                //dtpEnrollmentDate.Enabled = false;
                //dtpHireDate.Enabled = false;
                //txtBuscar.Visible = true;
                cbxCriterio.Visible = true;
                lblBuscar.Visible = true;
            }
        }

        private void cbxCriterio_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxCriterio.SelectedIndex)
            {
                case 0:
                    lblContrato.Text = "Contrato";
                    lblInscripcion.Text = "Inscripcion";
                    txtPersonID.Enabled = true;
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                    dtpEnrollmentDate.Enabled = false;
                    dtpHireDate.Enabled = false;
                    break;
                case 1:
                    lblContrato.Text = "Contrato";
                    lblInscripcion.Text = "Inscripcion";
                    txtPersonID.Enabled = false;
                    txtFirstName.Enabled = true;
                    txtLastName.Enabled = false;
                    dtpEnrollmentDate.Enabled = false;
                    dtpHireDate.Enabled = false;
                    break;
                case 2:
                    txtPersonID.Enabled = false;
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = true;
                    dtpEnrollmentDate.Enabled = false;
                    dtpHireDate.Enabled = false;
                    break;
                case 3:
                    lblContrato.Text = "Desde";
                    lblInscripcion.Text = "Hasta";
                    txtPersonID.Enabled = false;
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                    dtpEnrollmentDate.Enabled = true;
                    dtpHireDate.Enabled = true;
                    break;
                case 4:
                    lblContrato.Text = "Desde";
                    lblInscripcion.Text = "Hasta";
                    txtPersonID.Enabled = false;
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                    dtpEnrollmentDate.Enabled = true;
                    dtpHireDate.Enabled = true;
                    break;
            }
        }
    }
}
