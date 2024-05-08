using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

namespace ScriptTutorials
{
    public class InputData : MonoBehaviour
    {
        public InputDevice _rightController;
        public InputDevice _leftController;
        public InputDevice _HMD;

        void Update()
        {
            if (!_rightController.isValid || !_leftController.isValid || !_HMD.isValid)
                InitializeInputDevices();
        }

        private void InitializeInputDevices()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

            foreach (var device in devices)
            {
                if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
                    _rightController = device;
                else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
                    _leftController = device;
                else if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeadMounted))
                    _HMD = device;
            }
        }

        public Vector2 GetLeftThumbstickInput()
        {
            Vector2 thumbstickValue = Vector2.zero; // Inisialisasi dengan nilai default (0, 0)

            if (_leftController.isValid)
            {
                _leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbstickValue);
            }

            return thumbstickValue;
        }
    }
}
