﻿using HACGUI.Main;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using Xamarin.Forms.Dynamic;

namespace HACGUI.Services
{
    public class StatusService
    {

        static StatusService()
        {
            InitDefaultStatus();
        }

        public static ObservableDictionary<string, Status> Statuses;

        public static StatusBar Bar;

        public static void Start()
        {
            Statuses.CollectionChanged += CollectionChanged;
            Update();
        }

        public static void Stop()
        {
            Bar = null;
            InitDefaultStatus();
        }

        private static void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Update();
        }

        public static void Update()
        {
            Bar.Dispatcher.BeginInvoke(new Action(() =>
            {
                List<string> foundItems = new List<string>();
                foreach (StatusEntry entry in Bar.Items)
                {
                    entry.Shape.Fill = GetBrush(Statuses[entry.TextBox.Text]);
                    foundItems.Add(entry.TextBox.Text);
                }
                
                foreach (string toBeAdded in new List<string>(Statuses.Keys.Except(foundItems)))
                {
                    StatusEntry entry = new StatusEntry();
                    entry.TextBox.Text = toBeAdded;
                    entry.Shape.Fill = GetBrush(Statuses[toBeAdded]);
                    Bar.Items.Add(entry);
                }

                foreach(StatusEntry entry in new List<StatusEntry>(Bar.Items.Cast<StatusEntry>()))
                {
                    if (!Statuses.Keys.Contains(entry.TextBox.Text))
                        Bar.Items.Remove(entry);
                }
            })).Wait();
        }

        private static Brush GetBrush(Status status)
        {
            switch (status)
            {
                case Status.Incorrect:
                    return new SolidColorBrush(Colors.Red);
                case Status.OK:
                    return new SolidColorBrush(Colors.Green);
                case Status.Progress:
                    return new SolidColorBrush(Colors.Orange);
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
        }

        public static void InitDefaultStatus()
        {
            Statuses = new ObservableDictionary<string, Status>()
            {
                { "SD" , Status.Incorrect },
                { "NAND" , Status.Incorrect },
            };
        }

        public static Status SDStatus
        {
            get => Statuses["SD"];
            set => Statuses["SD"] = value;
        }

        public static Status NANDStatus
        {
            get => Statuses["NAND"];
            set => Statuses["NAND"] = value;
        }
        public enum Status
        {
            Incorrect, Progress, OK
        }
    }
}
