using System;
using System.Collections.Generic;
using System.Threading;

namespace BF1_AutoOpenBox
{
    class ClassKeysManager
    {
        // Keys holder
        private Dictionary<int, ClassKey> keys;

        // Update thread
        public static Thread thread = null;
        private int interval = 20; // 20 ms

        // Keys events
        public delegate void KeyHandler(int Id, string Name);
        public event KeyHandler KeyUpEvent;
        public event KeyHandler KeyDownEvent;

        // Key Up
        protected void OnKeyUp(int Id, string Name)
        {
            if (KeyUpEvent != null)
            {
                KeyUpEvent(Id, Name);
            }
        }

        // Key Down
        protected void OnKeyDown(int Id, string Name)
        {
            if (KeyDownEvent != null)
            {
                KeyDownEvent(Id, Name);
            }
        }

        // Init
        public ClassKeysManager()
        {
            keys = new Dictionary<int, ClassKey>();
            thread = new Thread(new ParameterizedThreadStart(Update));
            thread.Start();
        }

        // Add key
        public void AddKey(int keyId, string keyName)
        {
            if (!keys.ContainsKey(keyId))
            {
                keys.Add(keyId, new ClassKey(keyId, keyName));
            }
        }

        // Add key
        public void AddKey(System.Windows.Forms.Keys key)
        {
            int keyId = (int)key;
            if (!keys.ContainsKey(keyId))
            {
                keys.Add(keyId, new ClassKey(keyId, key.ToString()));
            }
        }

        // Is Key Down
        public bool IsKeyDown(int keyId)
        {
            ClassKey value;
            if (keys.TryGetValue(keyId, out value))
            {
                return value.IsKeyDown;
            }
            return false;
        }

        // Update Thread
        private void Update(object sender)
        {
            while (true)
            {
                if (keys.Count > 0)
                {
                    List<ClassKey> keysData = new List<ClassKey>(keys.Values);
                    if (keysData != null && keysData.Count > 0)
                    {
                        foreach (ClassKey key in keysData)
                        {
                            if (Convert.ToBoolean(ClassManaged.GetKeyState(key.Id) & ClassManaged.KEY_PRESSED))
                            {
                                if (!key.IsKeyDown)
                                {
                                    key.IsKeyDown = true;
                                    OnKeyDown(key.Id, key.Name);
                                }
                            }
                            else
                            {
                                if (key.IsKeyDown)
                                {
                                    key.IsKeyDown = false;
                                    OnKeyUp(key.Id, key.Name);
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(interval);
            }
        }
    }
}
