using System;

namespace Model.EF6
{
    public class DesktopMonitor
    {
        public int Availability { get; set; }
        public long Bandwidth { get; set; }
        public string Caption { get; set; }
        public long ConfigManagerErrorCode { get; set; }
        public bool ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public string Description { get; set; }
        public string DeviceID { get; set; }
        public int DisplayType { get; set; }
        public bool ErrorCleared { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime? InstallDate { get; set; }
        public bool IsLocked { get; set; }
        public long LastErrorCode { get; set; }
        public string MonitorManufacturer { get; set; }
        public string MonitorType { get; set; }
        public string Name { get; set; }
        public long PixelsPerXLogicalInch { get; set; }
        public long PixelsPerYLogicalInch { get; set; }
        public string PNPDeviceID { get; set; }
        public string PowerManagementCapabilities { get; set; }
        public bool PowerManagementSupported { get; set; }
        public long ScreenHeight { get; set; }
        public long ScreenWidth { get; set; }
        public string Status { get; set; }
        public int StatusInfo { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
    }
}