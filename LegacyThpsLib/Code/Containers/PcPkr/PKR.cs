using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LegacyThps.Containers
{
    public enum PkrVersion
    {
        PKR2 = 2,
        PKR3 = 3
    }

    public class PKR
    {
        public PkrVersion Version = PkrVersion.PKR2;

        public List<PKRFolder> folders = new List<PKRFolder>();
        public List<PKRFile> files = new List<PKRFile>();

        /// <summary>
        /// This function ignores files in the passed folder.
        /// You can use root game folder and it will only use all the folders within.
        /// </summary>
        /// <param name="folder"></param>
        public void AddSubfolders(string folder)
        {
            foreach (var dir in Directory.GetDirectories(folder, "*"))
                AddFolder(dir);
        }

        public void AddFolder(string folder)
        {
            var dirs = Directory.GetDirectories(folder, "*", SearchOption.AllDirectories);

            int totalfiles = files.Count;

            // convert all existing folders beforehand, including the empty ones
            foreach (var dir in dirs)
            {
                var foldername = dir.Replace(Path.GetDirectoryName(folder) + "\\", "");

                var fold = new PKRFolder() { name = foldername + "\\" };

                folders.Add(fold);

                // process files

                var filenames = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);

                foreach (var name in filenames)
                {
                    var filename = Path.GetFileName(name);
                    var file = new PKRFile(name);

                    files.Add(file);
                    fold.Files.Add(file);
                }

                // fix indexing

                fold.offset = (uint)totalfiles;
                fold.count = (uint)fold.Files.Count;
                totalfiles += fold.Files.Count;
            }


            // try zip compress in parallel
            /*
            var tasks = new List<Task>();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => { 
                    file.TryCompress();
                    GC.Collect();
                }));
            }

            Task.WaitAll(tasks.ToArray());
            */
        }


        public PKR()
        {
        }

        public PKR(string filename)
        {
            using (var br = new BinaryReader(File.OpenRead(filename)))
            {
                uint magic = br.ReadUInt32();

                switch (magic)
                {
                    case 0x32524B50: ReadPKR2(br); break;
                    case 0x33524B50: ReadPKR3(br); break;
                    default: throw new Exception($"Unsupported PKR version.");
                }
            }
        }

        public static PKR FromFile(string filename) => new PKR(filename);


        private void ReadPKR2(BinaryReader br)
        {
            Version = PkrVersion.PKR2;

            br.ReadInt32(); // 1?

            int numFolders = br.ReadInt32();
            int numFiles = br.ReadInt32();

            for (int i = 0; i < numFolders; i++)
                folders.Add(new PKRFolder(br));

            for (int i = 0; i < numFiles; i++)
                files.Add(new PKRFile(br, Version));

            foreach (var f in files)
                f.GetData(br);
        }

        private void ReadPKR3(BinaryReader br)
        {
            Version = PkrVersion.PKR3;

            // version 3 moves header to the end, so we have a pointer here instead
            br.BaseStream.Position = br.ReadInt32();

            // and now we just normally read PKR header with pkr3 file struct
            br.ReadInt32(); // 4?
            int numFolders = br.ReadInt32();
            int numFiles = br.ReadInt32();

            for (int i = 0; i < numFolders; i++)
                folders.Add(new PKRFolder(br));

            for (int i = 0; i < numFiles; i++)
                files.Add(new PKRFile(br, Version));

            foreach (var f in files)
                f.GetData(br);
        }

        public void Export(string path)
        {
           // var f2 = new Form2();

           // f2.Show();
           // f2.progressBar1.Maximum = files.Count;

            foreach (var pf in folders)
            {
                Directory.CreateDirectory(Path.Combine(path, pf.name));

                int index = 0;

                if (Version == PkrVersion.PKR2) index = (int)((pf.offset - (12 + folders.Count * 40)) / (32 + 16));
                if (Version == PkrVersion.PKR3) index = (int)pf.offset;

                for (int i = index; i < index + pf.count; i++)
                {
                  //  f2.progressBar1.Value = i;
                    string filename = path + "\\" + pf.name + files[i].name;
                    filename = filename.Replace("/", "\\");
                 //   f2.label1.Text = "Now extracting: " + filename;

                    files[i].Save(filename);

                    Application.DoEvents();
                }
            }

            // f2.Close();
        }

        public void Save(string path)
        {
            using (var bw = new BinaryWriter(File.OpenWrite(path)))
            {
                switch (Version)
                {
                    case PkrVersion.PKR2: WritePKR2(bw); break;
                    case PkrVersion.PKR3: WritePKR3(bw); break;
                    default: throw new Exception("Unsupported version.");
                }
            }
        }

        // mhpb
        public void WritePKR3(BinaryWriter bw)
        {
            if (Version != PkrVersion.PKR2 && Version != PkrVersion.PKR3)
                throw new Exception("unsupported version");

            bw.Write(0x33524B50);

            // offset to folders
            bw.Write((int)-1);

            foreach (var file in files)
            {
                file.offset = (int)bw.BaseStream.Position;
                bw.Write(file.data);
            }

            int pos_folders = (int)bw.BaseStream.Position;

            bw.Write((int)4);
            bw.Write(folders.Count);
            bw.Write(files.Count);

            foreach (var folder in folders)
                folder.Write(bw);

            foreach (var file in files)
                file.Write(bw, Version);

            bw.BaseStream.SetLength(bw.BaseStream.Position);

            // fix folders pointer
            bw.BaseStream.Position = 4;
            bw.Write(pos_folders);
        }

        // thps2
        public void WritePKR2(BinaryWriter bw)
        {
            if (Version != PkrVersion.PKR2 && Version != PkrVersion.PKR3)
                throw new Exception("unsupported version");

            bw.Write(0x32524B50);

            bw.Write((int)1); // whatever that is
            bw.Write(folders.Count);
            bw.Write(files.Count);

            foreach (var folder in folders)
                folder.Write(bw);

            foreach (var file in files)
                file.Write(bw, Version);

            foreach (var file in files)
            {
                file.offset = (int)bw.BaseStream.Position;
                bw.Write(file.data);
            }

            bw.BaseStream.SetLength(bw.BaseStream.Position);
        }
    }
}