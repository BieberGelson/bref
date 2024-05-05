using bref.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tekla.Structures.Model;

namespace bref
{
    public partial class Form1 : Form
    {
        Tekla.Structures.Model.Model modelo = new Tekla.Structures.Model.Model();
        mbase mod = new mbase();
        public Form1()
        {
            InitializeComponent();
        }
        Tekla.Structures.Model.UI.ModelObjectSelector selecionar = new Tekla.Structures.Model.UI.ModelObjectSelector();
        private void button1_Click(object sender, EventArgs e)
        {
            mod = new mbase();
            foreach (var obj in selecionar.GetSelectedObjects())
            {
                if (obj is Tekla.Structures.Model.Beam)
                {
                    var Viga = obj as Tekla.Structures.Model.Beam;
                    
                    mod.AdicionarObjetoModelo(new VigaModelo(Viga));
                
                }
                if (obj is Tekla.Structures.Model.ContourPlate)
                {
                    var CP = obj as ContourPlate;
                    mod.AdicionarObjetoModelo(new ChapaContornoModelo(CP));
                }
                if (obj is Tekla.Structures.Model.PolyBeam)
                {
                    var PB = obj as PolyBeam;
                    mod.AdicionarObjetoModelo(new PolybeamModelo(PB));
                }
                if (obj is Tekla.Structures.Model.BentPlate)
                { 
                    var BP = obj as BentPlate;
                    mod.AdicionarObjetoModelo(new BentPlateModelo(BP));
                }
            
            }
            mod.Salvar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var mod = mbase.Carregar("D:\\teste.mod");
            mod.CriarModeloTekla();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            
            //var cp = new ContourPlate();
            //cp.Profile.ProfileString = "CH9.5";
            //cp.Material.MaterialString = "A36";

            //var ponto1 = new Tekla.Structures.Geometry3d.Point(0, 0, 0);
            //var Chanfro1 = new Chamfer();
            //Chanfro1.X = 20;
            //Chanfro1.Y = 20;
            //Chanfro1.Type = Chamfer.ChamferTypeEnum.CHAMFER_LINE;
            //var ContourPoint1 = new ContourPoint(ponto1, Chanfro1);


            //var ContourPoint2 = new ContourPoint();
            //ContourPoint2.X = 1000;

            //var ContourPoint3 = new ContourPoint();
            //ContourPoint3.X = 1000;
            //ContourPoint3.Y = 1000;
            //ContourPoint3.Chamfer.Type = Chamfer.ChamferTypeEnum.CHAMFER_ARC;
            //ContourPoint3.Chamfer.X = 100;


            //var ContourPoint4 = new ContourPoint();
            //ContourPoint4.Y = 1000;

            //cp.Contour.AddContourPoint(ContourPoint1);
            //cp.Contour.AddContourPoint(ContourPoint2);
            //cp.Contour.AddContourPoint(ContourPoint3);
            //cp.Contour.AddContourPoint(ContourPoint4);

            //cp.Position.Depth = Position.DepthEnum.FRONT;

            //cp.Insert();

            foreach (var modelobject in selecionar.GetSelectedObjects())
            {
                if (modelobject is ContourPlate)
                {
                    //var contPlate = modelobject as ContourPlate;
                    //var PontoNovo = new ContourPoint();
                    //PontoNovo.X = 500;
                    //PontoNovo.Y = 150;

                    //contPlate.Contour.ContourPoints.Insert(2, PontoNovo);

                    //contPlate.Modify();

                    ////int contador = 0;
                    ////foreach (ContourPoint CP in contPlate.Contour.ContourPoints)
                    ////{

                    ////    var vigaRef = new Beam();
                    ////    vigaRef.Profile.ProfileString = "D12";
                    ////    vigaRef.Material.MaterialString = "A36";
                    ////    vigaRef.StartPoint = new Tekla.Structures.Geometry3d.Point(CP);
                    ////    vigaRef.EndPoint = new Tekla.Structures.Geometry3d.Point(CP);
                    ////    vigaRef.EndPoint.Z += 100;
                    ////    vigaRef.Name = contador.ToString();
                    ////    vigaRef.Insert();
                    ////    contador++;

                    ////}
                    //contPlate.Modify();
                }
                else if (modelobject is Tekla.Structures.Model.PolyBeam)
                {
                    var PolyB = modelobject as PolyBeam;

                }
                else if (modelobject is Tekla.Structures.Model.BentPlate)
                {
                    var BentP = modelobject as BentPlate;
                    var GeomEnum = BentP.Geometry.GetGeometryEnumerator();

              
                    var gset = BentP.Geometry.GetGeometryLegSections();

                    PolygonNode pnd1 = gset[0].GeometryNode as PolygonNode;
                    var CHP1 = new ContourPlate();
                    var CHP2 = new ContourPlate();

                    CHP1.Profile.ProfileString = CHP2.Profile.ProfileString = BentP.Profile.ProfileString;
                    CHP1.Material.MaterialString = CHP2.Material.MaterialString = BentP.Material.MaterialString;
                    CHP1.Name = CHP2.Name = BentP.Name;
                    CHP1.Finish = CHP2.Finish = BentP.Finish;
                    CHP1.Class = CHP2.Class = BentP.Class;
                    CHP1.PartNumber = CHP2.PartNumber = BentP.PartNumber;
                    CHP1.AssemblyNumber = CHP2.AssemblyNumber = BentP.AssemblyNumber;

                    foreach (var p in pnd1.Contour.ContourPoints)
                    {
                        CHP1.Contour.ContourPoints.Add(p);
                    }
                    CHP1.Insert();

                    PolygonNode pnd2 = gset[1].GeometryNode as PolygonNode;
                    foreach (var p in pnd2.Contour.ContourPoints)
                    {
                        CHP2.Contour.ContourPoints.Add(p);
                    }
                    CHP2.Insert();


                    var BentPNova = Tekla.Structures.Model.Operations.Operation.CreateBentPlateByParts(CHP1, CHP2);
                    

                }

            }
            modelo.CommitChanges();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var sel = new Tekla.Structures.Model.UI.ModelObjectSelector();

            List<ContourPlate> Chapas = new List<ContourPlate>();

            foreach(ContourPlate chapa in sel.GetSelectedObjects())
            {
                Chapas.Add(chapa);
            }

            var Bplate = Tekla.Structures.Model.Operations.Operation.CreateBentPlateByParts(Chapas[0], Chapas[1]);
            modelo.CommitChanges();
        }
    }
}
