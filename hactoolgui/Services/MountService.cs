﻿using DokanNet;
using LibHac;
using LibHac.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HACGUI.Services
{
    public class MountService
    {
        private static char[] DriveLetters = "CDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();

        public static readonly string PathSeperator = "/";
        
        private static Dictionary<MountableFileSystem, Tuple<Thread, char>> Mounted = new Dictionary<MountableFileSystem, Tuple<Thread, char>>();

        static MountService()
        {
            RootWindow.Current.Closed += (_, __) => UnmountAll();
        }

        public static void Mount(MountableFileSystem fs)
        {
            if (CanMount())
            {
                char drive = GetAvailableDriveLetter();
                Thread thread = new Thread(new ThreadStart(() => 
                {
                    Dokan.Mount(fs, $"{drive}:", DokanOptions.RemovableDrive | DokanOptions.WriteProtection);
                    Mounted.Remove(fs);
                }
                ));
                if (Mounted.ContainsKey(fs))
                    Unmount(fs);
                thread.Start();
                Mounted[fs] = new Tuple<Thread, char>(thread, drive);
            }
        }

        public static char GetAvailableDriveLetter()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            char[] currentDrives = new char[drives.Length];
            for (int i = 0; i < drives.Length; i++)
                currentDrives[i] = drives[i].Name[0];
            return DriveLetters.Except(currentDrives).First();  
        }

        public static void Unmount(MountableFileSystem fs)
        {
            Dokan.Unmount(Mounted[fs].Item2);
            string mountPoint = $"{Mounted[fs].Item2}:";
            Mounted[fs].Item1.Join();
            Dokan.RemoveMountPoint(mountPoint);
        }

        public static void UnmountAll()
        {
            foreach (MountableFileSystem fs in new List<MountableFileSystem>(Mounted.Keys))
                Unmount(fs);
            Mounted.Clear();
        }

        public static bool CanMount()
        {
            try
            {
                Dokan.DriverVersion.ToString();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class MountableFileSystem : IDokanOperations
    {
        public readonly string Name;
        private readonly IFileSystem Fs;
        private readonly Dictionary<IFile, FileStorage> OpenedFiles;

        public MountableFileSystem(IAttributeFileSystem fs, string name)
        {
            Fs = fs;
            Name = name;
            OpenedFiles = new Dictionary<IFile, FileStorage>();
        }

        public void Cleanup(string fileName, DokanFileInfo info)
        {
            foreach (IFile file in OpenedFiles.Keys)
                CloseFile(file);
        }

        public void CloseFile(string fileName, DokanFileInfo info)
        {
            CloseFile(Fs.OpenFile(fileName, OpenMode.Read));
        }

        public void CloseFile(IFile file)
        {
            if (OpenedFiles.ContainsKey(file))
            {
                OpenedFiles[file].Dispose();
                OpenedFiles.Remove(file);
            }
        }

        public NtStatus CreateFile(string fileName, DokanNet.FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, DokanFileInfo info)
        {
            return NtStatus.Success;
        }

        public NtStatus DeleteDirectory(string fileName, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus DeleteFile(string fileName, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, DokanFileInfo info)
        {
            return FindFilesWithPattern(fileName, "*", out files, info);
        }

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, DokanFileInfo info)
        {
            IDirectory directory = GetDirectory(fileName, OpenDirectoryMode.All);
            files = null;
            if (directory != null)
            {
                files = new List<FileInformation>();
                if (searchPattern.EndsWith("\"*")) // thanks windows
                    searchPattern = searchPattern.Replace("\"*", "*");
                foreach (DirectoryEntry entry in directory.EnumerateEntries(searchPattern, SearchOptions.Default))
                    files.Add(CreateInfo(entry));
                return NtStatus.Success;
            }
            return NtStatus.NotADirectory;
        }

        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, DokanFileInfo info)
        {
            streams = null;
            return NtStatus.NotImplemented;
        }

        public NtStatus FlushFileBuffers(string fileName, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, DokanFileInfo info)
        {
            freeBytesAvailable = 0;
            totalNumberOfBytes = 0;
            totalNumberOfFreeBytes = 0;
            return NtStatus.Success;
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, DokanFileInfo info)
        {
            if (GetDirectory(fileName, OpenDirectoryMode.All) != null)
                fileInfo = CreateInfo(GetDirectory(fileName, OpenDirectoryMode.All));
            else if (GetFile(fileName, OpenMode.Read) != null)
                fileInfo = CreateInfo(GetFile(fileName, OpenMode.Read), fileName);
            else
            {
                fileInfo = new FileInformation();
                return NtStatus.NoSuchFile;
            }
            return NtStatus.Success;
        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, DokanFileInfo info)
        {
            security = null;
            return NtStatus.NotImplemented;
        }

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, DokanFileInfo info)
        {
            volumeLabel = Name;
            features = FileSystemFeatures.ReadOnlyVolume; // TODO sort out write support
            fileSystemName = Name;
            maximumComponentLength = int.MaxValue; // idk lol
            return NtStatus.Success;
        }

        public NtStatus LockFile(string fileName, long offset, long length, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus Mounted(DokanFileInfo info)
        {
            return NtStatus.Success;
        }

        public NtStatus MoveFile(string oldName, string newName, bool replace, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, DokanFileInfo info)
        {
            IFile file = GetFile(fileName, OpenMode.Read);
            if (file != null)
            {
                FileStorage storage = OpenFile(file);
                if (storage != null)
                {
                    long distanceToEof = storage.Length - buffer.Length;

                    storage.Read(buffer, Math.Min(offset, distanceToEof), (int)Math.Min(buffer.Length, storage.Length), 0);
                    bytesRead = buffer.Length; // TODO accuracy
                    return NtStatus.Success;
                } else
                {
                    bytesRead = 0;
                    return NtStatus.Unsuccessful;
                }
            }
            bytesRead = 0;
            return NtStatus.NoSuchFile;
        }

        public NtStatus SetAllocationSize(string fileName, long length, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus SetEndOfFile(string fileName, long length, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, DokanFileInfo info)
        {
            return NtStatus.NotImplemented;
        }

        public NtStatus Unmounted(DokanFileInfo info)
        {
            foreach (IStorage storage in OpenedFiles.Values)
                storage.Dispose();
            OpenedFiles.Clear();
            return NtStatus.Success;
        }

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, DokanFileInfo info)
        {
            bytesWritten = 0;
            return NtStatus.NotImplemented;
        }

        private FileInformation CreateInfo(DirectoryEntry entry)
        {
            switch (entry.Type)
            {
                case DirectoryEntryType.File:
                    return CreateInfo(Fs.OpenFile(entry.FullPath, OpenMode.Read), entry.FullPath);
                case DirectoryEntryType.Directory:
                    return CreateInfo(Fs.OpenDirectory(entry.FullPath, OpenDirectoryMode.All));
            }
            return new FileInformation();
        }

        private FileInformation CreateInfo(IFile file, string path)
        {
            if (file != null) {
                FileStorage storage = OpenFile(file);
                return new FileInformation
                {
                    FileName = path,
                    Length = storage.Length,
                    Attributes = FileAttributes.ReadOnly
                };
            }
            else
                return new FileInformation();
        }

        private static FileInformation CreateInfo(IDirectory directory)
        {
            if (directory != null)
                return new FileInformation
                {
                    FileName = directory.FullPath,
                    Attributes = FileAttributes.Directory
                };
            else
                return new FileInformation();
        }

        public IFile GetFile(string name, OpenMode mode)
        {
            name = name.Replace($"{Path.DirectorySeparatorChar}", MountService.PathSeperator);
            if (name.StartsWith(MountService.PathSeperator))
                name = name.Substring(MountService.PathSeperator.Length);
            if(Fs.FileExists(name))
                return Fs.OpenFile(name, mode);
            return null;
        }

        public IDirectory GetDirectory(string name, OpenDirectoryMode mode)
        {
            name = name.Replace($"{Path.DirectorySeparatorChar}", MountService.PathSeperator);
            if (name.StartsWith(MountService.PathSeperator))
                name = name.Substring(MountService.PathSeperator.Length);
            if(Fs.DirectoryExists(name))
                return Fs.OpenDirectory(name, mode);
            return null;
        }

        public FileStorage OpenFile(IFile file)
        {
            IFile key = OpenedFiles.Keys.FirstOrDefault(f => f.Equals(file));
            if (key != null)
                return OpenedFiles[key];

            try
            {
                FileStorage storage = new FileStorage(file);
                OpenedFiles[file] = storage;
                return storage;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }
    }
}
