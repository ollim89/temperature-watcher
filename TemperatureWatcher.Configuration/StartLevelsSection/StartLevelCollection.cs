using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Configuration.StartLevelsSection
{
    public class StartLevelCollection : ConfigurationElementCollection
    {
        private string _elementName = "level";

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }

        public StartLevel this[int index]
        {
            get { return base.BaseGet(index) as StartLevel; }
        }

        protected override string ElementName
        {
            get
            {
                return _elementName;
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(_elementName, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new StartLevel();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            StartLevel startLevel = (StartLevel)element;
            return string.Format("{0}:{1}:{2}", startLevel.Hours, startLevel.Minutes, startLevel.Seconds);
        }
    }
}
