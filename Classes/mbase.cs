using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
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
            XmlSerializer serializer = new XmlSerializer(typeof(mbase), new Type[] { typeof(mbase), typeof(VigaModelo) });

            TextWriter writer = new StreamWriter("D:\\teste.mod");

            serializer.Serialize(writer, this);

            writer.Close();

        }
        public static mbase Carregar(string CaminhoArquivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(mbase), new Type[] { typeof(mbase), typeof(VigaModelo) });

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
