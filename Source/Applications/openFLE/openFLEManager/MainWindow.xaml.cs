//*********************************************************************************************************************
// MainWindow.xaml.cs
// Version 1.1 and subsequent releases
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
// --------------------------------------------------------------------------------------------------------------------
//
// Version 1.0
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  07/19/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows;
using GSF.IO;
using Microsoft.Win32;

namespace openFLEManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region [ Constructors ]

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region [ Properties ]

        public AboutWindow AboutWindow
        {
            get
            {
                return Resources["AboutWindow"] as AboutWindow;
            }
        }

        public ViewModel ViewModel
        {
            get
            {
                return Resources["ViewModel"] as ViewModel;
            }
        }

        #endregion

        #region [ Methods ]

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Load();
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            AboutWindow.Close();
        }

        private void EventDefinitionsBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string eventDefinitionsFile = ViewModel.DeviceDefinitionsFile;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.FileName = FilePath.GetAbsolutePath(eventDefinitionsFile);
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML Files|*.xml|All Files|*.*";

            if (openFileDialog.ShowDialog() ?? false)
                ViewModel.DeviceDefinitionsFile = openFileDialog.FileName;
        }

        private void DropFolderBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string dropFolder = ViewModel.DropFolder;
            BrowseFolders(ref dropFolder);
            ViewModel.DropFolder = dropFolder;
        }

        private void DebugFolderBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string debugFolder = ViewModel.DebugFolder;
            BrowseFolders(ref debugFolder);
            ViewModel.DebugFolder = debugFolder;
        }

        private void BrowseFolders(ref string folder)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.SelectedPath = FilePath.GetAbsolutePath(folder);

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                folder = folderDialog.SelectedPath;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Save();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to save configuration file. This may require elevated privileges.",
                    "openFLE Manager Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LaunchConsoleMonitor();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow.Owner = this;
            AboutWindow.ShowDialog();
        }

        private void AboutWindow_Closing(object sender, CancelEventArgs e)
        {
            if (IsLoaded)
            {
                e.Cancel = true;
                AboutWindow.Visibility = Visibility.Hidden;
            }
        }

        #endregion
    }
}
