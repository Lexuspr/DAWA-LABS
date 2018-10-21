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
    public partial class ManPerson : Form
    {
        SqlConnection conn;
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
            conn.Open();
            String sql = "SELECT * FROM Person";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            dgvListado.DataSource = dt;
            dgvListado.Refresh();
            conn.Close();
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            if (dtpEnrollmentDate.Text == "")
            {
                MessageBox.Show("Los siguientes campos no pueden estar vacios: Inscripcion");
            } else if (dtpHireDate.Text == "")
            {
                MessageBox.Show("Los siguientes campos no pueden estar vacios: Contrato");
            } else
            {
                conn.Open();
                String sp = "InsertPerson";
                SqlCommand cmd = new SqlCommand(sp, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@HireDate", dtpHireDate.Text);
                cmd.Parameters.AddWithValue("@EnrollmentDate", dtpEnrollmentDate.Text);

                int codigo = Convert.ToInt32(cmd.ExecuteScalar());
                MessageBox.Show("Se ha registrado nueva persona con el codigo: " + codigo);
                conn.Close();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            conn.Open();
            String sp = "UpdatePerson";
            SqlCommand cmd = new SqlCommand(sp, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PersonID", txtPersonID.Text);
            cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
            cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
            cmd.Parameters.AddWithValue("@HireDate", dtpHireDate.Text);
            cmd.Parameters.AddWithValue("@EnrollmentDate", dtpEnrollmentDate.Text);

            int resultado = cmd.ExecuteNonQuery();
            if (resultado > 0)
                MessageBox.Show("Se ha modificado el registro correctamente");

            conn.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            conn.Open();
            String sp = "DeletePerson";
            SqlCommand cmd = new SqlCommand(sp, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PersonID", txtPersonID.Text);

            int resultado = cmd.ExecuteNonQuery();
            if (resultado > 0)
                MessageBox.Show("Se ha eliminado el registro correctamente");

            conn.Close();
        }

        private void dgvListado_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvListado.SelectedRows.Count > 0)
            {
                txtPersonID.Text = dgvListado.SelectedRows[0].Cells[0].Value.ToString();
                txtFirstName.Text = dgvListado.SelectedRows[0].Cells[2].Value.ToString();
                txtLastName.Text = dgvListado.SelectedRows[0].Cells[1].Value.ToString();
                dtpHireDate.Text = dgvListado.SelectedRows[0].Cells[3].Value.ToString();
                dtpEnrollmentDate.Text = dgvListado.SelectedRows[0].Cells[4].Value.ToString();
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
                        conn.Open();
                        String sql = "SELECT * FROM Person WHERE PersonID LIKE '%" + txtPersonID.Text + "%'";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        SqlDataReader reader = cmd.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        
                        dgvListado.DataSource = dt;
                        dgvListado.Refresh();
                        conn.Close();
                        break;

                    case 1:
                        conn.Open();
                        sql = "SELECT * FROM Person WHERE FirstName LIKE '%" + txtFirstName.Text + "%'";
                        cmd = new SqlCommand(sql, conn);
                        reader = cmd.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(reader);

                        dgvListado.DataSource = dt;
                        dgvListado.Refresh();
                        conn.Close();
                        break;

                    case 2:
                        conn.Open();
                        sql = "SELECT * FROM Person WHERE LastName LIKE '%" + txtLastName.Text + "%'";
                        cmd = new SqlCommand(sql, conn);
                        reader = cmd.ExecuteReader();

                        dt = new DataTable();
                        dt.Load(reader);

                        dgvListado.DataSource = dt;
                        dgvListado.Refresh();
                        conn.Close();
                        break;

                    case 3:
                        conn.Open();
                        sql = "SELECT * FROM Person WHERE HireDate BETWEEN '" + dtpHireDate.Text + "' AND '" + dtpEnrollmentDate.Text + "'";
                        cmd = new SqlCommand(sql, conn);
                        reader = cmd.ExecuteReader();

                        dt = new DataTable();
                        dt.Load(reader);

                        dgvListado.DataSource = dt;
                        dgvListado.Refresh();
                        conn.Close();
                        break;

                    case 4:
                        conn.Open();
                        sql = "SELECT * FROM Person WHERE EnrollmentDate BETWEEN '" + dtpHireDate.Text + "' AND '" + dtpEnrollmentDate.Text + "'";
                        cmd = new SqlCommand(sql, conn);
                        reader = cmd.ExecuteReader();

                        dt = new DataTable();
                        dt.Load(reader);

                        dgvListado.DataSource = dt;
                        dgvListado.Refresh();
                        conn.Close();
                        break;

                }
            } else
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
