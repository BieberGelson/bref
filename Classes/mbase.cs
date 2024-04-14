using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
        
        public string Perfil { get; set; }
        public string Material { get; set; }
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
            
        }
        public VigaModelo()
        {
                
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
    }

    
}
