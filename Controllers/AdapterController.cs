using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml;
using Newtonsoft.Json;
namespace APIadapter.Controllers
{

    public class ImovilJSONAdapter : ImovilJSONOrigin
    {
        public string GetMobilesXMLSpecifications()
        {
            List<Movil> moviles = new List<Movil>();
            moviles.Add(new Movil
            {
                modelo = "Iphone 13",
                costo = 345.00
            });
            moviles.Add(new Movil
            {
                modelo = "Iphone 14",
                costo = 345.00
            });
            dynamic collectionMobiles = new { Apple = moviles };
            return JsonConvert.SerializeObject(collectionMobiles);

        }
    }

    public class ImovilXMLAdapter : ImovilXMLTarget
    {
        public XmlDocument GetMobilesXMLSpecifications(ImovilJSONOrigin movilJSONAdapter)
        {
            XmlDocument doc1=new XmlDocument();
            string jsonMovil = movilJSONAdapter.GetMobilesXMLSpecifications();
            XmlDocument? doc = JsonConvert.DeserializeXmlNode(jsonMovil, "Mobiles", true);
            if (doc is null){
                return doc1;
            }
            else{
                return doc;
            }
        }
    }

    public class Myclient
    {
        private ImovilXMLTarget _movilXmlTarget;
        private ImovilJSONOrigin _movilJSONOrigin;

        public Myclient(ImovilXMLTarget movilXmlTarget, ImovilJSONOrigin movilJSONOrigin)
        {
            _movilXmlTarget = movilXmlTarget;
            _movilJSONOrigin = movilJSONOrigin;
        }

        public XmlDocument GetMovilData()
        {
            return _movilXmlTarget.GetMobilesXMLSpecifications(_movilJSONOrigin);
        }
    }

    public interface ImovilJSONOrigin
    {
        String GetMobilesXMLSpecifications();
    }
    public interface ImovilXMLTarget
    {
        XmlDocument GetMobilesXMLSpecifications(ImovilJSONOrigin movilJSONAdapter);
    }

    public interface ImovilMP3Target
    {
        XmlDocument GetMobilesXMLSpecifications(ImovilJSONOrigin movilJSONAdapter);
    }

    public class Movil
    {
        public string? modelo { get; set; }
        public double costo { get; set; }
        
    }

[ApiController]
[Route("[controller]")]
    public class AdapterController : ControllerBase
    {    
        [HttpGet]
        public string Get()
        {
            Myclient cliente = new Myclient(new ImovilXMLAdapter(), new ImovilJSONAdapter());
            XmlDocument xml = cliente.GetMovilData();

            XmlNodeList lista = xml.GetElementsByTagName("Mobiles");
            XmlNodeList moviles = ((XmlElement)lista[0]).GetElementsByTagName("Apple");

            string result="";

            foreach (System.Xml.XmlElement xEle in moviles)
            {
                result+=xEle.OuterXml;
                result+=System.Environment.NewLine;
            }
            return result;
        }
    }

}