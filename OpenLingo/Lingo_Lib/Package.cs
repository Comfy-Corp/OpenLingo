using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingoLib              
{
    [Serializable]
    public class Package
    {
        public LingoProtocol CommandName;
        public object transmittedObject;
        public int queueNumber;

        public Package(LingoProtocol CommandName, object transmittedObject)
        {
            this.CommandName = CommandName;
            this.transmittedObject = transmittedObject;
        }

        public Package(LingoProtocol CommandName, object transmittedObject, int queueNumber)
        {
            this.CommandName = CommandName;
            this.transmittedObject = transmittedObject;
            this.queueNumber = queueNumber;
        }

        public override bool Equals(object obj)
        {
            if (obj is Package)
            {
                Package package = obj as Package;
                if (package.queueNumber == this.queueNumber)
                    if ((int)package.CommandName == (int)this.CommandName)
                        if (package.transmittedObject.Equals(this.transmittedObject))
                            return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
