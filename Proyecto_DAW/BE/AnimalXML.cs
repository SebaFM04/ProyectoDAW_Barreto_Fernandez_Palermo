using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[XmlRoot("Animales")]
public class AnimalesXML
{
    [XmlElement("Animal")]
    public List<Animal> Listado { get; set; } = new List<Animal>();
}
