using System;

namespace Model.EF6
{
    public class Processor
    {
        public int AddressWidth { get; set; }
        public int Architecture { get; set; }
        public string AssetTag { get; set; }
        public int Availability { get; set; }
        public string Caption { get; set; }
        public long Characteristics { get; set; }
        public long ConfigManagerErrorCode { get; set; }
        public string ConfigManagerUserConfig { get; set; }
        public int CpuStatus { get; set; }
        public string CreationClassName { get; set; }
        public long CurrentClockSpeed { get; set; }
        public int CurrentVoltage { get; set; }
        public int DataWidth { get; set; }
        public string Description { get; set; }
        public string DeviceID { get; set; }
        public bool? ErrorCleared { get; set; }
        public string ErrorDescription { get; set; }
        public long ExtClock { get; set; }
        public int Family { get; set; }
        public DateTime? InstallDate { get; set; }
        public long L2CacheSize { get; set; }
        public long L2CacheSpeed { get; set; }
        public long L3CacheSize { get; set; }
        public long L3CacheSpeed { get; set; }
        public long LastErrorCode { get; set; }
        public int Level { get; set; }
        public int LoadPercentage { get; set; }
        public string Manufacturer { get; set; }
        public long MaxClockSpeed { get; set; }
        public string Name { get; set; }
        public long NumberOfCores { get; set; }
        public long NumberOfEnabledCore { get; set; }
        public long NumberOfLogicalProcessors { get; set; }
        public string OtherFamilyDescription { get; set; }
        public string PartNumber { get; set; }
        public string PNPDeviceID { get; set; }
        public int[] PowerManagementCapabilities { get; set; }
        public bool PowerManagementSupported { get; set; }
        public string ProcessorId { get; set; }
        public int ProcessorType { get; set; }
        public int Revision { get; set; }
        public string Role { get; set; }
        public bool SecondLevelAddressTranslationExtensions { get; set; }
        public string SerialNumber { get; set; }
        public string SocketDesignation { get; set; }
        public string Status { get; set; }
        public int StatusInfo { get; set; }
        public string Stepping { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public long ThreadCount { get; set; }
        public string UniqueId { get; set; }
        public int UpgradeMethod { get; set; }
        public string Version { get; set; }
        public bool VirtualizationFirmwareEnabled { get; set; }
        public bool VMMonitorModeExtensions { get; set; }
        public long VoltageCaps { get; set; }
    }
}