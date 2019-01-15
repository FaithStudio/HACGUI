﻿using HACGUI.Extensions;
using HACGUI.Main.TitleManager.ApplicationWindow.Tabs;
using LibHac;
using LibHac.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HACGUI.Main.TitleManager
{
    public class ApplicationElement
    {
        static readonly ImageSource UnknownIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/UnknownTitle.jpg", UriKind.RelativeOrAbsolute));

        static readonly List<TitleType> Priority = new List<TitleType>() { TitleType.Application, TitleType.Patch, TitleType.AddOnContent };

        public List<Title> Titles { get; set; } = new List<Title>();
        public List<TitleElement> TitleElements
        {
            get
            {
                List<TitleElement> titles = new List<TitleElement>();
                foreach (Title title in Titles)
                    titles.Add(new TitleElement() { Title = title });
                return titles;
            }
        }

        public ImageSource Icon { get; set; } = UnknownIcon;

        public string Name
        {
            get {
                foreach (Title title in Titles)
                {
                    switch (title.Metadata.Type)
                    {
                        case TitleType.Patch:
                        case TitleType.Application:
                            return title.Name;
                    }
                }
                List<Title> titles = OrderTitlesByBest();
                titles.Reverse();
                return SystemTitleNames.GetName(titles.First());
            }
        }
        public ulong TitleId => FindBestTitle().Id;
        public ulong BaseTitleId => FindBestTitle().GetBaseTitleID();
        public string FriendlyVersion
        {
            get
            {
                List<Title> list = OrderTitlesByBest();
                list.Reverse();
                foreach (Title title in list)
                {
                    if (title.Metadata.Type != TitleType.AddOnContent)
                    {
                        if (title.Control != null)
                            return title.Control.DisplayVersion;
                        else
                            return title.Version.ToString();
                    }
                }
                return new TitleVersion(0).ToString();
            }
        }
        public TitleVersion Version
        {
            get
            {
                List<Title> list = OrderTitlesByBest();
                list.Reverse();
                foreach (Title title in list)
                {
                    if (title.Metadata.Type != TitleType.AddOnContent)
                        return title.Version;
                }
                return new TitleVersion(0);
            }
        }
        public List<TitleType> Types
        {
            get
            {
                HashSet<TitleType> set = new HashSet<TitleType>(); // ensure that there are no duplicates
                foreach (Title title in OrderTitlesByBest())
                    set.Add(title.Metadata.Type);
                List<TitleType> list = new List<TitleType>();
                list.AddRange(set);
                return list;
            }
        }
        public string TypesAsString => string.Join(", ", Types);

        public string BcatPassphrase
        {
            get
            {
                List<Title> titles = OrderTitlesByBest();
                Title title = titles.LastOrDefault((x) => x.Metadata.Type == TitleType.Patch);
                if (title == null)
                    title = titles.FirstOrDefault((x) => x.Metadata.Type == TitleType.Application);
                if (title == null)
                    return "";
                else
                    return title.Control.BcatPassphrase;
            }
        }

        public long Size
        {
            get
            {
                long size = 0;
                foreach (Title title in Titles)
                    size += title.GetSize();
                return size;
            }
        }

        public void Load()
        {
            GetIconAync().ContinueWith((source) => Icon = source.Result);
        }

        public Title FindBestTitle()
        {
            foreach(TitleType type in Priority)
            {
                Title title = Titles.FirstOrDefault(x => x?.Metadata.Type == type);
                if (title != null)
                    return title;
            }
            return Titles.First();
        }

        public List<Title> OrderTitlesByBest()
        {
            List<Title> list = new List<Title>();
            foreach (TitleType type in Priority) // first find the types that need to be in a specifc order
            {
                Title title = Titles.FirstOrDefault(x => x?.Metadata.Type == type);
                if (title != null)
                    list.Add(title);
            }
            IEnumerable<Title> unorganized = Titles.Except(list); // then add the types that don't need to be organized afterwards
            list.AddRange(unorganized);
            return list;
        }

        public async Task<ImageSource> GetIconAync()
        {
            List<Title> list = OrderTitlesByBest();
            list.Reverse();
            foreach (Title title in list)
            {
                Task<ImageSource> source = FindTitleIcon(title);
                if (source != null)
                    return await source;
            }

            return await RootWindow.Current.Submit(new Task<ImageSource>(() => UnknownIcon));
        }

        public Task<ImageSource> FindTitleIcon(Title title)
        {
            if (title.ControlNca != null)
            {
                NcaSection meta = title.ControlNca.Sections.FirstOrDefault(x => x?.Type == SectionType.Romfs);
                RomFsFileSystem controlFS = new RomFsFileSystem(title.ControlNca.OpenSection(meta.SectionNum, false, IntegrityCheckLevel.ErrorOnInvalid, false));
                string iconFile = null;
                foreach (KeyValuePair<string, RomfsFile> filekv in controlFS.FileDict)
                {
                    string name = filekv.Key; // just find any
                    if (name.StartsWith("/icon_") && name.EndsWith(".dat"))
                    {
                        iconFile = name;
                        break;
                    }
                }

                if (iconFile != null)
                    return RootWindow.Current.Submit(new Task<ImageSource>(() =>
                    {
                        try
                        {
                            JpegBitmapDecoder decoder = new JpegBitmapDecoder(controlFS.OpenFile(iconFile, OpenMode.Read).AsStream(), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                            decoder.Frames[0].Freeze();
                            return decoder.Frames[0];
                        } catch(Exception)
                        {
                            return null;
                        }
                    }));
            }
            return null;
        }
    }
}
