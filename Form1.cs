using bref.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bref
{
    public partial class Form1 : Form
    {
        mbase mod = new mbase();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tekla.Structures.Model.UI.ModelObjectSelector selecionar = new Tekla.Structures.Model.UI.ModelObjectSelector();
            foreach (var obj in selecionar.GetSelectedObjects())
            {
                if (obj is Tekla.Structures.Model.Beam)
                {
                    var Viga = obj as Tekla.Structures.Model.Beam;
                    
                    mod.AdicionarObjetoModelo(new VigaModelo(Viga));
                
                }
            
            }
            mod.Salvar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var mod = mbase.Carregar("D:\\teste.mod");
            mod.CriarModeloTekla();
        }
    }
}
