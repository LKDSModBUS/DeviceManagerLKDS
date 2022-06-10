using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagerLKDS.Classes
{
    public static class Enums
    {
        public enum AdressTest
        {
             [Description("0x12 0x00")] Adres32 = 0x00,
             [Description("0x12 0x10")] Adres33 = 0x01,
             [Description("0x12 0x20")] Adres34 = 0x02,
             [Description("0x12 0x30")] Adres35 = 0x03,
             [Description("0x12 0x40")] Adres36 = 0x04,
             [Description("0x12 0x50")] Adres37 = 0x05,
             [Description("0x12 0x60")] Adres38 = 0x06,
             [Description("0x12 0x70")] Adres39 = 0x07,
             [Description("0x12 0x80")] Adres40 = 0x08,
             [Description("0x12 0x90")] Adres41 = 0x09,
             [Description("0x12 0xA0")] Adres42 = 0x0A,
             [Description("0x12 0xB0")] Adres43 = 0x0B,
             [Description("0x12 0xC0")] Adres44 = 0x0C,
             [Description("0x12 0xD0")] Adres45 = 0x0D,
             [Description("0x12 0xE0")] Adres46 = 0x0E,
             [Description("0x12 0xF0")] Adres47 = 0x0F,
             [Description("0x13 0x00")] Adres48 = 0x10,
             [Description("0x13 0x10")] Adres49 = 0x11,
             [Description("0x13 0x20")] Adres50 = 0x12,
             [Description("0x13 0x30")] Adres51 = 0x13,
             [Description("0x13 0x40")] Adres52 = 0x14,
             [Description("0x13 0x50")] Adres53 = 0x15,
             [Description("0x13 0x60")] Adres54 = 0x16,
             [Description("0x13 0x70")] Adres55 = 0x17,
             [Description("0x13 0x80")] Adres56 = 0x18,
             [Description("0x13 0x90")] Adres57 = 0x19,
             [Description("0x13 0xA0")] Adres58 = 0x1A,
             [Description("0x13 0xB0")] Adres59 = 0x1B,
             [Description("0x13 0xC0")] Adres60 = 0x1C,
             [Description("0x13 0xD0")] Adres61 = 0x1D,
             [Description("0x13 0xE0")] Adres62 = 0x1E,
             [Description("0x13 0xF0")] Adres63 = 0x1F

        }
        
    }
}
