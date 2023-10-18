
using System;
using System.Runtime.InteropServices;


namespace WeLing.SDK
{
  
    public enum GyroType
    {
        DEFAULT = 0,
        ATTITUDE = 1101,
        ROTATIONRATE = 1102,
    }

    public class MonoPInvokeCallbackAttribute : Attribute
    {
        public MonoPInvokeCallbackAttribute() { }
    }

    public class PipeWrapper
    {
        public static string version = "3.12.1";


        public static Action<byte[], int> OnReceiveDataEvent;
        public static Action<string, byte[], int> OnReceiveDataWithKeyEvent;
        public static Action<GyroType, float[]> OnSensorEvent;


        private delegate void _OnReceiveData(IntPtr data, ref int size);
        private delegate void _OnReceiveDataWithKey(string key, IntPtr data, ref int size);
        private delegate void _OnSensorData(ref int type, IntPtr data, ref int len);


        [DllImport("NamedPipeClient", EntryPoint = "InitEx", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _InitEx();

        [DllImport("NamedPipeClient", EntryPoint = "Release", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _Release();

        [DllImport("NamedPipeClient", EntryPoint = "SendData", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _SendData(byte[] data, int size);

        [DllImport("NamedPipeClient", EntryPoint = "SendDataWithKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _SendDataWithKey(string key, byte[] data, int size);

        [DllImport("NamedPipeClient", EntryPoint = "SetOnSensorCallback", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _SetOnSensorCallback(_OnSensorData cb);

        [DllImport("NamedPipeClient", EntryPoint = "SetOnRecieveDataCallback", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _SetOnRecieveDataCallback(_OnReceiveData cb);

        [DllImport("NamedPipeClient", EntryPoint = "SetOnDataWithKeyCallback", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _SetOnDataWithKeyCallback(_OnReceiveDataWithKey cb);


        [MonoPInvokeCallback]
        static void SimpleCallBackImp(IntPtr data, ref int size)
        {
            if (OnReceiveDataEvent != null)
            {
                var tmp = new byte[size];
                Marshal.Copy(data, tmp, 0, size);
                OnReceiveDataEvent(tmp, size);
                //string dataString = System.Text.Encoding.Default.GetString(tmp);
            }
        }


        [MonoPInvokeCallback]
        static void WithKeyCallBackImp(string key, IntPtr data, ref int size)
        {
            if (OnReceiveDataWithKeyEvent != null)
            {
                var tmp = new byte[size];
                Marshal.Copy(data, tmp, 0, size);
                OnReceiveDataWithKeyEvent(key, tmp, size);
            }
        }

        [MonoPInvokeCallback]
        static void SensorCallBackImp(ref int type, IntPtr data, ref int len)
        {
            if (OnSensorEvent != null)
            {
                GyroType tmp_type = (GyroType)type;
                var tmp_data = new float[len];
                Marshal.Copy(data, tmp_data, 0, len);

                OnSensorEvent(tmp_type, tmp_data);
            }
        }

        //初始化通信管道
        public static bool Init()
        {
            _SetOnRecieveDataCallback(new _OnReceiveData(SimpleCallBackImp));
            _SetOnSensorCallback(new _OnSensorData(SensorCallBackImp));
            _SetOnDataWithKeyCallback(new _OnReceiveDataWithKey(WithKeyCallBackImp));

            return _InitEx();
        }


        public static void Uninit()
        {
            OnReceiveDataWithKeyEvent = null;
            OnReceiveDataEvent = null;
            OnSensorEvent = null;

            _Release();
        }

        public static bool SendData(byte[] data, int size)
        {
            return _SendData(data, size);
        }

        public static bool SendDataString(string dataString)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(dataString);
            int size = data.Length;
            return _SendData(data, size);
        }

        public static bool SendDataWithKey(string key, byte[] data, int size)
        {
            return _SendDataWithKey(key, data, size);
        }
    }
}