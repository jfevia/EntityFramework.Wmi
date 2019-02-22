using System;

namespace Model.EF6
{
    public class DiskDrive
    {
        public int Availability { get; set; }
        public long BytesPerSector { get; set; }
        public string Capabilities { get; set; }
        public string CapabilityDescriptions { get; set; }
        public string Caption { get; set; }
        public string CompressionMethod { get; set; }
        public long ConfigManagerErrorCode { get; set; }
        public bool ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public decimal? DefaultBlockSize { get; set; }
        public string Description { get; set; }
        public string DeviceID { get; set; }
        public bool ErrorCleared { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorMethodology { get; set; }
        public string FirmwareRevision { get; set; }
        public long Index { get; set; }
        public DateTime? InstallDate { get; set; }
        public string InterfaceType { get; set; }
        public long LastErrorCode { get; set; }
        public string Manufacturer { get; set; }
        public decimal MaxBlockSize { get; set; }
        public decimal MaxMediaSize { get; set; }
        public bool MediaLoaded { get; set; }
        public string MediaType { get; set; }
        public decimal MinBlockSize { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public bool NeedsCleaning { get; set; }
        public long NumberOfMediaSupported { get; set; }
        public long Partitions { get; set; }
        public string PNPDeviceID { get; set; }
        public string PowerManagementCapabilities { get; set; }
        public bool PowerManagementSupported { get; set; }
        public long SCSIBus { get; set; }
        public int SCSILogicalUnit { get; set; }
        public int SCSIPort { get; set; }
        public int SCSITargetId { get; set; }
        public long SectorsPerTrack { get; set; }
        public string SerialNumber { get; set; }
        public long Signature { get; set; }
        public decimal Size { get; set; }
        public string Status { get; set; }
        public int StatusInfo { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public decimal TotalCylinders { get; set; }
        public long TotalHeads { get; set; }
        public decimal TotalSectors { get; set; }
        public decimal TotalTracks { get; set; }
        public long TracksPerCylinder { get; set; }
    }
}