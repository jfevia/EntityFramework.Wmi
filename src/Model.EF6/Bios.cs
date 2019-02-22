using System;

namespace Model.EF6
{
    public class Bios
    {
        public string BiosCharacteristics { get; set; }
        public string BIOSVersion { get; set; }
        public string BuildNumber { get; set; }
        public string Caption { get; set; }
        public string CodeSet { get; set; }
        public string CurrentLanguage { get; set; }
        public string Description { get; set; }
        public byte EmbeddedControllerMajorVersion { get; set; }
        public byte EmbeddedControllerMinorVersion { get; set; }
        public string IdentificationCode { get; set; }
        public int InstallableLanguages { get; set; }
        public DateTime? InstallDate { get; set; }
        public string LanguageEdition { get; set; }
        public string ListOfLanguages { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string OtherTargetOS { get; set; }
        public bool PrimaryBIOS { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string SerialNumber { get; set; }
        public string SMBIOSBIOSVersion { get; set; }
        public int SMBIOSMajorVersion { get; set; }
        public int SMBIOSMinorVersion { get; set; }
        public bool SMBIOSPresent { get; set; }
        public string SoftwareElementID { get; set; }
        public int SoftwareElementState { get; set; }
        public string Status { get; set; }
        public byte SystemBiosMajorVersion { get; set; }
        public byte SystemBiosMinorVersion { get; set; }
        public int TargetOperatingSystem { get; set; }
        public string Version { get; set; }
    }
}