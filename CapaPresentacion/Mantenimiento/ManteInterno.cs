﻿using CapaEntidad;
using CapaLogica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CapaPresentacion.Mantenimiento
{
    public partial class ManteInterno : Form
    {


        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);


        public ManteInterno()
        {
            InitializeComponent();
            dataGridView1.DataSource = LogTaller.Instancia.listar_mante_interno();
            comboMante.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTaller.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void ManteInterno_Load(object sender, EventArgs e)
        {
            List<entMantenimiento> lista = LogMantenimiento.Instancia.mantenimientos_disp();

            foreach (entMantenimiento m in lista)
            {
                comboMante.Items.Add($"ID: {m.id}; Vehiculo: {m.placa_vehiculo}");
            }



            List<Taller> lista_taller = LogTaller.Instancia.listar_taller();
            foreach(Taller t in lista_taller)
            {
                comboTaller.Items.Add($"ID: {t.id} - Nombre:{t.nombre}");
            }


            
            PintarTreeView();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {

        }

        public void ActualizarGridView()
        {
            dataGridView1.Refresh();
            dataGridView1.DataSource =LogTaller.Instancia.listar_mante_interno();

        }

        private void PintarTreeView()
        {
            List<Tecnico> lista_tec = LogTecnico.Instancia.list_tecnicos();

            treeView1.Nodes.Clear();

            foreach (Tecnico tec in lista_tec)
            {
                TreeNode tallerNode = null;
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if (node.Text == tec.nombre_taller)
                    {
                        tallerNode = node;
                        break;
                    }
                }

                // Si no se encontró el nodo del taller, crearlo
                if (tallerNode == null)
                {
                    tallerNode = new TreeNode(tec.nombre_taller);
                    treeView1.Nodes.Add(tallerNode);
                }

                // Crear nodos separados para cada detalle del técnico
                TreeNode tecnicoNode = new TreeNode(tec.nombre);
                tecnicoNode.Nodes.Add(new TreeNode("Especialidad: " + tec.especialidad));
                tecnicoNode.Nodes.Add(new TreeNode("Teléfono: " + tec.telefono));

                // Agregar el nodo del técnico como hijo del nodo del taller
                tallerNode.Nodes.Add(tecnicoNode);
            }
        }
    }
}
