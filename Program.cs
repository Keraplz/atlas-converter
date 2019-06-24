using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtlasConverter
{
    public class Atlas
    {
        public uint base_w { get; set; } = 0;
        public uint base_h { get; set; } = 0;
        public IList<AtlasDataItem> adi { get; set; }
    }
    public class AtlasDataItem
    {
        public int reader_type_1 = 0;
        public int name_size = 0;
        public string filename = string.Empty;

        public int reader_type_2 { get; set; } = 0;

        public uint sprite_x { get; set; } = 0;
        public uint sprite_y { get; set; } = 0;
        public uint sprite_w { get; set; } = 0;
        public uint sprite_h { get; set; } = 0;

        public float bounding_box_x { get; set; } = 0;
        public float bounding_box_y { get; set; } = 0;
        public float bounding_box_w { get; set; } = 0;
        public float bounding_box_h { get; set; } = 0;
    }
    public class AtlasFile : BinaryReader
    {
        public AtlasFile(MemoryStream ms) : base(ms) { }
        public AtlasFile(string filepath) : base(new MemoryStream(File.ReadAllBytes(filepath))) { }

        readonly byte[] READER_BLOCK = new byte[]{ 0x03, 0x74, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x41, 0x74, 0x6C,
            0x61, 0x73, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2E, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65,
            0x41, 0x74, 0x6C, 0x61, 0x73, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2C, 0x20, 0x54, 0x65, 0x78, 0x74,
            0x75, 0x72, 0x65, 0x41, 0x74, 0x6C, 0x61, 0x73, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x50, 0x43,
            0x2C, 0x20, 0x56, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x30,
            0x2C, 0x20, 0x43, 0x75, 0x6C, 0x74, 0x75, 0x72, 0x65, 0x3D, 0x6E, 0x65, 0x75, 0x74, 0x72, 0x61, 0x6C,
            0x2C, 0x20, 0x50, 0x75, 0x62, 0x6C, 0x69, 0x63, 0x4B, 0x65, 0x79, 0x54, 0x6F, 0x6B, 0x65, 0x6E, 0x3D,
            0x6E, 0x75, 0x6C, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x75, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x41,
            0x74, 0x6C, 0x61, 0x73, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2E, 0x54, 0x65, 0x78, 0x74, 0x75,
            0x72, 0x65, 0x52, 0x65, 0x67, 0x69, 0x6F, 0x6E, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2C, 0x20, 0x54,
            0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x41, 0x74, 0x6C, 0x61, 0x73, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E,
            0x74, 0x50, 0x43, 0x2C, 0x20, 0x56, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x31, 0x2E, 0x30, 0x2E,
            0x30, 0x2E, 0x30, 0x2C, 0x20, 0x43, 0x75, 0x6C, 0x74, 0x75, 0x72, 0x65, 0x3D, 0x6E, 0x65, 0x75, 0x74,
            0x72, 0x61, 0x6C, 0x2C, 0x20, 0x50, 0x75, 0x62, 0x6C, 0x69, 0x63, 0x4B, 0x65, 0x79, 0x54, 0x6F, 0x6B,
            0x65, 0x6E, 0x3D, 0x6E, 0x75, 0x6C, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x2F, 0x4D, 0x69, 0x63, 0x72, 0x6F,
            0x73, 0x6F, 0x66, 0x74, 0x2E, 0x58, 0x6E, 0x61, 0x2E, 0x46, 0x72, 0x61, 0x6D, 0x65, 0x77, 0x6F, 0x72,
            0x6B, 0x2E, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2E, 0x52, 0x65, 0x63, 0x74, 0x61, 0x6E, 0x67,
            0x6C, 0x65, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };


        char Read_char()
        {
            byte[] buffer = new byte[1];
            base.Read(buffer, 0, 1);
            return System.Text.Encoding.UTF8.GetChars(buffer)[0];
        }
        int Read_int()
        {
            byte[] buffer = new byte[1];
            base.Read(buffer, 0, 1);
            return buffer[0];
        }
        uint Read_uint32()
        {
            byte[] buffer = new byte[4];
            base.Read(buffer, 0, 4);
            return System.BitConverter.ToUInt32(buffer, 0);
        }
        float Read_float()
        {
            byte[] buffer = new byte[4];
            base.Read(buffer, 0, 4);
            return System.BitConverter.ToSingle(buffer, 0);
        }
        int Read_7bit() => base.Read7BitEncodedInt();

        public new Atlas Read()
        {
            IList<AtlasDataItem> atlas_data_items = new List<AtlasDataItem>();

            // XNB header
            char format_identifier = Read_char();
            format_identifier += Read_char();
            format_identifier += Read_char();
            char platform = Read_char();
            int xnb_version = Read_int();
            int flag = Read_int();
            uint file_size = Read_uint32();

            // XNB type readers
            int type_reader_count = Read_7bit();


            for (int i = 0; i < READER_BLOCK.Length - 1; i++)
                Read_char();

            // XNB data
            uint atlas_frame_count = Read_uint32();


            for (int i = 0; i < atlas_frame_count; i++)
            {
                AtlasDataItem adi = new AtlasDataItem
                {
                    reader_type_1 = Read_int(),
                    name_size = Read_int(),
                    filename = ""
                };

                for (int n = 0; n < adi.name_size; n++)
                    adi.filename += Read_char();

                adi.reader_type_2 = Read_int();
                adi.sprite_x = Read_uint32();
                adi.sprite_y = Read_uint32();
                adi.sprite_w = Read_uint32();
                adi.sprite_h = Read_uint32();

                adi.bounding_box_x = Read_float();
                adi.bounding_box_y = Read_float();
                adi.bounding_box_w = Read_float();
                adi.bounding_box_h = Read_float();

                atlas_data_items.Add(adi);
            }

            return new Atlas
            {
                base_w = Read_uint32(),
                base_h = Read_uint32(),
                adi = atlas_data_items
            };
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            AtlasFile atlas = new AtlasFile("atlas.xnb");
            write_json(atlas.Read(), "atlas.json");
            Environment.Exit(Environment.ExitCode);
        }

        public struct Frames
        {
            public Sprite[] frames;
        }

        public struct Sprite
        {
            public string filename;
            public Frame frame;
            public bool rotated;
            public bool trimmed;
            public SpriteSourceSize spriteSourceSize;
            public SourceSize sourceSize;

            public Sprite(string filename, Frame frame, SpriteSourceSize spriteSourceSize, SourceSize sourceSize)
            {
                this.filename = filename;
                this.frame = frame;
                this.rotated = false;
                this.trimmed = false;
                this.spriteSourceSize = spriteSourceSize;
                this.sourceSize = sourceSize;

                if (this.spriteSourceSize.w != this.sourceSize.w && this.spriteSourceSize.h != this.sourceSize.h)
                    this.trimmed = true;
            }
        }

        public struct Frame
        {
            public int x;
            public int y;
            public int w;
            public int h;

            public Frame(int x, int y, int w, int h)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
            }
        }

        public struct SpriteSourceSize
        {
            public int x;
            public int y;
            public int w;
            public int h;

            public SpriteSourceSize(int x, int y, int w, int h)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
            }
        }

        public struct SourceSize
        {
            public int w;
            public int h;

            public SourceSize(int w, int h)
            {
                this.w = w;
                this.h = h;
            }
        }

        static void write_json(Atlas value, string filepath)
        {
            IList<Sprite> frameArray = new List<Sprite>();
            foreach (AtlasDataItem adi in value.adi)
            {
                frameArray.Add(new Sprite(adi.filename + ".png",
                    new Frame((int)adi.sprite_x, (int)adi.sprite_y, (int)adi.sprite_w, (int)adi.sprite_h),
                    new SpriteSourceSize((int)adi.bounding_box_x, (int)adi.bounding_box_y, (int)adi.bounding_box_w, (int)adi.bounding_box_h),
                    new SourceSize((int)value.base_w, (int)value.base_h)
                    ));
            }

            using (StreamWriter sw = new StreamWriter(filepath))
            {
                JsonSerializer serializer = new JsonSerializer { Formatting = Formatting.Indented };
                serializer.Serialize(sw, new Frames { frames = frameArray.ToArray() });
            }
        }
    }
}
