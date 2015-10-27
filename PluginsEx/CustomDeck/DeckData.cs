using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

namespace CustomDeck
{
    public sealed class DeckData
    {
        const string RelativePath = "\\settings\\CustomDeck.xml";
        string ConfigFile;
        public static DeckData Instance;

        public List<FleetDeck> DeckList;

        static DeckData()
        {
            Instance = new DeckData();

        }

        DeckData()
        {
            ConfigFile = System.Windows.Forms.Application.StartupPath + RelativePath;
        }

        public void LoadConfig()
        {
            DeckList = new List<FleetDeck>();
            if (System.IO.File.Exists(ConfigFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigFile);
                var Root = doc.DocumentElement;

                foreach (XmlElement subnode in Root)
                {
                    string DeckName = subnode.GetAttribute("DeckName");
                    string DeckData = subnode.GetAttribute("DeckData");
                    FleetDeck deck = new FleetDeck(DeckName, DeckData);
                    DeckList.Add(deck);
                }
            }
        }

        public void SaveConfig()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                if (!System.IO.File.Exists(ConfigFile))
                {
                    XmlElement xmlelem = doc.CreateElement("Config");
                    doc.AppendChild(xmlelem);
                }
                else
                {
                    doc.Load(ConfigFile);
                }

                var Root = doc.DocumentElement;

                Root.RemoveAll();
                foreach (var deck in DeckList)
                {
                    XmlElement Element = doc.CreateElement("Deck");
                    Element.SetAttribute("DeckName", deck.Name);
                    Element.SetAttribute("DeckData", deck.Json);
                    Root.AppendChild(Element);
                }
                //System.Security.SecurityElement.Escape(s);
                doc.Save(ConfigFile);
            }
            catch
            {
            }
        }

        public FleetDeck AddDeck(string name,string jsonstring)
        {
            FleetDeck deck=new FleetDeck(name,jsonstring);
            return null;
        }

        public void RemoveDeck(int deckid)
        {
            DeckList.RemoveAt(deckid);
        }
    }

    public class FleetDeck
    {
        public FleetDeck(string name, string data)
        {
            Name = name;
            Json = data;
        }
        public string Name
        {
            get;
            set;
        }

        public string Flagship
        {
            get
            {
                if (_fleets == null)
                    return null;
                if (_fleets.Fleets[0] == null)
                    return null;
                if (_fleets.Fleets[0].Ships[0] == null)
                    return null;
                if (_fleets.Fleets[0].Ships[0].Ship == null)
                    return null;
                return _fleets.Fleets[0].Ships[0].Ship.Name;
            }
        }

        string json;
        public string Json
        {
            get
            {
                return json;
            }
            set
            {
                json = value;
                if (json == null)
                    _fleets = null;
                else
                {
                    if (_fleets == null)
                        _fleets = new CustomFleets();
                    _fleets.ImportFromString(json);
                }
            }
        }

        CustomFleets _fleets;
        public CustomFleets Fleets
        {
            get;
            set;
        }
    }
}
