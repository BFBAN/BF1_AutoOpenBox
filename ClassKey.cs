using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1_AutoOpenBox
{
    class ClassKey
    {
        private string keyName;
        private int keyId;
        private bool keyDown;

        public ClassKey(int keyId, string keyName)
        {
            this.keyId = keyId;
            this.keyName = keyName;
        }

        public string Name
        {
            get
            {
                return keyName;
            }
        }

        public int Id
        {
            get
            {
                return keyId;
            }
        }

        public bool IsKeyDown
        {
            get
            {
                return keyDown;
            }
            set
            {
                keyDown = value;
            }
        }
    }
}
