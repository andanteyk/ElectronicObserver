using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Browser
{
    static class LoadResources
    {
        static public String getter(string Name)
        {
            return Properties.Resources.ResourceManager.GetString(Name);
        }
    }
}
