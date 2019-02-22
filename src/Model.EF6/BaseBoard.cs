using System;
using System.Collections.Generic;

namespace Model.EF6
{
    public class BaseBoard
    {
        public string Caption { get; set; }
        public string ConfigOptions { get; set; }
        public string CreationClassName { get; set; }
        public float Depth { get; set; }
        public string Description { get; set; }
        public float Height { get; set; }
        public bool HostingBoard { get; set; }
        public bool HotSwappable { get; set; }
        public DateTime? InstallDate { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string OtherIdentifyingInfo { get; set; }
        public string PartNumber { get; set; }
        public bool PoweredOn { get; set; }
        public string Product { get; set; }
        public bool Removable { get; set; }
        public bool Replaceable { get; set; }
        public string RequirementsDescription { get; set; }
        public bool RequiresDaughterBoard { get; set; }
        public string SerialNumber { get; set; }
        public string SKU { get; set; }
        public string SlotLayout { get; set; }
        public bool SpecialRequirements { get; set; }
        public string Status { get; set; }
        public string Tag { get; set; }
        public string Version { get; set; }
        public float Weight { get; set; }
        public float Width { get; set; }
    }
}