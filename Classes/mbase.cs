using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace bref.Classes
{
    public class mbase
    {

        string CaminhoArquivo { get; set; }
        public List<ObjetoModelo> ObjetosModelo { get; private set; }
        public void AdicionarObjetoModelo(ObjetoModelo objetoModelo)
        { 
            ObjetosModelo.Add(objetoModelo);
        }
        public mbase()
        {
            ObjetosModelo = new List<ObjetoModelo>();

        }
        public void Salvar()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(mbase), new Type[] { 
                typeof(mbase),
                typeof(VigaModelo),
                typeof(ChapaContornoModelo),
                typeof(PolybeamModelo),
                typeof(BentPlateModelo),
            });

            TextWriter writer = new StreamWriter("D:\\teste.mod");

            serializer.Serialize(writer, this);

            writer.Close();

        }
        public static mbase Carregar(string CaminhoArquivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(mbase), new Type[] { 
                typeof(mbase),
                typeof(VigaModelo),
                typeof(ChapaContornoModelo),
                typeof(PolybeamModelo),
                typeof(BentPlateModelo), });

            TextReader reader = new StreamReader(CaminhoArquivo);

            mbase file;
            file = (mbase)serializer.Deserialize(reader);

            reader.Close();

            return file;
            
        }
        public void CriarModeloTekla()
        {
            Model Modelo = new Model();
            foreach (var ObjetoModelo in ObjetosModelo)
            {
                if (ObjetoModelo is VigaModelo)
                {
                    var ObjetoViga = ObjetoModelo as VigaModelo;
                    var VigaTekla = ObjetoViga.RetornaVigaTekla();
                    VigaTekla.Insert();
                }
                if (ObjetoModelo is ChapaContornoModelo)
                { 
                    var ObjetoChapaModelo = ObjetoModelo as ChapaContornoModelo;
                    var ChapaContornoTekla = ObjetoChapaModelo.RetornaChapaContornoTekla();
                    ChapaContornoTekla.Insert();
                    ChapaContornoTekla.Modify();
                }
                if (ObjetoModelo is PolybeamModelo)
                {
                    var ObjetoPolyBeamModelo = ObjetoModelo as PolybeamModelo;
                    var PolybeamTekla = ObjetoPolyBeamModelo.RetornaPolyBeamTekla();
                    PolybeamTekla.Insert();
                }
                if (ObjetoModelo is BentPlateModelo)
                {
                    var ObjetoBentPlateModelo = ObjetoModelo as BentPlateModelo;
                    var BentPlateTekla = ObjetoBentPlateModelo.CriarBentPlateTekla();
                }
            }
            Modelo.CommitChanges();
                
        }
        public static PositionPlaneTekla ConvertePositionPlane(Position.PlaneEnum OpcaoPlane)
        {
            if (OpcaoPlane == Position.PlaneEnum.MIDDLE) { return PositionPlaneTekla.Middle; }
            else if (OpcaoPlane == Position.PlaneEnum.LEFT) { return PositionPlaneTekla.Left; }
            else return PositionPlaneTekla.Right;
        }
        public static PositionRotationTekla ConvertePositionRotation(Position.RotationEnum OpcaoRotation)
        {
            if (OpcaoRotation == Position.RotationEnum.FRONT) { return PositionRotationTekla.Front; }
            else if (OpcaoRotation == Position.RotationEnum.BACK) { return PositionRotationTekla.Back; }
            else if (OpcaoRotation == Position.RotationEnum.BELOW) { return PositionRotationTekla.Below; }
            else return PositionRotationTekla.Top;
        }
        public static PositionDepthTekla ConvertePositionDepth(Position.DepthEnum OpcaoDepth)
        {
            if (OpcaoDepth == Position.DepthEnum.MIDDLE) { return PositionDepthTekla.Middle; }
            else if (OpcaoDepth == Position.DepthEnum.FRONT) { return PositionDepthTekla.Front; }
            else return PositionDepthTekla.Behind;
        }

        public static Position.PlaneEnum RetornaPlaneTekla(PositionPlaneTekla Opcao)
        {
            if (Opcao == PositionPlaneTekla.Left) return Position.PlaneEnum.LEFT;
            else if (Opcao == PositionPlaneTekla.Right) return Position.PlaneEnum.RIGHT;
            else return Position.PlaneEnum.MIDDLE;

        }
        public static Position.RotationEnum RetornaRotationTekla(PositionRotationTekla Opcao)
        {
            switch (Opcao)
            {
                case PositionRotationTekla.Top:
                    return Position.RotationEnum.TOP;
                case PositionRotationTekla.Front:
                    return Position.RotationEnum.FRONT;
                case PositionRotationTekla.Back:
                    return Position.RotationEnum.BACK;
                default:
                    return Position.RotationEnum.BELOW;
            }
        
        }
        public static Position.DepthEnum RetornaDepthTekla(PositionDepthTekla Opcao)
        {
            switch (Opcao)
            {
                case PositionDepthTekla.Front:
                    return Position.DepthEnum.FRONT;
                case PositionDepthTekla.Behind:
                    return Position.DepthEnum.BEHIND;
                default:
                    return Position.DepthEnum.MIDDLE;
            }
        }
    }
  

    public class NumeracaoTekla
    {
        public string Prefix { get; set; }
        public int StartNumber { get; set; }
        public NumeracaoTekla(NumberingSeries NumeracaoTekla)
        {
            Prefix = NumeracaoTekla.Prefix;
            StartNumber = NumeracaoTekla.StartNumber;
        }
        public NumberingSeries RetornaNumberingTekla()
        {
            var Numbering = new NumberingSeries();
            Numbering.StartNumber = StartNumber;
            Numbering.Prefix = Prefix;
            return Numbering;
        
        }
        public NumeracaoTekla()
        {
                
        }
    }
    public enum ChamferTypesControleModelo
    {
        CHAMFER_ARC, CHAMFER_ARC_POINT, CHAMFER_LINE, CHAMFER_LINE_AND_ARC, CHAMFER_NONE, CHAMFER_ROUNDING, CHAMFER_SQUARE, CHAMFER_SQUARE_PARALLEL
    }
    public enum PositionPlaneTekla
    {
        Right, Middle, Left
    }
    public enum PositionRotationTekla
    {
        Front, Top, Back, Below
    }
    public enum PositionDepthTekla
    {
        Front, Middle, Behind
    }
    public class ObjetoModelo 
    {
        public Guid Guid { get; set; }
        public ObjetoModelo()
        {
            Guid = Guid.NewGuid();
        }
    }
    public class PecaModelo : ObjetoModelo
    {
        public string Nome { get; set; }
        public string Finish { get; set; }
        public string Class { get; set; }
        public string Perfil { get; set; }
        public string Material { get; set; }
        public NumeracaoTekla NumeracaoPeca { get; set; }
        public NumeracaoTekla NumeracaoConjunto { get; set; }
        public double PositionPlaneOffset { get; set; }
        public PositionPlaneTekla PositionPlane { get; set; }
        public double PositionRotationAngle { get; set; }
        public PositionRotationTekla PositionRotation { get; set; }
        public double PositionDepthOffset { get; set; }
        public PositionDepthTekla PositionDepth { get; set; }
        public PecaModelo()
        {
                
        }

    }
    public class VigaModelo : PecaModelo
    { 
        public Ponto3D PontoInicial { get; set; }
        public Ponto3D PontoFinal { get; set; }
        public VigaModelo(Tekla.Structures.Model.Beam VigaTekla)
        {
            Perfil = VigaTekla.Profile.ProfileString;
            Material = VigaTekla.Material.MaterialString;
            PontoInicial = new Ponto3D(VigaTekla.StartPoint);
            PontoFinal = new Ponto3D(VigaTekla.EndPoint);
            Nome = VigaTekla.Name;
            Finish = VigaTekla.Finish;
            Class = VigaTekla.Class;
            NumeracaoPeca = new NumeracaoTekla(VigaTekla.PartNumber);
            NumeracaoConjunto = new NumeracaoTekla(VigaTekla.AssemblyNumber);
            PositionPlane = mbase.ConvertePositionPlane(VigaTekla.Position.Plane);
            PositionPlaneOffset = VigaTekla.Position.PlaneOffset;
            PositionRotation = mbase.ConvertePositionRotation(VigaTekla.Position.Rotation);
            PositionRotationAngle = VigaTekla.Position.RotationOffset;
            PositionDepth = mbase.ConvertePositionDepth(VigaTekla.Position.Depth);
            PositionDepthOffset = VigaTekla.Position.DepthOffset;

            
        }
        public VigaModelo()
        {
             
        }
        public Tekla.Structures.Model.Beam RetornaVigaTekla() 
        {
            var VigaTekla = new Beam();
            VigaTekla.Name = Nome;
            VigaTekla.Profile.ProfileString = Perfil;
            VigaTekla.Material.MaterialString = Material;
            VigaTekla.Finish = Finish;
            VigaTekla.Class = Class;
            VigaTekla.AssemblyNumber = NumeracaoConjunto.RetornaNumberingTekla();
            VigaTekla.PartNumber = NumeracaoPeca.RetornaNumberingTekla();
            VigaTekla.Position.DepthOffset = PositionDepthOffset;
            VigaTekla.Position.RotationOffset = PositionRotationAngle;
            VigaTekla.Position.PlaneOffset = PositionPlaneOffset;
            VigaTekla.Position.Plane = mbase.RetornaPlaneTekla(PositionPlane);
            VigaTekla.Position.Rotation = mbase.RetornaRotationTekla(PositionRotation);
            VigaTekla.Position.Depth = mbase.RetornaDepthTekla(PositionDepth);
            VigaTekla.StartPoint = PontoInicial.RetornaPontoTekla();
            VigaTekla.EndPoint = PontoFinal.RetornaPontoTekla();


            return VigaTekla;
        
        }
    }
    public class ChapaContornoModelo : PecaModelo
    {
        public List<ContourPointModelo> ContornoModelo {  get; set; }
        public ChapaContornoModelo()
        {
            ContornoModelo = new List<ContourPointModelo>();
        }
        public ChapaContornoModelo(ContourPlate ChapaTekla)
        {
            Nome = ChapaTekla.Name;
            Perfil = ChapaTekla.Profile.ProfileString;
            Material = ChapaTekla.Material.MaterialString;
            Finish = ChapaTekla.Finish;
            Class = ChapaTekla.Class;
            NumeracaoPeca = new NumeracaoTekla(ChapaTekla.PartNumber);
            NumeracaoConjunto = new NumeracaoTekla(ChapaTekla.AssemblyNumber);
            PositionDepthOffset = ChapaTekla.Position.DepthOffset;
            PositionDepth = mbase.ConvertePositionDepth(ChapaTekla.Position.Depth);
            PositionPlaneOffset = ChapaTekla.Position.PlaneOffset;
            PositionPlane = mbase.ConvertePositionPlane(ChapaTekla.Position.Plane);
            PositionRotationAngle = ChapaTekla.Position.RotationOffset;
            PositionRotation = mbase.ConvertePositionRotation(ChapaTekla.Position.Rotation);
            ContornoModelo = new List<ContourPointModelo>();
            

            foreach (ContourPoint Ponto in ChapaTekla.Contour.ContourPoints)
            {
                ContornoModelo.Add(new ContourPointModelo(Ponto));
            }

        }
        public Tekla.Structures.Model.ContourPlate RetornaChapaContornoTekla()
        {
            var ChapaContornoTekla = new ContourPlate();
            ChapaContornoTekla.Name = Nome;
            ChapaContornoTekla.Profile.ProfileString = Perfil;
            ChapaContornoTekla.Material.MaterialString = Material;
            ChapaContornoTekla.Finish = Finish;
            ChapaContornoTekla.Class = Class;
            ChapaContornoTekla.AssemblyNumber = NumeracaoConjunto.RetornaNumberingTekla();
            ChapaContornoTekla.PartNumber = NumeracaoPeca.RetornaNumberingTekla();
            ChapaContornoTekla.Position.DepthOffset = PositionDepthOffset;
            ChapaContornoTekla.Position.RotationOffset = PositionRotationAngle;
            ChapaContornoTekla.Position.PlaneOffset = PositionPlaneOffset;
            ChapaContornoTekla.Position.Plane = mbase.RetornaPlaneTekla(PositionPlane);
            ChapaContornoTekla.Position.Rotation = mbase.RetornaRotationTekla(PositionRotation);
            ChapaContornoTekla.Position.Depth = mbase.RetornaDepthTekla(PositionDepth);
            
            int contador = 0;
            foreach (var ponto in ContornoModelo)
            {
                ChapaContornoTekla.Contour.ContourPoints.Insert(contador, ponto.RetornaPontoContornoTekla());
                contador++;
            }
            return ChapaContornoTekla;

        }
        
    }

    public class BentPlateModelo : PecaModelo
    {
        public List<ContourPointModelo> ContornoChapa1 { get; set; }
        public List<ContourPointModelo> ContornoChapa2 { get; set; }
        public BentPlateModelo()
        {
            ContornoChapa1 = new List<ContourPointModelo>();
            ContornoChapa2 = new List<ContourPointModelo>();
        }
        public BentPlateModelo(BentPlate ChapaTekla)
        {
            Nome = ChapaTekla.Name;
            Perfil = ChapaTekla.Profile.ProfileString;
            Material = ChapaTekla.Material.MaterialString;
            Finish = ChapaTekla.Finish;
            Class = ChapaTekla.Class;
            NumeracaoPeca = new NumeracaoTekla(ChapaTekla.PartNumber);
            NumeracaoConjunto = new NumeracaoTekla(ChapaTekla.AssemblyNumber);
            ContornoChapa1 = new List<ContourPointModelo>();
            ContornoChapa2 = new List<ContourPointModelo>();

            var GeometryLeg = ChapaTekla.Geometry.GetGeometryLegSections();
            PolygonNode Contorno1 = GeometryLeg[0].GeometryNode as PolygonNode;
            foreach (ContourPoint Ponto in Contorno1.Contour.ContourPoints)
            {
                ContornoChapa1.Add(new ContourPointModelo(Ponto));
            }
            PolygonNode Contorno2 = GeometryLeg[1].GeometryNode as PolygonNode;
            foreach (ContourPoint Ponto in Contorno2.Contour.ContourPoints)
            {
                ContornoChapa2.Add(new ContourPointModelo(Ponto));
            }

        }
        public Tekla.Structures.Model.BentPlate CriarBentPlateTekla()
        {
           


            var ChapaContorno1 = new ContourPlate();
            var ChapaContorno2 = new ContourPlate();

            ChapaContorno1.Name = ChapaContorno2.Name = Nome;
            ChapaContorno1.Material.MaterialString = ChapaContorno2.Material.MaterialString = Material;
            ChapaContorno1.Profile.ProfileString = ChapaContorno2.Profile.ProfileString = Perfil;
            ChapaContorno1.Finish = ChapaContorno2.Finish = Finish;
            ChapaContorno1.Class = ChapaContorno2.Class = Class;
            ChapaContorno1.PartNumber = ChapaContorno2.PartNumber = NumeracaoPeca.RetornaNumberingTekla();
            ChapaContorno1.AssemblyNumber = ChapaContorno2.AssemblyNumber = NumeracaoConjunto.RetornaNumberingTekla();

            foreach (var ponto in ContornoChapa1)
            {
                ChapaContorno1.Contour.ContourPoints.Add(ponto.RetornaPontoContornoTekla());
              
            }
            foreach (var ponto in ContornoChapa2)
            {
                ChapaContorno2.Contour.ContourPoints.Add(ponto.RetornaPontoContornoTekla());

            }
            ChapaContorno1.Insert();
            ChapaContorno2.Insert();
            //ArrayList al = new ArrayList() { ChapaContorno1, ChapaContorno2 };
            //var sel = new Tekla.Structures.Model.UI.ModelObjectSelector();
            //sel.Select(al);
            var BentPlateTekla = Tekla.Structures.Model.Operations.Operation.CreateBentPlateByParts(ChapaContorno1, ChapaContorno2);
            return BentPlateTekla;

        }

    }

    public class PolybeamModelo : PecaModelo
    {
        public List<ContourPointModelo> ContornoModelo { get; set; }
        public PolybeamModelo()
        {
            ContornoModelo = new List<ContourPointModelo>();
        }
        public PolybeamModelo(PolyBeam PolybeamTekla)
        {
            Nome = PolybeamTekla.Name;
            Perfil = PolybeamTekla.Profile.ProfileString;
            Material = PolybeamTekla.Material.MaterialString;
            Finish = PolybeamTekla.Finish;
            Class = PolybeamTekla.Class;
            NumeracaoPeca = new NumeracaoTekla(PolybeamTekla.PartNumber);
            NumeracaoConjunto = new NumeracaoTekla(PolybeamTekla.AssemblyNumber);
            PositionDepthOffset = PolybeamTekla.Position.DepthOffset;
            PositionDepth = mbase.ConvertePositionDepth(PolybeamTekla.Position.Depth);
            PositionPlaneOffset = PolybeamTekla.Position.PlaneOffset;
            PositionPlane = mbase.ConvertePositionPlane(PolybeamTekla.Position.Plane);
            PositionRotationAngle = PolybeamTekla.Position.RotationOffset;
            PositionRotation = mbase.ConvertePositionRotation(PolybeamTekla.Position.Rotation);
            ContornoModelo = new List<ContourPointModelo>();


            foreach (ContourPoint Ponto in PolybeamTekla.Contour.ContourPoints)
            {
                ContornoModelo.Add(new ContourPointModelo(Ponto));
            }

        }
        public Tekla.Structures.Model.PolyBeam RetornaPolyBeamTekla()
        {
            var PolybeamTekla = new PolyBeam();
            PolybeamTekla.Name = Nome;
            PolybeamTekla.Profile.ProfileString = Perfil;
            PolybeamTekla.Material.MaterialString = Material;
            PolybeamTekla.Finish = Finish;
            PolybeamTekla.Class = Class;
            PolybeamTekla.AssemblyNumber = NumeracaoConjunto.RetornaNumberingTekla();
            PolybeamTekla.PartNumber = NumeracaoPeca.RetornaNumberingTekla();
            PolybeamTekla.Position.DepthOffset = PositionDepthOffset;
            PolybeamTekla.Position.RotationOffset = PositionRotationAngle;
            PolybeamTekla.Position.PlaneOffset = PositionPlaneOffset;
            PolybeamTekla.Position.Plane = mbase.RetornaPlaneTekla(PositionPlane);
            PolybeamTekla.Position.Rotation = mbase.RetornaRotationTekla(PositionRotation);
            PolybeamTekla.Position.Depth = mbase.RetornaDepthTekla(PositionDepth);

            int contador = 0;
            foreach (var ponto in ContornoModelo)
            {
                PolybeamTekla.Contour.ContourPoints.Insert(contador, ponto.RetornaPontoContornoTekla());
                contador++;
            }
            return PolybeamTekla;

        }

    }
    public class ContourPointModelo : Ponto3D 
    { 
        public ChamferControleModelo Chamfer { get; set; }
        public ContourPointModelo()
        {
            
        }
        public ContourPointModelo(ContourPoint PontoTekla)
        {
            X = PontoTekla.X;
            Y = PontoTekla.Y;
            Z = PontoTekla.Z;
            Chamfer = new ChamferControleModelo(PontoTekla.Chamfer);

        }
        public ContourPoint RetornaPontoContornoTekla()
        {
            var ContourP = new ContourPoint();
            ContourP.X = X;
            ContourP.Y = Y;
            ContourP.Z = Z;
            ContourP.Chamfer.X = Chamfer.X;
            ContourP.Chamfer.Y = Chamfer.Y;
            ContourP.Chamfer.DZ1 = Chamfer.DZ1;
            ContourP.Chamfer.DZ2 = Chamfer.DZ2;
            var TipoChamferTeklaRef = Tekla.Structures.Model.Chamfer.ChamferTypeEnum.CHAMFER_NONE;
            Enum.TryParse<Tekla.Structures.Model.Chamfer.ChamferTypeEnum>(Chamfer.ChamferType.ToString(), out TipoChamferTeklaRef);
            ContourP.Chamfer.Type = TipoChamferTeklaRef;
            
            return ContourP;
        
        }
    }
    public class ChamferControleModelo 
    { 
        public ChamferTypesControleModelo ChamferType { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double DZ1 { get; set; }
        public double DZ2 { get; set; }
        public ChamferControleModelo()
        {
            ChamferType = ChamferTypesControleModelo.CHAMFER_NONE;
        }
        public ChamferControleModelo(Chamfer ChamferTekla)
        {
            X = ChamferTekla.X;
            Y = ChamferTekla.Y;
            DZ1 = ChamferTekla.DZ1;
            DZ2 = ChamferTekla.DZ2;
            var ChamferTypeRef = new ChamferTypesControleModelo();
            Enum.TryParse<ChamferTypesControleModelo>(ChamferTekla.Type.ToString(), out ChamferTypeRef);
            ChamferType = ChamferTypeRef;
        }

    }
    public class Ponto3D 
    {
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
        public double Z { get; set; } = 0;
        public Ponto3D()
        {
                
        }
        public Ponto3D(Tekla.Structures.Geometry3d.Point pontoTekla)
        {
            X = pontoTekla.X;
            Y = pontoTekla.Y;
            Z = pontoTekla.Z;
        }
        public Tekla.Structures.Geometry3d.Point RetornaPontoTekla()
        { 
            return new Tekla.Structures.Geometry3d.Point(X, Y, Z);
        
        }
    }

    
}
