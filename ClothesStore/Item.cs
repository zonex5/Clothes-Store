using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClothesStore
{
    class Item
    {
        public string id;
        public string name;
        public object tag;

        public override string ToString()
        {
            return name;
        }
    }
}
