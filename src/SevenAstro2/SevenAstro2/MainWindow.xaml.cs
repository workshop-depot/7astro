using Microsoft.Win32;
using SevenAstro2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Layout;

namespace SevenAstro2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Global.SetDateFormat();
            //Global.MainWindow = this;
        }

        SaveFileDialog _sfd;
        OpenFileDialog _ofd;
        int newDocNum = 0;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var cmd = sender as MenuItem;

            if (cmd != null)
            {
                switch (cmd.CommandParameter.Text())
                {
                    case "open":
                        {
                            OnOpenImp();
                        } break;
                    case "save":
                        {
                            OnSaveImp();
                        } break;
                    case "print":
                        {
                            OnPrintImp();
                        } break;
                    case "exit":
                        {
                            OnCloaseAction();
                        } break;
                    case "time_stepper":
                        {
                            if (this.Documents.Children.Count != 0)
                            {
                                if (Global.TimeStepperView == null) Global.TimeStepperView = new Views.TimeStepperView();

                                foreach (var child in this.Documents.Children)
                                {
                                    HandleCurrentDocument(child as LayoutDocument);
                                }

                                if (!Global.TimeStepperView.IsVisible) Global.TimeStepperView.Show();
                            }
                        } break;
                    case "edit_event":
                        {
                            if (this.Documents.Children.Count != 0)
                            {
                                if (Global.BirthDataView == null) Global.BirthDataView = new Views.BirthDataView();

                                foreach (var child in this.Documents.Children)
                                {
                                    HandleCurrentDocument(child as LayoutDocument);
                                }

                                if (!Global.BirthDataView.IsVisible) Global.BirthDataView.Show();
                            }
                        } break;
                    case "new":
                        {
                            OnNewImp();
                        } break;
                    case "convert_date":
                        {
                            if (Global.ConvertDateView == null) Global.ConvertDateView = new SevenAstro2.Views.ConvertDateView();

                            if (!Global.ConvertDateView.IsVisible) Global.ConvertDateView.Show();
                        } break;
                    case "locations":
                        {
                        } break;
                }
            }
        }

        private void OnNewImp()
        {
            newDocNum++;
            var title = string.Format("New {0}", newDocNum);

            var view = new Views.EventView();
            view.Event.BirthData.Name = title;
            view.Event.BirthData.PropertyChanged += BirthData_PropertyChanged;
            view.Event.Update();
            view.Event.UpdateAge();

            var frame = new Frame();
            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            frame.Navigate(view);

            var doc = new LayoutDocument();
            doc.Title = title;
            doc.Content = frame;
            doc.Closed += LayoutDocument_Closed;
            doc.Closing += LayoutDocument_Closing;
            doc.IsSelectedChanged += LayoutDocument_IsSelectedChanged;

            this.Documents.Children.Add(doc);
            this.Documents.SelectedContentIndex = this.Documents.ChildrenCount - 1;

            frame.Navigated += (object sender2, NavigationEventArgs e2) =>
            {
                if (Global.BirthDataView == null) Global.BirthDataView = new Views.BirthDataView();

                HandleCurrentDocument(doc, true);

                if (!Global.BirthDataView.IsVisible) Global.BirthDataView.Show();
            };
        }

        private void OnPrintImp()
        {
            if (this.Documents.Children.Count > 0)
            {
                LayoutDocument doc = null;

                foreach (var child in this.Documents.Children)
                {
                    var buffer = child as LayoutDocument;

                    if (buffer != null)
                    {
                        if (buffer.IsSelected)
                        {
                            doc = buffer;
                            break;
                        }
                    }
                }

                if (doc != null)
                {
                    if (doc.IsSelected)
                    {
                        var frame = doc.Content as Frame;
                        Views.EventView eventView = null;

                        if (frame != null)
                        {
                            eventView = frame.Content as Views.EventView;

                            if (eventView != null)
                            {
                                if (eventView.Event != null)
                                {
                                    var printView = new Views.PrintView(eventView.Event);
                                    printView.Show();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnSaveImp()
        {
            if (_sfd == null)
            {
                _sfd = new SaveFileDialog();
                _sfd.Filter = "Person (*.7astro)|*.7astro";
                _sfd.FileName = "Anonymous";
                _sfd.Title = "Save";
                _sfd.InitialDirectory = Global.PersonsDir;
            }

            if (this.Documents.Children.Count > 0)
            {
                LayoutDocument doc = null;

                foreach (var child in this.Documents.Children)
                {
                    var buffer = child as LayoutDocument;

                    if (buffer != null)
                    {
                        if (buffer.IsSelected)
                        {
                            doc = buffer;
                            break;
                        }
                    }
                }

                if (doc != null)
                {
                    if (doc.IsSelected)
                    {
                        var frame = doc.Content as Frame;
                        Views.EventView eventView = null;

                        if (frame != null)
                        {
                            eventView = frame.Content as Views.EventView;

                            if (eventView != null)
                            {
                                _sfd.FileName = System.IO.Path.Combine(
                                    Global.PersonsDir,
                                    (string.IsNullOrWhiteSpace(eventView.Event.BirthData.Name) || eventView.Event.BirthData.Name == "-" ? "Anonymous" : eventView.Event.BirthData.Name)) + ".7astro";

                                if (System.IO.File.Exists(_sfd.FileName))
                                {
                                    using (var writer = new System.IO.StreamWriter(_sfd.FileName, false, Encoding.UTF8))
                                    {
                                        writer.Write(eventView.Event.BirthData.JSONSerialize());
                                    }

                                    Saved(eventView);

                                    return;
                                }

                                var showed = _sfd.ShowDialog();

                                if (showed == true)
                                {
                                    using (var writer = new System.IO.StreamWriter(_sfd.FileName, false, Encoding.UTF8))
                                    {
                                        writer.Write(eventView.Event.BirthData.JSONSerialize());
                                    }

                                    Saved(eventView);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnOpenImp()
        {
            if (_ofd == null)
            {
                _ofd = new OpenFileDialog();
                _ofd.InitialDirectory = Global.PersonsDir;
                _ofd.Filter = "Person (*.7astro)|*.7astro";
                _ofd.Title = "Open";
            }

            var showed = _ofd.ShowDialog();

            if (showed == true)
            {
                var data = new Models.BirthData();

                using (var reader = new System.IO.StreamReader(_ofd.FileName, Encoding.UTF8))
                {
                    var str = reader.ReadToEnd().Trim();

                    data = str.JSONDeserialize<Models.BirthData>();
                }

                var title = data.Name;

                var view = new Views.EventView();
                view.Event.BirthData = data;
                view.Event.BirthData.PropertyChanged += BirthData_PropertyChanged;
                view.Event.Update();
                view.Event.UpdateAge();
                view.Event.UpdateJD();
                view.Event.UpdateTransient();
                view.SetAsJustOpened();

                var frame = new Frame();
                frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                frame.Navigate(view);

                var doc = new LayoutDocument();
                doc.Title = title;
                doc.Content = frame;
                doc.Closed += LayoutDocument_Closed;
                doc.Closing += LayoutDocument_Closing;
                doc.IsSelectedChanged += LayoutDocument_IsSelectedChanged;

                this.Documents.Children.Add(doc);
                this.Documents.SelectedContentIndex = this.Documents.ChildrenCount - 1;

                frame.Navigated += (object sender2, NavigationEventArgs e2) =>
                {
                    HandleCurrentDocument(doc, true);
                };
            }
        }

        void BirthData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            foreach (var child in this.Documents.Children)
            {
                HandleCurrentDocument(child as LayoutDocument);
            }
        }

        void LayoutDocument_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var doc = sender as LayoutDocument;

            if (doc != null)
            {
                var frame = doc.Content as Frame;
                Views.EventView eventView = null;

                if (frame != null)
                {
                    eventView = frame.Content as Views.EventView;
                    CircumstanceViewModel data = null;

                    if (eventView != null && eventView.Changed && !eventView.Saved && (data = eventView.DataContext as CircumstanceViewModel) != null && data.BirthData != null)
                    {
                        var res = MessageBox.Show(string.Format("Chart {0} is not saved.\r\nDo you want to close it anyway?", data.BirthData.Name), "Not Saved", MessageBoxButton.YesNo);

                        if (res == MessageBoxResult.No) e.Cancel = true;
                    }
                }
            }
        }

        private static void Saved(Views.EventView eventView)
        {
            var data = eventView.DataContext as CircumstanceViewModel;

            if (data != null && data.BirthData != null) MessageBox.Show(string.Format("'{0}' Saved", data.BirthData.Name));
            else MessageBox.Show("Saved");

            eventView.Saved = true;
        }

        void LayoutDocument_Closed(object sender, EventArgs e)
        {
            var doc = sender as LayoutDocument;

            if (doc != null)
            {
            }

            if (this.Documents.Children.Count == 0)
            {
                if (Global.BirthDataView != null)
                {
                    Global.BirthDataView.Close();

                    Global.BirthDataView = null;
                }

                if (Global.TimeStepperView != null)
                {
                    Global.TimeStepperView.Close();

                    Global.TimeStepperView = null;
                }

                if (Global.ConvertDateView != null)
                {
                    Global.ConvertDateView.Close();

                    Global.ConvertDateView = null;
                }
            }
        }

        private void OnCloaseAction()
        {
            if (Global.BirthDataView != null) Global.BirthDataView.Close();
            if (Global.TimeStepperView != null) Global.TimeStepperView.Close();
            this.Close();
        }

        //LayoutDocument _selectedDocument;
        private void LayoutDocument_IsSelectedChanged(object sender, EventArgs e)
        {
            var doc = sender as LayoutDocument;

            HandleCurrentDocument(doc);
        }

        private static void HandleCurrentDocument(LayoutDocument doc, bool assumeAsSelected = false)
        {
            if (doc != null)
            {
                if (doc.IsSelected || assumeAsSelected)
                {
                    var frame = doc.Content as Frame;
                    Views.EventView eventView = null;

                    if (frame != null)
                    {
                        eventView = frame.Content as Views.EventView;
                    }

                    //_selectedDocument = doc;
                    if (Global.BirthDataView != null)
                    {
                        if (eventView != null)
                        {
                            Global.BirthDataView.DataContext = eventView.Event;
                        }
                    }

                    if (Global.TimeStepperView != null)
                    {
                        if (eventView != null)
                        {
                            Global.TimeStepperView.DataContext = eventView.Event;
                        }
                    }

                    if (eventView != null)
                    {
                        try { doc.Title = string.Format("{0} ({1:yyyy/MM/dd HH:mm:ss})", eventView.Event.BirthData.Name, eventView.Event.BirthData.VDateTime); }
                        catch (Exception x) { Global.LogError(x); }
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            OnCloaseAction();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var child in this.Documents.Children)
            {
                var doc = child as LayoutDocument;

                if (doc != null)
                {
                    var frame = doc.Content as Frame;
                    Views.EventView eventView = null;

                    if (frame != null)
                    {
                        eventView = frame.Content as Views.EventView;
                        CircumstanceViewModel data = null;

                        if (eventView != null && eventView.Changed && !eventView.Saved && (data = eventView.DataContext as CircumstanceViewModel) != null && data.BirthData != null)
                        {
                            var res = MessageBox.Show(string.Format("Chart {0} is not saved.\r\nDo you want to close it anyway?", data.BirthData.Name), "Not Saved", MessageBoxButton.YesNo);

                            if (res == MessageBoxResult.No)
                            {
                                e.Cancel = true;

                                return;
                            }
                        }
                    }
                }
            }
        }

        private void CommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OnOpenImp();
        }

        private void SaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OnSaveImp();
        }

        private void NewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OnNewImp();
        }
    }
}
